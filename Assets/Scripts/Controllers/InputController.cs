using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    public MainController mainController;

    Player player;
    public bool doUpdate = false;
    public float speed;

    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        player = mainController.player;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (doUpdate) {
            Move();

            if (Input.GetKeyDown( KeyCode.Escape )) {
                mainController.UIController.ShowMenu();
            }

        }
        else {
            if (Input.GetKeyDown( KeyCode.Escape )) {
                mainController.UIController.ShowMenu( false );
            }
        }
    }

    void Move() {
        float horizontal = Input.GetAxisRaw( "Horizontal" );
        float vertical = Input.GetAxisRaw( "Vertical" );
        if (horizontal != 0 && vertical != 0) {
            horizontal /= 1.5f;
            vertical /= 1.5f;
        }

        Vector3 movementVector = new Vector3( horizontal, vertical,0 ).normalized * Time.deltaTime * speed;

        player.Move( movementVector );

    }
}
