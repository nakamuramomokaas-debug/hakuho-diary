using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//家具、壁、床それぞれのスクロールビューが持っているスクリプト
public class SetFurnitureViewContents : MonoBehaviour
{
    public EnumDefinition.ItemType ItemType;//なんのアイテムを管理しているか
    [SerializeField] RoomStatusManager roomStatusManager;
    [SerializeField] Item wallItemScr;
    [SerializeField] Item floorItemScr;
    [SerializeField] GameObject uiFurnitureCanvas;
    [SerializeField] GameObject contents;//コンテンツ
    [SerializeField] NagisItem nagisItemScr;

    [SerializeField] Image wallImage;
    [SerializeField] Image floorImage;
    private GameObject[] itemObjs;//コンテンツの中にある１つ１つのアイテム
    private int[] viewItemsNum;//タンスがもっている家具のアイテム数
    private int[] roomItemsNum;//部屋に置いてある家具のアイテム数
    private Item tmpItem;//つかんでいるとき確定していないアイテム

    void Awake()
    {
        var childCount = contents.transform.childCount;
        var itemObjList = new List<GameObject>();
        for (int i = 0; i < childCount; i++)
        {
            var itemObject = contents.transform.GetChild(i).gameObject;
            itemObjList.Add(itemObject);
        }
        itemObjs = itemObjList.ToArray();
        viewItemsNum = new int[itemObjs.Length];
        roomItemsNum = new int[itemObjs.Length];

        //Debug.Log("itemObjs:" + itemObjs.Length);
    }

    public void SetContents(Item[] items, int[] itemsNum)
    {
        //Debug.Log("items:" + items.Length);

        foreach(var item in itemObjs)
        {
            item.SetActive(false);
        }

        for(int i = 0; i < items.Length; i++)
        {
            var image = itemObjs[i].GetComponent<Image>();
            var scr = itemObjs[i].GetComponent<FurnitureHandController>();
            itemObjs[i].SetActive(true);
            image.sprite = items[i].GetSprite();
            scr.SetItemId(items[i].GetItemId());

            if(ItemType == EnumDefinition.ItemType.FurnitureItem)
            {
                var childObj = itemObjs[i].transform.GetChild(0).gameObject;
                var numText = childObj.GetComponent<TextMeshProUGUI>();
                numText.text = itemsNum[i].ToString();
                viewItemsNum[i] = itemsNum[i];
            }
        }
    }

    //家具はスライドさせて変更する必要があるので、スライドしてオブジェクト化ができるようにしておく
    public GameObject SetFurniturePrefab(int itemId)
    {
        for(int i = 0; i < itemObjs.Length; i++)
        {
            var scr = itemObjs[i].GetComponent<FurnitureHandController>();
            if(scr.GetItemId() == itemId)
            {
                if(ItemType == EnumDefinition.ItemType.FurnitureItem)
                {
                    //個数を受け取る
                    if(viewItemsNum[i] > 0)
                    {
                        //家具オブジェクトを生成して設定
                        SoundManager.instance.PlaySE(10);
                        GameObject itemInstance = Instantiate(itemObjs[i], this.uiFurnitureCanvas.transform, false);
                        itemInstance.tag = "Furniture";
                        var furnitureScr = itemInstance.GetComponent<FurnitureHandController>();
                        furnitureScr.IsInstanceObj = true;
                        furnitureScr.SetItemId(itemId);
                        foreach (Transform child in itemInstance.transform){ GameObject.Destroy(child.gameObject); }//個数のついたやつはいらないので消す
                        var itemScr = itemInstance.AddComponent<Item>();
                        nagisItemScr.SetItemScr(ref itemScr, itemId);
                        itemInstance.name = itemScr.GetName();
                        tmpItem = itemScr;

                        roomStatusManager.PutFurniture(itemScr.GetMyRoomStatusBaff());
                        
                        // 家具（大）のとき
                        if(itemScr.GetItemSize() != 0)
                        {
                            var size = itemScr.GetItemSize() + 1;
                            Vector2 sd = itemInstance.GetComponent<RectTransform>().sizeDelta;
                            sd.x *= size;
                            sd.y *= size;
                            itemInstance.GetComponent<RectTransform>().sizeDelta = sd;
                        }
                        

                        //ビューに表示する数の更新
                        roomItemsNum[i] ++;
                        viewItemsNum[i] --; 
                        var childObj = itemObjs[i].transform.GetChild(0).gameObject;
                        var numText = childObj.GetComponent<TextMeshProUGUI>();
                        numText.text = viewItemsNum[i].ToString();

                        return itemInstance;
                    }
                    else return null;
                }
                else if (ItemType == EnumDefinition.ItemType.WallItem)
                {
                    SoundManager.instance.PlaySE(10);
                    nagisItemScr.SetItemScr(ref wallItemScr, itemId);
                    //roomStatusManager.PutFurniture(wallItemScr.GetMyRoomStatusBaff());
                    roomStatusManager.PutWall(wallItemScr.GetMyRoomStatusBaff());
                    Sprite wallSprite = nagisItemScr.GetItemSprite(itemId);
                    wallImage.sprite = wallSprite;
                }
                else if (ItemType == EnumDefinition.ItemType.FloorItem)
                {
                    SoundManager.instance.PlaySE(10);
                    nagisItemScr.SetItemScr(ref floorItemScr, itemId);
                    roomStatusManager.PutFloor(floorItemScr.GetMyRoomStatusBaff());
                    Sprite floorSprite = nagisItemScr.GetItemSprite(itemId);
                    floorImage.sprite = floorSprite;
                }
            }
        }
        return null;
    }

    //家具をもどすための関数
    public void ReturnFurniture(int itemId)
    {
        for(int i = 0; i < itemObjs.Length; i++)
        {
            var contentsItemHandScr = itemObjs[i].GetComponent<FurnitureHandController>();
            if(contentsItemHandScr.GetItemId() == itemId)
            {
                if(ItemType == EnumDefinition.ItemType.FurnitureItem)
                {
                    SoundManager.instance.PlaySE(6);
                    //Debug.Log("置けなかった");
                    roomStatusManager.RestoreFurniture(tmpItem.GetMyRoomStatusBaff());

                    //ビューに表示する数の更新
                    viewItemsNum[i] ++; 
                    roomItemsNum[i] --;
                    var childObj = itemObjs[i].transform.GetChild(0).gameObject;
                    var numText = childObj.GetComponent<TextMeshProUGUI>();
                    numText.text = viewItemsNum[i].ToString();
                    break;//これ必要？？
                }
            }
        }
    }

    public int GetRoomItemsNum(int itemId)//現在タンスのなかにあるアイテムの数が取れる
    {
        for(int i = 0; i < itemObjs.Length; i++)
        {
            var contentsItemHandScr = itemObjs[i].GetComponent<FurnitureHandController>();
            if(contentsItemHandScr.GetItemId() == itemId)
            {
                if(ItemType == EnumDefinition.ItemType.FurnitureItem)
                {
                    return roomItemsNum[i];
                }
            }
        }

        return -1;
    }
}