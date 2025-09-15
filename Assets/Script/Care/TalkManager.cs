using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ngroDefine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    [SerializeField] NagiHpManager nagiHpManager;
    [SerializeField] GameObject lockObj;
    [SerializeField] CalenderManager calenderManager;
    [SerializeField] GameObject gabageGenerator;
    [SerializeField] GameObject normalUIObj;
    [SerializeField] BringUpChatManager chatManager;
    [SerializeField] NagiHpManager nagiHPManager;
    [SerializeField] NagiStatus nagiStatus;
    [SerializeField] GameObject[] nagiHpObj;
    int useHp = 0;//今回のターンの消費HP
    bool isNagiTalk;
    public int GetBathUseHp(){ return useHp; }

    public void SetLock(int ngHp)
    {
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

        useHp = Random.Range(1, 4);//1or3
        for(int i = 0 ; i < nagiHpObj.Length; i++)
        {
            nagiHpObj[i].SetActive(true);
            if(i == useHp - 1) break;
        }
        SetLock(nagiHpManager.GetNagiHp());
    }

    //凪の性格タイプをみて、それに合わせてトークをする(NormalUIから呼ばれる)
    public void StartTalk()
    {
        isNagiTalk = true;
        var talkNum = 0;
        switch(nagiStatus.GetNagiType())
        {
            case EnumDefinition.NagiEvolutionType.Normal:
                talkNum = Random.Range(10,14);
            break;
            case EnumDefinition.NagiEvolutionType.Dog:
                talkNum = Random.Range(10,16);
            break;
            case EnumDefinition.NagiEvolutionType.Cat:
                talkNum = Random.Range(10,16);
            break;
            default:
                talkNum = Random.Range(14,18);
            break;
        }

        chatManager.PlayChat(talkNum);
    }

    public void EndTalk()
    {
        if(isNagiTalk)
        {
            nagiHPManager.UseNagiHp(useHp);
        } 
        normalUIObj.SetActive(true);
        normalUIObj.GetComponent<NormalUIManager>().ActiveGabages();
        gabageGenerator.SetActive(true);
        isNagiTalk = false;
        this.gameObject.SetActive(false);
    }

    //選ばれた値を受け取って凪に。
    public void Choice(int choiceNum, PersonalityStatus personal)
    {
        nagiStatus.AddStatus(personal);

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

        if(str2 != "")//引数二つの場合
        {
            var str = "凪の" + str1 + "が " + tmp1 + " 上がった！ \n" +
                      "凪の" + str2 + "が " + tmp2 + " 上がった！ \n"; 
            chatManager.PlayChat(2, str);
        }
        else
        {
            var str = "凪の" + str1 + "が " + tmp1 + " 上がった！"; 
            chatManager.PlayChat(2, str);
        }
    }

    //選ばれた値を受け取って凪に。
    public void Answer(string answer)
    {
        chatManager.PlayChat(8, answer);
    }
}
