using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomChangeManager : MonoBehaviour
{
    //本当はenumにしたかった
    // Wall = 0
    // Floor = 1
    // Furniture = 2
    [SerializeField] GameObject gabageGenerator;
    [SerializeField] NagisItem nagisItemScr;
    [SerializeField] GameObject normalUIObj;
    [SerializeField] GameObject nagiUIObj;
    [SerializeField] SetFurnitureViewContents wallViewScr;
    [SerializeField] SetFurnitureViewContents floorViewScr;
    [SerializeField] SetFurnitureViewContents furnitureViewScr;
    [SerializeField] GameObject wallButton;
    [SerializeField] GameObject floorButton;
    [SerializeField] GameObject furnitureButton;

    
    Dictionary<Item, int> wallDict = new Dictionary<Item, int>();
    Dictionary<Item, int> floorDict = new Dictionary<Item, int>();
    Dictionary<Item, int> furnitureDict = new Dictionary<Item, int>();

    bool isEndStart;// 初期化済みか？

    void OnEnable()
    {
        if(!isEndStart) return;
        
        UpdateRoomChange();
    }

    void Start()
    {
        wallViewScr.ItemType = EnumDefinition.ItemType.WallItem;
        floorViewScr.ItemType = EnumDefinition.ItemType.FloorItem;
        furnitureViewScr.ItemType = EnumDefinition.ItemType.FurnitureItem;

        var tmpDict = nagisItemScr.GetInteriors();
        foreach(var item in tmpDict)
        {
            //Debug.Log(item.Key.GetName() + "+" + item.Value);
            if(item.Key.GetItemType() == EnumDefinition.ItemType.WallItem) wallDict.Add(item.Key, item.Value);
            else if(item.Key.GetItemType() == EnumDefinition.ItemType.FloorItem) floorDict.Add(item.Key, item.Value);
            else if(item.Key.GetItemType() == EnumDefinition.ItemType.FurnitureItem) furnitureDict.Add(item.Key, item.Value);
        }
        SetFurnitureList();
        floorViewScr.gameObject.SetActive(false);
        furnitureViewScr.gameObject.SetActive(false);
        isEndStart = true;
    }

    void SetFurnitureList()
    {
        Image wallImg = wallButton.GetComponent<Image>();
        Image floorImg = floorButton.GetComponent<Image>();
        Image furnitureImg = furnitureButton.GetComponent<Image>();

        wallImg.color = new Color32 (155, 155, 155, 255);
        floorImg.color = new Color32 (255, 255, 255, 255);
        furnitureImg.color = new Color32 (255, 255, 255, 255);

        wallViewScr.gameObject.SetActive(true);
        floorViewScr.gameObject.SetActive(true);
        furnitureViewScr.gameObject.SetActive(true);

        var wallKeys = new Item[wallDict.Keys.Count];
        var wallValues = new int[wallDict.Values.Count];
        wallDict.Keys.CopyTo(wallKeys, 0);
        wallDict.Values.CopyTo(wallValues, 0);
        wallViewScr.SetContents(wallKeys, wallValues);

        var floorKeys = new Item[floorDict.Keys.Count];
        var floorValues = new int[floorDict.Values.Count];
        floorDict.Keys.CopyTo(floorKeys, 0);
        floorDict.Values.CopyTo(floorValues, 0);
        floorViewScr.SetContents(floorKeys, floorValues);

        var furnitureKeys = new Item[furnitureDict.Keys.Count];
        var furnitureValues = new int[furnitureDict.Values.Count];
        furnitureDict.Keys.CopyTo(furnitureKeys, 0);
        furnitureDict.Values.CopyTo(furnitureValues, 0);
        furnitureViewScr.SetContents(furnitureKeys, furnitureValues);
    }

    public void ExitRoomChange()
    {
        SoundManager.instance.PlaySE(6);

        normalUIObj.SetActive(true);
        nagiUIObj.SetActive(true);
        normalUIObj.GetComponent<NormalUIManager>().ActiveGabages();
        gabageGenerator.SetActive(true);
        this.gameObject.SetActive(false);
        FurnitureAllNotMove();
    }

    public void TabChange(int type)
    {
        SoundManager.instance.PlaySE(4);

        //バグ：タブチェンジをしないと買ったものが更新されない。
        UpdateRoomChange();
        wallViewScr.gameObject.SetActive(false);
        floorViewScr.gameObject.SetActive(false);
        furnitureViewScr.gameObject.SetActive(false);

        Image wallImg = wallButton.GetComponent<Image>();
        Image floorImg = floorButton.GetComponent<Image>();
        Image furnitureImg = furnitureButton.GetComponent<Image>();

        wallImg.color = new Color32 (255, 255, 255, 255);
        floorImg.color = new Color32 (255, 255, 255, 255);
        furnitureImg.color = new Color32 (255, 255, 255, 255);

        if(type == 0)
        {
            wallViewScr.gameObject.SetActive(true);
            wallImg.color = new Color32 (155, 155, 155, 255);
        }
        else if(type == 1)
        {
            floorViewScr.gameObject.SetActive(true);
            floorImg.color = new Color32 (155, 155, 155, 255);
        }
        else if(type == 2)
        {
            furnitureViewScr.gameObject.SetActive(true);
            furnitureImg.color = new Color32 (155, 155, 155, 255);
        }
    }

    public void UpdateRoomChange()
    {
        //Dictionary全開放して読み込みなおす
        wallDict.Clear();
        floorDict.Clear();
        furnitureDict.Clear();
        var tmpDict = nagisItemScr.GetInteriors();
        foreach(var item in tmpDict)
        {
            //Debug.Log(item.Key.GetName() + "+" + item.Value);
            if(item.Key.GetItemType() == EnumDefinition.ItemType.WallItem) wallDict.Add(item.Key, item.Value);
            else if(item.Key.GetItemType() == EnumDefinition.ItemType.FloorItem) floorDict.Add(item.Key, item.Value);
            else if(item.Key.GetItemType() == EnumDefinition.ItemType.FurnitureItem)
            {
                var roomNum = furnitureViewScr.GetRoomItemsNum(item.Key.GetItemId());
                furnitureDict.Add(item.Key, item.Value - roomNum); //凪の部屋に置いてある分は数から引く
            }
        }
        SetFurnitureList();

        wallViewScr.gameObject.SetActive(true);
        floorViewScr.gameObject.SetActive(false);
        furnitureViewScr.gameObject.SetActive(false);
    }

    void FurnitureAllNotMove()
    {
        var furnitures = new List<GameObject>(GameObject.FindGameObjectsWithTag("Furniture"));
        if(furnitures == null) return;
        foreach(GameObject furniture in furnitures) 
        {
            var scr = furniture.GetComponent<FurnitureHandController>();
            scr.IsEditRoom = false;
        }
    }
}
