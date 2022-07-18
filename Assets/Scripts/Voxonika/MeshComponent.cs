using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

public class MeshComponent : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;
    public Chunk chunk;
    public Vector3 position;

    public void SetMesh(Mesh mesh)
    {
        position = chunk.position;
        
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
}
