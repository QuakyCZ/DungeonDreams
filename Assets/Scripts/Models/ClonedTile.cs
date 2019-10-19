using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TileType {
    Door,Floor,Wall, Empty
}
public class ClonedTile
{
    public int x { get; protected set; }
    public int y { get; protected set; }
    public TileType type { get; protected set; }
    public bool hasRoom;
    public bool roomEnclosure;
    public ClonedTile[,] neighbours;
    public RoomController roomController;
    private int _numberOfRooms;
    public int numberOfRooms { 
        get { 
            return _numberOfRooms; 
            }
        set {
            _numberOfRooms = value;
            if ( ( type == TileType.Wall || type == TileType.Door ) && _numberOfRooms == 2 ) {
                isDone = true;
            }
            else if ( type == TileType.Floor && _numberOfRooms == 1 ) {
                isDone = true;
            }
            else
                isDone = false;
        }
    }
    public bool isDone;

    public ClonedTile(int x, int y, TileType type, bool roomEnclosure, bool hasRoom = false) {
        roomController = MainController.Instance.roomController;
        neighbours = new ClonedTile[3, 3];
        this.x = x;
        this.y = y;
        this.type = type;
        this.roomEnclosure = roomEnclosure;
        this.hasRoom = hasRoom;
        isDone = false;
        numberOfRooms = 0;
        neighbours[1, 1] = this;
    }


}
