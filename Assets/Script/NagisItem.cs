using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NagisItem : MonoBehaviour
{
    [SerializeField] int money = 100000;
    [SerializeField] Item[] firstItem;
    [SerializeField] int[] firstNum;//firstItemと数が同じになるように
    [SerializeField] RectTransform nagiPosition;
    [SerializeField] MoneyUI moneyUIScr;

    private Dictionary<Item, int> items;

    private void Start()
    {
        items = new Dictionary<Item, int>();
        for(int i = 0;i < firstItem.Length;i++)
        {
            items.Add(firstItem[i], firstNum[i]);
            //Debug.Log("追加" + firstItem[i].GetName() + ":" + firstNum[i]);
        }
    }

    public Dictionary<Item, int> GetInteriors()
    {
        var interiors = new Dictionary<Item, int>();
        //Debug.Log("アイテムの数：" + items.Count);
        foreach(var item in items)
        {
            //Debug.Log("アイテムのタイプ：" + item.Key.GetItemType());
            if(item.Key.GetItemType() == EnumDefinition.ItemType.WallItem ||
                item.Key.GetItemType() == EnumDefinition.ItemType.FloorItem ||
                item.Key.GetItemType() == EnumDefinition.ItemType.FurnitureItem)
            {
                interiors.Add(item.Key, item.Value);
            }
        }
        // Debug.Log("アイテムの数：" + interiors.Count);
        return interiors;
    }

    public Dictionary<Item, int> GetConsmptionItems()
    {
        var interiors = new Dictionary<Item, int>();
        //Debug.Log("アイテムの数：" + items.Count);
        foreach(var item in items)
        {
            //Debug.Log("アイテムのタイプ：" + item.Key.GetItemType());
            if(item.Key.GetItemType() == EnumDefinition.ItemType.ConsumptionItem)
            {
                interiors.Add(item.Key, item.Value);
            }
        }
        // Debug.Log("アイテムの数：" + interiors.Count);
        return interiors;
    }

    public void SetItemScr(ref Item itemScr, int itemId)
    {
        foreach(var item in items.Keys)
        {
            if(item.GetItemId() == itemId)
            {
                item.CopyItemData(ref itemScr);
            }
        }
    }

    public Sprite GetItemSprite(int itemId)
    {
        foreach(var item in items.Keys)
        {
            if(item.GetItemId() == itemId)
            {
                return item.GetSprite();
            }
        }
        return null;
    }

    //凪の持っているアイテムの個数を取得
    public int GetItemNum(int itemId)
    {
        foreach(var item in items.Keys)
        {
            if(item.GetItemId() == itemId)
            {
                return items[item];
            }
        }
        return 0;
    }


    public EnumDefinition.ItemType GetItemType(int itemId)
    {
        foreach(var item in items.Keys)
        {
            if(item.GetItemId() == itemId)
            {
                return item.GetItemType();
            }
        }
        return 0;//とりあえず１番最初のやつ返す
    }

    public void AddItem(Item newItem)
    {
        var isExist = false;
        foreach(var item in items.Keys)
        {
            if(item.GetItemId() == newItem.GetItemId())//アイテムの個数を追加
            {
                isExist = true;
                items[item] += 1;
                break;
            }
        }
        if(!isExist)//新しいアイテムを追加
        {
            items.Add(newItem, 1);
        }
    }

    public void UseItem(int useItemId)//アイテムを使って消費する（効果があるものはここで反映させる）
    {
        nagiPosition.position = new Vector2(0f, 0f);

        foreach(var item in items.Keys)
        {
            if(item.GetItemId() == useItemId)//アイテムの個数を追加
            {
                items[item] -= 1;
                break;
            }
        }
    }

    public int GetNowMoney(){ return money; }
    public void PayMoney(int pay)
    {
        money -= pay; 
        moneyUIScr.UpdateMoney();
    }
    public void AddMoney(int add)
    {
        money += add; 
        moneyUIScr.UpdateMoney();
    }
}
