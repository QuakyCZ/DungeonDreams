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

    public GameObject weapon;

    float coolDown;

    bool charged = true;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = mainController.player;
        coolDown = (float)player.abilities.GetAbilityValue( Ability.attackSpeed );
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doUpdate) {
            if(charged == false) {
                coolDown -= Time.deltaTime;
                //Debug.Log( "CoolDown: " + coolDown.ToString() );
                if (coolDown <= 0) {
                    charged = true;
                    Debug.Log( "Charged" );
                }
            }            
            
            Attack();
        }
        
        
    }

    void FixedUpdate() {
        if (doUpdate) {
            Move();
        }
        
    }

    /// <summary>
    /// This method moves the player.
    /// </summary>
    void Move() {
        float horizontal = Input.GetAxisRaw( "Horizontal" );
        float vertical = Input.GetAxisRaw( "Vertical" );

        // Pokud sčítáš 2 vektory, jejich výslednice bude větší -> zmenši ji.
        if (horizontal != 0 && vertical != 0) {
            horizontal /= 1.5f;
            vertical /= 1.5f;
        }
        
        // Do not change this line!!!
        Vector3 movementVector = new Vector3( horizontal, vertical, 0 ).normalized * Time.deltaTime * speed;
        animator.SetFloat( "Horizontal", horizontal );
        animator.SetFloat( "Vertical", vertical );
        animator.SetFloat( "Magnitude", movementVector.magnitude );
        player.Move( movementVector );

        

    }

    void Attack() {
        if(Input.GetMouseButton( 0 ) && charged==true) {
            Debug.Log( "Attack" );
            player.Attack();
            charged = false;
            coolDown = player.abilities.GetAbilityValue( Ability.attackSpeed );
        }

    }
}
