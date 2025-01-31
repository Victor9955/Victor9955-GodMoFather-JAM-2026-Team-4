public enum Opcode : byte
{
    OnClientConnect,
    OnClientConnectResponse,
    OnOtherClientConnect,
    PlayerInputsData,
    ClientShoot,
    FromServerPlayerPosition,
    FromServerHealthUpdate,
    LeaderBoardUpdate,
    ClientDead,
    ClientRespawn
}
