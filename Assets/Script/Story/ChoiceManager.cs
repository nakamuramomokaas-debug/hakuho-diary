using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] GameObject[] choices;
    [SerializeField] ChatManager chatManagerSc;

    public void SetChoises(string[] choicesText)
    {
        for(int i = 0; i < choices.Length; i++)
        {
            if(i < choicesText.Length)
            {
                choices[i].SetActive(true);
                var text = choices[i].GetComponentInChildren<TextMeshProUGUI>();
                text.text = choicesText[i];
            }
            else
            {
                choices[i].SetActive(false);
            }
        }
    }

    public void PushButton(int choice)
    {
        chatManagerSc.SetChoice(choice);
        foreach (var i in choices)
        {
            i.SetActive(false);
        }
    }
    //2つの選択肢と３つの選択肢によって幅ちゃんと調整したい
}
