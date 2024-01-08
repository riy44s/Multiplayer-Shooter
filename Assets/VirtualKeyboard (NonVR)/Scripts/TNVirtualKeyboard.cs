using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TNVirtualKeyboard : MonoBehaviour
{
    public static TNVirtualKeyboard instance;

    public List<TMP_InputField> targetTextFields = new List<TMP_InputField>();
    private string words = "";

    public GameObject vkCanvas;

    void Start()
    {
        instance = this;
        HideVirtualKeyboard();
    }

    void Update()
    {
        // You can add any additional logic here if needed
    }

    public void KeyPress(string k)
    {
        words += k;
        UpdateTargetTextFields();
    }

    public void Del()
    {
        if (words.Length > 0)
        {
            words = words.Remove(words.Length - 1, 1);
            UpdateTargetTextFields();
        }
    }

    private void UpdateTargetTextFields()
    {
        foreach (var targetText in targetTextFields)
        {
            if (targetText != null)
            {
                targetText.text = words;
            }
        }
    }

    public void ShowVirtualKeyboard()
    {
        vkCanvas.SetActive(true);
    }

    public void HideVirtualKeyboard()
    {
        vkCanvas.SetActive(false);
    }

    public void AddTargetTextField(TMP_InputField targetText)
    {
        if (targetText != null && !targetTextFields.Contains(targetText))
        {
            targetTextFields.Add(targetText);
        }
    }

    public void RemoveTargetTextField(TMP_InputField targetText)
    {
        if (targetText != null && targetTextFields.Contains(targetText))
        {
            targetTextFields.Remove(targetText);
        }
    }
}
