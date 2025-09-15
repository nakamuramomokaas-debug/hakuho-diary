using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゴミオブジェクトを自動生成
public class GarbageGenerator : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject garbageObject;
    [SerializeField] float maxPosX = 500.0f;
    [SerializeField] float minPosX = -500.0f;
    [SerializeField] float maxPosY = -300.0f;
    [SerializeField] float minPosY = -500.0f;
    [SerializeField] float minAppearTime = 30.0f;
    [SerializeField] float maxAppearTime = 15.0f;
    //[SerializeField] int maxQuantity = 5;//一度に出現する最大値
    private Coroutine coroutine;

    void OnEnable() 
    {
        coroutine = StartCoroutine(LoopGarbageGenerate());
    }

    IEnumerator LoopGarbageGenerate()
    {
        while (true)
        {
            var nextTime = Random.Range(minAppearTime, maxAppearTime);
            yield return new WaitForSeconds(nextTime);

            GarbageGenerate();
        }
    }

    void GarbageGenerate()
    {
        var tagObjects = GameObject.FindGameObjectsWithTag("Garbage");
        if(tagObjects.Length >= 5) return;

        var prefab = (GameObject)Instantiate(garbageObject);
        var xPos = Random.Range(minPosX, maxPosX);
        var yPos = Random.Range(minPosY, maxPosY);
        prefab.transform.SetParent(canvas.transform, false);
        prefab.GetComponent<RectTransform>().anchoredPosition = new Vector3(xPos, yPos,0.0f);
    }
}
