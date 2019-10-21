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
    public Room room;
    public RoomController roomController;
    private int _numberOfRooms;
    public int numberOfRooms { 
        get { 
            return _numberOfRooms; 
            }
        set {

            _numberOfRooms = value;
            if ( _numberOfRooms < 0 )
                _numberOfRooms = 0;

            if ( type == TileType.Door && _numberOfRooms >= 2 ) {
                isDone = true;
            }
            else if ( !roomEnclosure && _numberOfRooms >= 1 ) {
                isDone = true;
            }
            else if ( type == TileType.Wall && _numberOfRooms == 1) {
                // if the wall is the border of the map, it has just 1 room.
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

    public List<ClonedTile> GetNeighbours() {
        ClonedTile tile = this;
        List<ClonedTile> nb = new List<ClonedTile>();
        ClonedTile[,] tiles = tile.roomController.worldGraph.tiles;
        int width= roomController.worldGraph.width;
        int height=roomController.worldGraph.height;
            
        if ( tile.x + 1 < width) {
            nb.Add( tiles[x + 1, y] );
        }
        if ( tile.x - 1 >= 0) {
            nb.Add( tiles[x - 1, y] );
        }
        if ( tile.y + 1 < height) {
            nb.Add( tiles[x, y+1] );
        }
        if ( tile.y - 1 >=0) {
            nb.Add( tiles[x, y-1] );
        }

        if( tile.x + 1 < width && tile.y + 1 < height ) {
            nb.Add( tiles[x + 1, y + 1] );
        }
        if ( tile.x + 1 < width && tile.y - 1 > 0 ) {
            nb.Add( tiles[x + 1, y - 1] );
        }
        if ( tile.x - 1 > 0 && tile.y + 1 < height ) {
            nb.Add( tiles[x - 1, y + 1] );
        }
        if ( tile.x - 1 > 0 && tile.y - 1 > 0 ) {
            nb.Add( tiles[x - 1, y - 1] );
        }

        Debug.Log( "Get neighbours for tile: X: " + x + " Y: " + y + " Count: " + nb.Count.ToString());
        return nb;
    }
}
