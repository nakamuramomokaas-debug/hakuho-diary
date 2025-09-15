using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatLogManager : MonoBehaviour
{
    [SerializeField] ChatManager chatManagerScr;
    [SerializeField] GameObject logObjectSc;//List１つ１つが持っているスクリプト
    [SerializeField] Sprite[] CharaIcons;

    void Start()
    {
        
    }

    //子要素を１つアクティブにする(ChatManagerが呼ぶ)
    public void AddLog(int charaId, string serif)
    {

    }
}
