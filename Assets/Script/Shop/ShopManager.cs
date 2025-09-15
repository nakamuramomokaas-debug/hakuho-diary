using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ShopManager（仕組みはRoomChabgeManagerとほぼ同じ）
public class ShopManager : MonoBehaviour
{
    [SerializeField] NagisItem nagisItemScr;
    [SerializeField] BaayasItem baayasItemScr;
    [SerializeField] BuyViewContents furnitureBuyViewScr;
    [SerializeField] BuyViewContents consumeBuyViewScr;
    [SerializeField] SellViewContents sellViewScr;
    [SerializeField] GameObject InteriorViewButton;
    [SerializeField] GameObject foodViewButton;

    Dictionary<Item, int> furnitureDict = new Dictionary<Item, int>();
    Dictionary<Item, int> consumeDict = new Dictionary<Item, int>();
    Dictionary<Item, int> sellDict = new Dictionary<Item, int>();

    void Start()
    {
        furnitureBuyViewScr.ItemType = EnumDefinition.ItemType.InteriorItem;
        consumeBuyViewScr.ItemType = EnumDefinition.ItemType.ConsumptionItem;
        sellViewScr.ItemType = EnumDefinition.ItemType.AllItem;

        var tmpbaayaDict = baayasItemScr.GetChoiceTypeItems(EnumDefinition.ItemType.AllItem);
        var tmpnagiDict = nagisItemScr.GetInteriors();
        foreach(var item in tmpbaayaDict)
        {
            //Debug.Log(item.Key.GetName() + "+" + item.Value);
           if(item.Key.GetItemType() == EnumDefinition.ItemType.WallItem ||
                item.Key.GetItemType() == EnumDefinition.ItemType.FloorItem ||
                item.Key.GetItemType() == EnumDefinition.ItemType.FurnitureItem)
            {
                furnitureDict.Add(item.Key, item.Value);
            }
            else if(item.Key.GetItemType() == EnumDefinition.ItemType.ConsumptionItem) consumeDict.Add(item.Key, item.Value);
        }
        foreach(var item in tmpnagiDict)
        {
            sellDict.Add(item.Key, item.Value);
        }
        SetShopList();//後
        consumeBuyViewScr.gameObject.SetActive(false);
        sellViewScr.gameObject.SetActive(false);
    }

    public void TabChange(int type)
    {//本当はマジックナンバーよくないけど....
        SoundManager.instance.PlaySE(4);
        furnitureBuyViewScr.gameObject.SetActive(false);
        consumeBuyViewScr.gameObject.SetActive(false);
        sellViewScr.gameObject.SetActive(false);

        Image interiorImg = InteriorViewButton.GetComponent<Image>();
        Image foodImg = foodViewButton.GetComponent<Image>();

        interiorImg.color = new Color32 (255, 255, 255, 255);
        foodImg.color = new Color32 (255, 255, 255, 255);

        if(type == 0) 
        {
            interiorImg.color = new Color32 (155, 155, 155, 255);
            furnitureBuyViewScr.gameObject.SetActive(true);
        }
        else if(type == 1)
        {
            foodImg.color = new Color32 (155, 155, 155, 255);
            consumeBuyViewScr.gameObject.SetActive(true);
        } 
        else if(type == 2) sellViewScr.gameObject.SetActive(true);
    }

    public void ExitShop()
    {
        SoundManager.instance.PlaySE(6);
        this.gameObject.SetActive(false);
    }

    void SetShopList()
    {
        Image interiorImg = InteriorViewButton.GetComponent<Image>();
        Image foodImg = foodViewButton.GetComponent<Image>();
        interiorImg.color = new Color32 (155, 155, 155, 255);
        foodImg.color = new Color32 (255, 255, 255, 255);

        furnitureBuyViewScr.gameObject.SetActive(true);
        consumeBuyViewScr.gameObject.SetActive(true);
        sellViewScr.gameObject.SetActive(true);

        var furnitureKeys = new Item[furnitureDict.Keys.Count];
        var furnitureValues = new int[furnitureDict.Values.Count];
        furnitureDict.Keys.CopyTo(furnitureKeys, 0);
        furnitureDict.Values.CopyTo(furnitureValues, 0);
        furnitureBuyViewScr.SetContents(furnitureKeys, furnitureValues);

        var consumeKeys = new Item[consumeDict.Keys.Count];
        var consumeValues = new int[consumeDict.Values.Count];
        consumeDict.Keys.CopyTo(consumeKeys, 0);
        consumeDict.Values.CopyTo(consumeValues, 0);
        consumeBuyViewScr.SetContents(consumeKeys, consumeValues);

        var sellKeys = new Item[sellDict.Keys.Count];
        var sellValues = new int[sellDict.Values.Count];
        sellDict.Keys.CopyTo(sellKeys, 0);
        sellDict.Values.CopyTo(sellValues, 0);
        sellViewScr.SetContents(sellKeys, sellValues);
    }

    public void UpdateShop()
    {
        SetShopList();
        consumeBuyViewScr.gameObject.SetActive(false);
        sellViewScr.gameObject.SetActive(false);
    }
}
