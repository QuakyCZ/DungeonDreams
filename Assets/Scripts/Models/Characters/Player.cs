using UnityEngine;

public class Player : Character{
    public Inventory inventory;
    [SerializeField] protected Camera minimapCamera;
    private UIController _uiController;

    #region metody

    protected override void Start() {
        base.Start();
        _uiController = FindObjectOfType<UIController>();
    }

    protected override void Awake() {
        Debug.Log("Player Awake");
        base.Awake();
        ResetCoolDown();
        inventory = Inventory.GetInstance(true);
    }

    protected override void Update() {
        if (stats.GetValue(Stats.health) <= 0) {
            Die();
        }

        base.Update();
    }

    protected override void Move(Vector3 moveVector) {
        base.Move(moveVector);
        if (moveVector.x < 0) {
            transform.localScale = new Vector3(-.5f, .5f, 0);
        }
        else if (moveVector.x > 0) {
            transform.localScale = new Vector3(.5f, .5f, 0);
        }

        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        minimapCamera.transform.position = Camera.main.transform.position;
    }

    public void MovePlayer(Vector2 moveVector) {
        Move(moveVector);
    }

    public void SetPosition(Vector2 position) {
        transform.position = position;
    }

    public override void ReceiveDamage(Damage dmg) {
        if (!ConfigFile.Get().GetOption("immune")) {
            base.ReceiveDamage(dmg);
        }
    }

    public void Heal() {
        if (inventory.GetValue(InventoryConsumable.healthPotion) > 0) {
            if (stats.GetValue(Stats.health) < stats.GetValue(Stats.maxHealth)) {
                int value = ConfigFile.Get().healPotion;
                if (stats.GetValue(Stats.health) + value > stats.GetValue(Stats.maxHealth)) {
                    value = stats.GetValue(Stats.maxHealth) - stats.GetValue(Stats.health);
                }

                inventory.ChangeValue(InventoryConsumable.healthPotion, 1, MathOperation.Remove);
                _uiController.RefreshVisibleValue(InventoryConsumable.healthPotion);
                stats.ChangeActualStats(Stats.health, value);
                _uiController.RefreshVisibleValue(Stats.health);
            }
            else {
                _uiController.Log(Language.GetString(GameDictionaryType.log, "maxHealth"));
            }
        }
        else {
            _uiController.Log(
                Language.GetString(GameDictionaryType.log, "notEnough"),
                new string[1] {gameObject.name}
            );
        }
    }

    #endregion
}