using UnityEngine;


public class Chest : Collectable{
    private Animator _animator;
    private bool _isOpened = false;
    private bool _isLooted = false;
    private bool _interacting = false;

    protected override void Start() {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    private void Loot() {
        int amnt = Random.Range(1, 5) * player.stats.GetValue(Stats.level);
        _isLooted = true;
        player.inventory.ChangeValue(InventoryDefault.gold, amnt, MathOperation.Add);
        uiController.RefreshVisibleValue(InventoryDefault.gold);
        Debug.Log(
            "Gained " + amnt + " gold. Now you have "
            + player.inventory.GetValue(InventoryDefault.gold) + " gold");
    }

    protected override void OnCollect() {
        if (interacting) {
            return;
        }

        interacting = true;
        if (_isOpened) {
            // The chest is opened but not looted.
            _animator.SetBool("isLooted", true);
            _animator.SetBool("isOpened", true);
            if (_isLooted == false) {
                Loot();
                _animator.SetInteger("isOpening", 0);
                doUpdate = false;
            }
        }
        else {
            // The chest is closed.
            PlaySound();
            _animator.SetInteger("isOpening", 1);
        }
    }

    public void IsOpened(int opened = 0) {
        _interacting = false;
        if (opened == 1) {
            _animator.SetInteger("isOpening", 0);
            _animator.SetBool("isOpened", true);
            _isOpened = true;
            interacting = false;
        }
        else if (opened == 0) {
            _animator.SetInteger("isOpening", 0);
            _animator.SetBool("isOpened", false);
            _isOpened = false;
            interacting = false;
        }
        else {
            _animator.SetInteger("isOpening", -1);
            _animator.SetBool("isOpened", true);
            _animator.SetBool("isLooted", true);
            interacting = false;
        }
    }
}