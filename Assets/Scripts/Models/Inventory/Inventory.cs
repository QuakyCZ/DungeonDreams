using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum InventoryType {
    Default, Consumable
}
public enum InventoryDefault {
    gold,
    key
}
public enum InventoryConsumable {
    healthPotion, manaPotion
}
public enum MathOperation {
    Add, Remove, Multiple, Divide
}

public class Inventory {

    private static Inventory instance;

    protected Dictionary<InventoryDefault, int>     inventoryDefault;
    protected Dictionary<InventoryConsumable, int>  inventoryConsumable;

    private Inventory() {

        inventoryDefault = new Dictionary<InventoryDefault, int>();
        inventoryDefault.Add( InventoryDefault.gold, 0 );
        inventoryDefault.Add( InventoryDefault.key, 0 );

        inventoryConsumable = new Dictionary<InventoryConsumable, int>();
        inventoryConsumable.Add( InventoryConsumable.healthPotion, 0 );
        inventoryConsumable.Add( InventoryConsumable.manaPotion, 0 );
    }

    public static Inventory GetInstance(bool startGame = false) {
        if(instance == null){
            instance = new Inventory();
        }
        else if ( startGame ) {
            instance = new Inventory();
        }
        return instance;
    }

    public int GetValue(InventoryConsumable inv) {
        return inventoryConsumable[inv];
    }
    public int GetValue(InventoryDefault inv) {
        
        return inventoryDefault[inv];
    }

    public void SetValue(InventoryConsumable inv, int value) {
        
        inventoryConsumable[inv] = value;
    }

    public void SetValue(InventoryDefault inv, int value) {
        inventoryDefault[inv] = value;
    }

    public void ChangeValue(InventoryConsumable inv, int value, MathOperation operation) {
        switch (operation) {
            case MathOperation.Add:
                inventoryConsumable[inv] += value;
                break;
            case MathOperation.Remove:
                inventoryConsumable[inv] -= value;
                break;
            case MathOperation.Multiple:
                inventoryConsumable[inv] *= value;
                break;
            case MathOperation.Divide:
                inventoryConsumable[inv] /= value;
                break;
        }
        
    }

    public void ChangeValue(InventoryDefault inv, int value, MathOperation operation) {
        switch (operation) {
            case MathOperation.Add:
            inventoryDefault[inv] += value;
            break;
            case MathOperation.Remove:
            inventoryDefault[inv] -= value;
            break;
            case MathOperation.Multiple:
            inventoryDefault[inv] *= value;
            break;
            case MathOperation.Divide:
            inventoryDefault[inv] /= value;
            break;
        }
    }

}
