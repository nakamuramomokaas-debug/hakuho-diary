using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

//SetFurnitureViewContentと同じような働きでそれぞれのビューがもっている
public class BuyViewContents : MonoBehaviour
{
    public EnumDefinition.ItemType ItemType;//FunitureとConsumeの2卓（WallとFloorもFurnitureに含む）
    //[SerializeField] GameObject uiFurnitureCanvas;
    [SerializeField] GameObject contents;//コンテンツ
    [SerializeField] NagisItem nagisItemScr;
    private GameObject[] itemObjs;//コンテンツの中にある１つ１つのアイテム
    private int[] viewItemsNum;//ショップがもっているアイテム数

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

        //Debug.Log("itemObjs:" + itemObjs.Length);
    }

    public void SetContents(Item[] items, int[] itemsNum)//マネージャーが呼ぶ
    {
        //Debug.Log("items:" + items.Length);

        foreach(var item in itemObjs)
        {
            item.SetActive(false);
        }
        
        //Debug.Log("アイテム数" + items.Length);
        for(int i = 0; i < items.Length; i++)
        {
            var image = itemObjs[i].transform.Find("ItemIcon").gameObject.GetComponent<Image>();
            //var nameObj = itemObjs[i].transform.Find("ItemName").gameObject;
            var nameText = itemObjs[i].transform.Find("ItemName").GetComponent<TextMeshProUGUI>();//nameObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var priceObj = itemObjs[i].transform.Find("price").gameObject;
            var priceText = priceObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            itemObjs[i].SetActive(true);
            image.sprite = items[i].GetSprite();
            nameText.text = items[i].GetName();
            priceText.text = items[i].GetPrice().ToString();

            var itemScr = itemObjs[i].GetComponent<ShopTmpItem>();
            itemScr.ContentItem = items[i];

            //買えないアイテムは非活性に
            if((itemScr.ContentItem.GetQuantityLimit() - nagisItemScr.GetItemNum(itemScr.ContentItem.GetItemId())) < 1 &&
                itemScr.ContentItem.GetQuantityLimit() != -1)
            {
                itemObjs[i].GetComponent<Button>().interactable = false;
                itemObjs[i].transform.Find("SoldOut").gameObject.SetActive(true);
            }
        }
    }

    public void UpdateShowContents(Item[] items, int[] itemsNum)//マネージャーがよぶ
    {
        //買えないアイテムを非活性にしたいだけ
        for(int i = 0; i < items.Length; i++)
        {
            var itemScr = itemObjs[i].GetComponent<ShopTmpItem>();
            itemScr.ContentItem = items[i];

            //買えないアイテムは非活性に
            if(itemScr== null)  continue;
            if((itemScr.ContentItem.GetQuantityLimit() - nagisItemScr.GetItemNum(itemScr.ContentItem.GetItemId())) < 1 &&
                itemScr.ContentItem.GetQuantityLimit() != -1)
            {
                itemObjs[i].GetComponent<Button>().interactable = false;
                itemObjs[i].transform.Find("SoldOut").gameObject.SetActive(true);
            }
        }
    }
}
