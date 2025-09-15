using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using DG.Tweening;

public class StoryTitle : MonoBehaviour
{
    [SerializeField] FadeImage fadeImageScr;
    [SerializeField] Image titleImage;//フェード用自分のイメージ
    [SerializeField] Sprite[] storytitleImages;
    int storyNum = -1;

    public void SetStoryTitle(int storyNumber)
    {
        //Debug.Log("ストーリー番号: " + storyNumber);
        storyNum = storyNumber;
        titleImage.sprite = storytitleImages[storyNumber];
        DOVirtual.DelayedCall(1, () => SoundManager.instance.PlaySE(3));
    }

    public void StartStory()//ボタンから呼ばれるので消しちゃダメ
    {
        fadeImageScr.FadeOut(true);
        DOVirtual.DelayedCall(1, () => 
        {
            this.gameObject.SetActive(false);
            //if(storyNumber) 
            Debug.Log(storyNum);
            if(storyNum == -1) SoundManager.instance.PlayBGM(1);
            else
            {
                if(storyNum == 2 || storyNum == 6 || storyNum == 8) SoundManager.instance.PlayBGM(1); //コミカル
                else if(storyNum == 5 || storyNum == 9) SoundManager.instance.PlayBGM(5);//悲しいBGM
                else SoundManager.instance.PlayBGM(6);//通常
            }
        });
    }
}
