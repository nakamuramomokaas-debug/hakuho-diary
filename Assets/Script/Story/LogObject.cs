using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LogObject : MonoBehaviour
{
    //チャットログ１個が持っている情報
    [SerializeField] Image charaIcon;
    [SerializeField] TextMeshProUGUI nameText; 
    [SerializeField] TextMeshProUGUI serifText;

    public void SetLog(string name, string serif, Sprite icon)
    {
        charaIcon.sprite = icon;
        nameText.text = name;
        serifText.text = serif;
    }
}
