using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public bool isLocalPlayer;

    [Header("UI")]

    public RectTransform healthBar;
    private float originalHealthBarSize;

    private void Start()
    {
        originalHealthBarSize = healthBar.sizeDelta.x;
    }

    [PunRPC]
    public void TakeDamage(int _damage)
    {
        health -= _damage;
        healthBar.sizeDelta = new Vector2(originalHealthBarSize * health / 100f, healthBar.sizeDelta.y);

        if(health <= 0)
        {
            if(isLocalPlayer)
            {
                RoomManager.instance.SpawnPlayer();
                RoomManager.instance.deaths++;
                RoomManager.instance.SetHashes();
            }              

            Destroy(gameObject);
        }
    }

}
