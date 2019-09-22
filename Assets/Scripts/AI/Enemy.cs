using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup healthGroup;

    private Transform target;

    public float speed = 2f;

    public Transform Target {
        get { return target; }
        set { target = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowTarget();
    }

    //public Transform Select() {
    //    return healthGroup.alpha = 1;
    //}

    private void FollowTarget() {
        if (target != null) {
            transform.position = Vector2.MoveTowards( transform.position, target.position, speed*Time.deltaTime );
        }
    }

}
