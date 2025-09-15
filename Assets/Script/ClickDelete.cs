using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClickDelete : MonoBehaviour
{
    public void DeleteThis () {
        this.gameObject.SetActive(false);
    }
}
