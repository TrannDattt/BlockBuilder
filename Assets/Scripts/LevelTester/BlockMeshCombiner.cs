using System.Collections.Generic;
using UnityEngine;

public class BlockMeshCombiner : MonoBehaviour{
    private void CombineMesh(){
        // MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        // CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        
        // for (int i = 0; i < meshFilters.Length; i++)
        // {
        //     combine[i].mesh = meshFilters[i].sharedMesh;
        //     combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        // }

        // Mesh combinedMesh = new();
        // combinedMesh.CombineMeshes(combine, true, true);

        // MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        // meshFilter.mesh = combinedMesh;

        // MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        // meshCollider.sharedMesh = combinedMesh;

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++) 
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;
        }

        // Tạo và gán mesh gộp
        Mesh combinedMesh = new Mesh();
        combinedMesh.name = "CombinedMesh";
        combinedMesh.CombineMeshes(combine, true, true);

        // Gán mesh vào MeshFilter và MeshCollider
        MeshFilter mfMain = GetComponent<MeshFilter>();
        if (mfMain == null) mfMain = gameObject.AddComponent<MeshFilter>();
        mfMain.sharedMesh = combinedMesh;

        MeshCollider mc = GetComponent<MeshCollider>();
        if (mc == null) mc = gameObject.AddComponent<MeshCollider>();
        mc.sharedMesh = combinedMesh;
        mc.convex = true;

        // var body = GetComponent<Rigidbody>();
        // body.isKinematic = false;
    }

    void Start()
    {
        CombineMesh();
    }
}
