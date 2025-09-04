using Cinemachine;
using Coffee.UIEffects;
using DG.Tweening;
using ENet6;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientPlayerData
{
    public int id;
    public string name;
    public int skin;
    public LerpToPos lerpToPos;
    public RotateFace rotateFace;
    public FranckoAnimation animation;
    public Transform transform;
    public Transform transformOr;
}

public class NetworkClient : MonoBehaviour
{
    public static NetworkClient instance;

    private ENet6.Host enetHost = null;
    private ENet6.Peer? serverPeer = null;

    public PacketBuilder packetBuilder = null;
    public int playerId;
    public Action ReicevedId;

    [SerializeField] ClientGlobalInfo clientInfo;
    [SerializeField] GameObject otherPlayerPrefab;
    [SerializeField] Tape tapePrefab;
    [SerializeField] TextMeshProUGUI TMPTimer;
    [SerializeField] int finalSecne;
    [SerializeField] AutoRunMovement autoRun;
    [SerializeField] PlayerMovement normal;
    public float stuntTimer = 4;
    [SerializeField] UIEffect stuntVisual;
    float playerOneScore;
    float playerTwoScore;

    Dictionary<int, ClientPlayerData> localPlayers = new();
    Dictionary<int, Tape> onlineTapes = new();

    public ushort tapeNum = 0;
    float timer = 0f;

    [SerializeField] List<Transform> allClues = new();

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

    void Awake()
    {
        if (!ENet6.Library.Initialize())
            throw new Exception("Failed to initialize ENet");

        if (Connect(clientInfo.ip))
        {
            packetBuilder = new PacketBuilder(serverPeer.Value,0);
            packetBuilder.SendPacket(new SendName(clientInfo.playerName,(ushort)clientInfo.skin));
        }

        instance = this;
    }

    private void OnApplicationQuit()
    {
        packetBuilder.peer.Disconnect(0);
        ENet6.Library.Deinitialize();
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
        Opcode opcode = (Opcode)Serialization.DeserializeU8(buffer, ref offset);
        switch (opcode)
        {
            case Opcode.SendPlayerId:
                {
                    SendPlayerId sendPlayerId = new SendPlayerId();
                    sendPlayerId.Deserialize(buffer, ref offset);
                    playerId = sendPlayerId.id;
                    UnityEngine.Random.InitState(sendPlayerId.seed);
                    foreach (Transform clue in allClues)
                    {
                        clue.GetChild(UnityEngine.Random.Range(0,clue.childCount)).gameObject.SetActive(true);
                    }
                    ReicevedId?.Invoke();
                    break;
                }
            case Opcode.SendPlayerState:
                {
                    SendPlayerState sendPlayerState = new SendPlayerState();
                    sendPlayerState.Deserialize(buffer, ref offset);
                    if(localPlayers.TryGetValue(sendPlayerState.id, out ClientPlayerData data))
                    {
                        data.lerpToPos.pos = sendPlayerState.pos;
                        data.transform.eulerAngles = new Vector3(0, sendPlayerState.or.eulerAngles.y, 0);
                        data.rotateFace.Dir = sendPlayerState.or;
                        data.animation.isRuning = sendPlayerState.isRunning;
                    }
                    break;
                }
            case Opcode.SendPlayerInit:
                {
                    SendPlayerInit sendPlayerInit = new SendPlayerInit();
                    sendPlayerInit.Deserialize(buffer, ref offset);
                    GameObject newPlayer = Instantiate(otherPlayerPrefab);
                    ClientPlayerData newPlayerData = new ClientPlayerData();
                    newPlayerData.id = sendPlayerInit.id;
                    newPlayerData.name = sendPlayerInit.name;
                    newPlayerData.transform = newPlayer.transform;
                    newPlayerData.lerpToPos = newPlayer.GetComponent<LerpToPos>();
                    newPlayerData.rotateFace = newPlayer.GetComponentInChildren<RotateFace>();
                    newPlayerData.animation = newPlayer.GetComponentInChildren<FranckoAnimation>();
                    newPlayerData.animation.face.material.mainTexture = clientInfo.skins[sendPlayerInit.skin];

                    localPlayers.Add(newPlayerData.id, newPlayerData);
                    break;
                }
            case Opcode.SpawnTape:
                {
                    SpawnTape spawnTapeServer = new SpawnTape();
                    spawnTapeServer.Deserialize(buffer, ref offset);
                    if(spawnTapeServer.doModify == 0)
                    {
                        Tape cash = Instantiate(tapePrefab, spawnTapeServer.pos, Quaternion.identity);
                        onlineTapes.Add(spawnTapeServer.tapeId, cash);
                        tapeNum = spawnTapeServer.tapeId;
                    }
                    else if(spawnTapeServer.doModify == 1)
                    {
                        onlineTapes[spawnTapeServer.tapeId].AddTapeAtPosOrIsToLong(spawnTapeServer.pos);
                    }
                    else if (spawnTapeServer.doModify == 2)
                    {
                        onlineTapes[spawnTapeServer.tapeId].Close(true, spawnTapeServer.playerId);
                    }
                    break;
                }
            case Opcode.Timer:
                {
                    Timer timerServer = new Timer();
                    timerServer.Deserialize(buffer, ref offset);
                    int totalSeconds = Mathf.Max(0, Mathf.FloorToInt(timerServer.timer));
                    int minutes = totalSeconds / 60;
                    int seconds = totalSeconds % 60;
                    TMPTimer.text = $"{minutes:00}:{seconds:00}";
                    timer = timerServer.timer;
                    CheckForTimeWin();
                    break;
                }
            case Opcode.Bar:
                {
                    WinObject win = new WinObject();
                    win.Deserialize(buffer, ref offset);
                    if(win.id == 0)
                    {
                        clientInfo.playerOneScore++;
                    }
                    else
                    {
                        clientInfo.playerTwoScore++;
                    }
                    break;
                }
            case Opcode.Attack:
                {
                    Attack attack = new Attack();
                    attack.Deserialize(buffer, ref offset);
                    StartCoroutine(Stunt());
                    break;
                }
        }
    }

    void CheckForWin()
    {
        if(Mathf.FloorToInt(playerOneScore + playerTwoScore) == 1)
        {
            FinalScene();
        }
    }

    void CheckForTimeWin()
    {
        if(Mathf.FloorToInt(timer) == 0)
        {
            FinalScene();
        }
    }

    void FinalScene()
    {
        SceneManager.LoadScene(finalSecne);
    }

    IEnumerator Stunt()
    {
        bool autoRunActive = autoRun.enabled;
        bool normalActive = normal.enabled;
        autoRun.enabled = false;
        normal.enabled = false;
        Camera.main.GetComponent<FPSCamera>().enabled = false;
        DOTween.To(() => stuntVisual.transitionRate, x => stuntVisual.transitionRate = x, 0f, 0.4f);
        yield return new WaitForSeconds(stuntTimer);
        DOTween.To(() => stuntVisual.transitionRate, x => stuntVisual.transitionRate = x, 1f, 0.4f);
        Camera.main.GetComponent<FPSCamera>().enabled = true;
        autoRun.enabled = autoRunActive;
        normal.enabled = normalActive;
    }
}