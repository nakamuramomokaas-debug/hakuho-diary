using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] NagiStatus nagiStatus;//今の凪のオブジェクト２パターン
    private Vector2 prevPos; //保存しておく初期position
    private RectTransform rectTransform; // 移動したいオブジェクトのRectTransform
    private RectTransform parentRectTransform; // 移動したいオブジェクトの親(Panel)のRectTransform
    private bool isCanMove = true;
    
    public void SetIsCanMove(bool iscan) { isCanMove = iscan; }//MealManagerが呼ぶ

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = rectTransform.parent as RectTransform;
    }

    // ドラッグ開始時の処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!isCanMove) return;
        // ドラッグ前の位置を記憶しておく
        // RectTransformの場合はpositionではなくanchoredPositionを使う
        prevPos = rectTransform.anchoredPosition;

        SoundManager.instance.PlaySE(0);
        SoundManager.instance.PlayLoopSe("PickUp");

        // nagiImages[0].SetActive(false);
        // nagiImages[1].SetActive(true);
        nagiStatus.SetNagiAnimation(EnumDefinition.NagiMotionType.Catch, nagiStatus.GetNagiType());
    }

    // ドラッグ中の処理
    public void OnDrag(PointerEventData eventData)
    {
        if(!isCanMove) return;
        // eventData.positionから、親に従うlocalPositionへの変換を行う
        // オブジェクトの位置をlocalPositionに変更する

        Vector2 localPosition = GetLocalPosition(eventData.position);
        rectTransform.anchoredPosition = localPosition;
    }

    public void OnPointerClick()
    {
        //Debug.Log("ポインター");
    }

    // ドラッグ終了時の処理(落とす処理)
    public void OnEndDrag(PointerEventData eventData)
    {
        if(!isCanMove) return;

        // オブジェクトをXのみドラッグ前の位置に戻す
        Vector2 localPosition = GetLocalPosition(eventData.position);
        Vector2 rePrevPos = prevPos;
        rePrevPos.x = localPosition.x;

        rectTransform.DOAnchorPos(new Vector2(rePrevPos.x, rePrevPos.y), 0.5f)
        .SetEase(Ease.OutBack);

        SoundManager.instance.StopLoopSE();
        SoundManager.instance.PlaySE(0);

        // nagiImages[0].SetActive(true);
        // nagiImages[1].SetActive(false);
        nagiStatus.SetNagiAnimation(EnumDefinition.NagiMotionType.Sit, nagiStatus.GetNagiType());
    }

    // ScreenPositionからlocalPositionへの変換関数
    private Vector2 GetLocalPosition(Vector2 screenPosition)
    {
        Vector2 result = Vector2.zero;

        // screenPositionを親の座標系(parentRectTransform)に対応するよう変換する.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, Camera.main, out result);

        return result;
    }
}
