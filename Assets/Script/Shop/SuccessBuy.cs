using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuccessBuy : MonoBehaviour
{
    [SerializeField] GameObject itemIconObj;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemExplanation;
    
    //購入確認から呼ばれる
    public void ShowPurchaseItem(Item itemScr)
    {
        itemIconObj.GetComponent<Image>().sprite = itemScr.GetSprite();
        itemName.text = itemScr.GetName();
        itemExplanation.text = itemScr.GetExplanation();
    }
    
    public void CloseWindow()
    {
        SoundManager.instance.PlaySE(6);
        this.gameObject.SetActive(false);
    }
}
