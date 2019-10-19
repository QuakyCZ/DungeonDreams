using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = .05f;
    Path path;
    int currWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating( "UpdatePath", 0f, .5f );
        
    }
    void UpdatePath() {
        if(seeker.IsDone())
            seeker.StartPath( rb.position, target.position, OnPathComplete );
    }
    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(path == null) {
            return;
        }
        if(currWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        }
        else {
            reachedEndOfPath = false;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currWaypoint]-rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce( force );

        float distance = Vector2.Distance(rb.position, path.vectorPath[currWaypoint]);

        if (distance < nextWaypointDistance) {
            currWaypoint++;
        }
    }
}
