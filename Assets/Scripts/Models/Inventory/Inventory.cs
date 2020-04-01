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
    healthPotion
}
public enum MathOperation {
    Add, Remove, Multiple, Divide
}

public class Inventory {

    private static Inventory instance;

    private Dictionary<InventoryDefault, int>     _inventoryDefault;
    private Dictionary<InventoryConsumable, int>  _inventoryConsumable;

    private Inventory() {

        _inventoryDefault = new Dictionary<InventoryDefault, int>();
        _inventoryDefault.Add( InventoryDefault.gold, 0 );
        _inventoryDefault.Add( InventoryDefault.key, 0 );

        _inventoryConsumable = new Dictionary<InventoryConsumable, int>();
        _inventoryConsumable.Add( InventoryConsumable.healthPotion, 0 );
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
        return _inventoryConsumable[inv];
    }
    public int GetValue(InventoryDefault inv) {
        
        return _inventoryDefault[inv];
    }

    public void SetValue(InventoryConsumable inv, int value) {
        
        _inventoryConsumable[inv] = value;
    }

    public void SetValue(InventoryDefault inv, int value) {
        _inventoryDefault[inv] = value;
    }

    public void ChangeValue(InventoryConsumable inv, int value, MathOperation operation) {
        switch (operation) {
            case MathOperation.Add:
                _inventoryConsumable[inv] += value;
                break;
            case MathOperation.Remove:
                _inventoryConsumable[inv] -= value;
                break;
            case MathOperation.Multiple:
                _inventoryConsumable[inv] *= value;
                break;
            case MathOperation.Divide:
                _inventoryConsumable[inv] /= value;
                break;
        }
        
    }

    public void ChangeValue(InventoryDefault inv, int value, MathOperation operation) {
        switch (operation) {
            case MathOperation.Add:
            _inventoryDefault[inv] += value;
            break;
            case MathOperation.Remove:
            _inventoryDefault[inv] -= value;
            break;
            case MathOperation.Multiple:
            _inventoryDefault[inv] *= value;
            break;
            case MathOperation.Divide:
            _inventoryDefault[inv] /= value;
            break;
        }
    }

}
