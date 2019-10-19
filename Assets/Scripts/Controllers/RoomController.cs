using System.Collections;
using System.Diagnostics;
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
        rooms.Add( new Room() );
        UnityEngine.Debug.Log( "Rooms: " + rooms.Count );
        worldGraph = new WorldGraph(walls.size.x, walls.size.y, walls, floor, doors, this);
        InstantiateRooms();
    }

    // Update is called once per frame
    void Update()
    {
        
    }       

    public void InstantiateRooms() {
        queue = new Queue<ClonedTile>();
        Room oldRoom = GetOutsideRoom().Clone();
        queue.Enqueue( oldRoom.Tiles[0] );
        UnityEngine.Debug.Log( "Tiles in outside room: " + GetOutsideRoom().Tiles.Count );
        UnityEngine.Debug.Log( "Tiles in old room: " + oldRoom.Tiles.Count );
        rooms[0] = null;
        rooms[0] = new Room();
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Room newRoom = GetOutsideRoom();
        while(oldRoom.Tiles.Count > 0 && sw.ElapsedMilliseconds<60000) {
            //Debug.Log( "New Room" );
            FloodFill( queue.Dequeue(), newRoom, oldRoom);
            if ( oldRoom.Tiles.Count > 0 ) {
                queue.Enqueue( oldRoom.Tiles[0] );
                newRoom = new Room();
                rooms.Add( newRoom );
            }
        }
        sw.Stop();
        foreach ( Room r in rooms ) {
            UnityEngine.Debug.Log( "Room: " + rooms.IndexOf(r) + " Tiles: " + r.Tiles.Count);
        }
    }

    void FloodFill(ClonedTile tile, Room newRoom, Room oldRoom ) {

        // If tile is null, we reached the outside of the map -> skip it.
        if ( tile == null ) {
            oldRoom.Tiles.Remove( tile );
            return;
        }

        oldRoom.Tiles.Remove( tile );


        // This tile is empty and it's not attached to the outside -> assign this tile to the outside room
        if (tile.type == TileType.Empty && GetOutsideRoom().Tiles.Contains( tile ) == false) {
            if ( GetOutsideRoom().Tiles.Contains( tile ) == false ) {
                GetOutsideRoom().AssignTile( tile );
            }
            tile.hasRoom = false;
            return;
        }
        // This tile is empty and it's assigned to the outside -> skip this tile.
        if (tile.type == TileType.Empty && GetOutsideRoom().Tiles.Contains(tile) == true )
            return;

        tile.numberOfRooms++;

        // If tile has already a room and is not a door -> skip it. 
        if ( tile.hasRoom && tile.type != TileType.Door )
            return;

        // We need to assign roomEnclosure tile to 2 rooms -> enqueue it again.
        if (tile.roomEnclosure && tile.isDone == false ) {
            oldRoom.Tiles.Add( tile );
        }

        // At this point the tile is not empty and is assigned to default room or it's a door assigned to 1 room (we need 2 rooms).
        newRoom.Tiles.Add( tile );
        tile.hasRoom = true;

        if ( tile.roomEnclosure && tile.isDone) {
            return;
        }
        // Enqueue tile's neighbours
        foreach(ClonedTile nb in tile.neighbours ) {
            if ( nb != null && nb != tile && queue.Contains(nb) == false && nb.isDone == false)
                queue.Enqueue( nb );
        }
        if ( queue.Count > 0 ) {
            FloodFill( queue.Dequeue(), newRoom, oldRoom );
            return;
        }

    }

    public void AddRoom( Room room ) {
        rooms.Add( room );
    }
    public Room GetOutsideRoom() {
        return rooms[0];
    }                                                                                                                                                                                                                               
    public void DeleteRoom( Room room ) {
        if ( room == GetOutsideRoom() ) {
            UnityEngine.Debug.LogError( "Terminated. You're trying to delete the Outside Room." );
            return;
        }
        UnassignTilesInRoom( room );
        rooms.Remove( room );
    }
    public void UnassignTilesInRoom( Room room ) {
        foreach ( ClonedTile t in room.Tiles ) {
            room.Tiles.Remove( t );
            t.hasRoom = false;
        }
    }


}
