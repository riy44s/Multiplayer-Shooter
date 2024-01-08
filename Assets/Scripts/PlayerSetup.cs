using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;

public class PlayerSetup : MonoBehaviour
{
    public Movement movement;

    public GameObject camera;

    public string nickname;
    public TextMeshPro nicknameText;

    public Transform TPweaponHolder;

    public GameObject PlayerCharacter;
    public PhotonView photonView;

    private void Start()
    {
        if (photonView.IsMine)
        { 
            PlayerCharacter.SetActive(false);
        }
    }

    public void IslocalPlayer()
    {
        TPweaponHolder.gameObject.SetActive(false);

         movement.enabled = true;

         camera.SetActive(true);
    }

    [PunRPC]
    public void SetTPWeapon(int _weaponIndex)
    {
        foreach(Transform _weapon in TPweaponHolder)
        {
            _weapon.gameObject.SetActive(false);
        }

        TPweaponHolder.GetChild(_weaponIndex).gameObject.SetActive(true);
    }


    [PunRPC]
    public void Setnickname(string _name)
    {
        nickname = _name;
        nicknameText.text = nickname;
    }

}
