using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stats {
    health,
    maxHealth,
    mana,
    maxMana,
    gold,
    level
}

public class PlayerStats {

    public Dictionary<Stats, int> values;
    public Dictionary<Stats, Stats> maxStats;

    public PlayerStats() {
        values = new Dictionary<Stats, int>();

        #region statsValues_values
        values.Add( Stats.health, 25 );
        values.Add( Stats.maxHealth, 25 );
        values.Add( Stats.mana, 20 );
        values.Add( Stats.maxMana, 20 );
        values.Add( Stats.gold, 0 );
        values.Add( Stats.level, 1 );
        #endregion

        maxStats = new Dictionary<Stats, Stats>();

        #region statsOposites_values
        maxStats.Add( Stats.health, Stats.maxHealth );
        maxStats.Add( Stats.mana, Stats.maxMana );
        #endregion
    }

    /// <summary>
    /// value++
    /// </summary>
    /// <param name="statsType">Stats type.</param>
    /// <param name="value">Value.</param>
    public void ChangeActualStats( Stats statsType, int value ) {
        Debug.Log( "ChangeStats for: " + statsType + " with value: " + value );
        if ( !maxStats.ContainsKey( statsType ) ) {
            values[statsType] += value;
        }
        else if ( values[statsType] + value <= GetValue( maxStats[statsType] ) && values[statsType] >= 0 ) {
            values[statsType] += value;
        }
    }

    /// <summary>
    /// Returns value of the stats.
    /// </summary>
    /// <returns>The stats value.</returns>
    /// <param name="stats">Stats.</param>
    public int GetValue( Stats stats ) {
        return values[stats];
    }
}
