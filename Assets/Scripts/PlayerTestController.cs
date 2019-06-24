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
    private GameObject mainTarget = null;
    private RaycastHit targetHit;
    private bool gravityOn = true;//this could be put in the game manager if we want***IMPORTANT*** Anytime gravity is turned off, and back on, vertical velocity needs to be set back to zero when gravity is turned back on. Otherwise the player won't fall, but will instead be transported back to the ground.

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
    [SerializeField]
    private float straightShotSpeed;
    [SerializeField]
    private float shootDistance;
    [SerializeField]
    private GameObject firePos;
    [SerializeField]
    private float shotSpeed;
    [SerializeField]
    private GameObject pullHook;
    [SerializeField]
    private float returnDis;
    private GameObject pullShot = null;
   // private bool pullShotOut = false;
    private bool pulling = false;
    private bool returnPull = false;
    private Vector3 pullPoint;
    private Vector3 firePoint;
    private bool pullHit = false;


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


    void Update(){
        if(pullShot != null)//if the pull hook is in the game it will run all of the features of the hook depending on its current state
            checkPullShot();
        movement();
        //test code
    }

    void movement(){
        getUserInputs();

        if(GameManager.Instance.CapVelocity){
            capVelocity();
        }

        if(characterController.isGrounded){
            verticalVelocity = -gravity * Time.deltaTime;
            if(Input.GetButtonDown("Jump")){
                verticalVelocity = jumpForce;
            }
        }else{
            //this is gravity falling. This can be disabled when we begin grappling and reapplied as needed.
            verticalVelocity -= gravity * Time.deltaTime;
        }
        if (pulling)
        {
            moveTowards(pullPoint);
        }
        if (Input.GetButtonUp("Fire1"))//this allows you to stick to the target spot until you release the mouse button also allows the player to stop the hook at any moment
        {
            arrivedAtTarget();
        }


        if (gravityOn) {
            inputs.y = verticalVelocity;//this is set here since we don't want to cap this velocity.
        }
        characterController.Move(inputs * Time.deltaTime);

    }

    private Vector3 getPlayerVelocity(){
        return characterController.velocity;
    }

    private void getUserInputs(){
        //inputs = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if(GameManager.Instance.CanMove) {
            inputs = Vector3.zero;
            inputs.x = Input.GetAxis("Horizontal");
            inputs.z = Input.GetAxis("Vertical");
            Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1f, 0f, 1f)).normalized;
            Vector3 baseInputs = (inputs.z * camForward + inputs.x * Camera.main.transform.right);//will be used both for sprinting and walking

            if (Input.GetButton("Sprint") && characterController.isGrounded) {
                inputs = baseInputs * sprintSpeed;
            } else {
                inputs = baseInputs * walkSpeed;
            }
        }
//fires the pull shot
        if(Input.GetButtonDown("Fire1") && pullShot == null)
        {
            //this feels like bad coiding but it seems to have fixed the one of the problems when sending everything out
            firePoint = Vector3.zero;
            pullPoint = Vector3.zero;
            pullShot = null;
            pulling = false;
            returnPull = false;
            mainTarget = null;
            pullHit = false;
            if (Physics.Raycast(this.transform.position, Camera.main.transform.forward, out targetHit, shootDistance))
            {
                mainTarget = targetHit.transform.gameObject;
                pullHit = true;
                pullPoint = targetHit.point;
            }
            firePoint = this.transform.position;
            //creates the pull shot prefab then rotates it 90 degrees so it is sitting in the right position
            pullShot = Instantiate(pullHook, firePos.transform.position, Quaternion.identity) as GameObject;
            Debug.Log("Cameras x rotation is " + Camera.main.transform.rotation.x * 100);
            pullShot.transform.Rotate(Camera.main.transform.rotation.x * 100, Camera.main.transform.rotation.y * 100, Camera.main.transform.rotation.z * 100);
            pullShot.transform.Rotate(90, 0, 0);

        }
    }

    private void checkPullShot()//used to fire and return the pull shot 
    {
        if(pullHit && !returnPull)//checks if the raycast sent out hit a target if it did and its not returning than fires the hook
        {
            Vector3 offset = pullPoint - pullShot.transform.position;
            if(offset.magnitude > 0.1f)//moves the hook tell it hits the target
            {
                offset = offset.normalized * shotSpeed;
                pullShot.transform.position = Vector3.MoveTowards(pullShot.transform.position, pullPoint, shotSpeed * Time.deltaTime);
            }
            else//once it hits the target it begings to pull the player toward the target
            {
                pulling = true;//once pulling is set to true the movetowards function will begin to be called
                gravityOn = false;
                inputs = Vector3.zero;
                GameManager.Instance.CanMove = false;
            }
        }
        else if(!returnPull && !pullHit)//if the raycast did not hit anything it will fire the hook out to the max distance than return back to the player
        {
            if(shootDistance > Vector3.Distance(pullShot.transform.position, firePoint))
            {
                pullShot.transform.Translate(Vector3.up * shotSpeed * Time.deltaTime);//moves the hook out from the players original spot its vector.up becuase the hook if rotated 90 degrees to look right
            }
            else
            {
                returnPull = true;
            }
        }
        if(returnPull)//once the hook should return it will begin returning the hook
        {
            if (returnDis < Vector3.Distance(pullShot.transform.position, Camera.main.transform.position))
            {
                pullShot.transform.position = Vector3.MoveTowards(pullShot.transform.position, Camera.main.transform.position, shotSpeed * 2 * Time.deltaTime);
            }
            else
            {
                Destroy(pullShot);
                pullShot = null;
                pulling = false;
                returnPull = false;
                mainTarget = null;
                pullHit = false;
                //pullPoint = Vector3.zero;
            }
        }

    }
    private void capVelocity(){
        inputs = Vector3.ClampMagnitude(inputs, maxSpeed);
    }

    private void moveTowards(Vector3 target){
        Vector3 offset = target - this.transform.position;

        if(offset.magnitude > 0.1f){
            offset = offset.normalized * straightShotSpeed;
            characterController.Move(offset * Time.deltaTime);
        }
    }

    void arrivedAtTarget(){
        //will be used when the player arrives at the target, so anything control is take from the user and gravity is turned off, all of this gets put back when it needs to be
        mainTarget = null;
        pulling = false;
        if (!Input.GetButton("Fire1"))
        {
            gravityOn = true;
            //pulling = false;
            verticalVelocity = 0f;
            GameManager.Instance.CanMove = true;
            returnPull = true;//sets the animation to return the pull
        }
    }

    void OnControllerColliderHit(ControllerColliderHit colliderHit){
        if(colliderHit.gameObject.Equals(mainTarget) && mainTarget != null)
        {
            arrivedAtTarget();
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