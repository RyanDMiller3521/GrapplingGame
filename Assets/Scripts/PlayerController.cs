using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody myRigidBody;
    private Vector3 inputs = Vector3.zero;
    private Transform Cam;                  // A reference to the main camera in the scenes transform.
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
    private bool forceMovement = true;

    void Start()
    {
        myCollider = GetComponent<CapsuleCollider>();
        disToGround = myCollider.height / 2;
        myRigidBody = GetComponent<Rigidbody>();
        Cam = Camera.main.transform;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            forceMovement = !forceMovement;
            Debug.Log("forceMovement is " + forceMovement);
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.CanMove == true)
            movement();
    }

    void movement()
    {
        getUserInputs();
        isGrounded = groundedChecker();
        // adds a vertical force so the character will jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            myRigidBody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
        //added both physics based movement and non pyschics based movement to test a few things just press backspace key to switch between the 2 different kinds of movement
        //i left all the other comments and things from the original scpit to make it easy to go back if we need to
        if (forceMovement)
        {
            //Debug.Log(myRigidBody.velocity);
            if (Input.GetButton("Sprint") && isGrounded)
            {
                Debug.Log(inputs * runSpeed);
                //todo: fix the difference between using force and not using force when sprinting and walking
                myRigidBody.AddRelativeForce(inputs * runSpeed, ForceMode.Force);//sprint using force
            }
            else
            {
                //walk without using force
                //myRigidBody.MovePosition(myRigidBody.position + inputs * walkSpeed * Time.fixedDeltaTime); // walking speed just moves the position of the character currently does not use velocity at all
                //attempting to fix problem. Uncomment the above line and comment out the below line to return to previous broken
                myRigidBody.AddRelativeForce(inputs * walkSpeed, ForceMode.Force);
                if (GameManager.Instance.CapVelocity)
                {
                    CapVelocity();
                }
            }
        }
        else
        {
            //Debug.Log(myRigidBody.velocity);
            if (Input.GetButton("Sprint") && isGrounded)
            {
                myRigidBody.MovePosition(myRigidBody.position + inputs * runSpeed * Time.fixedDeltaTime);//sprint using force
            }
            else
            {
                myRigidBody.MovePosition(myRigidBody.position + inputs * walkSpeed * Time.fixedDeltaTime); // walking speed just moves the position of the character currently does not use velocity at all
            }
        }
    }

    void getUserInputs(){
        // gets the inputs for the character movement
        inputs = Vector3.zero;
        inputs.x = Input.GetAxis("Horizontal");
        inputs.z = Input.GetAxis("Vertical");
        CamForward = Vector3.Scale(Cam.forward, new Vector3(1, 0, 1)).normalized;
        inputs = inputs.z * CamForward + inputs.x * Cam.right;
    }

    void CapVelocity()//caps the velocity of the player when just running on the ground
    {
        if (Input.GetButton("Sprint"))
        {
            myRigidBody.velocity = Vector3.ClampMagnitude(myRigidBody.velocity, maxSpeed);
        }
        else
            myRigidBody.velocity = Vector3.ClampMagnitude(myRigidBody.velocity, walkSpeed);
    }

    bool groundedChecker()
    {
        //checks to see if the player is touching the ground or anything below his feet
        return Physics.Raycast(transform.position, Vector3.down, disToGround);
    }

    void OnCollisionExit(Collision collision){
        Debug.Log("Collided");

    }
}

/*HERE IS THE ORIGINAL CODE IN CASE YOU DON'T LIKE WHAT I DID OR IT DOESN'T WORK THE SAME
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody myRigidBody;
    private Vector3 inputs = Vector3.zero;
    private Transform Cam;                  // A reference to the main camera in the scenes transform.
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

    }

    void FixedUpdate()
    {
        if (GameManager.Instance.CanMove == true)
            movement();

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

        //debugging code for sticking to the wall when sprinting
        if(inputs.x == 0 && inputs.z == 0){

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

 */