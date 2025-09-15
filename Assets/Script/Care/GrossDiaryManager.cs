using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrossDiaryManager : MonoBehaviour
{
    [SerializeField] GameObject storyGameObject;
    //凪の進化画像全部持っててほしい(現状凪オブジェクトがうまいことやってる)
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeReadStory()
    {
        SoundManager.instance.PlaySE(4);
        this.gameObject.SetActive(false);
        storyGameObject.SetActive(true);
    }
}
