using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuilderTool.Editor
{
    [System.Serializable]
    public class Unit : MonoBehaviour
    {
        public Vector3Int GetUnitPosOnMap(Tilemap map)
        {
            Vector3Int blockTile = map.WorldToCell(transform.position);
            Vector3Int tilePos = new(blockTile.x, blockTile.y, 0);
            return tilePos;
        }
    }
}
