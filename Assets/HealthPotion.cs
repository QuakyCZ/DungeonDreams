using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Collectable
{
    [SerializeField] protected int healthAmount;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update() {
        base.Update();
    }

    protected override void OnCollect() {
        if (player.stats.GetValue( Stats.health ) < player.stats.GetValue(Stats.maxHealth)) {
            base.OnCollect();
            player.stats.ChangeActualStats( Stats.health, healthAmount );
            uiController.RefreshVisibleValue( Stats.health );
            Destroy( gameObject );
        }
        
    }
}
