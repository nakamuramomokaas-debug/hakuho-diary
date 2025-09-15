using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] NagisItem nagisItemScr;
    [SerializeField] TextMeshProUGUI moneyText;

    void Start()
    {
        moneyText.text = nagisItemScr.GetNowMoney().ToString();
    }

    public void UpdateMoney()
    {
        moneyText.text = nagisItemScr.GetNowMoney().ToString();
    }
}
