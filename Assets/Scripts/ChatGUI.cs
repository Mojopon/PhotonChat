using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Collections.Generic;

public class ChatGUI : MonoBehaviour
{
    [SerializeField]
    private bool debug = false;

    [SerializeField]
    private ChatServer _ChatServer;
    void Start()
    {
        _onGuiAction = RenderStates;

        _ChatServer.ChatIsEnabledReactiveProperty
                   .Subscribe(flag => EnableChatGUI(flag))
                   .AddTo(gameObject);

        _ChatServer.ChatMessageObservable
                   .Where(x => _chatIsEnabled)
                   .Subscribe(x => 
                   {
                       messages.Add(x);
                       if (messages.Count > 15)
                           messages.RemoveAt(0);
                   })
                   .AddTo(gameObject);
    }

    private Action _onGuiAction = () => { };
    void OnGUI()
    {
        _onGuiAction();
    }

    private bool _chatIsEnabled;
    void EnableChatGUI(bool flag)
    {
        _chatIsEnabled = flag;

        if (flag)
            _onGuiAction = RenderChatInterface;
        else
            _onGuiAction = RenderStates;
    }

    public List<string> messages = new List<string>();

    private int chatHeight = 140;
    private Vector2 scrollPos = Vector2.zero;
    private string chatInput = "";
    private float lastUnfocusTime = 0;

    void RenderStates()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public static string MessageToDisplay = "Result Displayed here";
    void RenderChatInterface()
    {
        if(debug)
            GUILayout.Label(MessageToDisplay);

        GUILayout.BeginArea(new Rect(0, Screen.height - chatHeight, Screen.width, chatHeight));

        //Show scroll list of chat messages
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUI.color = Color.cyan;
        
        for (int i = messages.Count - 1; i >= 0; i--)
        {
            GUILayout.Label(messages[i]);
        }
        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        GUI.SetNextControlName("ChatField");
        chatInput = GUILayout.TextField(chatInput, GUILayout.MinWidth(300));
        if (Event.current.type == EventType.keyDown && Event.current.character == '\n')
        {
            if (GUI.GetNameOfFocusedControl() == "ChatField")
            {
                _ChatServer.SendChat(chatInput);
                chatInput = "";
                lastUnfocusTime = Time.time;
                GUI.FocusControl("");
                GUI.UnfocusWindow();
            }
            else
            {
                if (lastUnfocusTime < Time.time - 0.1f)
                {
                    GUI.FocusControl("ChatField");
                }
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }
}
