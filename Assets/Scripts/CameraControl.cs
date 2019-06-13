using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //Serialized fields
    [SerializeField]
    private Player player;
    [SerializeField][Range(10f, 50f)]
    private float lookSensitivity;

    //Standard fields
    //todo: 6/12/19 somehow limit pitch and yaw so the player cannot rotate endlessly
    private float pitch = 0.0f;
    private float yaw = 0.0f;

    void Start(){
        setCameraPos();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;//locks cursor to center of screen, we can add a crosshair later if we want
        lookSensitivity = 10f;
        verifyUnparented();
    }

    void Update(){
        setCameraRot();
        setCameraPos();
        //Debug.Log(transform.eulerAngles);
    }

    private void setCameraRot(){
        yaw += lookSensitivity * Input.GetAxis("Mouse X");
        pitch -= lookSensitivity * Input.GetAxis("Mouse Y");
        //Debug.Log("Pitch: " + pitch + "Yaw: " + yaw);

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    private void setCameraPos(){
        if(!this.transform.position.Equals(player.transform.position)){//just a slight code optimization. No need to set position if the position hasn't changed
            this.transform.position = player.transform.position;
        }
    }

    private void verifyUnparented(){
        //6/13/19 Currently the main camera does not need to be childed to anything, specifically the player. The position is being set in this script. This method will unparent the camera
        //if it has a parent
        if(this.transform.parent){
            Debug.Log("Removing");
            this.transform.parent = null;
        }
    }
}
