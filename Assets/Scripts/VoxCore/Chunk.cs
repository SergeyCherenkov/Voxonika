using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

public class Chunk
{
    public Vector3 position;
    public VertexData[,,] vertices;
    public List<Vector3> structures;

    public Chunk(Vector3 position)
    {
        this.position = position;
        vertices = new VertexData[16, 128, 16];
        structures = new List<Vector3>();
    }

    public void Generate()
    {
        for (int x = 0; x < 16; x++)
            for (int z = 0; z < 16; z++)
            {
                float landscape = Perlin.Fbm((position.x * 15 + x) / 400, (position.z * 15 + z) / 400, 5);

                float temperature = (Perlin.Fbm((position.x * 15 + x) / 600, (position.z * 15 + z) / 600, 3) + 0.5f) ;
                float rainfall = (Perlin.Fbm((position.x * 15 + x) / 600 + 1000, (position.z * 15 + z) / 600 + 1000, 3) + 0.5f) ;

                BiomType biomType = World.chunksMap[Mathf.RoundToInt(temperature * 10), Mathf.RoundToInt(rainfall * 10)];

                int surface = 61 + (int)(landscape * 40f);

                System.Random random = new System.Random();
                
                int chance = random.Next(1000);

                if (landscape > 0.0f)
                {
                    switch (biomType)
                        {
                            case BiomType.Forest:
                                landscape = ForestCurveMask(landscape);
                                if (chance < 10)
                                    structures.Add(new Vector3(x, surface, z));
                                break;
                            case BiomType.Desert:
                                landscape = DesertCurveMask(landscape);
                                break;
                            case BiomType.Taiga:
                                landscape = TaigaCurveMask(landscape);
                                if (chance < 10)
                                    structures.Add(new Vector3(x, surface, z));
                                break;
                            case BiomType.Savanna:
                                landscape = SavannaCurveMask(landscape);
                                if (chance < 5)
                                    structures.Add(new Vector3(x, surface, z));
                                break;
                            case BiomType.Jungle:
                                landscape = JungleCurveMask(landscape);
                                if (chance < 10)
                                    structures.Add(new Vector3(x, surface, z));
                                break;
                            case BiomType.Plains:
                                landscape = PlainsCurveMask(landscape);
                                if (chance < 5)
                                    structures.Add(new Vector3(x, surface, z));
                                break;
                        }
                }



                for (int y = 0; y < 128; y++)
                {
                    float density;
                    
                    if (y < surface)
                    {
                        density = 0.0f;
                        //density = Perlin.Noise((position.x * 15 + x) / 30, (position.y * 15 + y) / 30, (position.z * 15 + z) / 30) + 0.5f;
                    }
                    else
                    {
                        density = 1.0f;
                    }

                    if (y < 2)
                        density = 0.0f;

                    VertexType type;

                    if (density > 0.7f)
                        type = VertexType.Air;
                    else if (y >= 60 && y > surface - 4)
                    {
                        switch (biomType)
                        {
                            case BiomType.Taiga:
                                type = VertexType.Snow;
                                break;
                            case BiomType.Forest:
                                type = VertexType.Snow;
                                break;
                            case BiomType.Desert:
                                type = VertexType.Sand;
                                break;
                            case BiomType.Plains:
                                type = VertexType.Earth;
                                break;
                            default:
                                type = VertexType.Earth;
                                break;
                        }
                    }
                    else
                        type = VertexType.Stone;

                    if (y >= 58 && y <= 60 && surface >= 58 && surface <= 60)
                        type = VertexType.Sand;

                    vertices[x, y, z] = new VertexData(position * 16 + new Vector3(x, y, z), density, type);

                }
            }
    }

    private float ForestCurveMask(float t)
    {
        return QuadraticBezier(Vector2.zero, new Vector2(0.3f, 0.6f), new Vector2(1.0f, 0.6f), t).y;
    }

    private float DesertCurveMask(float t)
    {
        return QuadraticBezier(Vector2.zero, new Vector2(0.25f, 0.3f), new Vector2(1.0f, 0.3f), t).y;
    }

    private float TaigaCurveMask(float t)
    {
        return QuadraticBezier(Vector2.zero, new Vector2(0.3f, 0.4f), new Vector2(1.0f, 0.4f), t).y;
    }

    private float SavannaCurveMask(float t)
    {
        return QuadraticBezier(Vector2.zero, new Vector2(0.25f, 0.1f), new Vector2(1.0f, 0.1f), t).y;
    }

    private float JungleCurveMask(float t)
    {
        return QuadraticBezier(Vector2.zero, new Vector2(0.3f, 0.4f), new Vector2(1.0f, 0.4f), t).y;
    }

    private float PlainsCurveMask(float t)
    {
        return QuadraticBezier(Vector2.zero, new Vector2(0.25f, 0.2f), new Vector2(1.0f, 0.2f), t).y;
    }

    private Vector2 QuadraticBezier(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        return (1.0f - t) * (1.0f - t) * p0 + 2 * t * (1.0f - t) * p1 + t * t * p2;
    }
}