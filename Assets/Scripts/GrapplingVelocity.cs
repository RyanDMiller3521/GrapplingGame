using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GrapplingVelocity : MonoBehaviour
{
    public Camera cam;
    public RaycastHit hit; //want to change this to an object as an attempt

    public LayerMask cullingMask;
    public float maxDistance;

    public bool IsPulling;
    public Vector3 loc;

    public float speed = 10;
    public Transform hand;

    //public FirstPersonController FPC;
    public RigidbodyFirstPersonController FPC;
    public LineRenderer LR;

    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.CanMove = !GameManager.Instance.CanMove;
            Debug.Log(GameManager.Instance.CanMove);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            FindSpot();
        }

        if (IsPulling)
        {
            Pulling();
        }
        if (Input.GetButtonUp("Fire1") && IsPulling)
        {

            HookReturn();
        }

    }

    public void FindSpot()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistance, cullingMask))
        {
            IsPulling = true;
            loc = hit.point;
            GameManager.Instance.CanMove = false;
            LR.enabled = true;
            LR.SetPosition(1, loc);
        }
    }

    public void Pulling()
    {
        /*if (Vector3.Distance(transform.position, loc) > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, loc, speed * Time.deltaTime / Vector3.Distance(transform.position, loc)); // helps with the look depend on distance to the object
            LR.SetPosition(0, hand.position);
        }*/
        if (Vector3.Distance(transform.position, loc) > 0.5f)
        {

            LR.SetPosition(0, hand.position);
        }

        /*if(Vector3.Distance(transform.position, loc) < 0.5f)
        {
            IsPulling = false;
            GameManager.Instance.CanMove = true;
            LR.enabled = false;
        }*/
    }

    public void HookReturn()
    {
        IsPulling = false;
        GameManager.Instance.CanMove = true;
        LR.enabled = false;
    }
}
