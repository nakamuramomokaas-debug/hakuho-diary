using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

//日付変更モーションとか入れるならここ
public class CalenderManager : MonoBehaviour
{
    [SerializeField] RoomStatusManager roomStatus;
    [SerializeField] GameObject normalUIObj;
    [SerializeField] FadeImage fadeImageScr;
    [SerializeField] TurnChange dayEndfadeScr;
    [SerializeField] BringUpChatManager bringChatManager;
    [SerializeField] NagiStatus nagiStatus;
    [SerializeField] GameSceneManager gameSceneScr;
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] MyData myDataScr;
    [SerializeField] int startDay;//何日前から始まるか
    int nowDay = -1;//何日前から始まるか
    bool isTutorial;

    public bool GetIsTutorial() { return isTutorial; }

    //シーンが読み込まれるたびに呼ばれる
    void Awake() 
    {
        if(myDataScr.GetDataInt("Day") == -99999) //初期でなぜか-99999になる
        {
            //凪のお世話説明を表示
            dayText.text = startDay.ToString();
            nowDay = startDay;
            isTutorial = true;
        }
        else
        {
            nowDay = myDataScr.GetDataInt("Day");
            dayText.text = nowDay.ToString();
        } 
    }

    public void UpdateDay()//ノーマルUIが呼ぶ
    {
        if(myDataScr.GetDataInt("Day") == -99999) //初期でなぜか-99999になる
        {
            //凪のお世話説明を表示
            dayText.text = startDay.ToString();
            nowDay = startDay;
            isTutorial = true;
        }
        else
        {
            nowDay = myDataScr.GetDataInt("Day");
            dayText.text = nowDay.ToString();
        } 
    }

    //ごはんとかお風呂とか他のマネージャーがここの関数を呼ぶ
    public void EndToday()
    {
        //Debug.Log("今日が終わる");
        //進化タイミング
        if(nagiStatus.EvolutionCheck() != EnumDefinition.NagiEvolutionType.None)
        {
            var finishStory = myDataScr.GetDataInt("StoryIndex");
            myDataScr.SetDataInt("StoryIndex", finishStory + 1);
            myDataScr.SetDataInt("Day", nowDay - 1);
            //Debug.Log(nowDay-1);
            bringChatManager.PlayChat(0);
        }
        else
        {
            var finishStory = myDataScr.GetDataInt("StoryIndex");
            myDataScr.SetDataInt("StoryIndex", finishStory + 1);
            myDataScr.SetDataInt("Day", nowDay - 1);
            //Debug.Log(nowDay-1);

            var status = roomStatus.GetRoomStatus();
            StatusUp(status);
        }
    }

    void StatusUp(int[] status)
    {
        bool isChange = false;
        bool isHukusu = false;
        var str1 = "";
        var str2 = "";
        str1 = "凪の部屋によるボーナスをゲット！\n";
        
        if(status[0] != 0)
        {
            isChange = true;
            nagiStatus.AddStatusOne(1, status[0]);
            nagiStatus.AddStatusOne(4, status[0] * -1);
            str2 += "エゴ 思考 ";
        }
        if(status[1] != 0)
        {
            isChange = true;
            nagiStatus.AddStatusOne(2, status[1]);
            nagiStatus.AddStatusOne(5, status[1] * -1);
            if(str2 == null) str2 += "自立 好戦 ";
            else isHukusu = true;
        }
        if(status[2] != 0)
        {
            isChange = true;
            nagiStatus.AddStatusOne(3, status[2]);
            nagiStatus.AddStatusOne(1, status[2] * -1);
            if(str2 == null) str2 += "外交 協調 ";
            else isHukusu = true;
        }
        if(status[3] != 0)
        {
            isChange = true;
            nagiStatus.AddStatusOne(4, status[3]);
            nagiStatus.AddStatusOne(2, status[3] * -1);
            if(str2 == null) str2 += "直観 依存 ";
            else isHukusu = true;
        }
        if(status[4] != 0)
        {
            isChange = true;
            nagiStatus.AddStatusOne(5, status[4]);
            nagiStatus.AddStatusOne(3, status[4] * -1);
            if(str2 == null) str2 += "温厚 内向 ";
            else isHukusu = true;
        }

        if(isChange)
        {
            str1 = "凪の部屋によるボーナスをゲット！\n";
            if(isHukusu) str2 += "などに変化があった！";
            else str2 += "に変化があった！";
            var str = str1 + str2;
            bringChatManager.PlayChat(7, str);
        }
        else
        {
            nagiStatus.TurnEndMoney();
            //bringChatManager.PlayChat(6);
        }
    }
}
