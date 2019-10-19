using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public List<Collider2D> collisions;
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
