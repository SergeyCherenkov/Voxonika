using UnityEngine;

public class VertexData
{
    public Vector3 position;
    public float density;
    public VertexType type;

    public VertexData(Vector3 position, float density, VertexType type)
    {
        this.position = position;
        this.density = density;
        this.type = type;
    }
}
