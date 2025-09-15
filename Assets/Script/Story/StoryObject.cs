using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//セリフや表情の入ったスクリプタブルオブジェクトを１まとまりで持っておく
//これを、マネージャーに持たせる
public class StoryObject : MonoBehaviour
{
    public TextScriptableObject[] StoryScriptables;
}