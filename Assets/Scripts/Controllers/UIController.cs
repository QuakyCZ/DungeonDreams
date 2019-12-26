using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject menu;
    [SerializeField] protected GameObject GUI;
    public MainController mainController;

    public Text log;
    private bool doUpdate = true;

    private Player player;

    #region GameObjects
    public GameObject healthBar;
    public GameObject manaBar;
    #endregion

    #region Texts
    public Text armorText;
    public Text attackRangeText;
    public Text attackSpeedText;
    public Text strengthText;
    public Text goldText;
    public Text levelText;
    public Text speedText;
    #endregion

    #region Dictionaries
    public Dictionary <Stats, GameObject> lifeStatsGO;
    public Dictionary <Ability, Text> abilityGO;
    private Dictionary<Stats, Text> textStatsGO;
    private Dictionary<InventoryDefault, Text> inventoryTextsGO;
    #endregion

    void Awake() {
        Instantiate();
    }
    // Start is called before the first frame update
    void Start()
    {      

        // Set texts for abilities
        foreach (Ability ability in abilityGO.Keys) {
            //Debug.Log( "Changing UI for " + ability );
            //if(player.abilities == null ) {
            //    Debug.LogError( "player.abilities is null" );
            //}
            float value = player.abilities.GetAbilityValue( ability );
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

        player = FindObjectOfType<Player>();

        lifeStatsGO = new Dictionary<Stats, GameObject>();
        inventoryTextsGO = new Dictionary<InventoryDefault, Text>();
        textStatsGO = new Dictionary<Stats, Text>();
        abilityGO = new Dictionary<Ability, Text>();

        #region statsGO
        lifeStatsGO.Add( Stats.health, healthBar );
        lifeStatsGO.Add( Stats.mana, manaBar );
        #endregion
               
        
        inventoryTextsGO.Add( InventoryDefault.gold, goldText );

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
            int actualValue = player.stats.GetValue( stats );
            Stats oposite = player.stats.maxStats[stats];
            int maxValue = player.stats.GetValue( oposite );


            if (player.stats.GetValue( Stats.health ) <= 0) {
                ExitToMainMenu();
                Debug.Log( "You have died" );
            }

            float valueFraction = ( float )actualValue / maxValue;
            GameObject bar = lifeStatsGO[stats];
            bar.GetComponent<Image>().fillAmount = valueFraction;
            bar.GetComponentsInChildren<Text>()[0].text = actualValue.ToString() + "/" + maxValue.ToString();
        }
        else if ( textStatsGO.ContainsKey( stats ) ) {
            textStatsGO[stats].text = player.stats.GetValue( stats ).ToString();
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
        abilityGO[ability].text = ability.ToString() + ": " + player.abilities.GetAbilityValue(ability).ToString();
    }

    public void RefreshVisibleValue(InventoryDefault inv) {
        inventoryTextsGO[inv].text = player.inventory.GetValue(inv).ToString();
    }

    public void ToggleGUI(bool toggle) {
        GUI.SetActive(toggle);
    }
}
