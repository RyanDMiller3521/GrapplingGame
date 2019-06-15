﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody myRigidBody;
    private Vector3 inputs = Vector3.zero;
    private Transform Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 CamForward;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float maxSpeed;
    private bool isGrounded = true;
    private float disToGround;
    private CapsuleCollider myCollider;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<CapsuleCollider>();
        disToGround = myCollider.height / 2;
        myRigidBody = GetComponent<Rigidbody>();
        Cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CanMove == true)
            movement();
    }

    void FixedUpdate()
    {
        isGrounded = groundedChecker();
        Debug.Log(myRigidBody.velocity);
        if (Input.GetButton("Sprint") && isGrounded)
        {
            myRigidBody.AddRelativeForce(inputs * runSpeed, ForceMode.Force);
            Debug.Log(inputs * runSpeed);
            if (GameManager.Instance.CapVelocity)
            {
                CapVelocity();
            }
        }
        else
        {
            myRigidBody.MovePosition(myRigidBody.position + inputs * walkSpeed * Time.fixedDeltaTime); // walking speed just moves the position of the character currently does not use velocity at all
        }
    }

    void movement()
    {
        // gets the inputs for the character movement
        inputs = Vector3.zero;
        inputs.x = Input.GetAxis("Horizontal");
        inputs.z = Input.GetAxis("Vertical");
        CamForward = Vector3.Scale(Cam.forward, new Vector3(1, 0, 1)).normalized;
        inputs = inputs.z * CamForward + inputs.x * Cam.right;
        // adds a vertical force so the character will jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            myRigidBody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
    }

    void CapVelocity()//caps the velocity of the player when just running on the ground
    {
        myRigidBody.velocity = Vector3.ClampMagnitude(myRigidBody.velocity, maxSpeed);
    }

    bool groundedChecker()
    {
        //checks to see if the player is touching the ground or anything below his feet
        return Physics.Raycast(transform.position, Vector3.down, disToGround);
    }
}
