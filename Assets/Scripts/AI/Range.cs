using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    public Enemy parent;

    private void Start() {

    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            parent.Target = collision.transform;
        }
    }

    
}
