using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Garbage : MonoBehaviour, IPointerClickHandler
{ 
   public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.instance.PlaySE(14);
        this.GetComponent<RectTransform>().DOJumpAnchorPos(new Vector2(-522.0f, -50.0f), 300f, 1, 1.0f).OnComplete(() => 
        {
            GameObject g = GameObject.FindWithTag("Trash");
            g.GetComponent<Trash>().TrashCount += 1;
            SoundManager.instance.PlaySE(14);
            Destroy(this.gameObject);
        });
    }
}