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

    #region GameObjects
    public GameObject healthBar;
    public GameObject manaBar;
    #endregion

    #region Texts
    public Text armorText;
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
        abilityGO.Add( Ability.attackSpeed, attackSpeedText );
        abilityGO.Add( Ability.damage, damageText );
        abilityGO.Add( Ability.strength, strengthText );
        #endregion

        #region inGameTexts
        // Set text for healthbar and manabar
        foreach (Stats stat in player.stats.statsOposites.Keys) {
            int actualValue = player.stats.GetStatsValue(stat);
            Stats oposite = player.stats.statsOposites[stat];
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
        
        
    }

    #region public methods
    public void ChangeStats(Stats stats, int value) {
        player.stats.ChangeStats( stats, value );
        int actualValue = player.stats.GetStatsValue(stats);
        Stats oposite = player.stats.statsOposites[stats];
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

    public void ChangeAbilityTextValue(Ability ability, int value) {
        abilityGO[ability].text = ability.ToString() + ": " + value.ToString();
    }
    #endregion
}
