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
    public GameObject healthBar;
    public GameObject manaBar;
    public Dictionary <Stats, GameObject> statsGO;
    public MainController mainController;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = mainController.player;
        statsGO = new Dictionary<Stats, GameObject>();
        statsGO.Add( Stats.health, healthBar );
        statsGO.Add( Stats.mana, manaBar );

        foreach(Stats stat in player.stats.statsOposites.Keys) {
            int actualValue = player.stats.GetStatsValue(stat);
            Stats oposite = player.stats.statsOposites[stat];
            int maxValue = player.stats.GetStatsValue(oposite);
            statsGO[stat].GetComponentInChildren<Text>().text = actualValue.ToString() + "/" + maxValue.ToString();
        }
        
    }

    // Update is called once per frame
    void Update() {
        
        
    }

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
}
