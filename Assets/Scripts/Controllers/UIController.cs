using System.Collections.Generic;
using Models.Characters;
using Models.Files;
using Models.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers {
    public class UIController : MonoBehaviour {
        public GameObject menu = null;
        [SerializeField] protected GameObject GUI = null;
        [SerializeField] protected GameObject hintGO = null;
        public MainController mainController = null;


        private bool doUpdate = true;

        private Player _player = null;

        [Header("GUI")]

        #region GameObjects

        [Header("Bars")]
        public GameObject healthBar = null;
        #endregion


        #region Texts

        [Header("Abilities and inventory")] [SerializeField]
        private Text log = null;
        [SerializeField] private Text goldText = null;
        [SerializeField] private Text speedText = null;
        [SerializeField] private Text keyText = null;
        [SerializeField] private Text healthPotionText = null;
        #endregion

        [Header("Hint")] [SerializeField] private Text titleHint = null;
        [SerializeField] private Text attackHint = null;
        [SerializeField] private Text moveUpHint = null;
        [SerializeField] private Text moveDownHint = null;
        [SerializeField] private Text moveLeftHint = null;
        [SerializeField] private Text moveRightHint = null;
        [SerializeField] private Text usePotionHint = null;
        [SerializeField] private Text interactHint = null;
        [SerializeField] private Button closeHint = null;
        

        #region Dictionaries

        public Dictionary<Stats, GameObject> lifeStatsGO = null;
        public Dictionary<Ability, Text> abilityGO = null;
        private Dictionary<InventoryDefault, Text> inventoryTextsGO = null;
        private Dictionary<InventoryConsumable, Text> inventoryConsumableTextsGO = null;

        #endregion

        void Awake() {
            Instantiate();
        }

        // Start is called before the first frame update
        void Start() {
            SetUpLanguage();
            hintGO.SetActive(true);
            // Set texts for abilities
            RefreshVisibleValue(InventoryDefault.gold);
            RefreshVisibleValue(InventoryDefault.key);
            RefreshVisibleValue(InventoryConsumable.healthPotion);
        }

        private void SetUpLanguage() {
            titleHint.text = Language.GetString(GameDictionaryType.titles, "controls");
            attackHint.text = Language.GetString(GameDictionaryType.controls, "attack");
            moveUpHint.text = Language.GetString(GameDictionaryType.controls, "moveUp");
            moveDownHint.text = Language.GetString(GameDictionaryType.controls, "moveDown");
            moveLeftHint.text = Language.GetString(GameDictionaryType.controls, "moveLeft");
            moveRightHint.text = Language.GetString(GameDictionaryType.controls, "moveRight");
            usePotionHint.text = Language.GetString(GameDictionaryType.controls, "potion");
            interactHint.text = Language.GetString(GameDictionaryType.controls, "interact");
            closeHint.gameObject.GetComponentInChildren<Text>().text =
                Language.GetString(GameDictionaryType.buttons, "close");
        }

        // Update is called once per frame
        void Update() {
            if (doUpdate) {
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    ShowMenu(mainController.doUpdate);
                }
            }
        }

        private void Instantiate() {
            _player = FindObjectOfType<Player>();

            lifeStatsGO = new Dictionary<Stats, GameObject>();
            inventoryTextsGO = new Dictionary<InventoryDefault, Text>();
            inventoryConsumableTextsGO = new Dictionary<InventoryConsumable, Text>();
            abilityGO = new Dictionary<Ability, Text>();

            #region statsGO

            lifeStatsGO.Add(Stats.health, healthBar);

            #endregion


            inventoryTextsGO.Add(InventoryDefault.gold, goldText);
            inventoryTextsGO.Add(InventoryDefault.key, keyText);
            inventoryConsumableTextsGO.Add(InventoryConsumable.healthPotion, healthPotionText);
        }

        #region inGameMenu

        public void ShowMenu(bool enable = true) {
            // Also Resume Game button uses this method.
            mainController.PauseGameTime(enable);
            ToggleGUI(!enable);
            menu.SetActive(enable);
        }

        public void ExitToMainMenu() {
            SceneManager.LoadScene("MainMenu");
        }

        #endregion


        public void Log(string message) {
            log.text = message;
        }

        public void Log(string message, string[] parameters) {
            for (int i = 0; i < parameters.Length; i++) {
                message = message.Replace("{" + i + "}", parameters[i]);
            }

            log.text = message;
        }
    

        /// <summary>
        /// Changes the visible value of stats.
        /// </summary>
        /// <param name="stats">Stats.</param>
        public void RefreshVisibleValue(Stats stats) {
            if (lifeStatsGO.ContainsKey(stats)) {
                int actualValue = _player.stats.GetValue(stats);
                Stats opposite = _player.stats.maxStats[stats];
                int maxValue = _player.stats.GetValue(opposite);


                if (_player.stats.GetValue(Stats.health) <= 0) {
                    SceneManager.LoadScene("GameOverLose");
                    Debug.Log("You have died");
                }

                float valueFraction = (float) actualValue / maxValue;
                GameObject bar = lifeStatsGO[stats];
                bar.GetComponent<Image>().fillAmount = valueFraction;
                bar.GetComponentsInChildren<Text>()[0].text = actualValue.ToString() + "/" + maxValue.ToString();
            }
            else {
                Debug.LogError("statsDictionaries do not contain key " + stats);
            }
        }

        /// <summary>
        /// Changes the visible value of <paramref name="ability"/>.
        /// </summary>
        /// <param name="ability">Ability.</param>
        public void RefreshVisibleValue(Ability ability) {
            abilityGO[ability].text = ability.ToString() + ": " + _player.abilities.GetAbilityValue(ability);
        }

        public void RefreshVisibleValue(InventoryDefault inv) {
            inventoryTextsGO[inv].text = _player.inventory.GetValue(inv).ToString();
        }
    
        public void RefreshVisibleValue(InventoryConsumable inv) {
            inventoryConsumableTextsGO[inv].text = _player.inventory.GetValue(inv).ToString();
        }

        public void ToggleGUI(bool toggle) {
            GUI.SetActive(toggle);
        }

        public void ToggleHint() {
            hintGO.SetActive(!hintGO.activeSelf);
        }
    }
}