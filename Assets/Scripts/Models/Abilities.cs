using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities
{
    public int strength = 1;
    public int attackSpeed = 1;
    public int demage = 1;
    public int armor = 0;

    public Abilities(int strength = 1, int attackSpeed = 1, int demage = 1, int armor = 0) {
        this.strength = strength;
        this.attackSpeed = attackSpeed;
        this.demage = demage;
        this.armor = armor;
    }
}
