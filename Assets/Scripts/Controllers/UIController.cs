using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject menu;
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
    public Text damageText;
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


    // Start is called before the first frame update
    void Start()
    {
        Instantiate();

        // Set texts for abilities
        foreach (Ability ability in abilityGO.Keys) {
            ChangeVisibleValue( ability, player.abilities.GetAbilityValue( ability ) );
        }

        foreach (Stats stats in textStatsGO.Keys) {
            ChangeVisibleValue( stats, player.stats.GetValue( stats ) );
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
        SceneManager.LoadScene( "MainMenu", LoadSceneMode.Single );
    }
    #endregion


    public void Log(string message ) {
        log.text = message;
    }

    /// <summary>
    /// Changes the visible value of stats.
    /// </summary>
    /// <param name="stats">Stats.</param>
    /// <param name="value">Value.</param>
    public void ChangeVisibleValue(Stats stats, int value) {

        if (lifeStatsGO.ContainsKey( stats )) {
            int actualValue = player.stats.GetValue( stats );
            Stats oposite = player.stats.maxStats[stats];
            int maxValue = player.stats.GetValue( oposite );


            if (player.stats.GetValue( Stats.health ) <= 0) {
                mainController.UIController.ShowMenu();
                Debug.Log( "You have died" );
            }

            float valueFraction = ( float )actualValue / maxValue;
            GameObject bar = lifeStatsGO[stats];
            bar.GetComponent<Image>().fillAmount = valueFraction;
            bar.GetComponentsInChildren<Text>()[0].text = actualValue.ToString() + "/" + maxValue.ToString();
        }

        else if (textStatsGO.ContainsKey( stats )) {
            textStatsGO[stats].text = value.ToString();
        }
    }

    /// <summary>
    /// Changes the visible value of <paramref name="ability"/>.
    /// </summary>
    /// <param name="ability">Ability.</param>
    /// <param name="value">Value.</param>
    public void ChangeVisibleValue(Ability ability, float value) {
        abilityGO[ability].text = ability.ToString() + ": " + value.ToString();
    }

    public void ChangeVisibleValue(InventoryDefault inv, int value) {
        inventoryTextsGO[inv].text = value.ToString();
    }
}
