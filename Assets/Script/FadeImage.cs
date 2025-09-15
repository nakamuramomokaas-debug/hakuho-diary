using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeImage : MonoBehaviour
{
    Image image;//フェード用自分のイメージ
    bool isFadeIn;
    bool isFadeOut;

    public bool GetIsFadeIn() { return isFadeIn; }
    public bool GetIsFadeOut() { return isFadeOut; }

    void Start()
    {
        image = GetComponent<Image>();
    }

    //閉じる
    public void FadeOut(bool isInOut = false)
    {
        //Debug.Log("fadeout");
        isFadeOut = false;

        DOTween.ToAlpha(
            ()=> image.color,
            color => image.color = color,
            1f, // lpha 1は255
            1f // 所要時間
        ).OnComplete(
            () => {
                //this.gameObject.SetActive(false);
                isFadeOut = false;
                if(isInOut) FadeIn();
            }
        );
    }

    //開ける
    public void FadeIn()
    {
        //Debug.Log("fadein");
        isFadeIn = true;

        DOTween.ToAlpha(
            () => image.color,
            color => image.color = color,
            0f, // alpha
            1f // 所要時間
            ).OnComplete(
            () => {
                isFadeIn = false;
            }
        );
    }


}
