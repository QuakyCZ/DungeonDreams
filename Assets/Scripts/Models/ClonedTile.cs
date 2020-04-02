using System.Collections;
using System.Collections.Generic;
using Controllers;
using Models;
using UnityEngine;

public enum TileType {
    Door,
    Floor,
    Wall,
    Empty
}

public class ClonedTile {
    public int x { get; protected set; }
    public int y { get; protected set; }
    public int realX;
    public int realY;

    
    public TileType type { get; protected set; }
    public Room room;
    public RoomController roomController;

    
    //pathfinding
    public int gCost;
    public int hCost;
    public int FCost => gCost + hCost;
    public bool isWalkable=true;

    public ClonedTile cameFrom;
    

    public ClonedTile(int x, int y, int realX, int realY, TileType type) {
        roomController = MainController.Instance.roomController;
        this.x = x;
        this.y = y;
        this.realX = realX;
        this.realY = realY;
        this.type = type;
    }

}