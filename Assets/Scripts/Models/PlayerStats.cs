using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats {

    public Dictionary<Stats, int> statsValues;
    public Dictionary<Stats, Stats> statsOposites;

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

        statsOposites = new Dictionary<Stats, Stats>();

        #region statsOposites_values
        statsOposites.Add( Stats.health, Stats.maxHealth );
        statsOposites.Add( Stats.mana, Stats.maxMana );
        #endregion

        health = statsValues[Stats.health];
        maxHealth = statsValues[Stats.maxHealth];
        mana = statsValues[Stats.mana];
        maxMana = statsValues[Stats.maxMana];
    }

    public void ChangeStats(Stats statsType, int value) {
        statsValues[statsType] += value;
        if (statsValues[statsType] <= 0) {
            statsValues[statsType] = 0;
        }
        
    }

    public int GetStatsValue(Stats stats) {
        return statsValues[stats];
    }


}
