using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    #region parametry a proměnné
    public Camera mainCamera;
    public Player player;
    public GameObject playerGO;
    public InputController InputController;
    public UIController UIController;
    public PlayerStatsController playerStats;
    public bool doUpdate = false;
    #endregion

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

    #region metody
    public void PauseGameTime(bool pause = true) {
        InputController.doUpdate = !pause;
        doUpdate = !pause;
    }
    #endregion
}
