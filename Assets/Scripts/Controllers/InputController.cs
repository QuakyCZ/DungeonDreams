using System.Collections;
using System.Collections.Generic;
using Controllers;
using Models.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers {
    public class InputController : MonoBehaviour
    {
        private Player _player;
        public bool doUpdate = true;

        public GameObject menu;
        [SerializeField] private Canvas console = null;

        private UIController _uiController;

        // Start is called before the first frame update
        void Start()
        {
            //Debug.Log( "InputController start" );
            _player = FindObjectOfType<Player>();
            _uiController = FindObjectOfType<UIController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1)) {
                console.gameObject.SetActive(!console.isActiveAndEnabled);
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                _player.Heal();
            }
        }

        void FixedUpdate() {
            if (doUpdate) {
                //Debug.Log( "Move" );
                Move();
                _uiController.Log( "" );
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
            Vector3 movementVector = new Vector3( horizontal, vertical, 0 ).normalized * Time.deltaTime * _player.abilities.GetAbilityValue(Ability.speed);

            _player.MovePlayer( movementVector );
        }


    }
}

