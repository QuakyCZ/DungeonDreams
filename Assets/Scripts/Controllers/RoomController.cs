using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        rooms = new List<Room>();
        outsideRoom = new Room( true ); 
        rooms.Add( outsideRoom );
        Debug.Log( "Rooms: " + rooms.Count );
        worldGraph = WorldGraph.SetInstance((int)walls.localBounds.size.x, (int)walls.localBounds.size.y, walls, floor, doors, debugMap, this);
        floorQueue = new Queue<ClonedTile>();
        doorQueue = new Queue<ClonedTile>();
        DoRoomFloodFill();

    }

    public void DoRoomFloodFill() {
        Debug.Log( "InstantiateRooms" );

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

        foreach(ClonedTile t in map.GetTiles() ) {
            if ( t.type == TileType.Floor ) {
                floorQueue.Enqueue( t );
                break;
            }
        }
        ClonedTile startTile = floorQueue.Dequeue();
        floorQueue.Enqueue( startTile );
        Room newRoom = new Room();
        rooms.Add( newRoom );

        ClonedTile checkedTile = startTile;
        do {
            CheckTile( floorQueue.Dequeue(), newRoom );

            while ( floorQueue.Count == 0 && doorQueue.Count > 0 ) {

                ClonedTile door = doorQueue.Dequeue();
                Debug.Log( "Dequeuing door. Door count: " + doorQueue.Count );

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
                    newRoom = new Room();
                    rooms.Add( newRoom );
                    if ( newRoom.Doors.Contains( door ) == false )
                        newRoom.Doors.Add( door );
                    break;
                }

            }

        } while ( floorQueue.Count > 0 ) ;
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = roomSprite;
        tile.color = Color.green;
        foreach ( Room r in rooms ) {
            Debug.Log( "Room: " + rooms.IndexOf(r) + " Tiles: " + r.CountTiles());
            foreach(ClonedTile t in r.GetTiles() ) {

                worldGraph.debugMap.SetTile( new Vector3Int( t.realX, t.realY, 0 ), tile );
            }
        }
        tile.color = Color.red;
        worldGraph.debugMap.SetTile( new Vector3Int( startTile.realX, startTile.realY, 0 ), null );
        worldGraph.debugMap.SetTile( new Vector3Int( startTile.realX, startTile.realY,0 ), tile );
    }

    void CheckTile(ClonedTile startTile, Room newRoom) {
        if ( startTile.type == TileType.Door ) {
            Debug.Log( "Enqueuing door. Door count: " + doorQueue.Count );
            doorQueue.Enqueue( startTile );
            if(newRoom.Doors.Contains(startTile)==false)
                newRoom.Doors.Add( startTile );
            return;
        }

        if(startTile.type == TileType.Wall || startTile.room != null) {
            return;
        }

        // This tile has no room and it's a floor
        newRoom.AssignTile( startTile );

        foreach(ClonedTile nb in startTile.GetNeighbours() ) {
            if(nb.type == TileType.Door ) {
                doorQueue.Enqueue( nb );
                Debug.Log( "Enqueuing door. Door count: " + doorQueue.Count );
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
