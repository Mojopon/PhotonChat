using UnityEngine;
using System.Collections;
using UniRx;

public class LobbyGUI : MonoBehaviour
{
    private bool onLobby = false;

    void Start()
    {
        Matchmaker.StateObservable
                  .Select(x => x == MatchMakingState.JoinedLobby)
                  .Subscribe(x => onLobby = x);
    }

    private Vector2 scrollPos = Vector2.zero;
    void OnGUI ()
    {
        if (!PhotonNetwork.connected || !onLobby) return;

        bool norooms = PhotonNetwork.GetRoomList().Length == 0 ? true : false;

        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

        //Player name
        GUILayout.BeginHorizontal();

        if (norooms)
        {
            GUILayout.Label("No Rooms available.");
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(30);

        if (norooms)
        {
            if (GUILayout.Button("Create Room"))
            {
                GUILayout.BeginHorizontal();

                PhotonNetwork.CreateRoom("chat room", new RoomOptions() { MaxPlayers = 10 }, TypedLobby.Default);

                GUILayout.EndHorizontal();
            }
        }
        else
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach (RoomInfo game in PhotonNetwork.GetRoomList())
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(game.name + " " + game.playerCount + "/" + game.maxPlayers);
                if (GUILayout.Button("JOIN"))
                {
                    PhotonNetwork.JoinRoom(game.name);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }


        GUILayout.EndArea();

    }
}
