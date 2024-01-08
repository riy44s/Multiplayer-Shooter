using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sample : MonoBehaviourPunCallbacks
{

    public InputField create;
    public InputField joined;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connecting");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("player joined");
    }

    public void createroom()
    {
        PhotonNetwork.CreateRoom(create.text);

        PhotonNetwork.NickName = create.text;

        Debug.Log("Creating a Room" +create);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joined.text);

        Debug.Log("Joined Room" +joined);
    }


}
