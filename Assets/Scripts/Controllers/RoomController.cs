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
        for ( int i = 0; i < 60; i++ ) {
            Debug.Log( "Old room: " + defaultRoom.CountTiles() );

            Room newRoom = CreateNewRoom();
            Debug.Log( "New room: " + rooms.Count );

            ClonedTile tileToCheck = defaultRoom.GetTileAt(0);
            if ( tileToCheck.roomEnclosure ) {
                defaultRoom.AssignTile( tileToCheck );
                for ( int j = 0; j < defaultRoom.CountTiles(); j++ ) {
                    ClonedTile t = defaultRoom.GetTileAt( j );
                    if ( t.roomEnclosure ) {
                        tileToCheck = t;
                        break;
                    }
                }

            }

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
        Debug.Log("Fill room.");
        defaultRoom.UnassignTile( startTile );
        // If tile is null, we reached the outside of the map -> skip it.
        if ( SkipTile( startTile, newRoom ) ) {
            Debug.Log( "This tile was skipped." );
            return;
        }
        List<ClonedTile> nbs = startTile.GetNeighbours();

        if(startTile.roomEnclosure == false && startTile.type != TileType.Empty && nbs.Count < 4 ) {
            // There is a floor at the corner -> it shouldn't happen. If it does, delete this room and assign its tiles to the outside.
            Debug.LogWarning( "There is a tile in the corner wich doesn't enclose the room." );
            DeleteRoom( newRoom, GetOutsideRoom() );
            return;
        }

        // This tile is empty and it's assigned to the outside -> skip this tile.
        if ( startTile.type == TileType.Empty ) {
            Debug.Log( "This tile is empty." );
            // This tile is empty and it's not attached to the outside -> assign this tile to the outside room
            GetOutsideRoom().AssignTile( startTile );
            foreach ( ClonedTile nb in nbs ) {
                if ( nb.isDone == false && GetOutsideRoom().HasTile( nb ) == false )
                    FillRoom( nb, newRoom, defaultRoom );
            }
            return;
        }

        // At this point the tile is not empty and is assigned to default room or it's a door assigned to 1 room (we need 2 rooms).
        newRoom.AssignTile( startTile );

        // We need to assign roomEnclosure tile to 2 rooms -> enqueue it again.
        // If this tile's neighbour is outside -> assign it to the outside and return!
        // If this tile's border -> it's done if encloses just 1 room.
        if (startTile.roomEnclosure == true) {
            Debug.Log( "This tile has roomEnclosure." );
            CheckEnclosureTile( startTile, newRoom, nbs );
            return;
        }

        // Check tile's neighbours
        Debug.Log( "This tile is floor." );
        Debug.Log( "Neigbours to check: " + nbs.Count );
        foreach (ClonedTile nb in nbs) {
            if (nb.isDone == false && GetOutsideRoom().HasTile(nb) == false)
                FillRoom(nb,newRoom,defaultRoom);
        }
    }

    void CheckEnclosureTile(ClonedTile startTile, Room newRoom, List<ClonedTile> nbs) {
        // We have to check its neighbours.
        // If it has less than 4 nbs, it's border tile.
        if ( nbs.Count < 4 ) {
            int enclose = 0;

            foreach ( ClonedTile nb in nbs ) {
                if ( nb.roomEnclosure )
                    enclose++;
            }

            if ( enclose == 3 ) {
                // All neighbours have roomEnclosure -> we can't pick this tile again from another room -> assign it to the outside. (this is the shape: -|)
                // Return it into defaultRoom
                if ( startTile.isDone == false )
                    newRoom.UnassignTile( startTile );
                GetOutsideRoom().AssignTile( startTile );
                return;
            }
            if ( enclose == 2 ) {
                // If it has 2 enclosing nbs it encloses 1 room (shape: -' OR |)
                startTile.border = true;
                startTile.isDone = true;
                return;
            }
        }

        else {
            // This tile has 4 nbs -> it's not a border.
            if ( !startTile.isDone ) {
                GetOutsideRoom().AssignTile( startTile );
                return;
            }
            return;
        }
        return;
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
                                                                                                                                                                                                                                   
    public void DeleteRoom( Room room, Room outside ) {
        if ( room == GetOutsideRoom() ) {
            Debug.LogError( "Terminated. You're trying to delete the Outside Room." );
            return;
        }
        room.UnassignTiles(outside);
        room = null;
        rooms.Remove( room );
    }


}
