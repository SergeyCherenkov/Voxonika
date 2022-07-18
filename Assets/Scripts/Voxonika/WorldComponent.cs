using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldComponent : MonoBehaviour
{
    private World world;

    [SerializeField] private Transform player;

    [SerializeField] private GameObject chunkPrefab;

    [SerializeField] private int viewDistance;

    [SerializeField] private Vector3 oldPosition;
    [SerializeField] private Vector3 newPosition;

    [SerializeField] private GameObject treePrefab;

    public Dictionary<Vector3, MeshComponent> loadedChunks = new();

    private Channel<(Chunk, List<Vector3>, List<int>, List<Vector2>)> chunkQueue = new();

    private void Start() 
    {
        world = new World(0);
        oldPosition.x = (int)player.position.x / 16;
        oldPosition.z = (int)player.position.z / 16;

        LoadChunksFromDistance();
    }

    private void Update() 
    {
        newPosition.x = (int)player.position.x / 16;
        newPosition.z = (int)player.position.z / 16;

        if (oldPosition != newPosition)
            LoadChunksFromDistance();

        oldPosition = newPosition;

        if (chunkQueue.Count > 0)
        {
            var tuple = chunkQueue.Dequeue();

            CreateMeshChunk(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        }
    }

    private void LoadChunksFromDistance()
    {
        List<Vector3> forRemove = new();

        foreach (KeyValuePair<Vector3, MeshComponent> chunk in loadedChunks)
            if ((Mathf.Abs(newPosition.x - chunk.Key.x) > viewDistance || Mathf.Abs(newPosition.z - chunk.Key.z) > viewDistance) && chunk.Value != null)
                forRemove.Add(chunk.Key);

        foreach (Vector3 key in forRemove)
        {
            Destroy(loadedChunks[key].gameObject);
            loadedChunks.Remove(key);
        }

        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int z = -viewDistance; z <= viewDistance; z++)
            {
                Vector3 chunkPosition = new Vector3(x, 0, z) + newPosition;

                if(!loadedChunks.ContainsKey(chunkPosition))
                {
                    loadedChunks.Add(chunkPosition, null);
                    Thread loadChunkThread = new Thread(LoadChunk);
                    loadChunkThread.Start(chunkPosition);
                }
                
            }
        }
    }

    public void LoadChunk(object positionObj)
    {
        Vector3 position = (Vector3)positionObj;
        Chunk chunk = world.GenerateChunk(position);

        CalcuteMesh(chunk);
    }

    public void CalcuteMesh(Chunk chunk)
    {
        List<Vector3> vertices = new();
        List<int> triangles = new();
        List<Vector2> uvs = new();

        for (int x = 0; x < 15; x++)
            for (int y = 0; y < 127; y++)
                for (int z = 0; z < 15; z++)
                {
                    Vector3[] cornerCoords = new Vector3[8];
                    cornerCoords[0] = new Vector3(x, y, z) + new Vector3(0, 0, 0);
                    cornerCoords[1] = new Vector3(x, y, z) + new Vector3(1, 0, 0);
                    cornerCoords[2] = new Vector3(x, y, z) + new Vector3(1, 0, 1);
                    cornerCoords[3] = new Vector3(x, y, z) + new Vector3(0, 0, 1);
                    cornerCoords[4] = new Vector3(x, y, z) + new Vector3(0, 1, 0);
                    cornerCoords[5] = new Vector3(x, y, z) + new Vector3(1, 1, 0);
                    cornerCoords[6] = new Vector3(x, y, z) + new Vector3(1, 1, 1);
                    cornerCoords[7] = new Vector3(x, y, z) + new Vector3(0, 1, 1);

                    int cubeConfiguration = 0;

                    for (int i = 0; i < 8; i ++)
                        if (chunk.vertices[((int)cornerCoords[i].x), ((int)cornerCoords[i].y), ((int)cornerCoords[i].z)].density < 0.7f)
                            cubeConfiguration |= 1 << i;

                    int[] edgeIndices = Table.triangulation[cubeConfiguration];

                    for (int i = 0; i < 16; i+=3)
                    {
                        if (edgeIndices[i] == -1)
                            break;

                        int edgeIndexA = edgeIndices[i];
                        int a0 = Table.cornerIndexAFromEdge[edgeIndexA];
                        int a1 = Table.cornerIndexBFromEdge[edgeIndexA];

                        int edgeIndexB = edgeIndices[i+1];
                        int b0 = Table.cornerIndexAFromEdge[edgeIndexB];
                        int b1 = Table.cornerIndexBFromEdge[edgeIndexB];

                        int edgeIndexC = edgeIndices[i+2];
                        int c0 = Table.cornerIndexAFromEdge[edgeIndexC];
                        int c1 = Table.cornerIndexBFromEdge[edgeIndexC];

                        vertices.Add(InterpolateVertex(chunk, cornerCoords[a0], cornerCoords[a1]));
                        vertices.Add(InterpolateVertex(chunk, cornerCoords[b0], cornerCoords[b1]));
                        vertices.Add(InterpolateVertex(chunk, cornerCoords[c0], cornerCoords[c1]));

                        triangles.Add(triangles.Count);
                        triangles.Add(triangles.Count);
                        triangles.Add(triangles.Count);

                        VertexType type = chunk.vertices[(int)cornerCoords[a0].x, (int)cornerCoords[a0].y, (int)cornerCoords[a0].z].type;
                        if (type == VertexType.Air)
                            type = chunk.vertices[(int)cornerCoords[a1].x, (int)cornerCoords[a1].y, (int)cornerCoords[a1].z].type;
                        //Debug.Log(type);
                        switch (type)
                        {
                            case VertexType.Earth:
                                uvs.Add(new Vector2(0, 0.34f));
                                uvs.Add(new Vector2(0, 0.65f));
                                uvs.Add(new Vector2(0.32f, 0.34f));
                                break;
                            case VertexType.Snow:
                                uvs.Add(new Vector2(0.34f, 0));
                                uvs.Add(new Vector2(0.34f, 0.32f));
                                uvs.Add(new Vector2(0.65f, 0));
                                break;
                            case VertexType.Sand:
                                uvs.Add(new Vector2(0.34f, 0.34f));
                                uvs.Add(new Vector2(0.34f, 0.65f));
                                uvs.Add(new Vector2(0.65f, 0.34f));
                                break;
                            case VertexType.Stone:
                                uvs.Add(new Vector2(0, 0));
                                uvs.Add(new Vector2(0, 0.32f));
                                uvs.Add(new Vector2(0.32f, 0f));
                                break;
                            default:
                                uvs.Add(new Vector2(0, 0));
                                uvs.Add(new Vector2(0, 0.32f));
                                uvs.Add(new Vector2(0.32f, 0f));
                                break;
                        }

                    }
                }

        chunkQueue.Enqueue((chunk, vertices, triangles, uvs));
    }

    private Vector3 InterpolateVertex(Chunk chunk, Vector3 aPos, Vector3 bPos)
    {
        float aDensity = chunk.vertices[((int)aPos.x), ((int)aPos.y), ((int)aPos.z)].density;
        float bDensity = chunk.vertices[((int)bPos.x), ((int)bPos.y), ((int)bPos.z)].density;

        float t = (0.7f - aDensity) / (bDensity - aDensity);

        Vector3 position = aPos + t * (bPos - aPos);

        return position;
    }

    public void CreateMeshChunk(Chunk chunk, List<Vector3> vertices, List<int> triangles, List<Vector2> uv)
    {
        Mesh mesh = new Mesh();

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
        mesh.RecalculateNormals();
        mesh.Optimize();

        CreateChunk(chunk, mesh);
    }

    public void CreateChunk(Chunk chunk, Mesh mesh)
    {
        //if (loadedChunks[chunk.position] == null)
        //{
            GameObject chunkObject = Instantiate(chunkPrefab, chunk.position * 15, Quaternion.identity);
            MeshComponent meshComponent = chunkObject.GetComponent<MeshComponent>();
            meshComponent.chunk = chunk;
            meshComponent.SetMesh(mesh);
            loadedChunks[chunk.position] = meshComponent;

            foreach (Vector3 structure in chunk.structures)
            {
                GameObject tree = Instantiate(treePrefab, chunk.position * 15 + structure, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));
                float scale = UnityEngine.Random.Range(0.9f, 1.1f);
                tree.transform.localScale = new Vector3(scale, scale, scale);
                tree.transform.SetParent(chunkObject.transform);
            }
        //}
        //else
        //{
        //    loadedChunks[chunk.position].SetMesh(mesh);
        //}

    }
}
