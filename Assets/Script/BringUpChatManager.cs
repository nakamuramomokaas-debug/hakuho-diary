
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using ngroDefine;

public class BringUpChatManager : MonoBehaviour
{
    [SerializeField] GameObject storyScene;
    [SerializeField] FadeImage fadeImageScr;
    [SerializeField] TurnChange dayEndfadeScr;
    [SerializeField] GameObject normalUIObj;
    [SerializeField] GameObject chatUIObj;
    [SerializeField] MealManager mealManager;
    [SerializeField] BathManager bathManager;
    [SerializeField] NagiStatus nagiStatus;
    [SerializeField] TalkManager talkManager;

    [SerializeField] GameSceneManager gameSceneScr;
    [SerializeField] MyData myDataSc;
    [SerializeField] TalkChoiceManager choiceManagerSc;

    [SerializeField] StoryObject[] storys;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI serifText;
    [SerializeField] GameObject nameObj;
    //[SerializeField] Image NowCharaBody;
    //[SerializeField] Image NowCharaFace;
    [SerializeField] Image nextSerif;
    //[SerializeField] Image NowStill;

    // [SerializeField] Button autoButton;
    // [SerializeField] Button skipButton;
    // [SerializeField] Button logButton;
    // [SerializeField] GameObject skipCheckToast;
    // [SerializeField] GameObject logToast;

    //[SerializeField] Sprite[] BodyImages;//立ち絵を全部持っている
    //[SerializeField] Sprite[] FaceImages;//表情を全部持っている
    //[SerializeField] Sprite[] StillImages;//スチルを持っている
    
    [SerializeField] private float delayDuration = 0.1f;// 次の文字を表示するまでの時間[s]
    [SerializeField] private float delayAutoNextSerif = 1f;// 次の文字を表示するまでの時間[s]
    
    private Coroutine showCoroutine;

    private StoryObject storyObject;
    private int nowPageIndex; 
    private int nowSelifIndex; 


    int nowStoryIndex;//今回のストーリー
    private int pageNum; //今回のページ数
    private int serifNum;//今のページのセリフ数
    bool isSerifAnimation;//セリフのアニメーション中か

    bool isAuto;
    bool isSkipFlag;
    bool isChoice;//選択肢の場合は選択肢を選ぶまで次にいかない
    string[] nowChoices = new string[3];//選択肢（次に選択肢が来るまで保存される）

    bool isMeal;
    bool isBath;
    Tweener tween;

    //選ばれたら持っておく
    int nowChoiceNum;
    PersonalityStatus nowPersonal;

    void OnEnable()
    {
        tween.Kill();
    }
    
    //ChatのLoadとStartを一括にしたような感じ（引数はStorysのIndex番号で受け取る）
    public void PlayChat(int storyIndex, string chat = "", bool ismeal = false, bool isbath = false)
    {
        isMeal = ismeal;
        isBath = isbath;
        nowStoryIndex = storyIndex;
        nowSelifIndex = 0;
        nowPageIndex = 0;

        normalUIObj.SetActive(false);
        chatUIObj.SetActive(true);
        
        tween = nextSerif.rectTransform.DOAnchorPos(new Vector2(800f, 0f), 0.5f).SetLoops(-1, LoopType.Yoyo).SetLink(nextSerif.gameObject);//Re:数値べた書きよくない
        LoadSroty(storyIndex, chat);
    }

    //今回読み込まれるストーリーを他の画面から受け取る
    public void LoadSroty(int storyIndex, string chat = "")
    {
        nowStoryIndex = storyIndex;
        nowSelifIndex = 0;
        nowPageIndex = 0;

        storyObject = storys[storyIndex];
        pageNum = storyObject.StoryScriptables.Length;
        serifNum = storyObject.StoryScriptables[nowPageIndex].Selif.Length;
        //NowCharaBody.sprite = BodyImages[(int)storyObject.StoryScriptables[nowPageIndex].CharaBody];
        //NowCharaFace.sprite = FaceImages[(int)storyObject.StoryScriptables[nowPageIndex].CharaFace];

        if(storyObject.StoryScriptables[nowPageIndex].CharaId == EnumDefinition.Characters.NONE)
        {
            nameObj.SetActive(false);
        }
        else 
        {
            nameObj.SetActive(true);
            if(storyObject.StoryScriptables[nowPageIndex].CharaId.ToString() == "NONE") nameText.text = "";
            else nameText.text = storyObject.StoryScriptables[nowPageIndex].CharaId.ToString(); 
        }
        //nameText.text = storyObject.StoryScriptables[nowPageIndex].CharaId.ToString();
        
        //Debug.Log(storyObject.StoryScriptables[nowPageIndex].Selif[nowSelifIndex]);
        if(chat != "")
        {
            serifText.text = chat;
        }
        else serifText.text = storyObject.StoryScriptables[nowPageIndex].Selif[nowSelifIndex];

        Show();
    }
    
    //文字送り関数
    public void Show()
    {
        nextSerif.gameObject.SetActive(false);
        isSerifAnimation = true;

        // 前回の演出処理が走っていたら、停止
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        // １文字ずつ表示する演出のコルーチンを実行する
        showCoroutine = StartCoroutine(ShowCoroutine());
    }

    // １文字ずつ表示する演出のコルーチン
    private IEnumerator ShowCoroutine()
    {
        // 待機用コルーチン
        // GC Allocを最小化するためキャッシュしておく
        var delay = new WaitForSeconds(delayDuration);

        // テキスト全体の長さ
        var length = serifText.text.Length;
        
        // １文字ずつ表示する演出
        for (var i = 0; i < length; i++)
        {
            // 徐々に表示文字数を増やしていく
            serifText.maxVisibleCharacters = i;
            
            // 一定時間待機
            yield return delay;
        }

        // 演出が終わったら全ての文字を表示する
        serifText.maxVisibleCharacters = length;
        nextSerif.gameObject.SetActive(true);

        isSerifAnimation = false;
        showCoroutine = null;
        if(isAuto && !isSkipFlag)
        {
            nextSerif.gameObject.SetActive(false);
            Invoke(nameof(NextSerif), delayAutoNextSerif);
        } 
    }

    //クリックされたら次のページへ
    public void ClickSerif()
    {
        SoundManager.instance.PlaySE(8);

        //選択肢の場合は進まない
        if(isChoice) return;

        //文字送り演出コルーチンをとめる
        if(isSerifAnimation)
        {
            //Debug.Log("クリック：文字演出止める");
            StopCoroutine(showCoroutine);
            serifText.maxVisibleCharacters = serifText.text.Length;
            nextSerif.gameObject.SetActive(true);
            isSerifAnimation = false;
        }
        //文字送り途中じゃなかったら次へ進む
        else
        {
            //Debug.Log("クリック：次へ");
            NextSerif();
        }
    }

    //次のセリフを表示し、今のセリフをログに送る
    void NextSerif()
    {
        nextSerif.gameObject.SetActive(false);
        if(nowSelifIndex < serifNum - 1)//次のセリフへ
        {
            //Debug.Log("次のセリフへ");
            nowSelifIndex++;
        }
        else if(nowSelifIndex >= serifNum -1 && nowPageIndex < pageNum - 1)//１ページ送る
        {
            //Debug.Log("次のページへ");
            nowSelifIndex = 0;
            nowPageIndex++;
            serifNum = storyObject.StoryScriptables[nowPageIndex].Selif.Length;
            //StillCheck();
            SetChoises();
            //NowCharaBody.sprite = BodyImages[(int)storyObject.StoryScriptables[nowPageIndex].CharaBody];
            //NowCharaFace.sprite = FaceImages[(int)storyObject.StoryScriptables[nowPageIndex].CharaFace];
        }
        else//ストーリー終了！
        {
            nowSelifIndex = 0;
            serifNum = storyObject.StoryScriptables[nowPageIndex].Selif.Length;
            
            if(!SetChoises())
            {
                //Debug.Log("チャット終了");
                nameText.text = "";
                serifText.text = "";
                EndChat();
                return;
            }
            else
            {
                return;
            }
        }

        nameText.text = storyObject.StoryScriptables[nowPageIndex].CharaId.ToString();
        if(storyObject.StoryScriptables[nowPageIndex].Selif[nowSelifIndex] == "NONE")
        {
            serifText.text = "凪は進化した";
        }
        else
        {
            serifText.text = storyObject.StoryScriptables[nowPageIndex].Selif[nowSelifIndex];
        } 
        Show();
    }

    public void ChangeAutoStatus()
    {
        isAuto = !isAuto;

        if(isChoice) return;
        if(isAuto)
        {
            SoundManager.instance.PlaySE(4);
            if(nextSerif.gameObject.activeSelf) NextSerif();
        } 
        else
        {
            SoundManager.instance.PlaySE(6);
        } 
    }

    //選択肢がクリックされたら
    public void SetChoice(int choiceNum, PersonalityStatus personal)
    {
        SoundManager.instance.PlaySE(7);
        isChoice = false;
        
        var text = storyObject.StoryScriptables[nowPageIndex].AnswerSerif[choiceNum];

        nowChoiceNum = choiceNum;
        nowPersonal = personal;

        talkManager.Answer(text);

        //talkManager.Choice(choiceNum, personal);
        
        //EndChat();
        //ChoiceNumをどう使うかは自由
        //他のスクリプトがこのSetChoiseがクリックされたあとにここの関数の値をチェックして、自由に扱うのがきれいかも
        // これはそのままセーブデータにいれるテスト
        // myDataSc.SetDataString("LikeChara", nowChoices[choiceNum]);
        // Debug.Log("選択：" + myDataSc.GetDataString("LikeChara"));
    }

    //ストーリーが最後までいったら保存してお世話ページへ戻る
    void EndChat()
    {
        chatUIObj.SetActive(false);

        if(nowStoryIndex == 0)//進化は0
        {
           nagiStatus.EvolutionShow();
        }
        else if( nowStoryIndex == 6)//今日を終えるのは6
        {
            normalUIObj.SetActive(true);
            dayEndfadeScr.gameObject.SetActive(true);
            dayEndfadeScr.RotateFadeOut();
            SoundManager.instance.StopBGM();
            SoundManager.instance.PlaySE(5);
            DOVirtual.DelayedCall(2, () => 
            {
                storyScene.SetActive(true);
                transform.parent.gameObject.SetActive(false);
            });
        }
        else if (nowStoryIndex == 1 || nowStoryIndex == 7)//凪の進化後は1/７はルームステータスアップの後
        {
            nagiStatus.TurnEndMoney();
        }
        else if(nowStoryIndex == 9)//9はお金の後
        {
            PlayChat(6);//１日が終わる
        }
        else if(nowStoryIndex == 8)//受け答えの後にステータス上がる場合
        {
            talkManager.Choice(nowChoiceNum, nowPersonal);
        }
        else//トーク後のステータスアップ2はここ
        {
            normalUIObj.SetActive(true);
            if(isMeal) mealManager.ExitMeal();
            else if(isBath) bathManager.ExitBathConfirm(true);
            talkManager.EndTalk();
        }
    }

    //ログオブジェクトを追加
    void AddLog()
    {

    }

    // //スチルチェックしてスチルなら表示
    // void StillCheck()
    // {
    //     //スチルがあれば表示（スチルはさむときはフェードイン、フェードアウト入れたい）
    //     var stillNum = storyObject.StoryScriptables[nowPageIndex].Still;
    //     //NowStill.gameObject.SetActive(stillNum != -1);
    //     // if(stillNum != -1)
    //     // {
    //     //     NowStill.sprite = StillImages[stillNum];
    //     // }
    // }

    //選択肢なら表示
    bool SetChoises()
    {
        if(!storyObject.StoryScriptables[nowPageIndex].isChoice) return false;
        
        //Debug.Log("クリック：選択肢表示");
        isChoice = true;
        //Debug.Log("選択肢の数" + storyObject.StoryScriptables[nowPageIndex].Choices.Length);
        choiceManagerSc.SetChoises(storyObject.StoryScriptables[nowPageIndex].Choices,storyObject.StoryScriptables[nowPageIndex].PersonalityStatus);
        for(int i = 0; i < storyObject.StoryScriptables[nowPageIndex].Choices.Length; i++)
        {
            nowChoices[i] = storyObject.StoryScriptables[nowPageIndex].Choices[i];
        }

        return true;
    }
}
