using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingHook : MonoBehaviour
{
    public LineRenderer LR;
    public Transform hand;
    public Camera cam;
    public RaycastHit hit; //want to change this to an object as an attempt

    public LayerMask cullingMask;
    public float maxDistance;
    bool IsHooked = false;
    public ConfigurableJoint connection;
    public Vector3 loc;
    private Rigidbody rb;
    private GameObject connectPoint;
    private float ropeLength;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

        }
        if (Input.GetButtonDown("Fire2"))
        {
            FindSpot();
        }
        if (IsHooked)
            Hooked();
        if (Input.GetButtonUp("Fire2") && IsHooked)
        {

            HookReturn();
        }
    }
    public void FindSpot()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistance, cullingMask))
        {
            IsHooked = true;
            loc = hit.point;
            LR.enabled = true;
            LR.SetPosition(1, loc);
            connectPoint = new GameObject("ConnectionPoint");
            connectPoint.transform.position = loc;
            /*connectPoint.transform.position = loc;
            connectPoint.AddComponent<ConfigurableJoint>();
            connection = connectPoint.GetComponent<ConfigurableJoint>();
            connection.connectedBody = rb;*/

            ropeLength = Vector3.Distance(transform.position, connectPoint.transform.position);
        }
    }
    public void Hooked()
    {
        LR.SetPosition(0, hand.position);
        Vector3 testPos = rb.position + rb.velocity * Time.deltaTime;
        float dist = Vector3.Distance(transform.position, connectPoint.transform.position);
        Debug.Log(dist);
        if(dist < ropeLength)
        {
            Debug.Log("To far");
            //transform.position = Vector3.MoveTowards(transform.position, loc, .1f);
            //transform.position = (testPos - connectPoint.transform.position) * ropeLength;
        }
    }

    public void MoveLine()
    {
        LR.SetPosition(0, hand.position);
    }
    public void HookReturn()
    {
        IsHooked = false;
        LR.enabled = false;
        Destroy(connection);
    }
}
