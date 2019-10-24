using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGraph {
    public ClonedTile[,] tiles { get; protected set; }

    Tilemap walls;
    Tilemap floor;
    Tilemap door;
    public Tilemap debugMap;

    Dictionary<ClonedTile, TileBase> originalTiles;

    public int width;
    public int height;

    RoomController roomController;

    private static WorldGraph Instance;

    /// <summary>
    /// Use this to overwrite the instance.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="walls"></param>
    /// <param name="floor"></param>
    /// <param name="door"></param>
    /// <param name="debugMap"></param>
    /// <param name="roomController"></param>
    /// <returns></returns>
    public static WorldGraph SetInstance(int width, int height, Tilemap walls, Tilemap floor, Tilemap door, Tilemap debugMap, RoomController roomController) {
        Instance = new WorldGraph( width, height, walls, floor, door, debugMap, roomController );
        return Instance;
    }

    /// <summary>
    /// Use this to get the instance.
    /// </summary>
    /// <returns></returns>
    public static WorldGraph GetInstance() {
        if(Instance == null) {
            Debug.LogError( "WorldGraph: The instance is not set. Use SetInstance() before you try to get it." );
            return null;
        }
        return Instance;
    }


    private WorldGraph(int width, int height, Tilemap walls, Tilemap floor, Tilemap door, Tilemap debugMap, RoomController roomController ) {
        #region sizes
        this.width = width;
        this.height = height;
        #endregion

        tiles = new ClonedTile[width, height];

        this.roomController = roomController;

        FillMap( width, height, TileType.Empty );

        #region Tilemaps
        this.walls = walls;
        this.floor = floor;
        this.door = door;
        this.debugMap = debugMap;
        #endregion
               
        Debug.Log( width + " " + height );
        originalTiles = new Dictionary<ClonedTile, TileBase>();

        ImportTiles();
    }

    /// <summary>
    /// Fills all tiles in map with given TileType.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="type"></param>
    public void FillMap(int width, int height, TileType type) {
        for ( int y = 0; y < height; y++ ) {
            for ( int x = 0; x < width; x++ ) {
                tiles[x, y] = new ClonedTile( x, y, 0, 0, type, roomController );
            }
        }
    }

    /// <summary>
    /// Imports tiles from all tilemaps.
    /// </summary>
    private void ImportTiles() {
        int w_y = 0; // X coordinate in worldGraph
        for(int y = (int)walls.localBounds.min.y; y < (int)walls.localBounds.max.y; y++ ) {
            int w_x = 0; // Y coordinate in worldGraph

            for (int x = (int)walls.localBounds.min.x; x<walls.localBounds.max.x; x++ ) {
                Vector3Int pos = new Vector3Int( x, y, 0 );

                TileBase tile = floor.GetTile(pos);

                if( tile != null ) {
                    tiles[w_x, w_y].realX = x;
                    tiles[w_x, w_y].realY = y;
                    tiles[w_x, w_y].type = TileType.Floor;
                    originalTiles.Add( tiles[w_x, w_y], tile );
                }

                tile = walls.GetTile( pos );

                if (tile != null ) {
                    tiles[w_x, w_y].realX = x;
                    tiles[w_x, w_y].realY = y;
                    tiles[w_x, w_y].type = TileType.Wall;
                    originalTiles.Add( tiles[w_x, w_y], tile );
                }

                tile = door.GetTile( pos );
                if(tile!= null ) {
                    tiles[w_x, w_y].realX = x;
                    tiles[w_x, w_y].realY = y;
                    tiles[w_x, w_y].type = TileType.Door;
                    originalTiles.Add( tiles[w_x, w_y], tile );
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
            if ( originalTiles.ContainsKey( t ) ) {
                TileBase tile = originalTiles[t];
                debugMap.SetTile( new Vector3Int( t.realX, t.realY, 0 ), tile );
            }

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

