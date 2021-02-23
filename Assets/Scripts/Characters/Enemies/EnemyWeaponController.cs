using Models;
using Models.Characters;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour{
    [SerializeField] private int damage = 0;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Player")) {
            other.SendMessage("ReceiveDamage", new Damage{damageAmount = damage});
        }
    }
}
