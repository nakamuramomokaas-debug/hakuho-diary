using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ngroDefine;

//プレハブのインスペクタで設定してください
public class Item : MonoBehaviour
{
    // //宣言
    // [System.Serializable] public struct MyRoomStatusBaff
    // {//何パーセントバフがかかるか
    //     public int simple;//+エゴー協調-
    //     public int shape;//自立ー依存
    //     public int classic; //外交的ー内向的
    //     public int gaming;//直観ー理性
    //     public int japaneseTaste;//温厚ー好戦的
    // }

    //変数
    [SerializeField] Sprite iconImage;
    [SerializeField] string objName;
    [SerializeField] int itemId;
    [SerializeField] int itemSize;
    [SerializeField] int quantityLimit;//-1なら∞
    [SerializeField] bool isDayLimit;
    [SerializeField] int price;
    [SerializeField] int sellingPrice;
    [SerializeField] EnumDefinition.ItemType itemType;
    [SerializeField] string explanationShort;//説明文効能のみ
    [SerializeField] string explanation;//説明文
    [SerializeField] MyRoomStatusBaff myRoomStatusBaff;
    [SerializeField] int nostalgia;//なつき度変化形（アイテム用）
    [SerializeField] PersonalityStatus personal; //パーソナリティ変化形(アイテム用)
    //[SerializeField] int nosta;//行動力回復系（アイテム用）
    [SerializeField] string[] nagisComment;//凪のコメント（アイテム用）

    //ゲッター
    public int GetItemId () { return itemId; }
    public int GetItemSize () { return itemSize; }
    public string GetName() { return objName; }
    public int GetPrice() { return price; }
    public int GetSellPrice() { return sellingPrice; }
    public int GetQuantityLimit() { return quantityLimit; }
    public EnumDefinition.ItemType GetItemType(){ return itemType; }
    public Sprite GetSprite() { return iconImage; }
    public string GetExplanationShort() { return explanationShort; }
    public string GetExplanation() { return explanation; }
    public MyRoomStatusBaff GetMyRoomStatusBaff() { return myRoomStatusBaff; }
    public int GetNostalgia() { return nostalgia; }
    public PersonalityStatus GetPersonal() { return personal; }
    //public int GetNosta() { return nosta; }
    public string[] GetNagisComment() { return nagisComment; }

    //一括設定
    public void CopyItemData(ref Item item)//NagiItemとBaayaItemで使用
    {
        //Debug.Log("item");
        item.iconImage = iconImage;
        item.objName = objName;
        item.itemId = itemId;
        item.itemSize = itemSize;
        item.quantityLimit = quantityLimit;
        item.isDayLimit = isDayLimit;
        item.price = price;
        item.sellingPrice = sellingPrice;
        item.itemType = itemType;
        item.myRoomStatusBaff = myRoomStatusBaff;
        item.nostalgia = nostalgia;
        item.personal = personal;
        //item.nosta = nosta;
        item.nagisComment = nagisComment;
    }
}
