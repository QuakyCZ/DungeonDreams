using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Key : Collectable
{
    [SerializeField] protected TextMeshProUGUI numberText;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }


    protected override void OnCollect() {
        player.inventory.ChangeValue( InventoryDefault.key, 1, MathOperation.Add );
        int amnt = player.inventory.GetValue( InventoryDefault.key );
        if (amnt == 7) {
            numberText.text = "1";
        }
        else
            numberText.text = amnt.ToString() + "/7";
        gameObject.SetActive( false );
    }
}
