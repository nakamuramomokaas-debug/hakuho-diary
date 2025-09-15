using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyConfirm : MonoBehaviour
{
    [SerializeField] ShopManager shopManagerScr;
    [SerializeField] NagisItem nagisItemScr;
    [SerializeField] BaayasItem baayasItem;
    [SerializeField] SuccessBuy successPurchaseScr;
    [SerializeField] Button buyButton;
    [SerializeField] GameObject itemIconObj;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] TextMeshProUGUI nowQuantity;
    [SerializeField] TextMeshProUGUI canQuantity;
    [SerializeField] TextMeshProUGUI explanation;
    private int itemId;
    private Item confirmItem;

    public void SetBuyConfirmWindow(ShopTmpItem item)
    {
        SoundManager.instance.PlaySE(4);

        var itemScr = item.ContentItem;
        confirmItem = itemScr;
        this.gameObject.SetActive(true);

        itemId = itemScr.GetItemId();
        itemIconObj.GetComponent<Image>().sprite = itemScr.GetSprite();
        itemName.text = itemScr.GetName();
        price.text = itemScr.GetPrice().ToString();

        var nowNagiquantity = nagisItemScr.GetItemNum(itemScr.GetItemId());
        nowQuantity.text = nowNagiquantity.ToString();
        if(itemScr.GetQuantityLimit() == -1) canQuantity.text = "∞";
        else canQuantity.text = (itemScr.GetQuantityLimit() - nowNagiquantity).ToString();

        explanation.text = itemScr.GetExplanationShort();

        //凪のお金が足りない場合は購入ボタンを非活性に
        if(nagisItemScr.GetNowMoney() - itemScr.GetPrice() < 0) buyButton.interactable = false;
        else buyButton.interactable = true;
    }

    public void ReturnShop()//購入確認から戻るとき(このスクリプトが持っていると判別不可マネージャーにする)
    {
        SoundManager.instance.PlaySE(6);
        this.gameObject.SetActive(false);
    }

    //購入関数ーばあやと凪の持ち物にも影響する
    public void Purchase()
    {
        SoundManager.instance.PlaySE(7);
        baayasItem.PurchaseItem(itemId);
        this.gameObject.SetActive(false);
        successPurchaseScr.gameObject.SetActive(true);
        successPurchaseScr.ShowPurchaseItem(confirmItem);

        shopManagerScr.UpdateShop();//本当はマネージャーに通知するだけがいいけど....
    }
}
