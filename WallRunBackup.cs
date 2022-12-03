using UnityEngine.UI;
using UnityEngine;
using System.Collections;



public class WallRunBackup : MonoBehaviour
{

    public ParticleSystem WindParticle;
    public LayerMask whatIsWall;
    public float wallrunForce, maxWallSpeed;
    bool isWallRight, isWallLeft;
    public bool WallisRight { get => isWallRight; }
    public bool WallisLeft { get => isWallLeft; }
    public bool isWallRunning { get; private set; }
    public float Range;
    [Space(2f)]
    [SerializeField] move MoveScript;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameManager gm;
    [SerializeField] Transform FpsCam;
    [SerializeField] Transform orientation;
    [SerializeField] private Joystick Joystick;
    [SerializeField] float jumpForce = 350;
    [SerializeField] private float AngleTilt = 10f;
    Vector3 normalVector;
    Camera cam;
    float baseFOV;
    [SerializeField] private float TiltTime;
    private void OnEnable()
    {
        orientation = transform;
        cam = FpsCam.GetComponent<Camera>();
        baseFOV = cam.fieldOfView;   
    }

    private void Update()
    {
        CheckForWall();
        WallRunInput();
        if (isWallRunning)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV * 2f, Time.deltaTime * 8f);
            if (isWallLeft)
            {
                float zRot = Mathf.Lerp(0, -AngleTilt, TiltTime);
                FpsCam.localEulerAngles = new Vector3(FpsCam.localEulerAngles.x, FpsCam.localEulerAngles.y, zRot);
            }
            else if (isWallRight)
            {
                float zRot = Mathf.Lerp(0, AngleTilt, TiltTime);
                FpsCam.localEulerAngles = new Vector3(FpsCam.localEulerAngles.x, FpsCam.localEulerAngles.y, zRot);

            }

        }
        
    }

    public void WallJump()
    {
        if (!MoveScript.OnGround && isWallRunning)
        {
            Jump();
        }
    }
    
    private void Jump()
    {
        if (isWallLeft && !(Joystick.Horizontal > 0) || isWallRight && !(Joystick.Horizontal < 0))
        {
            rb.AddForce(Vector2.up * jumpForce * 0.75f);
            rb.AddForce(normalVector * jumpForce * 0.5f);
        }
        if (Joystick.Horizontal < 0 && isWallRight)
        {
            rb.AddForce(-orientation.right * jumpForce * 3.2f * 2f);
        }
        if (Joystick.Horizontal > 0 && isWallLeft)
        {
            rb.AddForce(orientation.right * jumpForce * 3.2f * 2f);
        }
        rb.AddForce(orientation.forward * jumpForce);
        rb.velocity = Vector3.zero;
    }
    public Transform Wall;
    private void WallRunInput()
    {
        if (MoveScript.OnGround)
            return;
        //Wallrun
        if (Joystick.Horizontal > 0.2f && isWallRight) StartWallrun();
        if (Joystick.Horizontal < -0.2f && isWallLeft) StartWallrun();
    }

    private void StartWallrun()
    {
        TimeWithoutInput = 0;
        BufferTime = 0;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.useGravity = false;
        isWallRunning = true;
        if (!WindParticle.isPlaying)
        {
            WindParticle.Play();
        }
        
        if (rb.velocity.magnitude <= maxWallSpeed)
        {
            rb.AddForce(orientation.forward * wallrunForce * Time.deltaTime, ForceMode.VelocityChange);

            if (isWallRight)
            {
                rb.AddForce(10 * orientation.right * wallrunForce * 1.5f / 5 * Time.deltaTime);
            }
            else
            {
                rb.AddForce(10 * -orientation.right * wallrunForce * 1.5f / 5 * Time.deltaTime);
            }
            
        }
    }
    private void StopWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true;
        if (WindParticle.isPlaying)
        {
            WindParticle.Stop();
            cam.fieldOfView = baseFOV;
        }

        Wall = null;
        float zRot = Mathf.Lerp(FpsCam.localEulerAngles.z, 0, TiltTime);
        FpsCam.localEulerAngles = new Vector3(FpsCam.localEulerAngles.x, FpsCam.localEulerAngles.y, zRot);
        
    }
   
    float TimeWithoutInput = 0;
    float BufferTime = 0;
    private void CheckForWall()
    {

        isWallRight = Physics.Raycast(transform.position, orientation.right, Range+0.5f, whatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, Range, whatIsWall);
        //leave wall run
        if (isWallRunning)
        {
            if (isWallLeft && Joystick.Horizontal > -0.5f)
                TimeWithoutInput += Time.deltaTime;
            if (isWallRight && Joystick.Horizontal < 0.5f)
                TimeWithoutInput += Time.deltaTime;
        }
        // || TimeWithoutInput > 0.7f

        if (!isWallLeft && !isWallRight && !MoveScript.OnGround) BufferTime += Time.deltaTime;
        if (BufferTime > 0.1f || TimeWithoutInput > 0.6f || !isWallRunning) StopWallRun();
    }
    public Text t;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("tmkb"))
        {
           
            t.text = "Level Complete";
            Invoke(nameof(Next), 0.5f);
        }
    }

    private void Next()
    {
        gm.CurrentLevelComplete();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 9)
        {
            
            normalVector = collision.GetContact(0).normal;
            Transform t = collision.collider.transform;
            if (t.GetComponent<Animator>() != null)
            {
                t.GetComponent<Animator>().enabled = false;
                StopCoroutine(StartWithDelay(t.GetComponent<Animator>()));
            }
            else if(t.parent.GetComponent<Animator>()!=null)
            {
                print(t.parent.name);
                t.parent.GetComponent<Animator>().enabled = false;
                StopCoroutine(StartWithDelay(t.parent.GetComponent<Animator>()));
            }
            else if(t.GetComponent<SwayScript>()!=null)
            {
                t.GetComponent<SwayScript>().enabled = false;
                StopCoroutine(StartWithDelay(t.GetComponent<SwayScript>()));
            }
            Wall = collision.collider.transform.GetChild(collision.collider.transform.childCount - 1); ;
            AudioManager a = AudioManager.instance;
            if(a!=null)
            {
                a.Play("WallEffect");
            }
            Vector3 v = transform.eulerAngles;
            transform.LookAt(Wall);
            transform.localEulerAngles = new Vector3(v.x, transform.localEulerAngles.y, v.z);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.layer == 9)
        {
            Transform t = collision.collider.transform;
            if (t.GetComponent<Animator>() != null)
            {
                StartCoroutine(StartWithDelay(t.GetComponent<Animator>()));
                if (t.name.Equals("RIght_Wall_Anim"))
                {
                    t.position = new Vector3(-54.4f, 32.9f, -424.7f);
                }
            }
            else if (t.parent.GetComponent<Animator>() != null)
            {

                StartCoroutine(StartWithDelay(t.parent.GetComponent<Animator>()));
            }
            else if(t.GetComponent<SwayScript>() != null)
            {
                StartCoroutine(StartWithDelay(t.GetComponent<SwayScript>()));
            }
        }
    }
    IEnumerator StartWithDelay(Behaviour c)
    {
        yield return new WaitForSeconds(1.5f);
        c.enabled = true;
    }
}
