using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class BathManager : MonoBehaviour
{
    [SerializeField] NagiStatus nagiStatus;
    [SerializeField] NagiHpManager nagiHpManager;
    [SerializeField] GameObject lockObj;
    [SerializeField] GameObject gabageGenerator;
    [SerializeField] FadeImage fadeImageScr;
    [SerializeField] BringUpChatManager chatManager;
    [SerializeField] GameObject normalUIObj;
    [SerializeField] GameObject confirmModal;
    [SerializeField] GameObject bathUIObj;
    [SerializeField] GameObject bathTimeObjs;
    [SerializeField] CalenderManager calenderManager;
    [SerializeField] NagiHpManager nagiHPManager;
    [SerializeField] SpongeHandController spongeScr;
    [SerializeField] GameObject[] selifEffectObject;
    [SerializeField] TextMeshProUGUI[] selifEffectText;
    [SerializeField] string[] bathText;
    
    [SerializeField] GameObject[] nagiHpObj;

    int useHp = 0;//今回のターンの消費HP
    public int GetBathUseHp(){ return useHp; }

    public void SetLock(int ngHp)
    {
        //Debug.Log(ngHp + "B" + useHp);
        lockObj.SetActive(ngHp < useHp);
        var button = lockObj.transform.parent.GetComponent<Button>();
        button.interactable = ngHp >= useHp;
    }

    public void StartBringUpScene()
    {
        foreach (var hp in nagiHpObj)
        {
            hp.SetActive(false);
        }

        useHp = Random.Range(2, 4);//2or3
        for(int i = 0 ; i < nagiHpObj.Length; i++)
        {
            nagiHpObj[i].SetActive(true);
            if(i == useHp - 1) break;
        }
        SetLock(nagiHpManager.GetNagiHp());
    }

    public void ExitBathConfirm(bool isBathFinish)//確認モーダルでいいえを選ぶもしくはお風呂がおわったら
    {
        SoundManager.instance.PlaySE(6);
        if(isBathFinish)
        {
            nagiHPManager.UseNagiHp(useHp);
        }
        normalUIObj.GetComponent<NormalUIManager>().ActiveGabages();
        normalUIObj.SetActive(true);
        gabageGenerator.SetActive(true);
        confirmModal.SetActive(true);
        bathTimeObjs.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void StartBath()
    {
        normalUIObj.SetActive(false);
        confirmModal.SetActive(false);
        bathTimeObjs.SetActive(false);
        bathUIObj.SetActive(true);
        spongeScr.StartBath();
    }

    public void FadeBath()
    {
        //フェードアウト、フェードイン処理がこのまえに入る
        SoundManager.instance.PlaySE(13);
        fadeImageScr.FadeOut(true);
        
        DOVirtual.DelayedCall(1, () => 
        {
            bathUIObj.SetActive(false);
            bathTimeObjs.SetActive(true);
        });
        // 3秒後に"hoge"がログ出力される
        DOVirtual.DelayedCall(2, () =>
        {
            //アニメーション
            //終わったらトーク呼んでExitMeal
            for(int i = 0; i < selifEffectObject.Length; i++)
            {
                selifEffectObject[i].SetActive(true);
                selifEffectText[i].text = bathText[i];

                var _target = selifEffectObject[i].GetComponent<RectTransform>();
                _target.DOScale(0.8f, 0.5f).SetEase(Ease.Linear, 5f)
                .OnComplete(()=>
                {
                    _target.DOScale(0f, 2f).SetEase(Ease.InBack, 3f)
                    .OnComplete(() =>
                    {
                        var plusNostalgia = Random.Range(5,11);
                        nagiStatus.AddNostalgia(plusNostalgia);
                        var statusBuff = Random.Range(1,6);
                        var str1 = "";
                        if(statusBuff == 1)
                        {
                            nagiStatus.AddStatusOne(1, -5);
                            str1 = "凪の協調が 5 上がった！";
                        }
                        else if(statusBuff == 2)
                        {
                            nagiStatus.AddStatusOne(2, -5);
                            str1 = "凪の依存が 5 上がった！";
                        }
                        else if(statusBuff == 3)
                        {
                            nagiStatus.AddStatusOne(3, -5);
                            str1 = "凪の内向が 5 上がった！";
                        }
                        else if(statusBuff == 4)
                        {
                            nagiStatus.AddStatusOne(4, -5);
                            str1 = "凪の思考が 5 上がった！";
                        }
                        else
                        {
                            nagiStatus.AddStatusOne(5, 5);
                            str1 = "凪の温厚が 5 上がった！";
                        }
                        
                        var str2 = "凪のなつき度が " + plusNostalgia + " 上がった！";

                        chatManager.PlayChat(2, (str1 + "\n" + str2), false, true);
                    });
                });
        }});
    }
}
