using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TileType {
    Door,Floor,Wall, Empty
}
public class ClonedTile
{
    public int x;
    public int y;
    public int realX;
    public int realY;
    public TileType type;
    public Room room;
    public RoomController roomController;

    public ClonedTile(int x, int y, int realX, int realY, TileType type, RoomController roomController) {
        this.roomController = roomController;
        this.x = x;
        this.y = y;
        this.realX = realX;
        this.realY = realY;
        this.type = type;
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

        //if( tile.x + 1 < width && tile.y + 1 < height ) {
        //    nb.Add( tiles[x + 1, y + 1] );
        //}
        //if ( tile.x + 1 < width && tile.y - 1 > 0 ) {
        //    nb.Add( tiles[x + 1, y - 1] );
        //}
        //if ( tile.x - 1 > 0 && tile.y + 1 < height ) {
        //    nb.Add( tiles[x - 1, y + 1] );
        //}
        //if ( tile.x - 1 > 0 && tile.y - 1 > 0 ) {
        //    nb.Add( tiles[x - 1, y - 1] );
        //}

        //Debug.Log( "Get neighbours for tile: X: " + x + " Y: " + y + " Count: " + nb.Count.ToString());
        return nb;
    }
}
