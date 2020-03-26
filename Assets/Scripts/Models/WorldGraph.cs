using System.Collections.Generic;
using Controllers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Models {
    public class WorldGraph {
        public ClonedTile[,] tiles;

        private int xOffset;
        private int yOffset;

        Tilemap walls;
        Tilemap floor;
        Tilemap door;
        public Tilemap debugMap;
        Dictionary<ClonedTile, TileBase> originalTiles;
        public int width;
        public int height;
        RoomController roomController;

        private const int MoveStraightCost = 10;
        private const int MoveDiagonalCost = 14;


        public WorldGraph(int width, int height, Tilemap walls, Tilemap floor, Tilemap door, Tilemap debugMap,
            RoomController roomController) {
            tiles = new ClonedTile[width, height];
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    tiles[x, y] = new ClonedTile(x, y, 0, 0, TileType.Empty);
                }
            }

            this.walls = walls;
            this.floor = floor;
            this.door = door;
            this.debugMap = debugMap;
            this.width = width;
            this.height = height;
            this.roomController = roomController;

            xOffset = (int) walls.localBounds.min.x;
            yOffset = (int) walls.localBounds.min.y;
            Debug.Log($"xOffset: {xOffset}, yOffset: {yOffset}");
            //Debug.Log( width + " " + height );
            originalTiles = new Dictionary<ClonedTile, TileBase>();

            ImportTiles();
        }

        public void ImportTiles() {
            ClonedTile clonedTile;
            int w_y = 0; // X coordinate in worldGraph


            for (int y = (int) walls.localBounds.min.y; y < (int) walls.localBounds.max.y; y++) {
                int w_x = 0; // Y coordinate in worldGraph

                for (int x = (int) walls.localBounds.min.x; x < walls.localBounds.max.x; x++) {
                    Vector3Int pos = new Vector3Int(x, y, 0);

                    TileBase tile = floor.GetTile(pos);

                    if (tile != null) {
                        clonedTile = new ClonedTile(w_x, w_y, x, y, TileType.Floor);
                        tiles[w_x, w_y] = clonedTile;
                        originalTiles.Add(clonedTile, tile);
                    }

                    tile = walls.GetTile(pos);

                    if (tile != null) {
                        clonedTile = new ClonedTile(w_x, w_y, x, y, TileType.Wall);
                        tiles[w_x, w_y] = clonedTile;
                        originalTiles.Add(clonedTile, tile);
                        clonedTile.isWalkable = false;
                    }

                    tile = door.GetTile(pos);
                    if (tile != null) {
                        clonedTile = new ClonedTile(w_x, w_y, x, y, TileType.Door);
                        tiles[w_x, w_y] = clonedTile;
                        originalTiles.Add(clonedTile, tile);
                        clonedTile.isWalkable = false;
                    }

                    w_x++;
                }

                w_y++;
            }

            int wallnumber = 0;
            int floornumber = 0;
            int doornumber = 0;
            int emptynumber = 0;
            foreach (ClonedTile t in tiles) {
                t.roomController = roomController;
                if (originalTiles.ContainsKey(t)) {
                    TileBase tile = originalTiles[t];
                    debugMap.SetTile(new Vector3Int(t.realX, t.realY, 0), tile);
                }

                switch (t.type) {
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

            Debug.Log("Walls: " + wallnumber + " Floor: " + floornumber + " Doors: " + doornumber + " Empty: " +
                      emptynumber);
        }

        public ClonedTile GetTileAt(int x, int y) {
            int _x = x - xOffset;
            int _y = y - yOffset;
            Debug.Log("Get tile at: " + _x + " " + _y);
            return tiles[_x, _y];
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start">Start position in the world.</param>
        /// <param name="end">End position in the world.</param>
        /// <returns></returns>
        public List<Vector2> FindVectorPath(Vector2 start, Vector2 end) {
            List<ClonedTile> path = FindPath(start, end);
            if (path == null) {
                return null;
            }
            else {
                List<Vector2> vectorPath = new List<Vector2>();
                foreach (var tile in path) {
                    vectorPath.Add(new Vector2(tile.realX+0.5f, tile.realY+0.5f));
                }

                return vectorPath;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start">Start position in the world.</param>
        /// <param name="end">End position in the world.</param>
        /// <returns></returns>
        public List<ClonedTile> FindPath(Vector2 start, Vector2 end) {
            ClonedTile startTile = GetTileAt((int) start.x, (int) start.y);
            ClonedTile endTile = GetTileAt((int) end.x, (int) end.y);

            List<ClonedTile> openList = new List<ClonedTile>();
            List<ClonedTile> closedTiles = new List<ClonedTile>();

            openList.Add(startTile);

            foreach (var node in tiles) {
                node.gCost = int.MaxValue;
                node.cameFrom = null;
            }

            startTile.gCost = 0;
            startTile.hCost = GetDistance(startTile, endTile);

            while (openList.Count > 0) {
                ClonedTile currentTile = GetLowestFCostTile(openList);
                if (currentTile == endTile) {
                    return CalculatePath(endTile);
                }

                openList.Remove(currentTile);
                closedTiles.Add(currentTile);

                foreach (ClonedTile neighbour in currentTile.GetNeighbours()) {
                    if (closedTiles.Contains(neighbour) == false && neighbour.isWalkable) {
                        int tentativeGCost = currentTile.gCost + GetDistance(currentTile, neighbour);
                        if (tentativeGCost < neighbour.gCost) {
                            neighbour.cameFrom = currentTile;
                            neighbour.gCost = tentativeGCost;
                            neighbour.hCost = GetDistance(neighbour, endTile);

                            if (!openList.Contains(neighbour)) {
                                openList.Add(neighbour);
                            }
                        }
                    }
                }
            }

            // no path
            return null;
        }

        private List<ClonedTile> CalculatePath(ClonedTile endTile) {
            List<ClonedTile> path = new List<ClonedTile>();
            path.Add(endTile);
            ClonedTile current = endTile;
            while (current.cameFrom != null) {
                path.Add(current.cameFrom);
                current = current.cameFrom;
            }
            path.Reverse();

            Debug.Log("Path length: " + path.Count);
            return path;
        }

        private ClonedTile GetLowestFCostTile(List<ClonedTile> tiles) {
            ClonedTile lowest = tiles[0];
            foreach (var tile in tiles) {
                if (tile.FCost < lowest.FCost) {
                    lowest = tile;
                }
            }

            return lowest;
        }

        private int GetDistance(ClonedTile a, ClonedTile b) {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MoveDiagonalCost * Mathf.Min(xDistance, yDistance) + MoveStraightCost * remaining;
        }
    }
}