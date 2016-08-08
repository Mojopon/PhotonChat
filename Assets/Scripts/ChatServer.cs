using UnityEngine;
using System.Collections;
using UniRx;
using System.Collections.Generic;
using System.Linq;

public class ChatServer : Photon.MonoBehaviour
{
    private ReactiveProperty<bool> _chatIsEnabledProperty = new ReactiveProperty<bool>();
    public IObservable<bool> ChatIsEnabledReactiveProperty { get { return _chatIsEnabledProperty.ToReadOnlyReactiveProperty(); } }

    private ISubject<string> _chatStreamSubject = new Subject<string>();
    public IObservable<string> ChatMessageObservable { get { return _chatStreamSubject.AsObservable(); } }

    private ReactiveCollection<string> _chatMessageLog = new ReactiveCollection<string>();
    public string[] ChatMessageLog { get { return _chatMessageLog.ToArray(); } }

    private MatchMakingState _currentState;
    void Start()
    {
        _chatIsEnabledProperty.Value = false;

        _chatMessageLog.ObserveAdd()
                       .Subscribe(x => _chatStreamSubject.OnNext(x.Value))
                       .AddTo(gameObject);

        Matchmaker.StateObservable
                       .Subscribe(x =>
                       {
                         _currentState = x;
                         OnStateChanged();
                         })
                       .AddTo(gameObject);
    }

    void OnStateChanged()
    {
        switch (_currentState)
        {
            case MatchMakingState.BeforeJoinLobby:
                {
                    break;
                }
            case MatchMakingState.JoinedLobby:
                {
                    break;
                }
            case MatchMakingState.JoinedRoom:
                {
                    _chatIsEnabledProperty.Value = true;
                    break;
                }
        }
    }

    public bool SendChat(string text)
    {
        var target = PhotonTargets.All;

        if(!string.IsNullOrEmpty(text))
        {
            photonView.RPC("SendChatMessage", target, text);
            return true;
        }

        return false;
    }

    [PunRPC]
    void SendChatMessage(string text, PhotonMessageInfo info)
    {
        AddMessage(text);
    }

    void AddMessage(string text)
    {
        _chatMessageLog.Add(text);
        if (_chatMessageLog.Count > 15)
            _chatMessageLog.RemoveAt(0);
    }
}
