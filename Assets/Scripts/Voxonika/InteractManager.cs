using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private WorldComponent worldComponent;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log(Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out RaycastHit hit, 5));
            
            Chunk chunk = worldComponent.loadedChunks[World.BlockPosToChunkPos(new Vector3Int((int)hit.point.x, (int)hit.point.y, (int)hit.point.z))].chunk;
            VertexData vertex = chunk.vertices[(int)chunk.position.x + (int)hit.point.x % 16, (int)hit.point.y, (int)chunk.position.z + (int)hit.point.z % 16];
            //Debug.Log(hit.transform.gameObject.name);
            vertex.density = 1.0f;
            vertex.type = VertexType.Air;
            worldComponent.CalcuteMesh(chunk);
        }
    }
}
