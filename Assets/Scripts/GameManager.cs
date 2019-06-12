using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject GameManager = new GameObject("GameManager");
                instance = GameManager.AddComponent<GameManager>();
                DontDestroyOnLoad(GameManager);
            }
            return instance;
        }
    }

    public bool CanMove {get; set;}
    // Start is called before the first frame update
    void Start()
    {
        CanMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
