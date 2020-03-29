using System.Collections;
using System.Collections.Generic;
using Controllers;
using Models.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject menu = null;
    [SerializeField] protected GameObject GUI = null;
    [SerializeField] protected GameObject hintGO = null;
    public MainController mainController = null;

    
    private bool doUpdate = true;

    private Player _player = null;

    #region GameObjects
    public GameObject healthBar = null;
    public GameObject manaBar = null;
    #endregion

    #region Texts
    public Text log = null;
    public Text armorText = null;
    public Text attackRangeText = null;
    public Text attackSpeedText = null;
    public Text strengthText = null;
    public Text goldText = null;
    public Text levelText = null;
    public Text speedText = null;
    public Text keyText = null;
    #endregion

    #region Dictionaries
    public Dictionary <Stats, GameObject> lifeStatsGO = null;
    public Dictionary <Ability, Text> abilityGO = null;
    private Dictionary<Stats, Text> textStatsGO = null;
    private Dictionary<InventoryDefault, Text> inventoryTextsGO = null;
    #endregion

    void Awake() {
        Instantiate();
    }
    // Start is called before the first frame update
    void Start()
    {
        hintGO.SetActive( true );
        // Set texts for abilities
        foreach (Ability ability in abilityGO.Keys) {
            //Debug.Log( "Changing UI for " + ability );
            //if(player.abilities == null ) {
            //    Debug.LogError( "player.abilities is null" );
            //}
            float value = _player.abilities.GetAbilityValue( ability );
            RefreshVisibleValue( ability);
        }

        foreach (Stats stats in textStatsGO.Keys) {
            RefreshVisibleValue( stats );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (doUpdate) {
            if (Input.GetKeyDown( KeyCode.Escape )) {
                ShowMenu( mainController.doUpdate );
            }
        }
    }
    private void Instantiate() {

        _player = FindObjectOfType<Player>();

        lifeStatsGO = new Dictionary<Stats, GameObject>();
        inventoryTextsGO = new Dictionary<InventoryDefault, Text>();
        textStatsGO = new Dictionary<Stats, Text>();
        abilityGO = new Dictionary<Ability, Text>();

        #region statsGO
        lifeStatsGO.Add( Stats.health, healthBar );
        lifeStatsGO.Add( Stats.mana, manaBar );
        #endregion
               
        
        inventoryTextsGO.Add( InventoryDefault.gold, goldText );
        inventoryTextsGO.Add(InventoryDefault.key,keyText);
        textStatsGO.Add( Stats.level, levelText );
       

        #region abilityGO        
        abilityGO.Add( Ability.armor, armorText );
        abilityGO.Add( Ability.attackRange, attackRangeText );
        abilityGO.Add( Ability.attackSpeed, attackSpeedText );
        abilityGO.Add( Ability.strength, strengthText );
        abilityGO.Add( Ability.speed, speedText );
        #endregion
    }
    #region inGameMenu
    public void ShowMenu(bool enable = true) { // Also Resume Game button uses this method.
        mainController.PauseGameTime( enable );
        menu.SetActive( enable );
    }

    public void ExitToMainMenu() {
        SceneManager.LoadScene( "MainMenu" );
    }
    #endregion


    public void Log(string message ) {
        log.text = message;
    }

    /// <summary>
    /// Changes the visible value of stats.
    /// </summary>
    /// <param name="stats">Stats.</param>
    public void RefreshVisibleValue(Stats stats) {
        if (lifeStatsGO.ContainsKey( stats )) {
            int actualValue = _player.stats.GetValue( stats );
            Stats opposite = _player.stats.maxStats[stats];
            int maxValue = _player.stats.GetValue( opposite );


            if (_player.stats.GetValue( Stats.health ) <= 0) {
                SceneManager.LoadScene("GameOverLose");
                Debug.Log( "You have died" );
            }

            float valueFraction = ( float )actualValue / maxValue;
            GameObject bar = lifeStatsGO[stats];
            bar.GetComponent<Image>().fillAmount = valueFraction;
            bar.GetComponentsInChildren<Text>()[0].text = actualValue.ToString() + "/" + maxValue.ToString();
        }
        else if ( textStatsGO.ContainsKey( stats ) ) {
            textStatsGO[stats].text = _player.stats.GetValue( stats ).ToString();
        }
        else {
            Debug.LogError( "statsDictionaries do not contain key " + stats );
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

    public void ToggleGUI(bool toggle) {
        GUI.SetActive(toggle);
    }

    public void ToggleHint() {
        hintGO.SetActive(!hintGO.activeSelf);
    }
}
