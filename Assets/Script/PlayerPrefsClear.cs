using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsClear : MonoBehaviour
{
    public void PlayerClear()
    {
        SoundManager.instance.PlaySE(3);
        PlayerPrefs.DeleteAll();
    }
}
