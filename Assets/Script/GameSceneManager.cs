using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class GameSceneManager : MonoBehaviour
{
    //ボタンを押したら実行する関数実行するためにはボタンへ関数登録が必要
    //int型の引数numberを宣言
    public void SceneChange(string sceneName)
    {
       // SceneManager.LoadScene(sceneName);
        SceneManager.LoadScene(sceneName);
        SoundManager.instance.StopBGM();
        //SoundManager.instance.PlayBGM(sceneName);
        //SoundManager.instance.PlaySE(seNum);
    }
}