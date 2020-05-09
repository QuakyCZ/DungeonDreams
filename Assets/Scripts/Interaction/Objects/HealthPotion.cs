using UnityEngine;


public class HealthPotion : Collectable{
    [SerializeField] protected int healthAmount;

    protected override void OnCollect() {
        base.OnCollect();
        player.inventory.ChangeValue(InventoryConsumable.healthPotion, 1, MathOperation.Add);
        uiController.RefreshVisibleValue(InventoryConsumable.healthPotion);
        Destroy(gameObject);
    }
}