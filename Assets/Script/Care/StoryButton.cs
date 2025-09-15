using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryButton : MonoBehaviour
{
    [SerializeField] GameObject lockObject;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] int storyIndex;

    public void SetLockObj(bool isActive)
    {
        lockObject.SetActive(isActive);
    }

    public string GetTitleText() { return title.text; }

    public int GetStoryIndex() { return storyIndex; }
}
