using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattleController : MonoBehaviour
{
    protected enum Character{player,boss,none}
    [Header("Stats")]
    [SerializeField] protected int playerHealth = 50;
    [SerializeField] protected int playerMana = 50;
    [SerializeField] protected int enemyHealth = 250;
    [SerializeField] protected int enemyMana = 100;
    [Header("Graphics")]
    [SerializeField] protected GameObject player;
    [SerializeField] protected GameObject boss;
    [SerializeField] protected float hitColorSpeed;
    protected Character hitCharacter = Character.none;
    protected float startTime;

    protected Dictionary<Character,GameObject> characterSprites;
    // Start is called before the first frame update
    void Start()
    {
        characterSprites = new Dictionary<Character, GameObject>();
        characterSprites.Add(Character.player,player);
        characterSprites.Add(Character.boss,boss);
        startTime = Time.time;
    }

    void Update()
    {
        if (hitCharacter != Character.none)
        {
            float t = Mathf.Sin(startTime - Time.time) * hitColorSpeed;
            characterSprites[hitCharacter].GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, t);
            if (t == Mathf.PI * 2)
            {

            }
            //hitCharacter = Character.none;
        }
    }

    public void PlayerAttack(){
        int dmg = Random.Range(1,30);
        hitCharacter = Character.boss;
    }

    public void EndTurn(){
        EnemyAttack();
    }
    
    ///////////////////////
    ///
    ///     ENEMY ATTACK
    ///
    ///////////////////////

    protected void EnemyAttack(){

    }
}
