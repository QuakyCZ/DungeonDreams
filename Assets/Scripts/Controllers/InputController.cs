using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    Player player;
    public bool doUpdate = true;

    public GameObject menu;

    float coolDown;

    bool charged = true;

    Animator animator;

    UIController uiController;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log( "InputController start" );
        player = FindObjectOfType<Player>();
        animator = GameObject.Find("Player").GetComponent<Animator>();
        uiController = FindObjectOfType<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doUpdate) {           
            Attack();
        }        
    }

    void FixedUpdate() {
        if (doUpdate) {
            //Debug.Log( "Move" );
            Move();
            uiController.Log( "" );
        }
        
    }

    /// <summary>
    /// This method moves the player.
    /// </summary>
    void Move() {
        //Debug.Log( "Move" );
        float horizontal = Input.GetAxisRaw( "Horizontal" );
        float vertical = Input.GetAxisRaw( "Vertical" );

        // Pokud sčítáš 2 vektory, jejich výslednice bude větší -> zmenši ji.
        if (horizontal != 0 && vertical != 0) {
            horizontal /= Mathf.Pow(2,0.5f);
            vertical /= Mathf.Pow(2, 0.5f);
        }

        // Do not change this line!!!
        Vector3 movementVector = new Vector3( horizontal, vertical, 0 ).normalized * Time.deltaTime * player.abilities.GetAbilityValue(Ability.speed);
        animator.SetFloat( "Horizontal", horizontal );
        animator.SetFloat( "Vertical", vertical );
        animator.SetFloat( "Magnitude", movementVector.magnitude );
        player.MovePlayer( movementVector );
    }

    void Attack() {
        if(Input.GetMouseButton( 0 )) {
            Debug.Log( "Attack" );
            player.Attack();
        }

    }
}
