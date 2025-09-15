using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ngroDefine;
using TMPro;

public class NagiStatus : MonoBehaviour
{
    [SerializeField] Slider egoSlider;
    [SerializeField] Slider ziritsuSlider;
    [SerializeField] Slider gaikoSlider;
    [SerializeField] Slider chokkanSlider;
    [SerializeField] Slider onkoSlider;

    [SerializeField] Slider kyouchoSlider;
    [SerializeField] Slider izonSlider;
    [SerializeField] Slider naikoSlider;
    [SerializeField] Slider sikoSlider;
    [SerializeField] Slider kousenSlider;

    [SerializeField] NostalgicSlider nostalgiaUIScr;
    [SerializeField] UpUIEffect nostalgiaEffect;
    [SerializeField] BringUpChatManager chatManager;
    //[SerializeField] Image nagiImage;
    [SerializeField] GameObject[] nagiSitPrefabs;//進化できる凪のObj(入れ替え)
    [SerializeField] GameObject[] nagiCatchPrefabs;//進化できる凪のObj(入れ替え)
    [SerializeField] Sprite[] nagiPhotos;//進化したときのイメージ
    [SerializeField] string[] nagiEvolutionNames;
    [SerializeField] string[] nagiEvolutionTexts;

    [SerializeField] Image nowNagiImage;
    [SerializeField] Image[] nagiEvolutionPhotosHistory;
    [SerializeField] TextMeshProUGUI[] nagiEvolutionName;//進化名前
    [SerializeField] TextMeshProUGUI[] nagiEvolutionText;//進化説明
    [SerializeField] EnumDefinition.NagiEvolutionType[] nagiEvolutionsHistory;
    [SerializeField] Animator nagiAnimator;
    [SerializeField] MyData myDataScr;
    [SerializeField] NagisItem nagisItem;
    int nowEvolutionStep = 0;

    EnumDefinition.NagiEvolutionType nagiEvolutionType = EnumDefinition.NagiEvolutionType.Normal;
    EnumDefinition.NagiMotionType nagiMotionType = EnumDefinition.NagiMotionType.Sit;

    PersonalityStatus parsonalStatus = new PersonalityStatus();
    int nostalgia = 0; //なつき度

    void Start() 
    {
        //凪のステータス初期設定
        parsonalStatus.egoist = 50;//実験でエゴ100にする
        parsonalStatus.independence = 50;
        parsonalStatus.diplomatic = 50;
        parsonalStatus.intuition = 50;
        parsonalStatus.Gentleness = 50;

        nagiEvolutionsHistory[nowEvolutionStep] = EnumDefinition.NagiEvolutionType.Normal;
        nowNagiImage.sprite = nagiPhotos[(int)nagiEvolutionsHistory[nowEvolutionStep]];
        nagiEvolutionPhotosHistory[nowEvolutionStep].sprite = nagiPhotos[(int)nagiEvolutionsHistory[nowEvolutionStep]];
        nagiEvolutionName[nowEvolutionStep].text = nagiEvolutionNames[(int)nagiEvolutionsHistory[nowEvolutionStep]];//進化名前
        nagiEvolutionText[nowEvolutionStep].text = nagiEvolutionTexts[(int)nagiEvolutionsHistory[nowEvolutionStep]];//進化説明

        UpdateSlider();
    }

    public void AddStatus(PersonalityStatus addStatus)
    {
        parsonalStatus.egoist += addStatus.egoist;
        parsonalStatus.independence += addStatus.independence;
        parsonalStatus.diplomatic += addStatus.diplomatic;
        parsonalStatus.intuition += addStatus.intuition;
        parsonalStatus.Gentleness += addStatus.Gentleness;

        //０以下と100以上のときの対応
        if(parsonalStatus.egoist < 0) parsonalStatus.egoist = 0;
        else if(parsonalStatus.egoist > 100) parsonalStatus.egoist = 100;

        if(parsonalStatus.independence < 0) parsonalStatus.independence = 0;
        else if(parsonalStatus.independence > 100) parsonalStatus.independence = 100;

        if(parsonalStatus.diplomatic < 0) parsonalStatus.diplomatic = 0;
        else if(parsonalStatus.diplomatic > 100) parsonalStatus.diplomatic = 100;

        if(parsonalStatus.intuition < 0) parsonalStatus.intuition = 0;
        else if(parsonalStatus.intuition > 100) parsonalStatus.intuition = 100;

        if(parsonalStatus.Gentleness < 0) parsonalStatus.Gentleness = 0;
        else if(parsonalStatus.Gentleness > 100) parsonalStatus.Gentleness = 100;

        UpdateSlider();
    }

    public void AddStatusOne(int i, int num)
    {
        if(i == 1) parsonalStatus.egoist += num;
        else if (i == 2) parsonalStatus.independence += num;
        else if (i == 3) parsonalStatus.diplomatic += num;
        else if (i == 4) parsonalStatus.intuition += num;
        else if (i == 5) parsonalStatus.Gentleness += num;

        //０以下と100以上のときの対応
        if(parsonalStatus.egoist < 0) parsonalStatus.egoist = 0;
        else if(parsonalStatus.egoist > 100) parsonalStatus.egoist = 100;

        if(parsonalStatus.independence < 0) parsonalStatus.independence = 0;
        else if(parsonalStatus.independence > 100) parsonalStatus.independence = 100;

        if(parsonalStatus.diplomatic < 0) parsonalStatus.diplomatic = 0;
        else if(parsonalStatus.diplomatic > 100) parsonalStatus.diplomatic = 100;

        if(parsonalStatus.intuition < 0) parsonalStatus.intuition = 0;
        else if(parsonalStatus.intuition > 100) parsonalStatus.intuition = 100;

        if(parsonalStatus.Gentleness < 0) parsonalStatus.Gentleness = 0;
        else if(parsonalStatus.Gentleness > 100) parsonalStatus.Gentleness = 100;

        UpdateSlider();
    }

    void UpdateSlider()
    {
        egoSlider.value = parsonalStatus.egoist;
        ziritsuSlider.value = parsonalStatus.independence;
        gaikoSlider.value = parsonalStatus.diplomatic; 
        chokkanSlider.value = parsonalStatus.intuition;
        onkoSlider.value = parsonalStatus.Gentleness;

        kyouchoSlider.value = 50 - parsonalStatus.egoist;
        izonSlider.value = 50 - parsonalStatus.independence;
        naikoSlider.value = 50 - parsonalStatus.diplomatic;
        sikoSlider.value = 50 - parsonalStatus.intuition;
        kousenSlider.value = 50 - parsonalStatus.Gentleness;
    }

    public void AddNostalgia(int add)
    {
        nostalgia += add;
        nostalgiaUIScr.UpdateNostalgia(nostalgia);
        nostalgiaEffect.PlayEffect(-240f);
        SoundManager.instance.PlaySE(15);
    }


    //ターン終わりにもらえるお金
    public void TurnEndMoney()
    {
        var rand = Random.Range(1, 3);

        if(nagiEvolutionType == EnumDefinition.NagiEvolutionType.Cat)
        {
            rand = Random.Range(1, 4);
        }
        if(nagiEvolutionType == EnumDefinition.NagiEvolutionType.SportsMan)
        {
            rand = Random.Range(3, 6);
        }
        nagisItem.AddMoney(rand * 10000);
        var str = "凪の生活資金、" + (rand * 10000).ToString() + "円を手に入れた！";

        chatManager.PlayChat(9, str);
        SoundManager.instance.PlaySE(16);
    }

    //進化判定用関数
    public EnumDefinition.NagiEvolutionType EvolutionCheck()
    {
        var nowDay = myDataScr.GetDataInt("Day");
        //Debug.Log("進化判定" + nowDay);
        if(nowDay == 5) 
        {
            var value1 = egoSlider.value + ziritsuSlider.value + naikoSlider.value + sikoSlider.value + kousenSlider.value;//猫
            var value2 = kyouchoSlider.value + izonSlider.value  + gaikoSlider.value + chokkanSlider.value + onkoSlider.value;
            if(value1 > value2) nagiEvolutionType = EnumDefinition.NagiEvolutionType.Cat;
            else nagiEvolutionType = EnumDefinition.NagiEvolutionType.Dog;
            return nagiEvolutionType;
        }
        else if(nowDay == 2)
        {
            nagiEvolutionType = EnumDefinition.NagiEvolutionType.SportsMan;
            return nagiEvolutionType;
        }

        return EnumDefinition.NagiEvolutionType.None;
    }

    //進化して見た目を変える関数
    public void EvolutionShow()
    {
       switch (nagiEvolutionType)
       {
        case  EnumDefinition.NagiEvolutionType.Cat:
            chatManager.PlayChat(1, "凪は にゃぎ に進化した");
            nagiEvolutionsHistory[nowEvolutionStep] = EnumDefinition.NagiEvolutionType.Cat;
            nowEvolutionStep += 1;
            nagiEvolutionType = EnumDefinition.NagiEvolutionType.Cat;
            nowNagiImage.sprite = nagiPhotos[(int)EnumDefinition.NagiEvolutionType.Cat];
            nagiEvolutionPhotosHistory[nowEvolutionStep].sprite = nagiPhotos[(int)EnumDefinition.NagiEvolutionType.Cat];
            nagiEvolutionName[nowEvolutionStep].text = nagiEvolutionNames[(int)EnumDefinition.NagiEvolutionType.Cat];//進化名前
            nagiEvolutionText[nowEvolutionStep].text = nagiEvolutionTexts[(int)EnumDefinition.NagiEvolutionType.Cat];//進化説明
            SetNagiAnimation(EnumDefinition.NagiMotionType.Sit, EnumDefinition.NagiEvolutionType.Cat);
            break;
        case  EnumDefinition.NagiEvolutionType.Dog:
            chatManager.PlayChat(1, "凪は 忠犬凪 に進化した");
            nagiEvolutionsHistory[nowEvolutionStep] = EnumDefinition.NagiEvolutionType.Dog;
            nowEvolutionStep += 1;
            nagiEvolutionType = EnumDefinition.NagiEvolutionType.Dog;
            nowNagiImage.sprite = nagiPhotos[(int)EnumDefinition.NagiEvolutionType.Dog];
            nagiEvolutionPhotosHistory[nowEvolutionStep].sprite = nagiPhotos[(int)EnumDefinition.NagiEvolutionType.Dog];
            nagiEvolutionName[nowEvolutionStep].text = nagiEvolutionNames[(int)EnumDefinition.NagiEvolutionType.Dog];//進化名前
            nagiEvolutionText[nowEvolutionStep].text = nagiEvolutionTexts[(int)EnumDefinition.NagiEvolutionType.Dog];//進化説明
            SetNagiAnimation(EnumDefinition.NagiMotionType.Sit, EnumDefinition.NagiEvolutionType.Dog);
            break;
        case  EnumDefinition.NagiEvolutionType.SportsMan:
            chatManager.PlayChat(1, "凪は スポーツ凪 に進化した");
            nagiEvolutionsHistory[nowEvolutionStep] = EnumDefinition.NagiEvolutionType.SportsMan;
            nowEvolutionStep += 1;
            nagiEvolutionType = EnumDefinition.NagiEvolutionType.SportsMan;
            nowNagiImage.sprite = nagiPhotos[(int)EnumDefinition.NagiEvolutionType.SportsMan];
            nagiEvolutionPhotosHistory[nowEvolutionStep].sprite = nagiPhotos[(int)EnumDefinition.NagiEvolutionType.SportsMan];
            nagiEvolutionName[nowEvolutionStep].text = nagiEvolutionNames[(int)EnumDefinition.NagiEvolutionType.SportsMan];//進化名前
            nagiEvolutionText[nowEvolutionStep].text = nagiEvolutionTexts[(int)EnumDefinition.NagiEvolutionType.SportsMan];//進化説明
            SetNagiAnimation(EnumDefinition.NagiMotionType.Sit, EnumDefinition.NagiEvolutionType.SportsMan);
            break;
        default:
            break;
       }

    }

    public EnumDefinition.NagiEvolutionType GetNagiType()
    {
        return nagiEvolutionType;
    }

    public Animator GetNagiAnimator()//凪の状態入れ替えのところでやりたいね
    {
        return nagiAnimator;
    }

    public void SetNagiAnimation(EnumDefinition.NagiMotionType nagiMotion, EnumDefinition.NagiEvolutionType nagiEvolution)
    {
        nagiMotionType = nagiMotion;
        nagiEvolutionType = nagiEvolution;
        // Debug.Log("凪もーしょん" + nagiMotionType + (int)nagiMotionType);
        // Debug.Log("凪エボリューション" + nagiEvolutionType + (int)nagiEvolutionType);
        if(nagiMotionType == EnumDefinition.NagiMotionType.Sit)
        {
            for(int i = 0; i < nagiSitPrefabs.Length; i++) 
            {
                if(i == ((int)nagiEvolutionType) - 1)
                {
                    nagiSitPrefabs[i].SetActive(true);
                    nagiAnimator = nagiSitPrefabs[i].GetComponent<Animator>();
                } 
                else 
                {
                    nagiSitPrefabs[i].SetActive(false);
                }
            }
            foreach(var nagiCatch in nagiCatchPrefabs)
            {
                nagiCatch.SetActive(false);
            }
        }
        else if(nagiMotionType == EnumDefinition.NagiMotionType.Catch)
        {
            for(int i = 0; i < nagiCatchPrefabs.Length; i++) 
            {
                if(i == ((int)nagiEvolutionType) - 1)
                {
                    nagiCatchPrefabs[i].SetActive(true);
                    nagiAnimator = nagiSitPrefabs[i].GetComponent<Animator>();
                }
                else 
                {
                    nagiCatchPrefabs[i].SetActive(false);
                }
            }
            foreach(var nagiNormal in nagiSitPrefabs) 
            {
                nagiNormal.SetActive(false);
            }
        }
    }
}
