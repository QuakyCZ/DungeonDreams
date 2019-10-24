using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ability {
    armor,
    attackSpeed,
    attackRange,
    strength,
    speed
}

public class Abilities
{
    #region parameters, variables, references
    private Dictionary<Ability, float> abilityValues;
    #endregion

    #region Constructors

    public Abilities(float armor = 0,  float attackSpeed = 0.3f, float attackRange = 3, float strength = 1, float speed = 2f ) {
        abilityValues = new Dictionary<Ability, float>();
        abilityValues.Add( Ability.armor, armor );
        abilityValues.Add( Ability.attackSpeed, attackSpeed );
        abilityValues.Add( Ability.attackRange, attackRange );
        abilityValues.Add( Ability.strength, strength );
        abilityValues.Add( Ability.speed, speed );
    }
    #endregion

    #region Methods

    /// <summary>
    /// Returns ability int value.
    /// </summary>
    /// <param name="ability"></param>
    /// <returns></returns>
    public float GetAbilityValue(Ability ability) {
        return abilityValues[ability];
    }

    /// <summary>
    /// Sets the value to the ability.
    /// </summary>
    /// <param name="ability"></param>
    /// <param name="value"></param>
    public void SetAbilityValue(Ability ability, int value) {
        abilityValues[ability] = value;
    }

    /// <summary>
    /// Ability value is equal to the sum of old and given value.
    /// </summary>
    /// <param name="ability"></param>
    /// <param name="value"></param>
    public void ChangeAbilityValue(Ability ability, int value) {
        abilityValues[ability] += value;
    }

    #endregion
}
