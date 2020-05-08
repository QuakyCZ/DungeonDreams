using System.Collections.Generic;
using Controllers;
using Models.Characters;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Models {
    public class WorldGraph {
        public readonly ClonedTile[,] tiles;

        private readonly int _xOffset;
        private readonly int _yOffset;

        private readonly Tilemap _walls;
        private readonly Tilemap _floor;
        private readonly Tilemap _door;
        public readonly Tilemap debugMap;
        private readonly Dictionary<ClonedTile, TileBase> _originalTiles;
        private readonly int _width;
        private readonly int _height;
        private readonly RoomController _roomController;

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

            this._walls = walls;
            this._floor = floor;
            this._door = door;
            this.debugMap = debugMap;
            this._width = width;
            this._height = height;
            this._roomController = roomController;

            _xOffset = (int) walls.localBounds.min.x;
            _yOffset = (int) walls.localBounds.min.y;
            Debug.Log($"xOffset: {_xOffset}, yOffset: {_yOffset}");
            //Debug.Log( width + " " + height );
            _originalTiles = new Dictionary<ClonedTile, TileBase>();

            ImportTiles();
        }

        public void ImportTiles() {
            ClonedTile clonedTile;
            int w_y = 0; // X coordinate in worldGraph


            for (int y = (int) _walls.localBounds.min.y; y < (int) _walls.localBounds.max.y; y++) {
                int w_x = 0; // Y coordinate in worldGraph

                for (int x = (int) _walls.localBounds.min.x; x < _walls.localBounds.max.x; x++) {
                    Vector3Int pos = new Vector3Int(x, y, 0);

                    TileBase tile = _floor.GetTile(pos);

                    if (tile != null) {
                        clonedTile = new ClonedTile(w_x, w_y, x, y, TileType.Floor);
                        tiles[w_x, w_y] = clonedTile;
                        _originalTiles.Add(clonedTile, tile);
                        clonedTile.isWalkable = true;
                    }

                    tile = _walls.GetTile(pos);

                    if (tile != null) {
                        clonedTile = new ClonedTile(w_x, w_y, x, y, TileType.Wall);
                        tiles[w_x, w_y] = clonedTile;
                        _originalTiles.Add(clonedTile, tile);
                        clonedTile.isWalkable = false;
                    }

                    tile = _door.GetTile(pos);
                    if (tile != null) {
                        clonedTile = new ClonedTile(w_x, w_y, x, y, TileType.Door);
                        tiles[w_x, w_y] = clonedTile;
                        _originalTiles.Add(clonedTile, tile);
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
                t.roomController = _roomController;
                if (_originalTiles.ContainsKey(t)) {
                    TileBase tile = _originalTiles[t];
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
            int _x = x - _xOffset;
            int _y = y - _yOffset;
            return tiles[_x, _y];
        }

        public List<ClonedTile> GetNeighbours(ClonedTile tile) {
            List<ClonedTile> nb = new List<ClonedTile>();
            int x = tile.x;
            int y = tile.y;
            if (x + 1 < _width) {
                nb.Add(tiles[x + 1, y]);
            }

            if (x - 1 >= 0) {
                nb.Add(tiles[x - 1, y]);
            }

            if (y + 1 < _height) {
                nb.Add(tiles[x, y + 1]);
            }

            if (y - 1 >= 0) {
                nb.Add(tiles[x, y - 1]);
            }

            //TOP RIGHT
            if (x + 1 < _width && y + 1 < _height && tiles[x, y + 1].isWalkable && tiles[x + 1, y].isWalkable) {
                nb.Add(tiles[x + 1, y + 1]);
            }

            //BOTTOM RIGHT
            if (x + 1 < _width && y - 1 > 0 && tiles[x, y - 1].isWalkable && tiles[x + 1, y].isWalkable) {
                nb.Add(tiles[x + 1, y - 1]);
            }

            //TOP LEFT
            if (tile.x - 1 > 0 && y + 1 < _height && tiles[x, y + 1].isWalkable && tiles[x - 1, y].isWalkable) {
                nb.Add(tiles[x - 1, y + 1]);
            }

            //BOTTOM LEFT
            if (tile.x - 1 > 0 && y - 1 > 0 && tiles[x - 1, y].isWalkable && tiles[x, y - 1].isWalkable) {
                nb.Add(tiles[x - 1, y - 1]);
            }

            //Debug.Log( "Get neighbours for tile: X: " + x + " Y: " + y + " Count: " + nb.Count.ToString());
            return nb;
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
                    vectorPath.Add(new Vector2(tile.realX + 0.5f, tile.realY + 0.5f));
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
            ClonedTile startTile = GetTileAt(
                Mathf.FloorToInt(start.x),
                Mathf.FloorToInt(start.y)
            );

            ClonedTile endTile = GetTileAt(
                Mathf.FloorToInt(end.x),
                Mathf.FloorToInt(end.y)
            );

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

                foreach (ClonedTile neighbour in GetNeighbours(currentTile)) {
                    if (closedTiles.Contains(neighbour) == false && neighbour.isWalkable && CheckEmptyTile(new Vector3(neighbour.realX,neighbour.realY))==false) {
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

            //Debug.Log("Path length: " + path.Count);
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

        private bool CheckEmptyTile(Vector3 position) {
            Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
            ClonedTile tile =
               GetTileAt(
                    Mathf.FloorToInt(position.x),
                    Mathf.FloorToInt(position.y)
                );
            foreach (var enemy in enemies) {
                ClonedTile enemyTile = GetTileAt(
                    Mathf.FloorToInt(enemy.transform.position.x),
                    Mathf.FloorToInt(enemy.transform.position.y)
                );
                if (tile == enemyTile) return true;
            }

            return false;
        }
    }
}