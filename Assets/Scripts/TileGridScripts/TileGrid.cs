using System;
using System.Collections.Generic;
using TileGridScripts.Enum;
using UnityEngine;

namespace TileGridScripts
{
    public class TileGrid 
    {
        public List<Tile> GridTiles { get; private set;}
        public int GridWidth { get; private set;}
        public int GridHeight { get; private set;}

        public Tile EnemySpawnPoint { get; private set;}
        
        public Tile RiverStartTile { get; private set;}
        public Tile RiverEndTile { get; private set;}

        
        public TileGrid (int gridWidth, int gridHeight)
        {
            GridTiles = new List<Tile>();
            GridWidth = gridWidth;
            GridHeight = gridHeight;
        }

        public void SetEnemySpawnPoint(Tile newEnemySpawnPoint)
        {
            EnemySpawnPoint =newEnemySpawnPoint;
        }
        
        public void SetRiverStartTile(int x,int y)
        {
            //RiverStartTile =newRiverStartTile;
        }
        
        public void SetRiverEndTile(int x,int y)
        {
            //RiverEndTile =newRiverEnd;
        }

        public void GridTilesReset()
        {
            GridTiles = new List<Tile>();
            for (var x = 0; x < GridWidth; x++)
            {
                for (var y = 0; y < GridHeight; y++)
                {
                    GridTiles.Add(new Tile(new Vector2Int(x,y)));
                    
                    Debug.DrawLine(new Vector3(x-0.5f,0,y-0.5f),new Vector3(x-0.5f,0,y+0.5f),Color.blue,100f);
                    Debug.DrawLine(new Vector3(x-0.5f,0,y-0.5f),new Vector3(x+0.5f,0,y-0.5f),Color.blue,100f);
                }
                
                Debug.DrawLine(new Vector3(-0.5f,0,GridHeight-0.5f),new Vector3(GridWidth-0.5f,0,GridHeight-0.5f),Color.blue,100f);
                Debug.DrawLine(new Vector3(GridWidth-0.5f,0,-0.5f),new Vector3(GridWidth-0.5f,0,GridHeight-0.5f),Color.blue,100f);
            }
        }


        public bool TileIsEmpty(int x, int y, TileType tileType = default)
        {
            foreach (var tile in GridTiles)
            {
                if (tileType == default)
                {
                    if (tile.tileXZ == new Vector2Int(x, y) & tile.tileType != default)
                        return false;
                }
                else
                {
                    if (tile.tileXZ == new Vector2Int(x, y) & tile.tileType == tileType)
                        return false;
                }
            }

            return true;
        }

        public int GetTileNeighbourIndex(Vector2Int tileXZ, TileType tileType, TileType tileType2 = default)
        {
            var x = tileXZ.x;
            var y = tileXZ.y;

            int GetNeighbourIndex(TileType type)
            {
                var neighbourIndex = 0;

                if (!TileIsEmpty(x, y - 1, type))
                    neighbourIndex += NeighbourIndex.Down;

                if (!TileIsEmpty(x, y + 1, type))
                    neighbourIndex += NeighbourIndex.Up;

                if (!TileIsEmpty(x - 1, y, type))
                    neighbourIndex += NeighbourIndex.Left;

                if (!TileIsEmpty(x + 1, y, type))
                    neighbourIndex += NeighbourIndex.Right;

                return neighbourIndex;
            }

            var index = GetNeighbourIndex(tileType);

            if (tileType2 != default)
            {
                index += GetNeighbourIndex(tileType2);
            }
            
            return index;
        }
        
        public int GetTileIndex(int x, int y)
        {
            var index = x * GridHeight + y;

            if (index > GridTiles.Count - 1)
                throw new IndexOutOfRangeException($"x: {x}, y: {y}");
            
            return index;
        }
    }
}
