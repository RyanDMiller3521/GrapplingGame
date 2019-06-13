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
    }

    void Update(){
        setCameraRot();
        setCameraPos();
        //Debug.Log(transform.eulerAngles);
    }

    private void setCameraRot(){
        yaw += lookSensitivity * Input.GetAxis("Mouse X");
        pitch -= lookSensitivity * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    private void setCameraPos(){
        if(!this.transform.position.Equals(player.transform.position)){//just a slight code optimization. No need to set position if the position hasn't changed
            this.transform.position = player.transform.position;
        }
    }
}
