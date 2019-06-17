using UnityEngine;

public class PlayerTestController : MonoBehaviour
{
    private CharacterController characterController;
    private Rigidbody rigidbody;
    private Vector3 inputs;
    private float disToGround;
    private CapsuleCollider myCollider;
    private float gravity = 9.81f * 9.81f;
    private float jumpForce = 40f;
    private float verticalVelocity;
    private bool isGrounded;

    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float sprintSpeed;

    void Start(){
        myCollider = GetComponent<CapsuleCollider>();
        disToGround = myCollider.height / 2;
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
        characterController = this.gameObject.GetComponent<CharacterController>();
        if(characterController.enabled){
            Destroy(rigidbody);
        }
    }


    void Update(){
        movement();
    }

    void movement(){
        getUserInputs();

        if(groundedChecker()){
            verticalVelocity = -gravity * Time.deltaTime;
            if(Input.GetButtonDown("Jump")){
                verticalVelocity = jumpForce;
            }
        }else{
            Debug.Log("Else called");
            verticalVelocity -= gravity * Time.deltaTime;
        }

        if(GameManager.Instance.CapVelocity){
            capVelocity();
        }
        inputs.y = verticalVelocity;//this is set here since we don't want to cap this velocity.
        characterController.Move(inputs * Time.deltaTime);

    }

    private void getUserInputs(){
        //inputs = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        inputs = Vector3.zero;
        inputs.x = Input.GetAxis("Horizontal");
        inputs.z = Input.GetAxis("Vertical");
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1f, 0f, 1f)).normalized;
        Vector3 baseInputs = (inputs.z * camForward + inputs.x * Camera.main.transform.right);//will be used both for sprinting and walking

        bool isGrounded = groundedChecker();
        Debug.Log(isGrounded ? "Grounded" : "Not grounded");
        if(Input.GetButton("Sprint") && isGrounded){
            inputs = baseInputs * sprintSpeed;
        }else{
            inputs = baseInputs * walkSpeed;
        }
    }

    bool groundedChecker()
    {
        //checks to see if the player is touching the ground or anything below his feet
        return Physics.Raycast(transform.position, Vector3.down, disToGround);
    }

    private void capVelocity(){
        inputs = Vector3.ClampMagnitude(inputs, maxSpeed);
    }
}
