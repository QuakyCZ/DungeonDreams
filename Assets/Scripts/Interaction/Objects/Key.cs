using Controllers;
using Models.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction.Objects {
    public class Key : Collectable
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }


        protected override void OnCollect() {
            player.inventory.ChangeValue( InventoryDefault.key, 1, MathOperation.Add );
            uiController.RefreshVisibleValue(InventoryDefault.key);
            gameObject.SetActive( false );
        }
    }
}
