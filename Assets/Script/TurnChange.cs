using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TurnChange : MonoBehaviour
{
    [SerializeField] GameObject rotateObj;
    [SerializeField] FadeImage fadeImageScr;

    void Awake()
    {
        if(rotateObj == null) rotateObj = this.gameObject;
        rotateObj.transform.Rotate(new Vector3(0, 0, 180));
    }

    public void StartBringUpScene()
    {
        rotateObj.transform.Rotate(new Vector3(0, 0, 180));
    }

    public void RotateFadeOut()
    {
        rotateObj.transform.DORotate(new Vector3(0,0,0), 1, RotateMode.Fast).OnComplete(
        () => 
        {
            fadeImageScr.FadeOut();
        }
    ); 
    }
}
