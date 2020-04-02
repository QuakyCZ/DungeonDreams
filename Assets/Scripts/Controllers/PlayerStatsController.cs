using Models.Characters;
using UnityEngine;

namespace Controllers {
    public class PlayerStatsController : MonoBehaviour {
        #region parameters, variables, references

        public Player player;

        private UIController _uiController;

        #region Other

        public MainController mainController;

        #endregion

        #endregion

        // Start is called before the first frame update
        void Awake() {
            player = FindObjectOfType<Player>();
            _uiController = FindObjectOfType<UIController>();
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
            player.stats.ChangeActualStats(Stats.health, -amount);
            _uiController.RefreshVisibleValue(Stats.health);
        }

        public void UseMana(int amount) {
            player.stats.ChangeActualStats(Stats.mana, -amount);
            _uiController.RefreshVisibleValue(Stats.mana);
        }


        public void ChangeValue(Stats stats, int value) {
            player.stats.ChangeActualStats(stats, value);
            _uiController.RefreshVisibleValue(stats);
        }

        public void ChangeValue(Ability ability, int value) {
            player.abilities.ChangeAbilityValue(ability, value);
            _uiController.RefreshVisibleValue(ability);
        }

        public int GetStatsValue(Stats stats) {
            return player.stats.GetValue(stats);
        }

        #endregion
    }
}