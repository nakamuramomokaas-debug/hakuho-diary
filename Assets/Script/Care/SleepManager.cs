using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepManager : MonoBehaviour
{
    [SerializeField] GameObject gabageGenerator;
    [SerializeField] GameObject normalUIObj;
    [SerializeField] GameObject confirmModal;
    [SerializeField] CalenderManager calenderManager;

    public void StartBringUpScene()
    {
        confirmModal.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void ExitSleepConfirm()//確認モーダルでいいえを選ぶ
    {
        SoundManager.instance.PlaySE(6);
        normalUIObj.GetComponent<NormalUIManager>().ActiveGabages();
        normalUIObj.SetActive(true);
        gabageGenerator.SetActive(true);
        confirmModal.SetActive(true);
        this.gameObject.SetActive(false);
        FurnitureAllNotMove();
    }

    public void Sleep()
    {
        normalUIObj.SetActive(false);
        confirmModal.SetActive(false);
        calenderManager.EndToday();
    }

    void FurnitureAllNotMove()
    {
        var furnitures = new List<GameObject>(GameObject.FindGameObjectsWithTag("Furniture"));
        if(furnitures == null) return;
        foreach(GameObject furniture in furnitures) 
        {
            var scr = furniture.GetComponent<FurnitureHandController>();
            scr.IsEditRoom = false;
        }
    }
}
