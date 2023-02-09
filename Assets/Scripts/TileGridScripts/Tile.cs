using TileGridScripts.Enum;
using UnityEngine;

namespace TileGridScripts
{
    public class Tile
    {
        public Vector2Int tileXZ;
        public GameObject tileObject = default;
    
        public Tile previousPathTile = default;

        public TileType tileType;
    
        public Tile(Vector2Int newTileXZ)
        {
            tileXZ = newTileXZ;
        }
    }
}
