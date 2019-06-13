//used half of the code I had, and some from here: https://forum.unity.com/threads/solved-how-to-clamp-camera-rotation-on-the-x-axis-fps-controller.526871/
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //Serialized fields
    [SerializeField]
    private Player player;
    [SerializeField][Range(10f, 50f)]
    private float lookSensitivity;

    //Standard fields

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

    float rotX = 0f;
    float rotY = 0f;
    float RotSpeed = 10f;

    private void setCameraRot(){
        rotX += Input.GetAxis("Mouse X")*lookSensitivity;
        rotY += Input.GetAxis("Mouse Y") * lookSensitivity;
        rotY = Mathf.Clamp(rotY, -60f, 90f);
        transform.eulerAngles = new Vector3(-rotY, rotX, 0.0f);
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
