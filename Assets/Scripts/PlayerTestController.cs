using UnityEditor;
using UnityEngine;

public class PlayerTestController : MonoBehaviour
{
    private CharacterController characterController;
    private Rigidbody rigidbody;
    private Vector3 inputs;
    private float disToGround;
    private CapsuleCollider myCollider;
    private float verticalVelocity;
    private Vector3 currentVelocity;

    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float sprintSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float gravity = 9.81f * 9.81f;


    void Start(){
        myCollider = GetComponent<CapsuleCollider>();
        disToGround = myCollider.height / 2;
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
        characterController = this.gameObject.GetComponent<CharacterController>();

        //this should not be left in the final. This is just for testing purposes. If we decide to use character controller, the rigid body should be removed and this block won't be needed
        if(characterController.enabled){
            Destroy(rigidbody);
        }
    }


    Vector3 mainTarget = Vector3.zero;
    RaycastHit hit;
    void Update(){
        movement();


        //test code
        if(Input.GetKey(KeyCode.Q)) {
            if(Physics.Raycast(this.transform.position, Camera.main.transform.forward,  out hit, 20f)){
                mainTarget = hit.transform.position;
                mainTarget = hit.point;
            }
        }
        if(mainTarget != Vector3.zero){
            moveTowards(mainTarget);
        }
    }

    void movement(){
        getUserInputs();

        if(characterController.isGrounded){
            verticalVelocity = -gravity * Time.deltaTime;
            if(Input.GetButtonDown("Jump")){
                verticalVelocity = jumpForce;
            }
        }else{
            //this is gravity falling. This can be disabled when we begin grappling and reapplied as needed.
            verticalVelocity -= gravity * Time.deltaTime;
        }

        if(GameManager.Instance.CapVelocity){
            capVelocity();
        }
        inputs.y = verticalVelocity;//this is set here since we don't want to cap this velocity.
        characterController.Move(inputs * Time.deltaTime);

    }

    private Vector3 getPlayerVelocity(){
        return characterController.velocity;
    }

    private void getUserInputs(){
        //inputs = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        inputs = Vector3.zero;
        inputs.x = Input.GetAxis("Horizontal");
        inputs.z = Input.GetAxis("Vertical");
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1f, 0f, 1f)).normalized;
        Vector3 baseInputs = (inputs.z * camForward + inputs.x * Camera.main.transform.right);//will be used both for sprinting and walking

        if(Input.GetButton("Sprint") && characterController.isGrounded){
            inputs = baseInputs * sprintSpeed;
        }else{
            inputs = baseInputs * walkSpeed;
        }
    }

    private void capVelocity(){
        inputs = Vector3.ClampMagnitude(inputs, maxSpeed);
    }

    private void moveTowards(Vector3 target){
        Vector3 offset = target - this.transform.position;

        if(offset.magnitude > 0.1f){
            offset = offset.normalized * walkSpeed;//walkspeed can be replaced with a shoot speed if we want
            characterController.Move(offset * Time.deltaTime);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit colliderHit){
        if(!colliderHit.gameObject.tag.Equals("Environment") && mainTarget != Vector3.zero){
            mainTarget = Vector3.zero;
        }
    }
}

//ended up not using this but thought it might be helpful later
/*
[CustomEditor(typeof(PlayerTestController))]
public class PlayerTestControllerEditor : Editor{
    override
    public void OnInspectorGUI(){
        var playerTestController = target as PlayerTestController;

        playerTestController.useRealWorldGravity = GUILayout.Toggle(playerTestController.useRealWorldGravity, "Use Real Gravity");

        if(!playerTestController.useRealWorldGravity){
            playerTestController.gravity = EditorGUILayout.FloatField("Gravity Value: ", playerTestController.gravity);
        }
    }
}
*/