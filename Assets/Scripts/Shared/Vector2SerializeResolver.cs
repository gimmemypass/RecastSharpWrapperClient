using LiteNetLib.Utils;

public partial struct Vector2Serialize : INetSerializable
{
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(X);
        writer.Put(Y);
    }

    public void Deserialize(NetDataReader reader)
    {
        X = reader.GetFloat();
        Y = reader.GetFloat();
    }
}