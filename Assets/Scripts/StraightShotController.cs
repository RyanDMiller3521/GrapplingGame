using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightShotController : MonoBehaviour{
    // Start is called before the first frame update
    private GameObject currentPlayer;
    private PlayerTestController playerController;

    void Start(){
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
        playerController = currentPlayer.GetComponent<PlayerTestController>();
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnCollisionEnter(Collision collision){
        //todo: check whether or not the collision should be grappleable
        playerController.setHookArrived(true);
    }

}
