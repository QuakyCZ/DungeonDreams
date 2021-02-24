using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stats {
    health,
    maxHealth,
    mana,
    maxMana,
    level
}

public class CharacterStats
{
    public Dictionary<Stats, int> values;
    public Dictionary<Stats, Stats> maxStats;

    public CharacterStats(int health = 25, int maxHealth = 25, int mana = 20, int maxMana = 20, int level = 1) {
        values = new Dictionary<Stats, int>();
        maxStats = new Dictionary<Stats, Stats>();
        InitializeValues(health, maxHealth, mana, maxMana, level);        
    }

    private void InitializeValues(int health, int maxHealth, int mana, int maxMana, int level) {
        values.Add( Stats.health, health );
        values.Add( Stats.maxHealth, maxHealth );
        values.Add( Stats.mana, mana );
        values.Add( Stats.maxMana, maxMana );
        values.Add( Stats.level, level );

        maxStats.Add( Stats.health, Stats.maxHealth );
        maxStats.Add( Stats.mana, Stats.maxMana );

    }

    /// <summary>
    /// Returns value of the stats.
    /// </summary>
    /// <returns>The stats value.</returns>
    /// <param name="stats">Stats.</param>
    public int GetValue(Stats stats) {
        return values[stats];
    }

    /// <summary>
    /// value++
    /// </summary>
    /// <param name="statsType">Stats type.</param>
    /// <param name="value">Value.</param>
    public void ChangeActualStats(Stats statsType, int value) {
        Debug.Log( "ChangeStats for: " + statsType + " with value: " + value );
        if (!maxStats.ContainsKey( statsType )) {
            // if there is no limitation...
            values[statsType] += value;
        }
        else if (values[statsType] + value <= GetValue( maxStats[statsType] ) && values[statsType] >= 0) {
            values[statsType] += value;
        }
    }
}
