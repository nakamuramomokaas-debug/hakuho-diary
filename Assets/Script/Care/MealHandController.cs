using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

//FurnitureHandControllerを踏襲してつくる
public class MealHandController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // [SerializeField] AudioClip sound1;
    // [SerializeField] AudioClip sound2;
    // [SerializeField] SePlayer sePlayerSc;
    [SerializeField] GameObject mealUI;
    [SerializeField] SetMealViewContents mealScrollViewScr;
    [SerializeField] NagisItem nagisItemScr;

    private Vector2 prevPos; //保存しておく初期position
    private RectTransform rectTransform; // 移動したいオブジェクトのRectTransform
    private RectTransform parentRectTransform; // 移動したいオブジェクトの親(Panel)のRectTransform
    private bool isDrag;
    private Coroutine coroutine;
    private int itemId;
    private GameObject itemInstanceObj;
    private bool isExistSlideObj;

    private bool isCanPlace;//その場所におけるか？
    
    private GameObject tmpMeal = null;
    private Item tmpItem;

    void Awake()
    {
        // rectTransform = GetComponent<RectTransform>();
        // parentRectTransform = rectTransform.parent as RectTransform;
    }

    public void SetItemId(int id) { itemId = id; }
    public int GetItemId() { return itemId; }
    public bool GetIsCanPlace() { return isCanPlace; }

    // ドラッグ開始時の処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        //ドラッグアンドドロップで新しく食べ物を実体化
        SoundManager.instance.PlaySE(10);
        itemInstanceObj = mealScrollViewScr.SetMealPrefab(itemId);
        if(itemInstanceObj == null) //壁と床の場合はNullなのでリターンしていい。おける数がないときも
        {
            isExistSlideObj = false; 
            return; 
        }
        tmpMeal = itemInstanceObj;
        isExistSlideObj = true;
        rectTransform = itemInstanceObj.GetComponent<RectTransform>();
        parentRectTransform = rectTransform.parent as RectTransform;
        tmpItem = itemInstanceObj.GetComponent<Item>();
        //rectTransformのアンカーは中央に
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        prevPos = GetLocalPosition(eventData.position);  

        mealUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(200f, 0), 0.6f)
        .SetEase(Ease.OutBack); 
    }

    // ドラッグ中の処理
    public void OnDrag(PointerEventData eventData)
    {
        if(!isExistSlideObj) return;

        var scr = tmpMeal.GetComponent<MealHandController>();
        isCanPlace = scr.isCanPlace;
        if(isCanPlace) 
        {
            //Debug.Log("食べたい顔");//OnTriggerEnter2Dのところでもいいかも
        }

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

    // ドラッグ終了時の処理(落とす処理)ここ入ることないかも
    public void OnEndDrag(PointerEventData eventData)
    {
        if(!isExistSlideObj) return;
        
        if(tmpMeal != null)//生成したときは操作しているのがContentsのなかみなので生成したオブジェクトを参照するb
        {
            var mealManager = mealUI.GetComponent<MealManager>();
            var scr = tmpMeal.GetComponent<MealHandController>();
            SoundManager.instance.PlaySE(9);
            isCanPlace = scr.isCanPlace;

            if(isCanPlace) 
            {
                nagisItemScr.UseItem(scr.GetItemId());//凪のアイテム消費
                var _target = tmpMeal.GetComponent<RectTransform>();//アイテムがなくなるアニメーション
                _target.position = new Vector3(-0.25f, 0f, 0f);
                _target.DOScale(0f, 3f).SetEase(Ease.Linear, 5f)
                .OnComplete(()=>
                {
                    mealManager.AnimationSerif(tmpItem);
                });
            }
            else
            {
                mealScrollViewScr.ReturnMeal(itemId);
                Destroy(tmpMeal);
                mealUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.6f)
                .SetEase(Ease.OutBack); 
            }
        }
    }

    // ScreenPositionからlocalPositionへの変換関数
    Vector2 GetLocalPosition(Vector2 screenPosition)
    {
        Vector2 result = Vector2.zero;

        // screenPositionを親の座標系(parentRectTransform)に対応するよう変換する.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, Camera.main, out result);

        return result;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Nagi")) isCanPlace = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Nagi")) isCanPlace = false;
    }
}
