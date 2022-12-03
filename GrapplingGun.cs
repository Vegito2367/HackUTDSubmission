using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrapplingGun : MonoBehaviour
{
    
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, _camera, player;
   [SerializeField] private float maxDistance = 25f;
    private SpringJoint joint;
    Transform currentTarget;
    float distance;
    [SerializeField] WallRunBackup wb;
    [SerializeField] move MoveScript;
    [SerializeField] AudioManager am;
    [SerializeField] GameManager gm;
    [SerializeField] private LineRenderer lr;
    [SerializeField] Rigidbody rb;
    [SerializeField] float FOVmod;
    [SerializeField] float baseFOV;
    Camera cam;
    void OnEnable()
    {
        maxDistance = 35f + (10f * PlayerDataStorer.GrappleUpgradeLevel);
        if (AudioManager.instance != null)
            am = AudioManager.instance;
        cam = _camera.GetComponent<Camera>();
        distance = maxDistance;
    }
    
    void LateUpdate()
    {
        DrawRope();
    }
    #region ZombieCode_AdddownForce
    //IEnumerator AddDownForce()
    //{
    //    while (IsGrappling())
    //    {
    //        rb.AddForce(Vector3.down * 10f * Time.deltaTime);
    //        yield return new WaitForSeconds(1f);
    //    }
    //}
    #endregion
    public void LerpFOV()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, FOVmod * baseFOV, Time.deltaTime * 8f);  
    }

    public void RevLerpFOV()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV, Time.deltaTime * 8f);
    }
    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    public void StartGrapple()
    {
        if (MoveScript.OnGround)
            return;
        foreach (GameObject item in gm.GrapplePoints)
        {
            if (IsGrappling())
                break;
            float d = Vector3.Distance(item.transform.position, player.position);
            if (d < distance)
            {
                distance = d;
                currentTarget = item.transform;
                currentTarget.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (currentTarget == null)
            return;
        if (am != null)
        {
            am.Play("GrapplingGun");
        }
        LerpFOV();
        MoveScript.sprint= true;
        grapplePoint = currentTarget.position;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;
        

        float distanceFromPoint = Vector3.Distance(player.position, grapplePoint) - 5f;
        //The distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        //Adjust these values to fit your game.
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }

    
    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    public void StopGrapple()
    {
        if (currentTarget == null)
            return;
        MoveScript.sprint = false;
        lr.positionCount = 0;
        if(currentTarget.GetChild(1).gameObject!=null)
        {
            currentTarget.GetChild(1).gameObject.SetActive(false);
        }
        currentTarget = null;
        distance = maxDistance;
        grapplePoint = Vector3.zero;
        cam.fieldOfView = baseFOV;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;
    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}