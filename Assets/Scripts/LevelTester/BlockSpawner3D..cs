using UnityEngine;
using System.Collections.Generic;
using BuilderTool.Enums;
using BuilderTool.Helpers;
using BuilderTool.LevelEditor;
using System.Linq;

public class BlockSpawner3D : Singleton<BlockSpawner3D>
{
    [SerializeField] private List<Block3DPrefabEntry> blockPrefabs;

    public void SpawnBlock(EditorBlock block2D, Vector3 pos, Quaternion rotation){
        var blockPreb = blockPrefabs.FirstOrDefault((block) => block.shape == block2D.Shape);
        var spawnedBlock = Instantiate(blockPreb.prefab, pos, rotation, transform);
        spawnedBlock.InitBlock(block2D.PrimaryColor);
    }
}

[System.Serializable]
public class Block3DPrefabEntry
{
    public EShape shape;
    public Block3D prefab;
}
