using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UpUIEffect : MonoBehaviour
{
    Vector2 firstTransformPos;
    RectTransform rectTransform;
    Image image;

    void Start()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
        firstTransformPos = rectTransform.position;
        image = this.gameObject.GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0f);
    }

    public void PlayEffect(float posY = 0f)
    {   
        rectTransform.position = firstTransformPos;
        image.color = new Color(1f, 1f, 1f, 1f);

        rectTransform.DOAnchorPos(new Vector2(0, posY), 0.6f).SetEase(Ease.OutBack);
        image.DOFade(0f, 1f);
    }
}
