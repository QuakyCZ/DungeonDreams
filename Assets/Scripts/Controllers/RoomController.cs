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
    Queue<ClonedTile> queue;
    // Start is called before the first frame update
    void Start()
    {
        rooms = new List<Room>();
        AddRoom( new Room() );
        Debug.Log( "Rooms: " + rooms.Count );
        worldGraph = new WorldGraph(walls.cellBounds.size.x, walls.cellBounds.size.y, walls, floor, doors, this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void InstantiateRooms() {
        Debug.Log( "InstantiateRooms" );
        foreach(ClonedTile t in worldGraph.tiles ) {
            GetOutsideRoom().AssignTile( t );
        }

        queue = new Queue<ClonedTile>();
        Room defaultRoom = GetOutsideRoom().Clone();

        queue.Enqueue( defaultRoom.GetTileAt(0) );
        //Debug.Log( "Tiles in outside room: " + GetOutsideRoom().Tiles.Count );
        //Debug.Log( "Tiles in old room: " + oldRoom.Tiles.Count );
        rooms[0].UnassignTiles();
        //Debug.Log( "Tiles in old room: " + oldRoom.Tiles.Count );

        while (defaultRoom.CountTiles() > 0) {
            //Debug.Log( "Old room: " + oldRoom.Tiles.Count );

            Debug.Log( "New room: " + rooms.Count );
            queue.Enqueue( defaultRoom.GetTileAt(0) );
            FillRoom( queue.Dequeue(), CreateNewRoom(), defaultRoom );

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
        // If tile is null, we reached the outside of the map -> skip it.
        if ( SkipTile(startTile,newRoom) )
            return;

        // This tile is empty and it's assigned to the outside -> skip this tile.
        if ( startTile.type == TileType.Empty && GetOutsideRoom().HasTile(startTile) == true ) {
            startTile.numberOfRooms = 1;
            return;
        }

        startTile.numberOfRooms++;

        // This tile is empty and it's not attached to the outside -> assign this tile to the outside room
        if (startTile.type == TileType.Empty && GetOutsideRoom().HasTile( startTile ) == false) {
            GetOutsideRoom().AssignTile( startTile );
            return;
        }

        // At this point the tile is not empty and is assigned to default room or it's a door assigned to 1 room (we need 2 rooms).
        newRoom.AssignTile( startTile );

        // We need to assign roomEnclosure tile to 2 rooms -> enqueue it again.
        if (startTile.roomEnclosure == true && startTile.isDone == false ) {
            defaultRoom.AssignTile( startTile );
            return;
        }

        if ( startTile.roomEnclosure) {
            return;
        }
        // Enqueue tile's neighbours
        foreach(ClonedTile nb in startTile.GetNeighbours()) {
            if (queue.Contains(nb) == false && nb.isDone == false)
                queue.Enqueue( nb );
        }
        if ( queue.Count > 0 ) {
            FillRoom( queue.Dequeue(), newRoom, defaultRoom );
        }
    }

    private bool SkipTile(ClonedTile tile, Room newRoom ) {
        if ( tile == null ) {
            return true;
        }
        if ( tile.isDone )
            return true;

        if ( newRoom.HasTile( tile ) )
            return true;
        return false;
    }

    public void AddRoom( Room room ) {
        if(!rooms.Contains(room))
            rooms.Add( room );
    }

    public Room GetOutsideRoom() {
        return rooms[0];
    }
                                                                                                                                                                                                                                   
    public void DeleteRoom( Room room ) {
        if ( room == GetOutsideRoom() ) {
            Debug.LogError( "Terminated. You're trying to delete the Outside Room." );
            return;
        }
        room.UnassignTiles();
        rooms.Remove( room );
    }
    //public void UnassignTilesInRoom( Room room ) {
    //    foreach ( ClonedTile t in room.Tiles ) {
    //        room.Tiles.Remove( t );
    //        t.hasRoom = false;
    //    }
    //}


}
