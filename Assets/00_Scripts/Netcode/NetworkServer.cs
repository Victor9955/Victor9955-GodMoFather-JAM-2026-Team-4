using ENet6;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServerPlayerData
{
    public int id;
    public int skin = 0;
    public string name = "";
    public PacketBuilder packetBuilder;
}

public class NetworkServer : MonoBehaviour
{
    private ENet6.Host enetHost = null;

    Dictionary<Peer, ServerPlayerData> players = new();

    private int playersNumber = 0;
    
    ushort seed = 0;

    [SerializeField] float timer = 0f;


    bool runTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!ENet6.Library.Initialize())
            throw new Exception("Failed to initialize ENet");

        seed = (ushort)UnityEngine.Random.Range(0, 10000);
        CreateServer("localhost");
    }

    public bool CreateServer(string addressString)
    {
        ENet6.Address address = Address.BuildAny(AddressType.IPv6);
        address.Port = 14769;

        Debug.Log("Creating server : " + address.GetIP());

        // On recréé l'host à la connexion pour l'avoir en IPv4 / IPv6 selon l'adresse
        if (enetHost != null)
            enetHost.Dispose();

        enetHost = new ENet6.Host();
        enetHost.Create(AddressType.Any, address, 10, 0);

        return true;
    }
    private int lastWholeSecond = -1;

    private void Update()
    {
        if(runTimer)
        {
            timer -= Time.deltaTime;

            int currentWholeSecond = Mathf.FloorToInt(timer);

            if (currentWholeSecond != lastWholeSecond)
            {
                lastWholeSecond = currentWholeSecond;

                foreach (var player in players.Values)
                {
                    player.packetBuilder.SendPacket(new Timer(timer));
                }
            }
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
                        ClientConnected(evt.Peer);
                        break;

                    case ENet6.EventType.Disconnect:
                        Debug.Log("Disconnect");
                        break;

                    case ENet6.EventType.Receive:
                        byte[] buffer = new byte[1024];
                        evt.Packet.CopyTo(buffer);
                        HandleMessage(evt.Peer, buffer);
                        break;

                    case ENet6.EventType.Timeout:
                        Debug.Log("Timeout");
                        break;
                }
            }
            while (enetHost.CheckEvents(out evt) > 0);
        }
    }

    public void ClientConnected(Peer peer)
    {
        ServerPlayerData playerData = new ServerPlayerData();
        playerData.packetBuilder = new PacketBuilder(peer, 0);
        playerData.id = playersNumber;
        playersNumber++;
        playerData.packetBuilder.SendPacket(new SendPlayerId((byte)playerData.id, seed));
        players.Add(peer, playerData);
    }

    private void OnApplicationQuit()
    {
        ENet6.Library.Deinitialize();
    }

    private void HandleMessage(Peer peer, byte[] buffer)
    {
        int offset = 0;
        Opcode opcode = (Opcode)Serialization.DeserializeU8(buffer, ref offset);
        switch (opcode)
        {
            case Opcode.SendName:
                {
                    SendName sendNamePacket = new SendName();
                    sendNamePacket.Deserialize(buffer, ref offset);
                    players[peer].name = sendNamePacket.name;
                    players[peer].skin = sendNamePacket.skin;
                    if (players.Count >= 2)
                    {
                        runTimer = true;
                        foreach (var player in players.Values)
                        {
                            foreach (var other in players.Values)
                            {
                                if (player.id != other.id)
                                {
                                    player.packetBuilder.SendPacket(new SendPlayerInit((byte)other.id, other.name, (ushort)other.skin));
                                }
                            }
                        }
                    }
                    break;
                }
            case Opcode.SendPlayerState:
                {
                    SendPlayerState playerStatePacket = new SendPlayerState();
                    playerStatePacket.Deserialize(buffer, ref offset);
                    foreach (var player in players.Values)
                    {
                        if(player.id != playerStatePacket.id)
                        {
                            player.packetBuilder.SendPacket(new SendPlayerState(playerStatePacket.id, playerStatePacket.pos, playerStatePacket.or, playerStatePacket.isRunning));
                        }
                    }
                    break;
                }
            case Opcode.SpawnTape:
                {
                    SpawnTape spawnTape = new SpawnTape();
                    spawnTape.Deserialize(buffer, ref offset);
                    foreach (var player in players.Values)
                    {
                        if (player.id != players[peer].id)
                        {
                            player.packetBuilder.SendPacket(spawnTape);
                        }
                    }
                    break;
                }
            case Opcode.Bar:
                {
                    WinObject bar = new WinObject();
                    bar.Deserialize(buffer, ref offset);
                    foreach (var player in players.Values) 
                    {
                        player.packetBuilder.SendPacket(bar);
                    }
                    break;
                }
            case Opcode.Attack:
                {
                    Attack attack = new Attack();
                    attack.Deserialize(buffer, ref offset);
                    foreach (var player in players.Values)
                    {
                        if(player.id != players[peer].id)
                        {
                            player.packetBuilder.SendPacket(attack);
                        }
                    }
                    break;
                }

        }
    }
}
