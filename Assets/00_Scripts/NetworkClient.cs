using Cinemachine;
using ENet6;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public InitData initData;
    public Transform playerTransform;
    public SpaceMovement spaceMovement;
}


public class NetworkClient : MonoBehaviour
{
    private ENet6.Host enetHost = null;
    private ENet6.Peer? serverPeer = null;


    PlayerData ownPlayer;
    PacketBuilder packetBuilder = null;

    Dictionary<uint, PlayerData> playersInitData = new();

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject client;
    [SerializeField] GameObject otherClient;
    [SerializeField] ClientGlobalInfo clientInfo;

    [SerializeField] private float tickRate = 1 / 30;
    private float tickTime;

    public bool Connect(string addressString)
    {
        ENet6.Address address = new ENet6.Address();
        if (!address.SetHost(ENet6.AddressType.Any, addressString))
        {
            Debug.LogError("failed to resolve \"" + addressString + "\"");
            return false;
        }

        address.Port = 14769;
        Debug.Log("connecting to " + address.GetIP());


        // On recréé l'host à la connexion pour l'avoir en IPv4 / IPv6 selon l'adresse
        if (enetHost != null)
            enetHost.Dispose();

        enetHost = new ENet6.Host();
        enetHost.Create(address.Type, 1, 0);
        serverPeer = enetHost.Connect(address, 0);

        // On laisse la connexion se faire pendant un maximum de 50 * 100ms = 5s
        for (uint i = 0; i < 50; ++i)
        {
            ENet6.Event evt = new ENet6.Event();
            if (enetHost.Service(100, out evt) > 0)
            {
                Debug.Log("Successfully connected !");
                packetBuilder = new PacketBuilder(serverPeer.Value, 0);
                // Nous avons un événement, la connexion a soit pu s'effectuer (ENET_EVENT_TYPE_CONNECT) soit échoué (ENET_EVENT_TYPE_DISCONNECT)
                break; //< On sort de la boucle
            }
        }

        if (serverPeer.Value.State != PeerState.Connected)
        {
            Debug.LogError("connection to \"" + addressString + "\" failed");
            return false;
        }

        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!ENet6.Library.Initialize())
            throw new Exception("Failed to initialize ENet");

        if(Connect("localhost"))
        {
            ownPlayer = new PlayerData() { initData = new InitData() { clientInitData = new ClientInitData() { matId = (byte) clientInfo.matId, playerName = clientInfo.playerName, skinId = (byte) clientInfo.skinId } } };
            packetBuilder.SendPacket(new ClientInitData(clientInfo.playerName, clientInfo.skinId, clientInfo.matId));
        }
    }

    private void OnApplicationQuit()
    {
        ENet6.Library.Deinitialize();
    }

    private void Update()
    {
        if (Time.time >= tickTime)
        {
            tickTime += tickRate;

            //tick reseau d'envoie d'inputs
            SendPlayerInputs();
        }
    }

    void FixedUpdate()
    {
        ENet6.Event evt = new ENet6.Event();
        if (enetHost.Service(0, out evt) > 0)
        {
            do
            {
                switch (evt.Type)
                {
                    case ENet6.EventType.None:
                        Debug.Log("?");
                        break;

                    case ENet6.EventType.Connect:
                        Debug.Log("Connect");
                        break;

                    case ENet6.EventType.Disconnect:
                        Debug.Log("Disconnect");
                        serverPeer = null;
                        break;

                    case ENet6.EventType.Receive:
                        byte[] buffer = new byte[1024];
                        evt.Packet.CopyTo(buffer);
                        HandleMessage(buffer);
                        Debug.Log("Receive");
                        break;

                    case ENet6.EventType.Timeout:
                        Debug.Log("Timeout");
                        break;
                }
            }
            while (enetHost.CheckEvents(out evt) > 0);
        }
    }
    private void HandleMessage(byte[] buffer)
    {
        int offset = 0;
        Opcode opcode = (Opcode)Serialization.DeserializeU8(buffer,ref offset);
        Debug.Log("Opcode" + opcode.ToString());
        switch (opcode)
        {
            case Opcode.OnClientConnectResponse:
            {
                ConnectServerInitData responseFromConnect = new();
                responseFromConnect.Deserialize(buffer, ref offset);
                //ownPlayerNumber = responseFromConnect.playerNum;
                GameObject player = Instantiate(client, responseFromConnect.playerStartPos, Quaternion.identity);
                player.GetComponent<ClientSkinLoader>().LoadSkin(clientInfo.skinId, clientInfo.matId);

                ownPlayer.initData.serverClientInitData = responseFromConnect;
                ownPlayer.playerTransform = player.transform;
                ownPlayer.spaceMovement = player.GetComponent<SpaceMovement>();

                virtualCamera.Follow = player.transform;
                virtualCamera.LookAt = player.transform;
                break;
            }

            case Opcode.OnOtherClientConnect:
            {
                InitData dataFromServer = new();
                dataFromServer.Deserialize(buffer, ref offset);
                GameObject player = Instantiate(otherClient, dataFromServer.serverClientInitData.playerStartPos, Quaternion.identity);
                player.GetComponent<ClientSkinLoader>().LoadSkin(dataFromServer.clientInitData.skinId, dataFromServer.clientInitData.matId);
                player.GetComponent<ClientNameLoader>().LoadName(dataFromServer.clientInitData.playerName);
                playersInitData.Add(dataFromServer.serverClientInitData.playerNum, new PlayerData() { playerTransform = player.transform, initData = dataFromServer });
                break;
            }
        }
    }

    public void SendPlayerInputs ()
    {
        if (ownPlayer.spaceMovement)
        {
            packetBuilder.SendPacket(new PlayerInputData(ownPlayer.spaceMovement._moveInput, ownPlayer.playerTransform.rotation));
        }
    }
}
