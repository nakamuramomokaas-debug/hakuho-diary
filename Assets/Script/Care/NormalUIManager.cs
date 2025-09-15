using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NormalUIManager : MonoBehaviour
{
    [SerializeField] TurnChange turnChangeScr;
    [SerializeField] GameObject gabageGenerator;
    [SerializeField] CalenderManager calenderManager;
    [SerializeField] GameObject tutorialBoard;
    [SerializeField] FadeImage fadeImageScr;
    [SerializeField] GameObject roomChangeUIObj;
    [SerializeField] GameObject mealManagerObj;
    [SerializeField] GameObject bathManagerObj;
    [SerializeField] GameObject sleepManagerObj;
    [SerializeField] GameObject grossDiaryObj;
    [SerializeField] GameObject readStoryDiaryObj;
    [SerializeField] TalkManager talkManager;
    [SerializeField] GameObject shopManagerObj;
    [SerializeField] GameObject nagiUIObj;
    List<GameObject> gabages = new List<GameObject>();
    //ゴミ生成非アクティブにしたかったらそのオブジェクトも非アクティブにするだけでいいかも？

    public void StartBringUpScene()
    {
        turnChangeScr.gameObject.SetActive(false);
        if (!calenderManager.GetIsTutorial()) EndTutorialBoard();
        fadeImageScr.FadeIn();
        calenderManager.UpdateDay();
        DOVirtual.DelayedCall(1, () => 
        {
            SoundManager.instance.PlayBGM(3);
        });
    }

    void OnEnable()
    {
        // bathManagerObj.GetComponent<BathManager>().SetLock(nagiH);
        // mealManagerObj.GetComponent<MealManager>().SetLock();
        // talkManager.SetLock();
    }

    // 画像が増えても大丈夫なように
    public void SetTutorialBoard() //ボタンからも呼ばれるようになった野江publicに
    {
        SoundManager.instance.PlaySE(7);
        tutorialBoard.SetActive(true);
    }
    
    public void EndTutorialBoard()
    {
        SoundManager.instance.PlaySE(4);
        tutorialBoard.SetActive(false);
    }

    public void ShopChange()
    {
        SoundManager.instance.PlaySE(7);

        if(fadeImageScr.GetIsFadeOut() || turnChangeScr.gameObject.activeSelf) return;
        shopManagerObj.SetActive(true);
    }
    
    public void RoomChange()
    {
        SoundManager.instance.PlaySE(7);

        if(fadeImageScr.GetIsFadeOut() || turnChangeScr.gameObject.activeSelf) return;
        roomChangeUIObj.SetActive(true);
        //roomChangeUIObj.GetComponent<RoomChangeManager>().TabChange(0);
        gabages = new List<GameObject>(GameObject.FindGameObjectsWithTag("Garbage"));
        gabageGenerator.SetActive(false);
        nagiUIObj.SetActive(false);
        this.gameObject.SetActive(false);
        FurnitureAllCanMove();

        if(gabages == null) return;
        foreach(GameObject gabage in gabages) gabage.SetActive(false);
    }

    public void MealChange()
    {
        SoundManager.instance.PlaySE(7);

        if(fadeImageScr.GetIsFadeOut() || turnChangeScr.gameObject.activeSelf) return;
        gabages = new List<GameObject>(GameObject.FindGameObjectsWithTag("Garbage"));
        gabageGenerator.SetActive(false);
        mealManagerObj.SetActive(true);
        this.gameObject.SetActive(false);

        if(gabages == null)return;
        foreach(GameObject gabage in gabages) gabage.SetActive(false);

        // var rect = mealManagerObj.GetComponent<RectTransform>();
        // rect.anchoredPosition = new Vector2(0,0);
    }
    
    public void BathChange()
    {
        SoundManager.instance.PlaySE(7);

        if(fadeImageScr.GetIsFadeOut() || turnChangeScr.gameObject.activeSelf) return;
        bathManagerObj.SetActive(true);
        this.gameObject.SetActive(false);

        gabageGenerator.SetActive(false);
        if(gabages == null)return;
        foreach(GameObject gabage in gabages) gabage.SetActive(false);
        // var rect = bathManagerObj.GetComponent<RectTransform>();
        // rect.anchoredPosition = new Vector2(0,0);
    }

    public void TalkChange()
    {
        SoundManager.instance.PlaySE(7);

        if(fadeImageScr.GetIsFadeOut() || turnChangeScr.gameObject.activeSelf) return;
        gabages = new List<GameObject>(GameObject.FindGameObjectsWithTag("Garbage"));
        gabageGenerator.SetActive(false);
        this.gameObject.SetActive(false);

        if(gabages == null)return;
        foreach(GameObject gabage in gabages) gabage.SetActive(false);

        talkManager.StartTalk();
        // var rect = mealManagerObj.GetComponent<RectTransform>();
        // rect.anchoredPosition = new Vector2(0,0);
    }

    public void GrossDiary()
    {
        SoundManager.instance.PlaySE(7);

        if(fadeImageScr.GetIsFadeOut() || turnChangeScr.gameObject.activeSelf) return;
        grossDiaryObj.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void ExitGrossDiary()
    {
        SoundManager.instance.PlaySE(6);

        grossDiaryObj.SetActive(false);
        readStoryDiaryObj.SetActive(false);
        this.gameObject.SetActive(true);
    }

    public void SleepChange()
    {
        SoundManager.instance.PlaySE(7);
        if(fadeImageScr.GetIsFadeOut() || turnChangeScr.gameObject.activeSelf) return;
        sleepManagerObj.SetActive(true);
        this.gameObject.SetActive(false);

        gabageGenerator.SetActive(false);
        if(gabages == null) return;
        foreach(GameObject gabage in gabages)
        {
            if(gabage == null) continue;
            gabage.SetActive(false);
        }
        // var rect = bathManagerObj.GetComponent<RectTransform>();
        // rect.anchoredPosition = new Vector2(0,0);
    }

    public void ActiveGabages()
    {
        foreach(GameObject gabage in gabages)
        {
            if(gabage == null) continue;
            gabage.SetActive(true);
        } 
    }

    void FurnitureAllCanMove()
    {
        var furnitures = new List<GameObject>(GameObject.FindGameObjectsWithTag("Furniture"));
        if(furnitures == null) return;
        foreach(GameObject furniture in furnitures) 
        {
            var scr = furniture.GetComponent<FurnitureHandController>();
            scr.IsEditRoom = true;
        }
    }
}
