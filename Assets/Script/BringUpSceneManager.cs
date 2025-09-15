using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringUpSceneManager : MonoBehaviour
{
    [SerializeField] NormalUIManager normalUIManager;
    [SerializeField] SleepManager sleepManager;
    [SerializeField] TurnChange turnChange;
    [SerializeField] NagiHpManager nagiHPManager;
    [SerializeField] BathManager bathManager;
    [SerializeField] MealManager mealManager;
    [SerializeField] TalkManager talkManager;

   void OnEnable() 
   {
     normalUIManager.StartBringUpScene();
     sleepManager.StartBringUpScene();
     turnChange.StartBringUpScene();
     nagiHPManager.StartBringUpScene();
     bathManager.StartBringUpScene();
     mealManager.StartBringUpScene();
     talkManager.StartBringUpScene();
   }
}
