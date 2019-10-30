using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
    sword,
    bow,
    none
}

public class Weapon : MonoBehaviour {
    public int damage;
    public WeaponType type { get; protected set; }

    public List<Collider2D> collisions { get; protected set; }
    //public float range;
    // Start is called before the first frame update
    void Start()
    {
        collisions = new List<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D collision) {
        collisions.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        collisions.Remove( collision );
    }
}
