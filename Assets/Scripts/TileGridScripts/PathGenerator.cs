using System;
using System.Collections.Generic;
using System.Linq;
using TileGridScripts.Enum;
using Random = UnityEngine.Random;

namespace TileGridScripts
{
    public class PathGenerator
    {
        private int _height;
        private int _width;


        public TileGrid GeneratePath(TileGrid tileGrid)
        {
            _height = tileGrid.GridHeight;
            _width = tileGrid.GridWidth;
            
            tileGrid.GridTilesReset();
            
            var y = _height / 2;
            var x = 0;
        
            while (x < _width)
            {
                var index = tileGrid.GetTileIndex(x, y);
                tileGrid.GridTiles[index].tileType = TileType.Path;

                var validMove = false;

                while (!validMove)
                {
                    var move = Random.Range(0, 3);
                    
                    if (x % 2 == 0 || x>(_width-2))
                    {
                        x++;
                        break;
                    }
                
                    switch (move)
                    {
                        case 0:
                            x++;
                            validMove = true;
                            break;
                        case 1:
                            if (tileGrid.TileIsEmpty(x, y + 1) && y < (_height-2))
                            {
                                y++; 
                                validMove = true;
                            }
                            break;
                        case 2:
                            if (tileGrid.TileIsEmpty(x, y - 1) && y > 2)
                            {
                                y--; 
                                validMove = true;
                            }
                            break;
                    } 
                }
                tileGrid.SetEnemySpawnPoint(tileGrid.GridTiles[index]);
            }
            return tileGrid;
        }

        public int GetPathCount(TileGrid tileGrid)
        {
            return tileGrid.GridTiles.Count(tile => tile.tileType == TileType.Path);
        } 
        
    }
}
