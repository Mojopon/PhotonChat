using UnityEngine;
using System.Collections;
using UniRx;
using Photon;
using System;

public enum MatchMakingState
{
    BeforeJoinLobby,
    JoinedLobby,
    JoinedRoom,
}

public class Matchmaker : PunBehaviour
{


    private static ISubject<MatchMakingState> _stateSubject = null;
    public static IObservable<MatchMakingState> StateObservable
    {
        get
        {
            if (_stateSubject == null) _stateSubject = new BehaviorSubject<MatchMakingState>(MatchMakingState.BeforeJoinLobby);
            return _stateSubject.AsObservable();
        }
    }

    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public override void OnJoinedLobby()
    {
        _stateSubject.OnNext(MatchMakingState.JoinedLobby);

        if (PhotonNetwork.GetRoomList().Length == 0)
        {
            PhotonNetwork.CreateRoom(null);
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinedRoom()
    {
        _stateSubject.OnNext(MatchMakingState.JoinedRoom);
    }
}
