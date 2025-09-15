using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] NagiStatus nagiStatusScr;
    private int trashCount;
    private int oldTrashCount;
    
    void Start()
    {
        trashCount = 0;
        oldTrashCount = 0;
    }

    void Update() 
    {
        if(trashCount != oldTrashCount && trashCount < 6)//１日５かいまで
        {
            nagiStatusScr.AddNostalgia(1);
            oldTrashCount = trashCount;
        }
    }

    public int TrashCount
    {
        get { return trashCount; }
        set { trashCount = value; }
    }
    
}
