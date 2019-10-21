using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Room {
    private List<ClonedTile> Tiles;
    public List<ClonedTile> Doors;

    public Room() {
        Tiles = new List<ClonedTile>();
        Doors = new List<ClonedTile>();
    }
    private Room(Room other) {
        Tiles = new List<ClonedTile>();
        foreach(ClonedTile t in other.Tiles ) {
            Tiles.Add( t );
        }
    }
    virtual public Room Clone() {
        return new Room(this);
    }

    public void AssignTile(ClonedTile t ) {
        if ( !Tiles.Contains( t ) ) {
            Tiles.Add( t );
        }
        if ( t.hasRoom == false )
            t.hasRoom = true;
        t.numberOfRooms++;
        t.room = this;
    }
    public void AssignTiles( List<ClonedTile> tiles ) {
        foreach ( ClonedTile tile in tiles ) {
            AssignTile( tile );
        }
    }
    public void UnassignTiles(Room defaultRoom) {
        foreach(ClonedTile t in Tiles ) {
            defaultRoom.AssignTile( t );
            t.numberOfRooms--;
            t.numberOfRooms--;
        }
        Tiles.Clear();
    }
    public void UnassignTile( ClonedTile t ) {
        Tiles.Remove( t );
        t.numberOfRooms--;
    }
    //public List<ClonedTile> GetTiles() {
    //    return Tiles;
    //}
    public void ChangeTile(ClonedTile originalTile, ClonedTile newTile ) {
        if(HasTile(originalTile))
            Tiles[Tiles.IndexOf( originalTile )] = newTile;
        else {
            Debug.LogError( "ChangeTile: You're trying to change tile which is not in array. Aborted." );
            return;
        }
    }
    public void ChangeTile(int index, ClonedTile newTile ) {
        if(HasTileAt(index))
            Tiles[index] = newTile;
    }
    public bool HasTile(ClonedTile tile ) {
        if ( Tiles.Contains( tile ) )
            return true;
        return false;
    }
    public ClonedTile GetTileAt(int index ) {
        if ( index < Tiles.Count )
            return Tiles[index];

        Debug.LogError( "GetTileAt: You're trying to get tile outside of the array!" );
        return null;
    }
    public int CountTiles() {
        return Tiles.Count;
    }
    public bool HasTileAt(int index ) {
        if ( index < CountTiles() )
            return true;
        return false;
    }

}
