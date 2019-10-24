using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsController : MonoBehaviour
{
    #region parameters, variables, references
    public Player player;
    float seconds = 1f;

    private UIController uiController;

    #region Other
    public MainController mainController;
    #endregion

    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<Player>();
        uiController = FindObjectOfType<UIController>();
              
                      
        
        


    }

    // Update is called once per frame
    void Update() {
        //seconds -= Time.deltaTime;
        //if (seconds <= 0) {
        //    ChangeStats( Stats.mana, 1 );
        //    seconds = 1;
        //}
    }

    #region public methods
    public void TakeDamage(int amount) {
        uiController.ChangeVisibleValue( Stats.health, -amount );
    }

    public void UseMana(int amount) {
        uiController.ChangeVisibleValue( Stats.mana, -amount );
    }

    

    public void ChangeValue(Stats stats, int value ) {
        player.stats.ChangeActualStats( stats, value );
        uiController.ChangeVisibleValue( stats, value );
    }

    public void ChangeValue(Ability ability, int value ) {
        player.abilities.ChangeAbilityValue( ability, value );
        uiController.ChangeVisibleValue( ability, value );
    }

    public int GetStatsValue(Stats stats ) {
        return player.stats.GetValue( stats );
    }
    #endregion
}
