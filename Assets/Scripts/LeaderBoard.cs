
using UnityEngine;
using System.Linq;
using Photon.Pun;
using TMPro;
using System;
using Photon.Pun.UtilityScripts;

public class LeaderBoard : MonoBehaviour
{
     public GameObject playerHolder;

    [Header("Options")]
    [SerializeField] float refreshRate = 1f;

    [Header("UI")]
    [SerializeField] public GameObject[] slots;
    [Space]
    [SerializeField] public TextMeshProUGUI[] scoreTexts;
    [SerializeField] public TextMeshProUGUI[] nameTexts;
     public TextMeshProUGUI[] kdTexts;

    public static LeaderBoard instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InvokeRepeating(nameof(Refresh), 1f, refreshRate);
    }

    public void Refresh()
    {
       foreach(var slot in slots)
       {
            slot.SetActive(false);
       }

        // Linq Method
        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

        int i = 0;
        foreach(var player in sortedPlayerList)
        {
            slots[i].SetActive(true);

            if (player.NickName == "")
                player.NickName = "unnamed";

            nameTexts[i].text = player.NickName;
            scoreTexts[i].text = player.GetScore().ToString();

            if (player.CustomProperties["kills"] != null)
            {
                kdTexts[i].text = player.CustomProperties["kills"] + "/" + player.CustomProperties["deaths"];
            }
            else
            {
                kdTexts[i].text = "0/0";
            }

            i++;
        }
    }

    void Update()
    {
      /*  if (RoomManager.instance != null && RoomManager.instance.timeRemaining < 0)
        {
            playerHolder.SetActive(true);
        }
        else
        {
           // playerHolder.SetActive(Input.GetKey(KeyCode.Tab));
        }*/
    }

}
