using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{

    public PhotonView playerSetupView;

    [Header("Switching Anim")]
    public Animation _animation;
    public AnimationClip switchingClip;

    private int selectedWeapon = 0;

    void Start()
    {
        SelectedWeapon();
    }

   
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon += 1;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedWeapon <= 0)
            { 
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon -= 1;
            }
        }

        if(previousSelectedWeapon != selectedWeapon)
        {
            SelectedWeapon();
        }

        SwitchMobile();
    }

    public void SwitchMobile()
    {
        if (SimpleInput.GetButtonDown("Switch"))
        {
            selectedWeapon = (selectedWeapon + 1) % transform.childCount;
            SelectedWeapon();
        }

    }

    void SelectedWeapon()
    {
        playerSetupView.RPC("SetTPWeapon", RpcTarget.All, selectedWeapon);

        if(selectedWeapon >= transform.childCount)
        {
            selectedWeapon = transform.childCount - 1;
        }

        _animation.Stop();
        _animation.Play(switchingClip.name);

        int i = 0;

        foreach(Transform _weapon in transform)
        {
            if(i == selectedWeapon)
            {
                _weapon.gameObject.SetActive(true);
            }
            else
            {
                _weapon.gameObject.SetActive(false);
            }

            i++;
        }
    }
}
