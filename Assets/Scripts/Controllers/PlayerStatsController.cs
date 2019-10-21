using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsController : MonoBehaviour
{
    #region parameters, variables, references
    public Player player;
    float seconds = 1f;

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
    #endregion

    #region Dictionaries
    public Dictionary <Stats, GameObject> lifeStatsGO;
    public Dictionary <Ability, Text> abilityGO;
    private Dictionary<Stats, Text> textStatsGO;
    #endregion

    #region Other
    public MainController mainController;
    #endregion

    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        player = mainController.player;

        #region statsGO
        lifeStatsGO = new Dictionary<Stats, GameObject>();
        lifeStatsGO.Add( Stats.health, healthBar );
        lifeStatsGO.Add( Stats.mana, manaBar );
        #endregion

        #region inventoryGO
        textStatsGO = new Dictionary<Stats, Text>();
        textStatsGO.Add( Stats.gold, goldText );
        textStatsGO.Add( Stats.level, levelText );
        #endregion

        #region abilityGO
        abilityGO = new Dictionary<Ability, Text>();
        abilityGO.Add( Ability.armor, armorText );
        abilityGO.Add( Ability.attackRange, attackRangeText );
        abilityGO.Add( Ability.attackSpeed, attackSpeedText );
        abilityGO.Add( Ability.damage, damageText );
        abilityGO.Add( Ability.strength, strengthText );
        #endregion

        #region inGameTexts
        // Set text for healthbar and manabar
        foreach (Stats stat in player.stats.maxStats.Keys) {
            int actualValue = player.stats.GetValue(stat);
            Stats oposite = player.stats.maxStats[stat];
            int maxValue = player.stats.GetValue(oposite);
            lifeStatsGO[stat].GetComponentInChildren<Text>().text = actualValue.ToString() + "/" + maxValue.ToString();
        }
        

        // Set texts for abilities
        foreach (Ability ability in abilityGO.Keys) {
            ChangeVisibleValue( ability, player.abilities.GetAbilityValue( ability ) );
        }

        foreach (Stats stats in textStatsGO.Keys ) {
            ChangeVisibleValue( stats, GetStatsValue( stats ) );
        }
        #endregion


    }

    // Update is called once per frame
    void Update() {
        //seconds -= Time.deltaTime;
        //if (seconds <= 0) {
        //    ChangeStats( Stats.mana, 1 );
        //    seconds = 1;
        //}
    }

    #region public methods
    public void TakeDamage(int amount) {
        ChangeVisibleValue( Stats.health, -amount );
    }

    public void UseMana(int amount) {
        ChangeVisibleValue( Stats.mana, -amount );
    }

    /// <summary>
    /// Changes the visible value of stats.
    /// </summary>
    /// <param name="stats">Stats.</param>
    /// <param name="value">Value.</param>
    public void ChangeVisibleValue(Stats stats, int value) {

        if ( lifeStatsGO.ContainsKey( stats ) ) {           
            int actualValue = player.stats.GetValue( stats );
            Stats oposite = player.stats.maxStats[stats];
            int maxValue = player.stats.GetValue( oposite );


            if ( player.stats.GetValue( Stats.health ) <= 0 ) {
                mainController.UIController.ShowMenu();
                Debug.Log( "You have died" );
            }

            float valueFraction = ( float )actualValue / maxValue;
            GameObject bar = lifeStatsGO[stats];
            bar.GetComponent<Image>().fillAmount = valueFraction;
            bar.GetComponentsInChildren<Text>()[0].text = actualValue.ToString() + "/" + maxValue.ToString();
        }

        else if ( textStatsGO.ContainsKey( stats ) ) {
            textStatsGO[stats].text = value.ToString();
        }
    }

    /// <summary>
    /// Changes the visible value of <paramref name="ability"/>.
    /// </summary>
    /// <param name="ability">Ability.</param>
    /// <param name="value">Value.</param>
    private void ChangeVisibleValue( Ability ability, float value ) {
        abilityGO[ability].text = ability.ToString() + ": " + value.ToString();
    }

    public void ChangeValue(Stats stats, int value ) {
        player.stats.ChangeActualStats( stats, value );
        ChangeVisibleValue( stats, value );
    }

    public void ChangeValue(Ability ability, int value ) {
        player.abilities.ChangeAbilityValue( ability, value );
        ChangeVisibleValue( ability, value );
    }

    public int GetStatsValue(Stats stats ) {
        return player.stats.GetValue( stats );
    }
    #endregion
}
