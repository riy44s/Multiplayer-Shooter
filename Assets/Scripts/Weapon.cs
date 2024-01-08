using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private int damage;
    [SerializeField] private float fireRate;
    private float nextFire;

    [Header("VFX")]
    [SerializeField] private GameObject hitVFX;

    [Header("Ammo")]
    [SerializeField] private int mag = 5;
    [SerializeField] private int ammo = 10;
    [SerializeField] private int magAmmo = 30;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI magText;
    [SerializeField] private TextMeshProUGUI ammoText;

    [Header("Reload")]
    [SerializeField] private Animation anim;
    [SerializeField] private AnimationClip reload;

    [Header("Recoil Settings")]
    [Range(0, 2)]
    [SerializeField] private float recoverPercent = 0.7f;
    [Space]
    [SerializeField] private float recoilUp = 1f;
    [SerializeField] private float recoilBack = 0f;

    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;
    private float recoilLength;
    private float recoverLength;
    private bool recoiling;
    private bool recovering;

    public bool isShooting;
    public bool isReloading;

    void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;

        originalPosition = transform.localPosition;

        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;
    }

    void Update()
    {
        if(nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        if(isShooting && SimpleInput.GetButton("Fire") || Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0 && anim.isPlaying == false)
        {
            nextFire = 1 / fireRate;

            ammo--;

            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo;

            Fire();
        }

        /*else
        {
            if (Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0 && anim.isPlaying == false)
            {
                nextFire = 1 / fireRate;

                ammo--;

                magText.text = mag.ToString();
                ammoText.text = ammo + "/" + magAmmo;

                Fire();
            }
        }*/

        if (isReloading && SimpleInput.GetButton("Reload") || Input.GetKeyDown(KeyCode.R) && mag > 0)
        {
            Reload();
            
        }

        if (recoiling)
        {
            Recoil();
        }

        if (recovering)
        {
            Recoveing();
        }
    }

    public void ShootButton()
    {
        isShooting = SimpleInput.GetButton("Fire");
        Debug.Log("Button Pressed");
    }

    public void ReloadButton()
    {
        isReloading = SimpleInput.GetButton("Reload");
    }

    void Reload()
    {
        anim.Play(reload.name);

        if (mag > 0 && magAmmo > 0)
        {
            mag--;         
            magAmmo -= 10;
            ammo = 10;
        }

        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }

    void Fire()
    {
        recoiling = true;
        recovering = false;

        Ray ray = new Ray(camera.transform.position,camera.transform.forward);

        RaycastHit hit;

       // PhotonNetwork.LocalPlayer.AddScore(0);

        if (Physics.Raycast(ray.origin, ray.direction,out hit,100f))
        {
            PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
            AudioManager.Instance.PlaySFX("Shoot");

            if (hit.transform.gameObject.GetComponent<Health>())
            {
                PhotonNetwork.LocalPlayer.AddScore(damage);

                if(damage >= hit.transform.gameObject.GetComponent<Health>().health)
                {

                    RoomManager.instance.kills++;
                    RoomManager.instance.SetHashes();

                   // PhotonNetwork.LocalPlayer.AddScore(100);
                }

                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }

        if (hit.transform.gameObject.CompareTag("Bot"))
        {
            Destroy(hit.transform.gameObject);
            PhotonNetwork.LocalPlayer.AddScore(100);
            RoomManager.instance.kills++;
            RoomManager.instance.SetHashes();
            RoomManager.instance.SpawnBots();
        }

    }

    void Recoil()
    {
        Vector3 finalPosition = new Vector3(originalPosition.x,originalPosition.y + recoilUp,originalPosition.z - recoilBack);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity,recoilLength);

        if(transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = true;
        }
    }

    void Recoveing()
    {
        Vector3 finalPosition = originalPosition;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = false;
        }
    }

  
}
