using UnityEngine;

public class World
{
    public int seed;

    public static BiomType[,] chunksMap = new BiomType[11, 11]
    {
        { BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga },
        { BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga },
        { BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga, BiomType.Taiga },
        { BiomType.Plains, BiomType.Plains, BiomType.Plains, BiomType.Forest, BiomType.Forest, BiomType.Forest, BiomType.Forest, BiomType.Forest, BiomType.Forest, BiomType.Taiga, BiomType.Taiga},
        { BiomType.Savanna, BiomType.Savanna, BiomType.Savanna, BiomType.Plains, BiomType.Forest, BiomType.Forest, BiomType.Forest, BiomType.Forest, BiomType.Forest, BiomType.Taiga, BiomType.Taiga},
        { BiomType.Desert, BiomType.Savanna, BiomType.Savanna, BiomType.Savanna, BiomType.Plains, BiomType.Plains, BiomType.Forest, BiomType.Forest, BiomType.Forest, BiomType.Forest, BiomType.Taiga},
        { BiomType.Desert, BiomType.Desert, BiomType.Savanna, BiomType.Savanna, BiomType.Plains, BiomType.Plains, BiomType.Plains, BiomType.Forest, BiomType.Forest, BiomType.Forest, BiomType.Forest},
        { BiomType.Desert, BiomType.Desert, BiomType.Desert, BiomType.Savanna, BiomType.Plains, BiomType.Plains, BiomType.Plains, BiomType.Forest, BiomType.Jungle, BiomType.Jungle, BiomType.Forest },
        { BiomType.Desert, BiomType.Desert, BiomType.Desert, BiomType.Savanna, BiomType.Plains, BiomType.Plains, BiomType.Plains, BiomType.Forest, BiomType.Jungle, BiomType.Jungle, BiomType.Jungle },
        { BiomType.Desert, BiomType.Desert, BiomType.Desert, BiomType.Savanna, BiomType.Savanna, BiomType.Plains, BiomType.Plains, BiomType.Plains, BiomType.Jungle, BiomType.Jungle, BiomType.Jungle },
        { BiomType.Desert, BiomType.Desert, BiomType.Desert, BiomType.Desert, BiomType.Savanna, BiomType.Savanna, BiomType.Plains, BiomType.Plains, BiomType.Jungle, BiomType.Jungle, BiomType.Jungle }
    };

    public World(int seed)
    {
        this.seed = seed;

        System.Random random = new System.Random(seed);
        random.NextBytes(Perlin.perm);
    }

    public Chunk GenerateChunk(Vector3 position)
    {
        Chunk chunk = new Chunk(position);
        chunk.Generate();
        return chunk;
    }

    public static Vector3Int BlockPosToChunkPos(Vector3 position)
    {
        int x = ((int)position.x) / 16;
        int z = ((int)position.z) / 16;
        return new Vector3Int(x, 0, z);
    }
}