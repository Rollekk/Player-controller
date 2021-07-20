using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public Transform weaponEnd;
    public Transform playerCamera;
    public Transform player;

    [Space]
    private LineRenderer ropeLine;
    private Vector3 ropeEnd;
    [SerializeField] private SpringJoint springJoint;
    public LayerMask grappleMask;

    [Space]
    public float grappleDistance;

    // Start is called before the first frame update
    void Start()
    {
        ropeLine = GetComponent<LineRenderer>();   
    }

    private void LateUpdate()
    {
        if(springJoint) DrawRope();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Grapple();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

       // if(springJoint.)
    }

    void Grapple()
    {
        RaycastHit grappleHit;
        if(Physics.Raycast(playerCamera.position, playerCamera.forward, out grappleHit ,grappleDistance, grappleMask))
        {
            ropeEnd = grappleHit.point;
            springJoint = player.gameObject.AddComponent<SpringJoint>();
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.connectedAnchor = ropeEnd;

            float distanceFromPoint = Vector3.Distance(player.position, ropeEnd);

            springJoint.maxDistance = distanceFromPoint * 0.05f;
            springJoint.minDistance = distanceFromPoint * 0.01f;

            springJoint.spring = 4.5f;
            springJoint.damper = 7f;
            springJoint.massScale = 4.5f;

            ropeLine.positionCount = 2;
        }
    }

    void StopGrapple()
    {
        ropeLine.positionCount = 0;
        Destroy(springJoint);
    }

    void DrawRope()
    {
        ropeLine.SetPosition(0, weaponEnd.position);
        ropeLine.SetPosition(1, ropeEnd);
    }
}
