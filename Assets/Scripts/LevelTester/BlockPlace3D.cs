//using BuilderTool.Enums;
//using BuilderTool.Helpers;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

////namespace BuilderTool.LevelTester
////{
//public class BlockPlace3D : MonoBehaviour
//{
//    [SerializeField] private Grid3D grid3D;

//    public void PlaceBlock3DOnGrid(Vector2Int gridPos, GameObject blockShape3D)
//    {
//        if (grid3D == null)
//        {
//            Debug.LogError("Không thể đặt prefab vì không có tham chiếu đến Grid3D! Hãy gán nó trong Inspector.");
//            return;
//        }

//        if (blockShape3D == null)
//        {
//            Debug.LogError("BlockShape3D is null!");
//            return;
//        }

//        float gridSize = grid3D.GridSpaceSize;

//        Vector3 worldPosition = new Vector3(gridPos.x * gridSize, gridPos.y * gridSize, 0.1f);
//        GameObject newBlock = Instantiate(blockShape3D, worldPosition, Quaternion.identity);

//        newBlock.transform.SetParent(grid3D.transform, false);

//        Debug.Log($"Đã đặt prefab {blockShape3D.name} tại vị trí grid: {gridPos}");
//    }
//}
//        //private Dictionary<EShape3D, List<Vector3Int>> _blockDict = new Dictionary<EShape3D, List<Vector3Int>>
//        //{
//        //    {EShape3D.Cross, new List<Vector3Int> {new (5,5), new(5,0) }},
//        //};


//        //private List<Vector3Int> GetBlockTile3D (EShape3D blockshape3D)
//        //{
//        //    if (!_blockDict.ContainsKey(blockshape3D)) 
//        //    {
//        //        Debug.Log ("Shape 3D not found");
//        //        return null;
//        //    }

//        //    Debug.Log ("Shape 3D found");
//        //    return _blockDict[blockshape3D];
//        //}
//    //}

////}
