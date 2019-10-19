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
        for(int y = 0; y<height; y++ ) { 
            for(int x = 0; x<width; x++ ) {

                Vector3Int position = new Vector3Int( x, y, 0 );

                if(walls.HasTile(position) ) {
                    ClonedTile t = new ClonedTile( x, y, TileType.Wall, true );
                    tiles[x, y] = t;
                    originalTiles.Add( t, walls.GetTile( position ) );
                    //Debug.Log( "Cloning wall" );
                }

                else if ( door.HasTile( position ) ) {
                    ClonedTile t = new ClonedTile( x, y, TileType.Door, true );
                    tiles[x, y] = t;
                    originalTiles.Add( t, door.GetTile( position ) );
                }

                else if ( floor.HasTile( position ) ) {
                    ClonedTile t = new ClonedTile( x, y, TileType.Floor, false );
                    tiles[x, y] = t;
                    originalTiles.Add( t, floor.GetTile( position ) );
                }

                else {
                    ClonedTile t = new ClonedTile( x, y, TileType.Empty, true ); ;
                    tiles[x, y] = t;
                    originalTiles.Add( t, null );
                }

            }
        }
        int wallnumber = 0;
        int floornumber = 0;
        int doornumber = 0;
        int emptynumber = 0;
        foreach (ClonedTile t in tiles ) {
           
            t.roomController = roomController;
            roomController.GetOutsideRoom().Tiles.Add( t );
            t.hasRoom = false;



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

