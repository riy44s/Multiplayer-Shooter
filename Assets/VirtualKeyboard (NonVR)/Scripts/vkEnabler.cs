using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class vkEnabler : MonoBehaviour
{
    void Start()
    {
        // Add this TMP_InputField to the virtual keyboard's targetTextFields
        TNVirtualKeyboard.instance.AddTargetTextField(gameObject.GetComponent<TMP_InputField>());
    }

    void Update()
    {
        // You can add any additional logic here if needed
    }

    public void ShowVirtualKeyboard()
    {
        TNVirtualKeyboard.instance.ShowVirtualKeyboard();
    }
}
