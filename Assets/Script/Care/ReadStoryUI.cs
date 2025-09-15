using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ReadStoryUI : MonoBehaviour
{
    [SerializeField] GameObject storyScene;
    [SerializeField] GameObject grossDiaryGameObject;
    [SerializeField] GameObject StoryButtonParent;
    [SerializeField] MyData myDataScr;
    [SerializeField] GameObject checkToast;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] ChatManager chatManager;

    int tmpStoryIndex = -1;

    void OnEnable()
    {
        //ストーリーがどこまで読み込まれたか確認してからLOCK状態とそうでないものにわける
        var buttons = new List<GameObject>();
        var index = 0;
        checkToast.SetActive(false);

        //ボタンをゲットする
        foreach(Transform storyButton in StoryButtonParent.transform) {
            buttons.Add(storyButton.gameObject);
		}
        foreach(var button in buttons)
        {
            var buttonScr = button.GetComponent<StoryButton>();
            var buttonB = button.GetComponent<Button>();
            if(myDataScr.GetDataInt("StoryIndex") <= index - 1)
            {
                buttonScr.SetLockObj(true);
                buttonB.interactable = false;
            } 
            else
            {
                buttonScr.SetLockObj(false);
                buttonB.interactable = true;
            }
            index++;
        }
    }

    public void ChangeGrossDiary()
    {
        SoundManager.instance.PlaySE(4);
        this.gameObject.SetActive(false);
        grossDiaryGameObject.SetActive(true);
    }

    public void CheckToastOn(StoryButton buttonScr)
    {
        SoundManager.instance.PlaySE(4);
        titleText.text = buttonScr.GetTitleText();
        tmpStoryIndex = buttonScr.GetStoryIndex();
        checkToast.SetActive(true);
    }

    //ダイアログを消す
    public void ReturnDiary()
    {
        SoundManager.instance.PlaySE(6);
        checkToast.SetActive(false);
    }

    //どれかストーリー押されたら確認モーダルを出してから。
    //ストーリー側は、このモードの時はプレイヤープレフスを書き換えないような仕組みを作る
    public void Story()
    {
        //normalUIObj.SetActive(true);

        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySE(5);

        DOVirtual.DelayedCall(2, () => 
        {
            checkToast.SetActive(false);
            storyScene.SetActive(true);
            chatManager.StoryRead(tmpStoryIndex);
        });
    }
}
