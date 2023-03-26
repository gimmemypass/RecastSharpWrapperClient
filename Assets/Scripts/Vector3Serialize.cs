using UnityEngine;

public partial struct Vector3Serialize
{
    public Vector3Serialize(Vector3 source)
    {
        X = source.x;
        Y = source.y;
        Z = source.z;
    }

    public Vector3 AsVector => new(X, Y, Z);

    public void SetVector3Serialize(Vector3 source)
    {
        X = source.x;
        Y = source.y;
        Z = source.z;
    }
}