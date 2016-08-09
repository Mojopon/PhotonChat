using UnityEngine;
using System.Collections;
using UniRx;
using Photon;
using System;

public enum MatchMakingState
{
    BeforeJoin,
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
            if (_stateSubject == null) _stateSubject = new BehaviorSubject<MatchMakingState>(MatchMakingState.BeforeJoin);
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
        
        /*
        if (PhotonNetwork.GetRoomList().Length == 0)
        {
            ChatGUI.MessageToDisplay = "Created a room";
            PhotonNetwork.CreateRoom("chat room", new RoomOptions() { MaxPlayers = 10 }, TypedLobby.Default);
        }
        else
        {
            ChatGUI.MessageToDisplay = "Joined a room";
            PhotonNetwork.JoinRandomRoom();
        }
        */
    }

    public override void OnJoinedRoom()
    {
        _stateSubject.OnNext(MatchMakingState.JoinedRoom);
    }
}