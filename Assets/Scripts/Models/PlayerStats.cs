using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats {

    public Dictionary<Stats, int> statsValues;
    public Dictionary<Stats, Stats> maxStats;

    public int health {
        get { return statsValues[Stats.health]; }
        set { statsValues[Stats.health] = value; }
    }    

    public int maxHealth {
        get { return statsValues[Stats.maxHealth]; }
        set { statsValues[Stats.maxHealth] = value; }
    }

    public int mana {
        get { return statsValues[Stats.mana]; }
        set { statsValues[Stats.mana] = value; }
    }
    public int maxMana {
        get { return statsValues[Stats.maxMana]; }
        set { statsValues[Stats.maxMana] = value; }
    }


    public PlayerStats() {
        statsValues = new Dictionary<Stats, int>();

        #region statsValues_values
        statsValues.Add( Stats.health, 25 );
        statsValues.Add( Stats.maxHealth, 25 );
        statsValues.Add( Stats.mana, 20 );
        statsValues.Add( Stats.maxMana, 20 );
        #endregion

        maxStats = new Dictionary<Stats, Stats>();

        #region statsOposites_values
        maxStats.Add( Stats.health, Stats.maxHealth );
        maxStats.Add( Stats.mana, Stats.maxMana );
        #endregion

        health = statsValues[Stats.health];
        maxHealth = statsValues[Stats.maxHealth];
        mana = statsValues[Stats.mana];
        maxMana = statsValues[Stats.maxMana];
    }

    public void ChangeActualStats(Stats statsType, int value) {
        if(statsValues[statsType] + value <= GetStatsValue(maxStats[statsType]) && statsValues[statsType]>=0) {
            statsValues[statsType] += value;
        }   
        
    }

    public int GetStatsValue(Stats stats) {
        return statsValues[stats];
    }


}
