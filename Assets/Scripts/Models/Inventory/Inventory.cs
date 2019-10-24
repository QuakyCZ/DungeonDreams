using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum InventoryType {
    Default, Consumable
}
public enum InventoryDefault {
    gold
}
public enum InventoryConsumable {
    healthPotion, manaPotion
}
public enum MathOperation {
    Add, Remove, Multiple, Divide
}

public class Inventory {

    private static Inventory instance;

    private UIController uiController;

    protected Dictionary<InventoryDefault, int>     inventoryDefault;
    protected Dictionary<InventoryConsumable, int>  inventoryConsumable;

    private Inventory(UIController uiController) {
        this.uiController = uiController;
        inventoryDefault    =   new Dictionary<InventoryDefault, int>();
        inventoryDefault.Add( InventoryDefault.gold, 0 );

        inventoryConsumable =   new Dictionary<InventoryConsumable, int>();
        inventoryConsumable.Add( InventoryConsumable.healthPotion, 0 );
        inventoryConsumable.Add( InventoryConsumable.manaPotion, 0 );
    }

    public static Inventory GetInstance(UIController uiController = null) {
        if(instance == null){
            instance = new Inventory(uiController);
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
        uiController.ChangeVisibleValue( inv, value );
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
        uiController.ChangeVisibleValue( inv, inventoryDefault[inv] );
    }

}
