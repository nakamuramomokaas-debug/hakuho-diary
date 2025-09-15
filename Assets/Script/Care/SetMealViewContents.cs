using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//SetFurnitureViewContentsを踏襲
public class SetMealViewContents : MonoBehaviour
{
    [SerializeField] GameObject uiMealCanvas;
    [SerializeField] GameObject contents;//コンテンツ
    [SerializeField] NagisItem nagisItemScr;

    private GameObject[] itemObjs;//コンテンツの中にある１つ１つのアイテム
    private int[] viewItemsNum;//タンスがもっている家具のアイテム数

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
    }

    public void SetContents(Item[] items, int[] itemsNum)
    {
        if(itemObjs == null) return;

        foreach(var item in itemObjs)
        {
            item.SetActive(false);
        }

        for(int i = 0; i < items.Length; i++)
        {
            var image = itemObjs[i].GetComponent<Image>();
            var scr = itemObjs[i].GetComponent<MealHandController>();
            itemObjs[i].SetActive(true);
            image.sprite = items[i].GetSprite();
            scr.SetItemId(items[i].GetItemId());

            var childObj = itemObjs[i].transform.GetChild(0).gameObject;
            var numText = childObj.GetComponent<TextMeshProUGUI>();
            numText.text = itemsNum[i].ToString();
            viewItemsNum[i] = itemsNum[i];
        }
    }

    //食べ物はスライドさせて変更する必要があるので、スライドしてオブジェクト化ができるようにしておく
    //食べ物の場合凪のアイテムからも減るのでその処理を入れる
    public GameObject SetMealPrefab(int itemId)
    {
        for(int i = 0; i < itemObjs.Length; i++)
        {
            var scr = itemObjs[i].GetComponent<MealHandController>();
            if(scr.GetItemId() == itemId)
            {
                //if(ItemType == EnumDefinition.ItemType.ConsumptionItem)
                {
                    //個数を受け取る
                    if(viewItemsNum[i] > 0)
                    {
                        //オブジェクトを生成して設定
                        //Debug.Log("オブジェクトを生成");
                        GameObject itemInstance = Instantiate(itemObjs[i], this.uiMealCanvas.transform, false);
                        var mealScr = itemInstance.GetComponent<MealHandController>();
                        mealScr.SetItemId(itemId);
                        foreach (Transform child in itemInstance.transform){ GameObject.Destroy(child.gameObject); }//個数のついたやつはいらないので消す
                        var itemScr = itemInstance.AddComponent<Item>();
                        nagisItemScr.SetItemScr(ref itemScr, itemId);
                        itemInstance.name = itemScr.GetName();

                        // 家具（大）のとき
                        // Vector2 sd = itemInstance.GetComponent<RectTransform>().sizeDelta;
                        // sd.x *= 2;
                        // sd.x *= 2;
                        // itemInstance.GetComponent<RectTransform>().sizeDelta = sd;

                        //ビューに表示する数の更新
                        viewItemsNum[i] --; 
                        var childObj = itemObjs[i].transform.GetChild(0).gameObject;
                        var numText = childObj.GetComponent<TextMeshProUGUI>();
                        numText.text = viewItemsNum[i].ToString();

                        return itemInstance;
                    }
                    else return null;
                }
            }
        }
        return null;
    }

    //家具をもどすための関数
    public void ReturnMeal(int itemId)
    {
        for(int i = 0; i < itemObjs.Length; i++)
        {
            var contentsItemHandScr = itemObjs[i].GetComponent<MealHandController>();
            if(contentsItemHandScr.GetItemId() == itemId)
            {
                //ビューに表示する数の更新
                viewItemsNum[i] ++; 
                var childObj = itemObjs[i].transform.GetChild(0).gameObject;
                var numText = childObj.GetComponent<TextMeshProUGUI>();
                numText.text = viewItemsNum[i].ToString();
            }
        }
    }
}
