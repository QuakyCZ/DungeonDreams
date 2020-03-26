using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.Tilemaps;
public class RoomController : MonoBehaviour
{
    [SerializeField]
    private Tilemap floor;
    [SerializeField]
    private Tilemap walls;
    [SerializeField]
    private Tilemap doors;
    [SerializeField]
    private Tilemap debugMap;

    public WorldGraph worldGraph;
    public List<Room> rooms;
    public Sprite roomSprite;

    Queue<ClonedTile> floorQueue;
    Queue<ClonedTile> doorQueue;

    public Room outsideRoom;

    public void CreateWorldGraph() {
        rooms = new List<Room>();
        outsideRoom = new Room( true ); 
        rooms.Add( outsideRoom );
        worldGraph = new WorldGraph((int)walls.localBounds.size.x, (int)walls.localBounds.size.y, walls, floor, doors, debugMap, this);
        floorQueue = new Queue<ClonedTile>();
        doorQueue = new Queue<ClonedTile>();
        DoRoomFloodFill();
        Debug.Log( $"Found {rooms.Count} rooms" );
    }
    
    public void DoRoomFloodFill() {
        Room map = new Room();
        // Move all empty tiles into the outsideRoom and other into the map.
        outsideRoom.UnassignTiles();
        map.UnassignTiles();

        foreach (ClonedTile t in worldGraph.tiles ) {
            if ( t.type == TileType.Empty )
                outsideRoom.AssignTile( t );
            else {
                map.AssignTile( t );
            }
            t.room = null;
        }

        //Put any floor tile into the queue
        foreach(ClonedTile t in map.GetTiles() ) {
            if ( t.type == TileType.Floor ) {
                floorQueue.Enqueue( t );
                break;
            }
        }
        
        // Get the first tile
        ClonedTile startTile = floorQueue.Dequeue();
        floorQueue.Enqueue( startTile );

        // Create the first room
        Room newRoom = CreateNewRoom();

        //ClonedTile checkedTile = startTile;
        while (floorQueue.Count > 0) {
            CheckTile( floorQueue.Dequeue(), newRoom );

            //if the floorQueue is empty and there are uncheched doors.
            while ( floorQueue.Count == 0 && doorQueue.Count > 0 ) {
                ClonedTile door = doorQueue.Dequeue();

                // Check if there is new unassigned room behind the door.
                bool isNewRoom = false;

                foreach (ClonedTile nb in door.GetNeighbours() ) {
                    if ( nb.type == TileType.Floor && nb.room == null) {
                        floorQueue.Enqueue( nb );
                        isNewRoom = true;
                        break;
                    }
                    isNewRoom = false;
                }
                if ( isNewRoom == false ) {
                    continue;
                }
                else {
                    newRoom = CreateNewRoom();
                    if ( newRoom.Doors.Contains( door ) == false )
                        newRoom.Doors.Add( door );
                    break;
                }
            }
        } 

        // Show the rooms in the debug map
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = roomSprite;
        tile.color = Color.green;
        foreach ( Room r in rooms ) {
            //Debug.Log( "Room: " + rooms.IndexOf(r) + " Tiles: " + r.CountTiles());
            foreach(ClonedTile t in r.GetTiles() ) {
                worldGraph.debugMap.SetTile( new Vector3Int( t.realX, t.realY, 0 ), tile );
            }
        }
        tile.color = Color.red;
        worldGraph.debugMap.SetTile( new Vector3Int( startTile.realX, startTile.realY, 0 ), null );
        worldGraph.debugMap.SetTile( new Vector3Int( startTile.realX, startTile.realY,0 ), tile );
    }

    void CheckTile(ClonedTile tile, Room newRoom) {
        // if the tile is door -> put it into the doorQueue and leave this void.
        if ( tile.type == TileType.Door ) {
            doorQueue.Enqueue( tile );
            // if the room doesn't contain the door tile assign it.
            if(newRoom.Doors.Contains(tile)==false)
                newRoom.Doors.Add( tile );
            return;
        }
        // if the tile is wall or the tile has a room -> return
        if(tile.type == TileType.Wall || tile.room != null) {
            return;
        }

        // This tile has no room and it's a floor -> assign it to the current newRoom
        newRoom.AssignTile( tile );

        // Enqueue tile's neighbours
        foreach(ClonedTile nb in tile.GetNeighbours() ) {
            if(nb.type == TileType.Door ) {
                doorQueue.Enqueue( nb );
                //Debug.Log( "Enqueuing door. Door count: " + doorQueue.Count );
            }
            else if(nb.type == TileType.Floor && nb.room == null ) {
                floorQueue.Enqueue( nb );
            }
        }


    }

    Room CreateNewRoom() {
        Room newRoom = new Room();
        AddRoom( newRoom );
        return newRoom;
    }


    public void AddRoom( Room room ) {
        if(!rooms.Contains(room))
            rooms.Add( room );
    }
                                                                                                                                                                                                                                   
    public void DeleteRoom( Room room, Room defaultRoom ) {
        room.Destroy();
        room = null;
        rooms.Remove( room );
    }

    public void InstantiateObj(Object o ) {
        Instantiate( o );
    }
}
