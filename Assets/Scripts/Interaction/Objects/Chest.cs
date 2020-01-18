using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    protected Animator animator;
    protected bool isOppened = false;
    protected bool isLooted = false;
    protected override void Start() {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected void Loot() {
        int amnt = Random.Range( 1, 5 ) * player.stats.GetValue(Stats.level);
        isLooted = true;
        player.inventory.ChangeValue( InventoryDefault.gold, amnt, MathOperation.Add );
        uiController.RefreshVisibleValue( InventoryDefault.gold );
        Debug.Log( "Gained " + amnt + " gold. Now you have " + player.inventory.GetValue( InventoryDefault.gold ) + " gold" );
    }
    protected override void OnCollect() {
        if ( isOppened == true) {
            animator.SetBool( "isLooted", true );
            animator.SetBool( "isOppened", true );
            if (isLooted == false) {
                Loot();
                animator.SetInteger( "isOppening", 0 );
            }
            else {
                animator.SetInteger( "isOppening", -1 );
            }        
            
            
        }
        else if(isOppened == false) {
            animator.SetInteger( "isOppening", 1 );
            animator.SetBool( "isOppened", false );
            if (isLooted) {
                animator.SetBool( "isLooted", true );
            }
            else {
                animator.SetBool( "isLooted", false );                
            }
        }     
        
    }

    public void IsOppened(int oppened = 0) {
        if(oppened == 1) {
            animator.SetInteger( "isOppening", 0 );
            animator.SetBool( "isOppened", true );
            isOppened = true;
            
        }
        else if(oppened == 0) {
            animator.SetInteger( "isOppening", 0 );
            animator.SetBool( "isOppened", false );
            isOppened = false;
        }
        else {
            animator.SetInteger( "isOppening", -1 );
            animator.SetBool( "isOppened", true );
            animator.SetBool( "isLooted", true );            
        }
    }


}
