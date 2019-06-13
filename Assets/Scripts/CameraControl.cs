using UnityEditor.Timeline;
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
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += lookSensitivity * mouseX;
        if(yaw > 360) yaw = 0; else if(yaw < 0) yaw = 360;//this keeps the value between 0 and 360. This will make limiting rotation a bit easier
        pitch -= lookSensitivity * mouseY;
        if(pitch > 360) pitch = 0; else if(pitch < 0) pitch = 360;//same thing as the yaw restrictions
        //Debug.Log("Pitch: " + pitch + "Yaw: " + yaw);
        pitch = fixPitchVal(pitch, mouseX);
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    private float fixPitchVal(float pitch, float mouseX){
        //this will fix the pitch value so that we can limit our rotation.
        if((pitch >= 0 && pitch <= 60) || (pitch >= 230 && pitch <= 360)){
            return pitch;
        }else if(pitch > 60 && pitch < 230){
            if(Input.GetAxis("Mouse X") < 0){
                pitch = 60;
            }else if(mouseX > 0){
                pitch = 230;
            }
        }
        return pitch;
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
