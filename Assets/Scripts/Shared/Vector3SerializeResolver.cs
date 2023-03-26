using LiteNetLib.Utils;

public partial struct Vector3Serialize : INetSerializable
{
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(X);
        writer.Put(Y);
        writer.Put(Z);
    }

    public void Deserialize(NetDataReader reader)
    {
        X = reader.GetFloat();
        Y = reader.GetFloat();
        Z = reader.GetFloat();
    }
}