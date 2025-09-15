using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//泡、こすられて一定値（Serializeで設定）に達したら白くなる
public class Babble : MonoBehaviour
{
    [SerializeField] SpongeHandController spongeScr;
    [SerializeField] Image bubbleImage;
    [SerializeField] float bubbleHp = 5.0f;

    public bool IsHit;

    float bubbleNowHp;
    bool isClean;
    bool isReset;

    public bool IsClean() { return isClean; }

    public void BubbleReset()
    {
        bubbleNowHp = bubbleHp;
        bubbleImage.color = new Color32 (155, 155, 155, 255);
        isClean = false;
        isReset = true;
    }

    public void Wash(float time)
    {
        if(!isReset) return;
        SoundManager.instance.PlayLoopSe("Sponge");
        //Debug.Log("Wash" + time + "hp:" + bubbleNowHp);
        bubbleNowHp -= time;
        if(bubbleNowHp < 0f) CleanBubble();
    }

    void CleanBubble()
    {
        //Debug.Log("綺麗になった");
        SoundManager.instance.PlaySE(12);
        SoundManager.instance.StopLoopSE();
        bubbleNowHp = bubbleHp;
        bubbleImage.color = new Color32 (255, 255, 255, 255);
        isClean = true;
        isReset = false;
        spongeScr.IsCleanAll();
    }
}
