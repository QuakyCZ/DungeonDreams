using System.Collections.Generic;
using UnityEngine;


public sealed class Room{
    private List<ClonedTile> Tiles;
    public List<ClonedTile> Doors;
    public bool isOutside;

    public Room(bool isOutside = false) {
        Tiles = new List<ClonedTile>();
        Doors = new List<ClonedTile>();
        this.isOutside = isOutside;
    }

    private Room(Room other) {
        Tiles = new List<ClonedTile>();
        foreach (ClonedTile t in other.Tiles) {
            Tiles.Add(t);
        }

        foreach (ClonedTile d in other.Doors) {
            Doors.Add(d);
        }

        isOutside = other.isOutside;
    }

    public Room Clone() {
        return new Room(this);
    }

    public void AssignTile(ClonedTile t) {
        Tiles.Add(t);
        t.room = this;
    }

    public void AssignTiles(List<ClonedTile> tiles) {
        foreach (ClonedTile tile in tiles) {
            AssignTile(tile);
        }
    }

    public void UnassignTile(ClonedTile t) {
        Tiles.Remove(t);
    }

    public void UnassignTiles() {
        Tiles.Clear();
    }

    public List<ClonedTile> GetTiles() {
        return Tiles;
    }

    public void ChangeTile(ClonedTile originalTile, ClonedTile newTile) {
        if (HasTile(originalTile))
            Tiles[Tiles.IndexOf(originalTile)] = newTile;
        else {
            Debug.LogError("ChangeTile: You're trying to change tile which is not in array. Aborted.");
            return;
        }
    }

    public void ChangeTile(int index, ClonedTile newTile) {
        if (HasTileAt(index))
            Tiles[index] = newTile;
    }

    public bool HasTile(ClonedTile tile) {
        if (Tiles.Contains(tile))
            return true;
        return false;
    }

    public ClonedTile GetTileAt(int index) {
        if (index < Tiles.Count)
            return Tiles[index];

        Debug.LogError("GetTileAt: You're trying to get tile outside of the array!");
        return null;
    }

    public int CountTiles() {
        return Tiles.Count;
    }

    public bool HasTileAt(int index) {
        if (index < CountTiles())
            return true;
        return false;
    }

    public void Destroy() {
        UnassignTiles();
        Doors.Clear();
    }
}