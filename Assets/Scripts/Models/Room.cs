using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Room {
    public List<ClonedTile> Tiles { get; protected set; }

    public Room() {
        Tiles = new List<ClonedTile>();
    }
    private Room(Room other) {
        this.Tiles = other.Tiles;
    }
    virtual public Room Clone() {
        return new Room(this);
    }

    public void AssignTile(ClonedTile t ) {
        if ( !Tiles.Contains( t ) ) {
            Tiles.Add( t );
        }
    }

    public void UnassignTile( ClonedTile t ) {
        Tiles.Remove( t );
    }
}
