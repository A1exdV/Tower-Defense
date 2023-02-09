using System;
using System.Collections;
using System.Collections.Generic;
using TileGridScripts.Enum;
using UnityEngine;

namespace TileGridScripts
{
    public class TileGridInitializer : MonoBehaviour
    {
        [Header("Grid")]
        [SerializeField] private int gridWidth;
        [SerializeField] private int gridHeight;

        [Header("Path")]
        [SerializeField] private int targetPathSize = 25;
        [SerializeField] private bool canBeGreater = true;
        [SerializeField] private List<GameObject> pathTile;
    
        [Header("Objects")]
        [SerializeField] private bool allowRiver = false;
        [SerializeField] private List<GameObject> riverTile;
    
        [Tooltip("if 0 number will be random")]
        [SerializeField] private int treesNumber = 8;
        [SerializeField] private List<GameObject> treeTile;
    
        [Tooltip("if 0 number will be random")]
        [SerializeField] private int rocksNumber = 8;
        [SerializeField] private List<GameObject> rockTile;
    
        [Tooltip("if 0 number will be random")]
        [SerializeField] private int crystalsNumber = 4;
        [SerializeField] private List<GameObject> crystalTile;
    
        
        private TileGrid _tileGrid;
        private PathGenerator _pathGenerator;
        private RiverGenerator _riverGenerator;
        private GameObject _grid;
    
        private void Start()
        {
            _grid = new GameObject("GridTiles");
            _pathGenerator = new PathGenerator();
            _riverGenerator = new RiverGenerator();
            
            InitializeGrid(gridWidth,gridHeight);
            
            GeneratePath();
            
            if(allowRiver)
                GenerateRiver();
            
            StartCoroutine(InitializePathTiles());
            StartCoroutine(InitializeRiverTiles());
        }

        #region Initialization -----------------------------------------------------------------------------------------
        
        private void InitializeGrid(int newGridWidth, int newGridHeight)
        {
            _tileGrid = new TileGrid(newGridWidth,newGridHeight);
        }
        
        public void InitializePrefab()
        {
        
        }

        public void InitializeGeneration(int newGridWidth, int newGridHeight,int newTargetPath,bool newCanBeGreater)
        {

        }
        
        #endregion

        #region Path ---------------------------------------------------------------------------------------------------
        
        private void GeneratePath()
        {
            if(canBeGreater)
                do
                {
                    PathInitialize();
                } while (_pathGenerator.GetPathCount(_tileGrid) < targetPathSize);
            else
            {
                do
                {
                    PathInitialize();
                } while (_pathGenerator.GetPathCount(_tileGrid) != targetPathSize);
            }
        }
    
        private void PathInitialize()
        {
            _tileGrid = _pathGenerator.GeneratePath(_tileGrid);
        }
    
        private IEnumerator InitializePathTiles()
        {
            Tile previousPathTile = null;
            foreach (var tile in _tileGrid.GridTiles)
            {
                if (tile.tileType == TileType.Path)
                {
                    var newTileObject = SetTilePathVisual(tile);
                    newTileObject.transform.parent = _grid.transform;
                    newTileObject.name = "PathTile";
                    newTileObject.tag = "Path";
                    tile.tileObject = newTileObject;
                    tile.previousPathTile = previousPathTile;
                    previousPathTile = tile;

                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        private GameObject SetTilePathVisual(Tile tile)
        {
            var newTileObject = _tileGrid.GetTileNeighbourIndex(tile.tileXZ,TileType.Path,TileType.Bridge) switch
            {
                1 => Instantiate(pathTile[5], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 180, 0)),
                2 => Instantiate(pathTile[5], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, -90, 0)),
                3 => Instantiate(pathTile[2], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, -90, 0)),
                4 => Instantiate(pathTile[0], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 90, 0)),
                5 => Instantiate(pathTile[2], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 180, 0)),
                6 => Instantiate(pathTile[1], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 90, 0)),
                7 => Instantiate(pathTile[3], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 180, 0)),
                8 => Instantiate(pathTile[5], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 0, 0)),
                9 => Instantiate(pathTile[1], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 0, 0)),
                10 => Instantiate(pathTile[2], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 0, 0)),
                11 => Instantiate(pathTile[3], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, -90, 0)),
                12 => Instantiate(pathTile[2], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 90, 0)),
                13 => Instantiate(pathTile[3], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 90, 0)),
                14 => Instantiate(pathTile[3], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 0, 0)),
                15 => Instantiate(pathTile[4], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 0, 0)),
                _ => throw new ArgumentException(_tileGrid.GetTileNeighbourIndex(tile.tileXZ,TileType.Path).ToString() +", " + tile.tileXZ)
            };
            return newTileObject;
        }
        
        #endregion

        #region River --------------------------------------------------------------------------------------------------
        
        private void GenerateRiver()
        {
            _tileGrid = _riverGenerator.GenerateRiver(_tileGrid);
        }

        private IEnumerator InitializeRiverTiles()
        {
            foreach (var tile in _tileGrid.GridTiles)
            {
                if (tile.tileType == TileType.Bridge)
                {
                    var newTileObject = SetTileBridgeVisual(tile);
                    newTileObject.transform.parent = _grid.transform;
                    newTileObject.name = "BridgeTile";
                    newTileObject.tag = "Bridge";
                    tile.tileObject = newTileObject;

                    yield return new WaitForSeconds(0.1f);
                }
                if (tile.tileType == TileType.River)
                {
                    var newTileObject = SetTileRiverVisual(tile);
                    newTileObject.transform.parent = _grid.transform;
                    newTileObject.name = "RiverTile";
                    newTileObject.tag = "River";
                    tile.tileObject = newTileObject;

                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        
        private GameObject SetTileRiverVisual(Tile tile)
        {
            var newTileObject = _tileGrid.GetTileNeighbourIndex(tile.tileXZ,TileType.River,TileType.Bridge) switch
            {
                1 => Instantiate(riverTile[1], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 180, 0)),
                2 => Instantiate(riverTile[1], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, -90, 0)),
                3 => Instantiate(riverTile[2], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, -90, 0)),
                4 => Instantiate(riverTile[1], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 90, 0)),
                5 => Instantiate(riverTile[2], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 180, 0)),
                6 => Instantiate(riverTile[1], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 90, 0)),
                8 => Instantiate(riverTile[1], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 0, 0)),
                9 => Instantiate(riverTile[1], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 0, 0)),
                10 => Instantiate(riverTile[2], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 0, 0)),
                12 => Instantiate(riverTile[2], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 90, 0)),
                _ => throw new ArgumentException(_tileGrid.GetTileNeighbourIndex(tile.tileXZ,TileType.River,TileType.Bridge).ToString() +", " + tile.tileXZ)
            };
            return newTileObject;
        }
        
        private GameObject SetTileBridgeVisual(Tile tile)
        {
            var newTileObject = _tileGrid.GetTileNeighbourIndex(tile.tileXZ,TileType.River) switch
            {
                6 => Instantiate(riverTile[0], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 90, 0)),
                9 => Instantiate(riverTile[0], new Vector3(tile.tileXZ.x, 0, tile.tileXZ.y), Quaternion.Euler(0, 0, 0)),
                _ => throw new ArgumentException(_tileGrid.GetTileNeighbourIndex(tile.tileXZ,TileType.River,TileType.Bridge).ToString() +", " + tile.tileXZ)
            };
            return newTileObject;
        }
        
        #endregion
    }
}
