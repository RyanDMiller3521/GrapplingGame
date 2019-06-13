using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody myRigidBody;
    private Vector3 inputs = Vector3.zero;
    private bool isGrounded = true;
    //private Transform groundChecker;
    public float walkSpeed;
    public float runSpeed;
    public float jumpHeight;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        //groundChecker = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CanMove == true)
            movement();
    }

    void FixedUpdate()
    {
        myRigidBody.MovePosition(myRigidBody.position + inputs * walkSpeed * Time.fixedDeltaTime);
    }

    void movement()
    {
        inputs = Vector3.zero;
        inputs.x = Input.GetAxis("Horizontal");
        inputs.z = Input.GetAxis("Vertical");
        //if (inputs != Vector3.zero)
            //transform.forward = inputs;
    }
}
