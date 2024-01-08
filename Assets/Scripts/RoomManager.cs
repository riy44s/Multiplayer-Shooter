using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    public GameObject Player;

    [Space]
    public Transform[] spawnPoints;

    [Space]
    public GameObject roomCam;

    [Space]
    public GameObject nameUI;
    public GameObject connectingUI;

    private string nickname = "unnamed";

    public string roomNameToJoin = "test";

    [HideInInspector]
    public int kills = 0;
    [HideInInspector]
    public int deaths = 0;

    public TextMeshProUGUI killsText;
    public TextMeshProUGUI deathsText;
    public GameObject active;

    public Transform[] spawnPointsBots;
    public GameObject Bots;


    public int timeRemaining = 120;
    public TextMeshProUGUI TimeText;

    public GameObject gameOver;
    public GameObject playerHolder;
    public GameObject playerLeavesOnlobby;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InvokeRepeating("Timer", 1, 1);
    }

    public void ChangeNickName(string _name)
    {
        nickname = _name;
    }

    public void JoinButtonPressed()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null);
        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("We're conncted and in a room!");

        roomCam.SetActive(false);

        SpawnPlayer();
        SpawnBot();
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer != null)
        {
         
            Text playerNameText = playerLeavesOnlobby.GetComponentInChildren<Text>();

            if (playerNameText != null)
            {
                playerNameText.text = otherPlayer.NickName + " has left.";
            }
          
            playerLeavesOnlobby.SetActive(true);
        }
        else
        {
            Debug.Log("A player without a name has left the room.");
        }
    }

    public void SpawnPlayer()
    {
        AudioManager.Instance.musicSource.Stop();
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject _player = PhotonNetwork.Instantiate(Player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IslocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;

        _player.GetComponent<PhotonView>().RPC("Setnickname",RpcTarget.AllBuffered,nickname);
        PhotonNetwork.LocalPlayer.NickName = nickname;
        active.SetActive(true);
        PhotonNetwork.LocalPlayer.AddScore(100);

        if (EnemyAI.instance != null)
        {
            EnemyAI.instance.SetEnemyTarget(_player.transform);
        }
    }

    public void SetHashes()
    {
        try
        {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;

            hash["kills"] = kills;
            hash["deaths"] = deaths;

            if (active.activeInHierarchy)
            {
                killsText.text = "Kills: " + kills.ToString();
                deathsText.text = "Deaths: " + deaths.ToString();
            }
           

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        catch
        {

        }
    }

    public void SpawnBot()
    {
        for (int i = 0; i < spawnPointsBots.Length; i++)
        {
            Transform spawnPointBot = spawnPointsBots[i];
            GameObject _Bots = PhotonNetwork.Instantiate(Bots.name, spawnPointBot.position, Quaternion.identity);

            if (EnemyAI.instance != null)
            {
                EnemyAI.instance.enemyTarget = _Bots.transform;
                EnemyAI.instance.SetNextWaypoint();
                EnemyAI.instance.SetPrevousWaypoint();
                EnemyAI.instance.enabled = true;
            }

            PhotonNetwork.LocalPlayer.AddScore(100);
        }
    }

    public void SpawnBots()
    {
        Transform spawnPointBot = spawnPointsBots[Random.Range(0, spawnPointsBots.Length)];

        GameObject _Bots = PhotonNetwork.Instantiate(Bots.name, spawnPointBot.position, Quaternion.identity);

        EnemyAI.instance.enemyTarget = _Bots.transform;

        EnemyAI.instance.SetNextWaypoint();
        EnemyAI.instance.SetPrevousWaypoint();
        EnemyAI.instance.enabled = true;

        PhotonNetwork.LocalPlayer.AddScore(100);
    }

    public void Timer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining--;
            int minute = timeRemaining / 60;
            int seconds = timeRemaining - minute * 60;
            TimeText.text = "Match Remaining : " + minute + " : " + seconds + "";
        }
        else
        {
            GameOver();
        } 

    }

    void GameOver()
    {
        Time.timeScale = 0;
        gameOver.SetActive(true);
        playerHolder.SetActive(true);
    }

   public void Quit()
   {
        Application.Quit();
   }

  
}
