using System;
using System.Collections.Generic;
using System.Linq;
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

        public void GridTilesReset()
        {
            GridTiles = new List<Tile>();
            for (var x = 0; x < GridWidth; x++)
            {
                for (var y = 0; y < GridHeight; y++)
                {
                    GridTiles.Add(new Tile(new Vector2Int(x,y)));
                }
            }
        }
        
        
        public bool TileIsEmpty(int x, int y,TileType tileType = default)
        {
            foreach (var tile in GridTiles)
            {
                if (tileType == default)
                {
                    if (tile.tileXZ == new Vector2Int(x, y)& tile.tileType != default) 
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
    
        public int GetTileNeighbourIndex(Vector2Int tileXZ, TileType tileType,TileType tileType2 = default)
        {
            var x = tileXZ.x;
            var y = tileXZ.y;
        
            var index = 0;

            if (!TileIsEmpty(x, y-1,tileType))
            {
                index +=1;
            }
            if (!TileIsEmpty(x, y+1,tileType))
            {
                index += 8;
            }
            if (!TileIsEmpty(x-1, y,tileType))
            {
                index += 2;
            }
            if (!TileIsEmpty(x+1, y,tileType))
            {
                index += 4;
            }

            if (tileType2 != default)
            {
                if (!TileIsEmpty(x, y-1,tileType2))
                {
                    index +=1;
                }
                if (!TileIsEmpty(x, y+1,tileType2))
                {
                    index += 8;
                }
                if (!TileIsEmpty(x-1, y,tileType2))
                {
                    index += 2;
                }
                if (!TileIsEmpty(x+1, y,tileType2))
                {
                    index += 4;
                }
            }
            return index;
        }
        
        public int GetTileIndex(int x, int y)
        {
            foreach (var tile in GridTiles.Where(tile => tile.tileXZ == new Vector2Int(x, y)))
            {
                return GridTiles.IndexOf(tile);
            }
            throw new Exception(x.ToString() + y.ToString());
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
