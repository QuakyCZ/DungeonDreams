using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public Camera mainCamera;
    public Player player;
    public GameObject playerGO;
    public InputController InputController;
    public bool doUpdate = false;

    // Start is called before the first frame update
    void Awake()
    {
        player = new Player( playerGO );
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (doUpdate) {

        }
    }

    public void PauseGame(bool pause = true) {
        InputController.doUpdate = !pause;
        doUpdate = !pause;
    }
}
