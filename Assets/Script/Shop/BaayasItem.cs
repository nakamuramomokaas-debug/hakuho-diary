using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//仕組みはNagisItemとほぼ同じ
public class BaayasItem : MonoBehaviour
{
    [SerializeField] Item[] serializeShopItem;
    [SerializeField] NagisItem nagisItemScr;
    //[SerializeField] MoneyUI moneyUIScr;
    //[SerializeField] int[] firstNum;//ショップの場合凪の持ち物をみて初期値から計算すればいいのでいらない？

    private Dictionary<Item, int> shopItems;

    private void Awake()
    {
        //インスペクターで設定したアイテムを読み込む
        shopItems = new Dictionary<Item, int>();
        for(int i = 0;i < serializeShopItem.Length;i++)
        {
            shopItems.Add(serializeShopItem[i], 0);
            //Debug.Log("追加" + shopItem[i].GetName() + ":" + firstNum[i]);
        }
    }

    //凪の持ち物を参照して書き換える処理が必要
    public void UpdateItemQuantity()
    {

    }

    //ショップから特定のものだけ取り出す
    public Dictionary<Item, int> GetChoiceTypeItems(EnumDefinition.ItemType itemType)
    {
        var interiors = new Dictionary<Item, int>();
        //Debug.Log("アイテムの数：" + shopItems.Count);
        foreach(var item in shopItems)
        {
            //Debug.Log("アイテムのタイプ：" + item.Key.GetItemType());
            if(itemType == EnumDefinition.ItemType.AllItem)
            {
                interiors.Add(item.Key, item.Value);
            }
            else if(itemType == EnumDefinition.ItemType.InteriorItem && 
            (item.Key.GetItemType() == EnumDefinition.ItemType.WallItem ||
            item.Key.GetItemType() == EnumDefinition.ItemType.FloorItem ||
            item.Key.GetItemType() == EnumDefinition.ItemType.FurnitureItem))
            {
                interiors.Add(item.Key, item.Value);
            }
            else if(item.Key.GetItemType() == itemType)
            {
                interiors.Add(item.Key, item.Value);
            }
        }
        // Debug.Log("アイテムの数：" + interiors.Count);
        return interiors;
    }

    public void SetshopItemScr(ref Item shopItemScr, int itemId)
    {
        foreach(var item in shopItems.Keys)
        {
            if(item.GetItemId() == itemId)
            {
                item.CopyItemData(ref shopItemScr);
            }
        }
    }

    //ばあやの持っているアイテムの個数を取得
    public int GetItemNum(int itemId)
    {
        foreach(var item in shopItems.Keys)
        {
            if(item.GetItemId() == itemId)
            {
                return shopItems[item];
            }
        }
        return -1;
    }

    public Sprite GetshopItemSprite(int itemId)
    {
        foreach(var item in shopItems.Keys)
        {
            if(item.GetItemId() == itemId)
            {
                return item.GetSprite();
            }
        }
        return null;
    }

    public EnumDefinition.ItemType GetItemType(int itemId)
    {
        foreach(var item in shopItems.Keys)
        {
            if(item.GetItemId() == itemId)
            {
                return item.GetItemType();
            }
        }
        return 0;//とりあえず１番最初のやつ返す
    }

    public void PurchaseItem(int itemId)
    {
        foreach(var item in shopItems.Keys)
        {
            if(item.GetItemId() == itemId)
            {
                nagisItemScr.PayMoney(item.GetPrice());
                nagisItemScr.AddItem(item);
                //moneyUIScr.UpdateMoney();
                break;
            }
        }
    }
}
