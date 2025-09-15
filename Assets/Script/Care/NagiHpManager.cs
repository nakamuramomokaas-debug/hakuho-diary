using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NagiHpManager : MonoBehaviour
{
    [SerializeField] GameObject[] nagiHpObj;
    [SerializeField] BathManager bathManager;
    [SerializeField] MealManager mealManager;
    [SerializeField] TalkManager talkManager;
    int nagiHp;

    public int GetNagiHp() { return nagiHp; }

    public void StartBringUpScene()
    {
        nagiHp = 3;
        foreach (var hp in nagiHpObj)
        {
            hp.SetActive(true);
        }
        bathManager.SetLock(nagiHp);
        mealManager.SetLock(nagiHp);
        talkManager.SetLock(nagiHp);
    }

    public void UseNagiHp(int useHp)
    {
        nagiHp -= useHp;
        for(int i = nagiHpObj.Length - 1; i >= nagiHp; i--)
        {
            nagiHpObj[i].SetActive(false);
        }
        //HP以下のボタンはグレーアウトして押せなくする
        //HPが0になったらターンチェンジフラグを立てる
        bathManager.SetLock(nagiHp);
        mealManager.SetLock(nagiHp);
        talkManager.SetLock(nagiHp);
    }

    public void AddHp(int add)
    {
        nagiHp += add;
        foreach (var hp in nagiHpObj)
        {
            hp.SetActive(true);
        }
    }
}
