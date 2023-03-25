using System;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [SerializeField] private string address = "localhost";
    [SerializeField] private int port = 4242;
    [SerializeField] private string secretKey = "";
    private EventBasedNetListener listener;
    private NetManager netManager;

    private void Start()
    {
        listener = new EventBasedNetListener();
        listener.NetworkReceiveEvent += ListenerOnNetworkReceiveEvent;
        listener.PeerConnectedEvent += ListenerOnPeerConnectedEvent;
        listener.PeerDisconnectedEvent += ListenerOnPeerDisconnectedEvent;
        listener.NetworkErrorEvent += ListenerOnNetworkErrorEvent;
        
        netManager = new NetManager(listener);
        netManager.Start();
        netManager.MaxConnectAttempts = 20;
        Debug.Log($"Try connecting to {address} : {port.ToString()}");
        var netPeer = netManager.Connect(address, port, secretKey);
    }

    private void Update()
    {
        netManager.PollEvents();
    }

    private void ListenerOnNetworkErrorEvent(IPEndPoint endpoint, SocketError socketerror)
    {
        Debug.Log("Network error"); 
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
        Debug.Log($"We got message from server : {reader.GetString(100)}");
        reader.Recycle();
    }
}