using System.Collections.Generic;
using System.Linq;
using TileGridScripts.Enum;
using TileGridScripts.Utils;
using UnityEngine;

namespace TileGridScripts
{
    public class RiverGenerator
    {
        private int _height;
        private int _width;
        private TileGrid _tempGrid;

        public TileGrid GenerateRiver(TileGrid tileGrid)
        {
            _height = tileGrid.GridHeight;
            _width = tileGrid.GridWidth;
            
            while (true)
            {
                var (isSuccess, oldTileTypes) = TryToGenerateRiver(tileGrid);

                if (!isSuccess)
                {
                    tileGrid.GridTiles.ForEachIndexed((tile, index) => tile.tileType = oldTileTypes[index]);
                }
                else
                {
                    break;
                }
            }
            
            return _tempGrid;
        }

        private (bool, List<TileType>) TryToGenerateRiver(TileGrid tileGrid)
        {
            _tempGrid = tileGrid;

            var oldTileTypes = tileGrid.GridTiles.Select(tile => tile.tileType).ToList();
            
            var x = Random.Range(1, _width-1);
            var y = _height-1;
            _tempGrid.SetRiverStartTile(x,y);
            while (y>=0)
            {
                var index = _tempGrid.GetTileIndex(x, y);
                
                var failLeft = false;
                var failBottom = false;
                var failRight = false;
                
                _tempGrid.GridTiles[index].tileType = _tempGrid.GridTiles[index].tileType == TileType.Path ? TileType.Bridge : TileType.River;
                
                //set new river start tile

                var validMove = false;
                
                while (!validMove)
                {
                    if (failLeft == true & failBottom == true & failRight == true)
                    {
                        Debug.Log("Retrying river generation");
                        return (false, oldTileTypes);
                    }
                    
                    if (y % 2 == 0)
                    {
                        if(!_tempGrid.TileIsEmpty(x, y-1) & !CheckForStraitPath(_tempGrid, x, y-1))
                        {
                            return (false, oldTileTypes);
                        }
                        y--;
                        break;
                    }
                    
                    var move = Random.Range(0, 3);//0-left, 1-down, 2-right

                    switch (move)
                    {
                        case 0:
                            if (_tempGrid.TileIsEmpty(x-1, y)& x>1)
                            {
                                x--;
                                validMove = true;
                            }
                            else if (CheckForStraitPath(_tempGrid,x-1, y)& x>1)
                            {
                                x--;
                                validMove = true;
                            }
                            else
                            {
                                failLeft = true;
                            }
                            break;
                        
                        case 1:
                            if (_tempGrid.TileIsEmpty(x, y-1))
                            {
                                y--;
                                validMove = true;
                            }
                            else if (CheckForStraitPath(_tempGrid, x, y-1))
                            {
                                y--;
                                validMove = true;
                            }
                            else
                            {
                                failBottom = true;
                            }
                            break;
                        
                        case 2:
                            if (_tempGrid.TileIsEmpty(x+1, y) & x<_width-1)
                            {
                                x++;
                                validMove = true;
                            }
                            else if (CheckForStraitPath(_tempGrid,x+1, y) & x<_width-1)
                            {
                                x++;
                                validMove = true;
                            }
                            else
                            {
                                failRight = true;
                            }
                            break;
                    }
                }
            }
            _tempGrid.SetRiverEndTile(x,y);
            return (true, oldTileTypes);
        }


        private bool CheckForStraitPath(TileGrid tileGrid,int x, int y)
        {
            if (!tileGrid.TileIsEmpty(x, y, TileType.Path))
            {
                switch (tileGrid.GetTileNeighbourIndex(new Vector2Int(x, y), TileType.Path))
                {
                    case 6:
                    case 9:
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }
    }
}
