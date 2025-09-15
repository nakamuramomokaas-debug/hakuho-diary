using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

//家具のスライダーのコンテンツの中にあるアイテム１つ１つが持っている関数

public class FurnitureHandController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool IsEditRoom = true;
    public bool IsInstanceObj;
    // [SerializeField] AudioClip sound1;
    // [SerializeField] AudioClip sound2;
    // [SerializeField] SePlayer sePlayerSc;
    [SerializeField] SetFurnitureViewContents furnitureScrollViewScr;

    private Vector2 prevPos; //保存しておく初期position
    private RectTransform rectTransform; // 移動したいオブジェクトのRectTransform
    private RectTransform parentRectTransform; // 移動したいオブジェクトの親(Panel)のRectTransform
    private bool isDrag;
    private Coroutine coroutine;
    private int itemId;
    private GameObject itemInstanceObj;
    private bool isFurniture;
    private bool isExistSlideObj;

    private bool isCanPlace;//その場所におけるか？
    
    private GameObject tmpFurniture = null;

    void Awake()
    {
        // rectTransform = GetComponent<RectTransform>();
        // parentRectTransform = rectTransform.parent as RectTransform;
        if(furnitureScrollViewScr.ItemType == EnumDefinition.ItemType.FurnitureItem) isFurniture = true;
    }

    public void SetItemId(int id) { itemId = id; }
    public int GetItemId() { return itemId; }
    public bool GetIsCanPlace() { return isCanPlace; }

    //床と壁はボタンになっているのでクリックのみで切り替え可能
    public void OnWallFloorClick()
    {
        if(!isFurniture)
        {
            furnitureScrollViewScr.SetFurniturePrefab(itemId);
            furnitureScrollViewScr.ReturnFurniture(itemId);
        }
    }

    // ドラッグ開始時の処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!IsEditRoom) return;
        //Debug.Log("スライド");
        //インスタンス化された家具の場合、移動のみ可能
        if(IsInstanceObj)
        {
            rectTransform = GetComponent<RectTransform>();
            parentRectTransform = rectTransform.parent as RectTransform;

            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            prevPos = GetLocalPosition(eventData.position);
        }
        //ドラッグアンドドロップで新しい家具を生成する場合
        else
        {
            itemInstanceObj = furnitureScrollViewScr.SetFurniturePrefab(itemId);
            if(itemInstanceObj == null) //壁と床の場合はNullなのでリターンしていい。おける数がないときも
            {
                isExistSlideObj = false; 
                return; 
            }
            tmpFurniture = itemInstanceObj;
            isExistSlideObj = true;
            rectTransform = itemInstanceObj.GetComponent<RectTransform>();
            parentRectTransform = rectTransform.parent as RectTransform;

            //rectTransform.sizeDelta = new Vector2(300.0f, 300.0f);//サイズ変更はここで可能

            //rectTransformのアンカーは中央に
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            prevPos = GetLocalPosition(eventData.position);   
        }
    }

    // ドラッグ中の処理
    public void OnDrag(PointerEventData eventData)
    {
        if(!IsEditRoom) return;
        if(!isFurniture || (!isExistSlideObj && !IsInstanceObj)) return;

        // eventData.positionから、親に従うlocalPositionへの変換を行う
        // オブジェクトの位置をlocalPositionに変更する
        if(eventData.position == null) return;
        Vector2 localPosition = GetLocalPosition(eventData.position);
        rectTransform.anchoredPosition = localPosition;

        if(!isDrag)
        {
            isDrag = true;

            //決め打ちいやだここの中でEnum定義する
            //coroutine = StartCoroutine(PlayLoopSe(1, 1.0f));
        }
    }

    // ドラッグ終了時の処理(落とす処理)
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log( this.gameObject.name + ":" + isFurniture +":" + isExistSlideObj + ":" + IsInstanceObj + "+" + isCanPlace);
        if(!IsEditRoom) return;
        if(!isFurniture || (!isExistSlideObj && !IsInstanceObj)) return;
        
        if(tmpFurniture != null)//生成したときは操作しているのがContentsのなかみなので生成したオブジェクトを参照するb
        {
            var scr = tmpFurniture.GetComponent<FurnitureHandController>();
            isCanPlace = scr.isCanPlace;
            if(isCanPlace) return;
            furnitureScrollViewScr.ReturnFurniture(itemId);
            Destroy(tmpFurniture);
        }
        // オブジェクトをXのみドラッグ前の位置に戻す
        else if(!isCanPlace && IsInstanceObj) 
        {
            furnitureScrollViewScr.ReturnFurniture(itemId);
            var itemScr = this.GetComponent<Item>();
            Destroy(this.gameObject);
        }
    }

    // IEnumerator PlayLoopSe(int i, float time)
    // {
    //     while (true)
    //     {
    //         sePlayerSc.SePlay(i);
    //         yield return new WaitForSeconds(time);
    //     }
    // }

    // void StopLoopSe()
    // {
    //     StopCoroutine(coroutine);
    // }

    // public void OnPointerClick()
    // {
    //     Debug.Log("ポインター");
    // }

    // ScreenPositionからlocalPositionへの変換関数
    private Vector2 GetLocalPosition(Vector2 screenPosition)
    {
        Vector2 result = Vector2.zero;

        // screenPositionを親の座標系(parentRectTransform)に対応するよう変換する.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, Camera.main, out result);

        return result;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Floor")) isCanPlace = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Floor")) isCanPlace = false;
    }
}
