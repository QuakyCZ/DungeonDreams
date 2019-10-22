using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    #region parametry a proměnné
    protected Camera mainCamera;
    [HideInInspector] public Player player;
    public GameObject playerGO;

    #region Controllers
    protected UIController uiController;
    protected PlayerStatsController playerStatsController;
    protected RoomController roomController;
    protected InputController inputController;
    #endregion

    protected bool doUpdate;
    [SerializeField] protected GameObject spawn;
    #endregion

    public static MainController Instance { get; protected set;}

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null ) {
            Instance = this;
        }
    }

    protected virtual void InstantiateVariables() {
        player = new Player( playerGO );
        player.weapon = FindObjectOfType<Weapon>();
        mainCamera = Camera.main;
        player.SetPosition( spawn.transform.position );
    }
    // Update is called once per frame
    void Update()
    {
        if (doUpdate) {
        }
    }

    #region metody
    public void PauseGameTime(bool pause = true) {
        inputController.doUpdate = !pause;
        doUpdate = !pause;
    }
    #endregion
}
