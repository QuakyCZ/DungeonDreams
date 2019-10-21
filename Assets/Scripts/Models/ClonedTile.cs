﻿using System.Collections;
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
    public RoomController roomController;
    private int _numberOfRooms;
    public int numberOfRooms { 
        get { 
            return _numberOfRooms; 
            }
        set {
            _numberOfRooms = value;
            if ( roomEnclosure && _numberOfRooms >= 2 ) {
                isDone = true;
            }
            else if ( !roomEnclosure && _numberOfRooms >= 1 ) {
                isDone = true;
            }
            else
                isDone = false;
        }
    }
    public bool isDone;

    public ClonedTile(int x, int y, TileType type, bool roomEnclosure, bool hasRoom = false) {
        roomController = MainController.Instance.roomController;
        this.x = x;
        this.y = y;
        this.type = type;
        this.roomEnclosure = roomEnclosure;
        this.hasRoom = hasRoom;
        isDone = false;
        numberOfRooms = 0;
    }

    private ClonedTile(ClonedTile other ) {
        roomController = other.roomController;
        x = other.x;
        y = other.y;
        type = other.type;
        roomEnclosure = other.roomEnclosure;
        hasRoom = other.hasRoom;
        isDone = other.isDone;
        numberOfRooms = other.numberOfRooms;
    }

    public List<ClonedTile> GetNeighbours() {
        ClonedTile tile = this;
        List<ClonedTile> nb = new List<ClonedTile>();
        ClonedTile[,] tiles = tile.roomController.worldGraph.tiles;
        Debug.Log( "Get neighbours for tile: X: " + x + " Y: " + y );
            
            if ( tile.x + 1 < roomController.worldGraph.width && tiles[tile.x + 1, tile.y] != null) {
                nb.Add( tiles[x + 1, y] );
            }
            if ( tile.x - 1 >= 0 && tiles[tile.x - 1, tile.y] != null
            ) {
                nb.Add( tiles[x - 1, y] );
            }
            if ( tile.y + 1 < roomController.worldGraph.width && tiles[tile.x, tile.y + 1] != null ) {
                nb.Add( tiles[x, y+1] );
            }
            if ( tile.y - 1 >=0 && tiles[tile.x, tile.y - 1] != null ) {
                nb.Add( tiles[x, y-1] );
            }
            

        return nb;
    }

    public ClonedTile Clone() {
        return new ClonedTile( this );
    }
}
