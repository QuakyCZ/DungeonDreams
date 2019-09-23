using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ability {
    armor,
    attackSpeed,
    damage,
    strength
    
}

public class Abilities
{
    #region parameters, variables, references
    private Dictionary<Ability, int> abilityValues;

    public int armor {
        get { return abilityValues[Ability.armor]; }
        protected set { abilityValues[Ability.armor] = value; }
    }
    public int attackSpeed {
        get { return abilityValues[Ability.attackSpeed]; }
        protected set { abilityValues[Ability.attackSpeed] = value; }
    }
    public int damage {
        get { return abilityValues[Ability.damage]; }
        protected set { abilityValues[Ability.damage] = value; }
    }

    public int strength {
        get { return abilityValues[Ability.strength]; }
        protected set { abilityValues[Ability.strength] = value; }
    }
    #endregion

    #region Constructors

    public Abilities(int strength = 1, int attackSpeed = 1, int damage = 1, int armor = 0) {
        abilityValues = new Dictionary<Ability, int>();
        abilityValues.Add( Ability.armor, armor );
        abilityValues.Add( Ability.attackSpeed, attackSpeed );
        abilityValues.Add( Ability.damage, damage );
        abilityValues.Add( Ability.strength, strength );


        this.strength = abilityValues[Ability.strength];
        this.attackSpeed = abilityValues[Ability.attackSpeed];
        this.damage = abilityValues[Ability.damage];
        this.armor = abilityValues[Ability.armor];        
    }
    #endregion

    #region Methods

    /// <summary>
    /// Returns ability int value.
    /// </summary>
    /// <param name="ability"></param>
    /// <returns></returns>
    public int GetAbilityValue(Ability ability) {
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
