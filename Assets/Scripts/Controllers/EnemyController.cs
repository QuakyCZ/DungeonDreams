using System.Collections;
using System.Collections.Generic;
using Interaction.Objects;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]public List<GameObject> enemyGOs;
    public EnemyPrefabs enemyPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        enemyGOs = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
}
