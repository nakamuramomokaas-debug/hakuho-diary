using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

//こするバブルとの当たり判定
public class SpongeHandController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // [SerializeField] AudioClip sound1;
    // [SerializeField] AudioClip sound2;
    // [SerializeField] SePlayer sePlayerSc;
    [SerializeField] BathManager bathManager;
    [SerializeField] FadeImage fadeImageScr;
    [SerializeField] NagiStatus nagiStatus;
    [SerializeField] BringUpChatManager chatManager;
    [SerializeField] GameObject bathUI;//親。お風呂全体
    [SerializeField] GameObject bathobj;//お風呂でくつろぐ凪の
    [SerializeField] Babble[] bubbleScr;
    [SerializeField] GameObject[] selifEffectObject;
    [SerializeField] TextMeshProUGUI[] selifEffectText;
    [SerializeField] string[] bathText;

    private Vector2 prevPos; //保存しておく前のposition
    private RectTransform rectTransform; // 移動したいオブジェクトのRectTransform
    private RectTransform parentRectTransform; // 移動したいオブジェクトの親(Panel)のRectTransform

    private bool isDrag;
    private Coroutine coroutine;

    //private bool isBubble;//泡と重なっているか
    
    // ドラッグ開始時の処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = rectTransform.parent as RectTransform;

        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        prevPos = GetLocalPosition(eventData.position);
    }

    // ドラッグ中の処理
    public void OnDrag(PointerEventData eventData)
    {
        // eventData.positionから、親に従うlocalPositionへの変換を行う
        // オブジェクトの位置をlocalPositionに変更する
        if(eventData.position == null) return;
        Vector2 localPosition = GetLocalPosition(eventData.position);
        rectTransform.anchoredPosition = localPosition;
        var dis = Vector2.Distance(prevPos, localPosition);

        isDrag = dis > 5.0f;
        prevPos = localPosition;
    }

    // ドラッグ終了時の処理(落とす処理)ここ入ることないかも
    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
    }

    public void StartBath()
    {
        foreach(var bubble in bubbleScr)
        {
           bubble.BubbleReset();
        }
    }

    //泡が全部きれいになったか(Bubbleから呼ばれる)
    public bool IsCleanAll()
    {
        foreach(var bubble in bubbleScr)
        {
           if(!bubble.IsClean()) return false;
        }
        FadeBath();
        return true;
    }

    void Update()
    {
        if(!isDrag) return;
        foreach(var bubble in bubbleScr)
        {
            if(bubble.IsHit && isDrag) bubble.Wash(Time.deltaTime);
        }
    }

    void  FadeBath() 
    {
        bathManager.FadeBath();
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
        if(other.CompareTag("Bubble"))
        {
            var bubble = other.GetComponent<Babble>();
            if(bubble.IsClean()) return;
            bubble.IsHit = true;
            //isBubble = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Bubble"))
        {
            var bubble = other.GetComponent<Babble>();
            bubble.IsHit = false;
            //isBubble = false;
        }
    }
}
