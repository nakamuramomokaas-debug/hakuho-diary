using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    //--シングルトン終わり--

    public AudioSource audioSourceBGM;
    public AudioClip[] audioClipsBGM;

    public AudioSource audioSourceLoopSE; 
    public AudioClip[] audioClipsLoopSE; 

    public AudioSource audioSourceSE;
    public AudioClip[] audioClipsSE;

    public void PlayBGM(int num)
    {
        audioSourceBGM.Stop();
        audioSourceBGM.clip = audioClipsBGM[num];
        Debug.Log("BGM" + num);
        // switch (sceneName)
        // {
        //     default:
        //         break;
        //     case "Title":
        //         Debug.Log("BGM1");
        //         audioSourceBGM.clip = audioClipsBGM[0];
        //         break;
        //     case "Game":
        //         Debug.Log("BGM2");
        //         audioSourceBGM.clip = audioClipsBGM[1];
        //         break;
        // }
        audioSourceBGM.Play();
    }

    public void StopBGM()
    {
        audioSourceBGM.Stop();
    }

    public void PlayLoopSe(string SEName)
    {
        if(audioSourceLoopSE.isPlaying) return;
        audioSourceLoopSE.Stop();
        switch (SEName)
        {
            default:
                break;
            case "PickUp":
                audioSourceLoopSE.clip = audioClipsLoopSE[0];
                break;
            case "Sponge":
                audioSourceLoopSE.clip = audioClipsLoopSE[1];
                break;
        }
        audioSourceLoopSE.Play();
    }

    public void StopLoopSE()
    {
        audioSourceLoopSE.Stop();
    }

    public void PlaySE(int index)
    {
        Debug.Log("プレイSE" + index);
        audioSourceSE.PlayOneShot(audioClipsSE[index]); 
    }
}
