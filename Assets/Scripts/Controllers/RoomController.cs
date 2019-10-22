using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class RoomController : MonoBehaviour
{
    public Tilemap floor;
    public Tilemap walls;
    public Tilemap doors;
    public WorldGraph worldGraph;
    public List<Room> rooms;
    // Start is called before the first frame update
    void Start()
    {
        rooms = new List<Room>();
        AddRoom( new Room() );
        Debug.Log( "Rooms: " + rooms.Count );
        worldGraph = new WorldGraph(walls.cellBounds.size.x-1, walls.cellBounds.size.y-1, walls, floor, doors, this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void InstantiateRooms() {
        Debug.Log( "InstantiateRooms" );

        Room defaultRoom = new Room();
        foreach (ClonedTile t in worldGraph.tiles ) {
            if ( t.type == TileType.Empty )
                GetOutsideRoom().AssignTile( t );
            else {
                defaultRoom.AssignTile( t );
                t.numberOfRooms--;
                t.hasRoom = false;
            }

        }

        //while (defaultRoom.CountTiles() > 0) {
        //    //Debug.Log( "Old room: " + oldRoom.Tiles.Count );

        //    Debug.Log( "New room: " + rooms.Count );
        //    queue.Enqueue( defaultRoom.GetTileAt(0) );

        //    FillRoom( queue.Dequeue(), CreateNewRoom(), defaultRoom );

        //}
        while ( defaultRoom.CountTiles()>0) {
            Debug.Log( "Old room: " + defaultRoom.CountTiles() );

            Room newRoom = CreateNewRoom();
            Debug.Log( "New room: " + rooms.Count );

            ClonedTile tileToCheck = defaultRoom.GetTileAt(0);

            FillRoom( tileToCheck, newRoom, defaultRoom );
            if ( newRoom.CountTiles() <= 1 )
                DeleteRoom( newRoom, defaultRoom );
        }

        foreach ( Room r in rooms ) {
            Debug.Log( "Room: " + rooms.IndexOf(r) + " Tiles: " + r.CountTiles());
        }
    }

    Room CreateNewRoom() {
        Room newRoom = new Room();
        AddRoom( newRoom );
        return newRoom;
    }

    void FillRoom(ClonedTile startTile, Room newRoom, Room defaultRoom ) {
        defaultRoom.UnassignTile( startTile );
        if ( startTile == null ) {
            return;
        }
        if ( startTile.isDone )
            return;

        if (startTile.room != defaultRoom && startTile.type != TileType.Door)
            return;

        if(startTile.type == TileType.Empty ) {
            defaultRoom.UnassignTile( startTile );
            GetOutsideRoom().AssignTile( startTile );
            return;
        }

        // Tile is not done, is not null and is not in newRoom.

        if ( startTile.roomEnclosure) {
            startTile.room = null;
            if ( startTile.type == TileType.Door) {
                newRoom.Doors.Add( startTile );
                defaultRoom.AssignTile( startTile );
            }
            return;
        }

        newRoom.AssignTile( startTile );

        foreach(ClonedTile nb in startTile.GetNeighbours() ) {
            if(nb.type == TileType.Empty ) {
                DeleteRoom( newRoom,defaultRoom );
                return;
            }
            FillRoom( nb, newRoom, defaultRoom );
        }

    }

    public void AddRoom( Room room ) {
        if(!rooms.Contains(room))
            rooms.Add( room );
    }

    public Room GetOutsideRoom() {
        return rooms[0];
    }
                                                                                                                                                                                                                                   
    public void DeleteRoom( Room room, Room defaultRoom ) {
        if ( room == GetOutsideRoom() ) {
            Debug.LogError( "Terminated. You're trying to delete the Outside Room." );
            return;
        }
        room.UnassignTiles(defaultRoom);
        room = null;
        rooms.Remove( room );
    }


}
