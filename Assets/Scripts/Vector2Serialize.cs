using UnityEngine;

public partial struct Vector2Serialize
{
    public Vector2Serialize(Vector2 source)
    {
        X = source.x;
        Y = source.y;
    }
        
    public Vector2 AsVector => new(X, Y);

    public void SetVector2Serialize(Vector2 source)
    {
        X = source.x;
        Y = source.y;
    }
}