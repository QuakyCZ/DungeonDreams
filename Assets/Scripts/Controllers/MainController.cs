using Models;
using Models.Characters;
using UnityEngine;


public class MainController : MonoBehaviour{
    #region parametry a proměnné

    public Camera mainCamera;
    public Player player;
    public GameObject playerGO;
    public InputController InputController;
    public UIController UIController;
    public PlayerStatsController playerStatsController;
    public RoomController roomController;
    public bool doUpdate = false;
    public GameObject spawn;
    public WorldGraph worldGraph;
    public GameObject lineGO;

    #endregion

    public static MainController Instance { get; protected set; }

    // Start is called before the first frame update
    void Awake() {
        if (Instance == null) {
            Instance = this;
        }

        ConfigFile.SetUp();
        Language.SetUp();
        player = FindObjectOfType<Player>();
        mainCamera = Camera.main;
        player.SetPosition(spawn.transform.position);
        roomController.CreateWorldGraph();
        worldGraph = roomController.worldGraph;
    }

    // Update is called once per frame
    void Update() {
        if (doUpdate) { }
    }

    #region metody

    public void PauseGameTime(bool pause = true) {
        InputController.doUpdate = !pause;
        doUpdate = !pause;
    }

    #endregion
}