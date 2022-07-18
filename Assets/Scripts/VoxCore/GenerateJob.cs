using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

public struct GenerateJob : IJobParallelFor
{
    public NativeArray<float> densities;
    public NativeArray<Vector3> positions;
    public NativeArray<VertexType> types;
    public Vector3 position;

    public void Execute(int index)
    {
        int z = index % 16;
        int y = index / 16 % 128;
        int x = index / 16 / 128;

        float density;

        float landscape = Perlin.Fbm((position.x * 15 + x) / 30, (position.z * 15 + z) / 30, 3);
        if (y < 64 + (landscape - 0.5f) * 20)
        {
            density = Perlin.Noise((position.x * 15 + x) / 30, (position.y * 15 + y) / 30, (position.z * 15 + z) / 30);

        }
        else
        {
            density = 1.0f;
        }

        VertexType type;

        if (density > 0.7f)
            type = VertexType.Air;
        else
            type = VertexType.Stone;

        densities[index] = density;
        positions[index] = position * 16 + new Vector3(x, y, z);
        types[index] = type;
    }
}