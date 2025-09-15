using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ChatManager : MonoBehaviour
{
    [SerializeField] GameObject bringupScene;
    [SerializeField] FadeImage fadeImageScr;
    [SerializeField] StoryTitle storyTitleScr;
    [SerializeField] GameSceneManager gameSceneScr;
    [SerializeField] ChatLogManager chatLogManagerSc;
    [SerializeField] MyData myDataScr;
    [SerializeField] ChoiceManager choiceManagerSc;

    [SerializeField] StoryObject[] storys;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI serifText;
    [SerializeField] GameObject nameObj;
    [SerializeField] Image BGImage;
    [SerializeField] Image NowCharaImage;
    [SerializeField] Image nextSerif;
    [SerializeField] Image NowStill;

    [SerializeField] Button autoButton;
    [SerializeField] Button skipButton;
    [SerializeField] Button logButton;
    [SerializeField] GameObject skipCheckToast;
    [SerializeField] GameObject logToast;

    [SerializeField] Sprite[] BGImages;//背景を全部持っている。
    [SerializeField] Sprite[] CharaImages;//立ち絵を全部持っている
    [SerializeField] Sprite[] StillImages;//スチルを持っている
    
    [SerializeField] private float delayDuration = 0.1f;// 次の文字を表示するまでの時間[s]
    [SerializeField] private float delayAutoNextSerif = 1f;// 次の文字を表示するまでの時間[s]
    [SerializeField] GameObject ThankYouPlayingObj;

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

    bool isFade;
    Tweener tween;

    bool isReadStory = false;//読み返し機能中か？
    int readStoryIndex = -1;

    void OnEnable()
    {
        nowPageIndex = 0;
        nowSelifIndex = 0;

        isSkipFlag = false;
        skipCheckToast.SetActive(false);

        tween.Kill();
        tween = nextSerif.rectTransform.DOAnchorPos(new Vector2(800f, 0f), 0.5f).SetLoops(-1, LoopType.Yoyo).SetLink(nextSerif.gameObject);//Re:数値べた書きよくない
        
        fadeImageScr.FadeIn();
        if(isReadStory) return; 
        else if(myDataScr.GetDataInt("StoryIndex") == -99999) LoadStory(0);
        else LoadStory(myDataScr.GetDataInt("StoryIndex"));
    }

    public void StoryRead(int index)
    {
        isReadStory = true;
        readStoryIndex = index;
        LoadStory(readStoryIndex);
    }

    void StoryToStory()//ストーリーが終わってまたストーリーの時
    {
        nowPageIndex = 0;
        nowSelifIndex = 0;

        isSkipFlag = false;
        skipCheckToast.SetActive(false);
        
        tween.Kill();
        tween = nextSerif.rectTransform.DOAnchorPos(new Vector2(800f, 0f), 0.5f).SetLoops(-1, LoopType.Yoyo).SetLink(nextSerif.gameObject);//Re:数値べた書きよくない
        
        fadeImageScr.FadeIn();
        if(myDataScr.GetDataInt("StoryIndex") == -99999) LoadStory(0);
        else LoadStory(myDataScr.GetDataInt("StoryIndex"));
    }

    //今回読み込まれるストーリーを他の画面から受け取る
    public void LoadStory(int storyIndex)
    {
        if(storyIndex >= 12)//エンディング
        {
            SoundManager.instance.StopBGM();
            SoundManager.instance.PlayBGM(4);
            ThankYouPlayingObj.SetActive(true);
            return;
        }

        //Debug.Log("ロードストーリー： " + storyIndex);
        storyTitleScr.gameObject.SetActive(true);
        storyTitleScr.SetStoryTitle(storyIndex);

        nowStoryIndex = storyIndex;
        nowSelifIndex = 0;
        nowPageIndex = 0;

        storyObject = storys[storyIndex];
        pageNum = storyObject.StoryScriptables.Length;
        serifNum = storyObject.StoryScriptables[nowPageIndex].Selif.Length;
        StillCheck();


        BGImage.sprite = BGImages[(int)storyObject.StoryScriptables[nowPageIndex].BGImage];

        if(storyObject.StoryScriptables[nowPageIndex].CharaBodyImage == EnumDefinition.CharacterBody.NONE) NowCharaImage.gameObject.SetActive(false);
        else
        {
            NowCharaImage.gameObject.SetActive(true);
            NowCharaImage.sprite = CharaImages[(int)storyObject.StoryScriptables[nowPageIndex].CharaBodyImage];
        }

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
        
        serifText.text = storyObject.StoryScriptables[nowPageIndex].Selif[nowSelifIndex];

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
           // Debug.Log("クリック：次へ");
            NextSerif();
        }
    }

    //次のセリフを表示し、今のセリフをログに送る
    void NextSerif()
    {
        nextSerif.gameObject.SetActive(false);

        if(nowSelifIndex < serifNum - 1)//次のセリフへ
        {
            nowSelifIndex++;
        }
        else if(nowSelifIndex >= serifNum -1 && nowPageIndex < pageNum - 1)//１ページ送る
        {
            if(nowStoryIndex == 10 && nowPageIndex == 17) SoundManager.instance.PlayBGM(2);//バトル！
            nowSelifIndex = 0;
            nowPageIndex++;
            serifNum = storyObject.StoryScriptables[nowPageIndex].Selif.Length;
            StillCheck();
            SetChoises();
            BGImage.sprite = BGImages[(int)storyObject.StoryScriptables[nowPageIndex].BGImage];
        }
        else//ストーリー終了
        {
            nameText.text = "";
            serifText.text = "";
            EndStory();
            return;
        }

        //Debug.Log("キャラID" + storyObject.StoryScriptables[nowPageIndex].CharaId);
        //Debug.Log("イメージ" + storyObject.StoryScriptables[nowPageIndex].CharaBodyImage);
        if(storyObject.StoryScriptables[nowPageIndex].CharaBodyImage == EnumDefinition.CharacterBody.NONE) NowCharaImage.gameObject.SetActive(false);
        else
        {
            NowCharaImage.gameObject.SetActive(true);
            NowCharaImage.sprite = CharaImages[(int)storyObject.StoryScriptables[nowPageIndex].CharaBodyImage];
        }

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
        serifText.text = storyObject.StoryScriptables[nowPageIndex].Selif[nowSelifIndex];
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
            autoButton.GetComponentInChildren<Image>().color = new Color(223.0f/255.0f, 186.0f/255.0f, 237.0f/255.0f, 255.0f/255.0f);
        } 
        else
        {
            SoundManager.instance.PlaySE(6);
            autoButton.GetComponentInChildren<Image>().color = new Color(255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f);
        } 
    }

    public void SkipCheck()
    {
        SoundManager.instance.PlaySE(4);
        isSkipFlag = true;
        skipCheckToast.SetActive(true);
    }

    public void LogActive()
    {
        SoundManager.instance.PlaySE(4);
        logToast.SetActive(true);
    }
    
    //スキップダイアログを消してストーリーに戻る
    public void ReturnStory()
    {
        SoundManager.instance.PlaySE(6);
        if(isAuto && nextSerif.gameObject.activeSelf)
        {
            nextSerif.gameObject.SetActive(false);
            Invoke(nameof(NextSerif), delayAutoNextSerif);
        } 
        isSkipFlag = false;
        skipCheckToast.SetActive(false);
    }

    public void SkipStory()
    {
        SoundManager.instance.PlaySE(7);
        nameText.text = "";
        serifText.text = "";
        EndStory();
    }

    //選択肢がクリックされたら
    public void SetChoice(int choiceNum)
    {
        //Debug.Log("選択肢クリック");
        SoundManager.instance.PlaySE(7);
        isChoice = false;
        ClickSerif();
        
        //ChoiceNumをどう使うかは自由
        //他のスクリプトがこのSetChoiseがクリックされたあとにここの関数の値をチェックして、自由に扱うのがきれいかも
        // これはそのままセーブデータにいれるテスト
        // myDataScrr.SetDataString("LikeChara", nowChoices[choiceNum]);
        // Debug.Log("選択：" + myDataScrr.GetDataString("LikeChara"));
    }

    //ストーリーが最後までいったら保存してお世話ページへ戻る
    void EndStory()
    {
        if(!isReadStory) myDataScr.SetDataInt("StoryIndex", nowStoryIndex);
        //Debug.Log("今終わったストーリー：" + myDataScr.GetDataInt("StoryIndex"));

        fadeImageScr.FadeOut();
        DOVirtual.DelayedCall(1, () => 
        {
            if(!isReadStory && myDataScr.GetDataInt("StoryIndex") >= 10)
            {
                myDataScr.SetDataInt("StoryIndex", nowStoryIndex + 1);
                fadeImageScr.FadeOut();
                DOVirtual.DelayedCall(1, () => 
                {
                    SoundManager.instance.StopBGM();
                    StoryToStory();
                });
            }
            else
            {
                if(isReadStory) 
                {
                    DOVirtual.DelayedCall(1, () => 
                    {
                        SoundManager.instance.PlayBGM(3);
                    });
                }
                readStoryIndex = -1;
                isReadStory = false;
                SoundManager.instance.StopBGM();
                bringupScene.SetActive(true);
                transform.parent.gameObject.SetActive(false);
            }
        });
    }

    //ログオブジェクトを追加
    void AddLog()
    {

    }

    //スチルチェックしてスチルなら表示
    void StillCheck()
    {
        //スチルがあれば表示（スチルはさむときはフェードイン、フェードアウト入れたい）
        var stillNum = storyObject.StoryScriptables[nowPageIndex].Still;
        NowStill.gameObject.SetActive(stillNum != -1);
        if(stillNum != -1)
        {
            NowStill.sprite = StillImages[stillNum];
        }
    }

    //選択肢なら表示
    void SetChoises()
    {
        if(!storyObject.StoryScriptables[nowPageIndex].isChoice) return;
        
        //Debug.Log("クリック：選択肢表示");
        isChoice = true;
        choiceManagerSc.SetChoises(storyObject.StoryScriptables[nowPageIndex].Choices);
        for(int i = 0; i < storyObject.StoryScriptables[nowPageIndex].Choices.Length; i++)
        {
            nowChoices[i] = storyObject.StoryScriptables[nowPageIndex].Choices[i];
        }
    }
}