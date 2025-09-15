using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ngroDefine;

[CreateAssetMenu(menuName = "Text_Scriptable")]
public class TextScriptableObject : ScriptableObject
{
    //必須
    public EnumDefinition.Characters CharaId;
    public EnumDefinition.CharacterBody CharaBodyImage;
    public EnumDefinition.BGs BGImage;
    public string[] Selif;

    //ない場合もある
    public int Still = -1; //スチルナンバー。ナシなら-1
    public bool isChoice = false; //０のときのnull防止のため
    public string[] Choices;//選択肢一覧。-1ならなくていい。
    public PersonalityStatus[] PersonalityStatus;//選択肢がある場合は選択肢の数だけ作る
    public string[] AnswerSerif;
}