using System.Collections;
using UnityEngine;

public class Scope : MonoBehaviour
{
    private bool isScoped = false;
    public Camera fpsCam;
    public bool mobile = false;

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            isScoped = !isScoped;

            if (isScoped)
                StartCoroutine(OnScoped());
            else
                OnUnScoped();

            if (mobile)
            {
                Aim();
            }
           
        }
    }

    public void Aim()
    {

        if (SimpleInput.GetButton("Aim"))
        {
            isScoped = !isScoped;

            if (isScoped)
                StartCoroutine(OnScoped());
            else
                OnUnScoped();
        }
    }

    void OnUnScoped()
    {
        fpsCam.fieldOfView = 60;
    }

    public IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(0.15f);
        fpsCam.fieldOfView = 30;
    }
}
