using LiteNetLib;
using LiteNetLib.Utils;
using Server.Shared.Commands;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [SerializeField] private string address = "localhost";
    [SerializeField] private int port = 4242;
    [SerializeField] private string secretKey = "";

    [SerializeField] private GameObject locationPrefab;
    [SerializeField] private GameObject characterPrefab;
    
    private EventBasedNetListener listener;
    private NetManager netManager; 
    
    private NetPacketProcessor netPacketProcessor;
    
    private GameObject locationInstance;
    private GameObject characterInstance;

    private void Start()
    {
        listener = new EventBasedNetListener();
        listener.NetworkReceiveEvent += ListenerOnNetworkReceiveEvent;
        listener.PeerConnectedEvent += ListenerOnPeerConnectedEvent;
        listener.PeerDisconnectedEvent += ListenerOnPeerDisconnectedEvent;
        
        netManager = new NetManager(listener);
        netManager.Start();
        netManager.MaxConnectAttempts = 20;
        Debug.Log($"Try connecting to {address} : {port.ToString()}");

        netPacketProcessor = new();
        netPacketProcessor.RegisterNestedType<Vector2Serialize>();
        netPacketProcessor.RegisterNestedType<Vector3Serialize>();
        netPacketProcessor.SubscribeReusable<PrepareLocationNetworkCommand, NetPeer>(OnPrepareLocationCommand);
        netPacketProcessor.SubscribeReusable<SpawnCharacterNetworkCommand, NetPeer>(OnSpawnCharacterCommand);
        
        netManager.Connect(address, port, secretKey);
    }

    private void OnSpawnCharacterCommand(SpawnCharacterNetworkCommand command, NetPeer netPeer)
    {
        characterInstance = Instantiate(characterPrefab, command.Position.AsVector, Quaternion.identity);
    }

    private void OnPrepareLocationCommand(PrepareLocationNetworkCommand command, NetPeer netPeer)
    {
        locationInstance = Instantiate(locationPrefab, Vector3.zero, Quaternion.identity);
        locationInstance.transform.localScale = new Vector3(command.LocationScale.X, 1, command.LocationScale.Y);
    }

    private void Update()
    {
        netManager.PollEvents();
    }

    private void ListenerOnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectinfo)
    {
        Debug.LogWarning($"Disconnected from server {disconnectinfo.Reason.ToString()}");
    }

    private void ListenerOnPeerConnectedEvent(NetPeer peer)
    {
        Debug.Log($"Successfully connected {address} : {port.ToString()}");
    }

    private void ListenerOnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliverymethod)
    {
        netPacketProcessor.ReadAllPackets(reader, peer);
    }
}