using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Stats {
    health,
    maxHealth,
    mana,
    maxMana
}

public class PlayerStatsController : MonoBehaviour
{
    #region parameters, variables, references

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
    #endregion

    #region Dictionaries
    public Dictionary <Stats, GameObject> statsGO;
    public Dictionary <Ability, Text> abilityGO;
    #endregion

    #region Other
    public MainController mainController;
    Player player;
    #endregion

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        player = mainController.player;

        #region statsGO
        statsGO = new Dictionary<Stats, GameObject>();
        statsGO.Add( Stats.health, healthBar );
        statsGO.Add( Stats.mana, manaBar );
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
            int actualValue = player.stats.GetStatsValue(stat);
            Stats oposite = player.stats.maxStats[stat];
            int maxValue = player.stats.GetStatsValue(oposite);
            statsGO[stat].GetComponentInChildren<Text>().text = actualValue.ToString() + "/" + maxValue.ToString();
        }
        

        // Set texts for abilities
        foreach (Ability ability in abilityGO.Keys) {
            ChangeAbilityTextValue( ability, player.abilities.GetAbilityValue( ability ) );
        }
        #endregion
    }

    // Update is called once per frame
    void Update() {
        seconds -= Time.deltaTime;
        if (seconds <= 0) {
            ChangeStats( Stats.mana, 1 );
            seconds = 1;
        }
    }

    #region public methods
    public void TakeDamage(int amount) {
        ChangeStats( Stats.health, -amount );
    }

    public void UseMana(int amount) {
        ChangeStats( Stats.mana, -amount );
    }

    private void ChangeStats(Stats stats, int value) {
        player.stats.ChangeActualStats( stats, value );
        int actualValue = player.stats.GetStatsValue(stats);
        Stats oposite = player.stats.maxStats[stats];
        int maxValue = player.stats.GetStatsValue(oposite);
        
        
        if (player.stats.health <= 0) {
            mainController.UIController.ShowMenu();
            Debug.Log( "You have died" );
        }

        float valueFraction = (float)actualValue/maxValue;
        GameObject bar = statsGO[stats];
        bar.GetComponent<Image>().fillAmount = valueFraction;
        bar.GetComponentsInChildren<Text>()[0].text = actualValue.ToString() + "/" + maxValue.ToString();  
    }

    public void ChangeAbilityTextValue(Ability ability, float value) {
        abilityGO[ability].text = ability.ToString() + ": " + value.ToString();
    }
    #endregion
}
