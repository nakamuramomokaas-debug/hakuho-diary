using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using ngroDefine;
using UnityEngine.UI;

//RoomChangeManagerを踏襲して作る
public class MealManager : MonoBehaviour
{
    [SerializeField] NagiHpManager nagiHpManager;
    [SerializeField] GameObject lockObj;
    [SerializeField] BringUpChatManager chatManager;
    [SerializeField] GameObject gabageGenerator;
    [SerializeField] NagisItem nagisItemScr;
    [SerializeField] RectTransform nagiTransform;
    [SerializeField] CalenderManager calenderManager;
    [SerializeField] HandController nagiHandController;
    [SerializeField] GameObject normalUIObj;
    //[SerializeField] GameObject nagiUIObj;
    [SerializeField] SetMealViewContents mealViewScr;
    [SerializeField] NagiStatus nagiStatus;
    [SerializeField] NagiHpManager nagiHPManager;
    [SerializeField] GameObject[] nagiHpObj;

    [SerializeField] GameObject[] selifEffectObject;
    [SerializeField] TextMeshProUGUI[] selifEffectText;

    Dictionary<Item, int> mealDict = new Dictionary<Item, int>();


    int useHp = 0;//今回のターンの消費HP
    bool isStart = false;

    public int GetMealUseHp(){ return useHp; }

    public void SetLock(int ngHp)
    {
        //Debug.Log(ngHp + "M" + useHp);
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

        useHp = Random.Range(1, 3);//1or2
        for(int i = 0 ; i < nagiHpObj.Length; i++)
        {
            nagiHpObj[i].SetActive(true);
            if(i == useHp - 1) break;
        }
        SetLock(nagiHpManager.GetNagiHp());
    }

    void Start()
    {
        var tmpDict = nagisItemScr.GetConsmptionItems();
        foreach(var item in tmpDict)
        {
            mealDict.Add(item.Key, item.Value);
        }
        SetMealList();
        isStart = true;
    }

    void OnEnable() 
    {
        if(!isStart) return;
        this.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0), 0.3f)
        .SetEase(Ease.OutBack); 

        nagiTransform.position = Vector3.zero;
        nagiHandController.SetIsCanMove(false);

        UpdateMeals();
    }

    void SetMealList()
    {
        var mealKeys = new Item[mealDict.Keys.Count];
        var mealValues = new int[mealDict.Values.Count];
        mealDict.Keys.CopyTo(mealKeys, 0);
        mealDict.Values.CopyTo(mealValues, 0);
        mealViewScr.SetContents(mealKeys, mealValues);
    }

    public void AnimationSerif(Item item)
    {
        //終わったらトーク呼んでExitMeal
        for(int i = 0; i < selifEffectObject.Length; i++)
        {
            if(item.GetNagisComment()[i] == null) continue;
            selifEffectObject[i].SetActive(true);
            selifEffectText[i].text = item.GetNagisComment()[i];

            var _target = selifEffectObject[i].GetComponent<RectTransform>();//アイテムがなくなるアニメーション
            //食べた後のセリフ
            nagiStatus.GetNagiAnimator().SetTrigger("IsEat");
            _target.DOScale(0.8f, 0.5f).SetEase(Ease.Linear, 5f)
            .OnComplete(()=>
            {
                _target.DOScale(0f, 2f).SetEase(Ease.InBack, 3f)
                .OnComplete(() =>
                {
                    //効果の繁栄
                    if(GetBuffUpString(item.GetPersonal()) != "")//パーソナリティ変化形アイテム
                    {
                        nagiStatus.AddStatus(item.GetPersonal());
                        chatManager.PlayChat(2, GetBuffUpString(item.GetPersonal()), true);
                    }
                    else if(item.GetNostalgia() != 0) //なつき度追加系アイテム
                    {
                        nagiStatus.AddNostalgia(item.GetNostalgia());
                        var str = "凪のなつき度が  " + item.GetNostalgia() + " あがった！";
                        chatManager.PlayChat(2, str, true);
                    }
                    else
                    {
                        chatManager.PlayChat(2, "凪はごはんを食べ終わった！", true);
                    }
                });
            });
        }
    }

    string GetBuffUpString(PersonalityStatus personal)
    {
        var tmp1 = 0;
        var tmp2 = 0;
        var str1 = "";
        var str2 = "";

        if(personal.egoist != 0) 
        {
            if(tmp1 == 0)
            {
                tmp1 = Mathf.Abs(personal.egoist);
                if(personal.egoist > 0) str1 = "エゴ";
                else str1 = "協調";
            } 
            else
            {
                tmp2 = Mathf.Abs(personal.egoist);
                if(personal.egoist > 0) str2 = "エゴ";
                else str2 = "協調";
            }
        }
        if(personal.independence != 0) 
        {
            if(tmp1 == 0)
            {
                tmp1 = Mathf.Abs(personal.independence);
                if(personal.independence > 0) str1 = "自立";
                else str1 = "依存";
            } 
            else
            {
                tmp2 = Mathf.Abs(personal.independence);
                if(personal.independence > 0) str2 = "自立";
                else str2 = "依存";
            }
        }
        if(personal.diplomatic != 0) 
        {
            if(tmp1 == 0)
            {
                tmp1 = Mathf.Abs(personal.diplomatic);
                if(personal.diplomatic > 0) str1 = "外交";
                else str1 = "内向";
            } 
            else
            {
                tmp2 = Mathf.Abs(personal.diplomatic);
                if(personal.diplomatic > 0) str2 = "外交";
                else str2 = "内向";
            }
        }
        if(personal.intuition != 0) 
        {
            if(tmp1 == 0)
            {
                tmp1 = Mathf.Abs(personal.intuition);
                if(personal.intuition > 0) str1 = "直観";
                else str1 = "思考";
            } 
            else
            {
                tmp2 = Mathf.Abs(personal.intuition);
                if(personal.intuition > 0) str2 = "直観";
                else str2 = "思考";
            }
        }
        if(personal.Gentleness != 0) 
        {
            if(tmp1 == 0)
            {
                tmp1 = Mathf.Abs(personal.Gentleness);
                if(personal.Gentleness > 0) str1 = "温厚";
                else str1 = "好戦";
            } 
            else
            {
                tmp2 = Mathf.Abs(personal.Gentleness);
                if(personal.Gentleness > 0) str2 = "温厚";
                else str2 = "好戦";
            }
        }

        if(str1 == "")//ない場合
        {
            return "";
        }
        if(str2 != "")//引数二つの場合
        {
            var str = "凪の" + str1 + "が " + tmp1 + " 上がった！ \n" +
                      "凪の" + str2 + "が " + tmp2 + " 上がった！ \n"; 
            return str;
        }
        else//引数１つの場合
        {
            var str = "凪の" + str1 + "が " + tmp1 + " 上がった！"; 
            return str;
        }
    }

    public void ExitMeal(bool isEat = true)
    {
        if(isEat) nagiHPManager.UseNagiHp(useHp);
        SoundManager.instance.PlaySE(6);
        nagiHandController.SetIsCanMove(true);

        normalUIObj.SetActive(true);
        //nagiUIObj.SetActive(true);
        normalUIObj.GetComponent<NormalUIManager>().ActiveGabages();
        gabageGenerator.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void UpdateMeals()
    {
        //Dictionary全開放して読み込みなおす
        mealDict.Clear();
        var tmpDict = nagisItemScr.GetConsmptionItems();
        foreach(var item in tmpDict)
        {
            mealDict.Add(item.Key, item.Value);
        }
        SetMealList();

        mealViewScr.gameObject.SetActive(true);
    }
}
