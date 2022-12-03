using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class move : MonoBehaviour
{
    [Header("Remove All Wall Run Related Stuff for script to work")]
    [Space]
    [SerializeField] private WallRunBackup wb;
    [SerializeField] private Joystick joystick;
    public bool sprint;
    public float SwipeMagnitude;
    [SerializeField] private RectTransform RightHalf;
    [SerializeField] private float MinimumSwipe;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera FpsCam;
    #region Variables
    [Range(100f, 500f)]
    public float speed = 350f;
    
    private float speedchange = 2;
    
    private float basefv = 60f;
    public LayerMask g;
    [SerializeField] private readonly float MinThreshold = 1f;
    [Range(500f, 1500f)]
    public float jumpf1 = 1000f;
    [Range(0f, 2f)]
    [SerializeField] private float fvmod = 2f;
    
    float pitch = 0f;
    [Range(1f, 90f)]
    [SerializeField] private float maxPitch = 85f;
    [Range(-1f, -90f)]
    [SerializeField] private float minPitch = -85f;
    [SerializeField] Transform GroundCheck;
    public float acceleration;
    public bool OnGround { get; private set; }

    float magnitude;
    Vector3 Change;
    #endregion
    private void OnEnable()
    {
        basefv = FpsCam.fieldOfView;
        SwipeMagnitude = PlayerPrefs.GetFloat("Sensitivity", 200f);
        if (!FindObjectOfType<GameManager>().IsInTutorial)
        {
            float f = PlayerPrefs.GetFloat("JoystickSize", 0.7f);
            joystick.GetComponent<RectTransform>().localScale = new Vector3(f, f);
        }
        
    }
    
    void Update()
    {
        Look();
        RotateCamera(InputVector);
        OnGround = Physics.Raycast(GroundCheck.position, Vector3.down, 0.3f, g);
    }
    public bool IsGrounded()
    {
        return OnGround;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Jumpad"))
        {
            OnJumpad = true;
        }
        else
            OnJumpad=false;
        
    }
    bool OnJumpad;
   [SerializeField] float JumpadCoeff;
    public void JumpMethod()
    {
        if (OnGround && !wb.isWallRunning)
        {
            rb.AddForce(Vector3.up * jumpf1);
        }
        else if(OnJumpad)
        {
            Vector3 JumpVec = (transform.forward * 2f + transform.up).normalized;
            rb.AddForce(JumpVec * jumpf1 *JumpadCoeff, ForceMode.Impulse);
            rb.AddForce(transform.forward * jumpf1*JumpadCoeff, ForceMode.Impulse);
        }
        
    }
    #region MovementFunction
    WaitForSeconds ws = new WaitForSeconds(0.1f);
    IEnumerator CheckAcceleration(Vector3 Velocity, Vector3 position)
    {
        
        yield return ws;
        float delta = rb.velocity.magnitude - Velocity.magnitude;
        if (delta < -1f)
        {
            acceleration = delta;
        }
        Vector3 v=transform.position - position;
        Change = new Vector3(v.x, 0, v.z); 
            
    }
    public void ChangeSprintTrue() => sprint = !sprint;
    public Text t;
    private void ImplementMovement()
    {
        if (wb.isWallRunning)
        {
            return;
        }


        float movex = joystick.Horizontal;
        float movey = joystick.Vertical;

        bool Issprint = sprint && movey > 0;
        float newspeed = speed;
        if (Issprint)
        {
            newspeed *= speedchange;
        }
        Vector3 vector = new Vector3(movex, 0, movey);
        vector.Normalize();
        Vector3 targetv = transform.TransformDirection(vector) * newspeed * Time.deltaTime;
        targetv.y = rb.velocity.y;

        
            rb.velocity = targetv;
            StartCoroutine(CheckAcceleration(rb.velocity, transform.position));
        if (acceleration < 0 && OnGround)
        {
            AddCounterForce(acceleration, Change);
        }

    }

    private void AddCounterForce(float _acceleration, Vector3 _direction)
    {
        if (!OnGround)
            return;
        magnitude = -_acceleration;
        rb.AddForce(_direction * magnitude * 100);
        acceleration = 0;
    }
#endregion
    
    #region Camera Function

    
    private Vector2 InputVector;
    void Look()
    {
        if (Input.touchCount > 0)
        {
            
            foreach (Touch touch in Input.touches)
            {

                if (RectTransformUtility.RectangleContainsScreenPoint(RightHalf, touch.position))
                {

                    if (touch.phase == TouchPhase.Moved)
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(RightHalf, touch.position + touch.deltaPosition))
                        {
                            InputVector.x = Mathf.Lerp(InputVector.x, touch.deltaPosition.x, 0.9f);
                            InputVector.y = Mathf.Lerp(InputVector.y, touch.deltaPosition.y, 0.9f);
                        }
                        else
                        {
                            InputVector = Vector2.zero;
                        }
                    }
                    else if (touch.phase == TouchPhase.Stationary)
                    {
                        InputVector = Vector2.zero;
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        InputVector = Vector2.zero;
                    }
                    else
                        InputVector = Vector2.zero;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    InputVector = Vector2.zero; 
                }
            }

        }
    }

    private void RotateCamera(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > MinimumSwipe || Mathf.Abs(dir.y) > MinimumSwipe)
        {
            dir = dir.normalized * SwipeMagnitude;
            float xInput = dir.x * Time.deltaTime;
            float yInput = dir.y * Time.deltaTime;
            transform.Rotate(0, xInput, 0);
            //now add on y input to pitch, and clamp it
            pitch -= yInput;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            //create the local rotation value for the camera and set it
            Quaternion rot = Quaternion.Euler(pitch, FpsCam.transform.localEulerAngles.y, FpsCam.transform.localEulerAngles.z);
            FpsCam.transform.localRotation = rot;

        }

    }
    #endregion

    void FixedUpdate()
    {
        ImplementMovement();
    }
    
}