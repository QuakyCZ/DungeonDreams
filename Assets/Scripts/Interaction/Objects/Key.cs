public class Key : Collectable{
    protected override void OnCollect() {
        player.inventory.ChangeValue(InventoryDefault.key, 1, MathOperation.Add);
        uiController.RefreshVisibleValue(InventoryDefault.key);
        gameObject.SetActive(false);
    }
}