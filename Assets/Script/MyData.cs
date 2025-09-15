using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PlayerPrefsによる自分のデータ
public class MyData : MonoBehaviour
{
    //stringData
    public void SetDataString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        //Debug.Log("好きなキャラを変更" + value);
    }

    public string GetDataString(string key)
    {
        var value = PlayerPrefs.GetString(key, "Nothing");
        //Debug.Log("好きなキャラは" + value);
        return value;
    }

    //intData
    public void SetDataInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public int GetDataInt(string key)
    {
        var value = PlayerPrefs.GetInt(key, -99999);
        return value;
    }

    //floatData
    public void SetDataFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public float GetDataFloat(string key)
    {
        var value = PlayerPrefs.GetFloat(key, -99999.99999f);
        return value;
    }

    //好きなキャラ key:LikeChara value:string
    //今もっているデータを見る（デバッグ用）
    public void ShowAllData()
    {
        //Debug.Log("好きなキャラ：" + GetDataString("LikeChara"));
    }
}