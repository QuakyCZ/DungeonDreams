using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGraph {
    public ClonedTile[,] tiles;
    Tilemap walls;
    Tilemap floor;
    Tilemap door;
    Dictionary<ClonedTile, TileBase> originalTiles;
    public int width;
    public int height;
    RoomController roomController;

    public WorldGraph(int width, int height, Tilemap walls, Tilemap floor, Tilemap door, RoomController roomController ) {
        tiles = new ClonedTile[width, height];
        for(int y = 0; y<height; y++ ) {
            for(int x = 0; x<width; x++ ) {
                tiles[x, y] = new ClonedTile( x, y, TileType.Empty, false );
            }
        }
        this.walls = walls;
        this.floor = floor;
        this.door = door;
        this.width = width;
        this.height = height;
        this.roomController = roomController;

        Debug.Log( width + " " + height );
        originalTiles = new Dictionary<ClonedTile, TileBase>();

        ImportTiles();
    }

    public void ImportTiles() {
        ClonedTile clonedTile;
        int w_y = 0; // X coordinate in worldGraph
        for(int y = -height/2; y<height/2; y++ ) {
            int w_x = 0; // Y coordinate in worldGraph

            for (int x = -width/2; x<width/2; x++ ) {
                Vector3Int pos = new Vector3Int( x, y, 0 );

                TileBase tile = floor.GetTile(pos);

                if( tile != null ) {
                    clonedTile = new ClonedTile( w_x, w_y, TileType.Floor, false );
                    tiles[w_x, w_y] = clonedTile;
                    originalTiles.Add( clonedTile, tile );
                }

                tile = walls.GetTile( pos );

                if (tile != null ) {
                    clonedTile = new ClonedTile( w_x, w_y, TileType.Wall, false );
                    tiles[w_x, w_y] = clonedTile;
                    originalTiles.Add( clonedTile, tile );
                }

                tile = door.GetTile( pos );
                if(tile!= null ) {
                    clonedTile = new ClonedTile( w_x, w_y, TileType.Door, false );
                    tiles[w_x, w_y] = clonedTile;
                    originalTiles.Add( clonedTile, tile );
                }
                w_x++;
            }
            w_y++;
        }

        int wallnumber = 0;
        int floornumber = 0;
        int doornumber = 0;
        int emptynumber = 0;
        foreach (ClonedTile t in tiles ) {           
            t.roomController = roomController;


            switch ( t.type ) {
                case TileType.Wall:
                    wallnumber++;
                    break;
                case TileType.Door:
                    doornumber++;
                    break;
                case TileType.Floor:
                    floornumber++;
                    break;
                default:
                    emptynumber++;
                    break;
            }
        }
        Debug.Log( "Walls: " + wallnumber + " Floor: " + floornumber + " Doors: " + doornumber + " Empty: " + emptynumber );
    }


}

