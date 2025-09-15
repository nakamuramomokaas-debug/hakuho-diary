using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//なつき度ゲージ
public class NostalgicSlider : MonoBehaviour
{
    [SerializeField] Slider nostalgiaSlider;

    public void UpdateNostalgia(int num)
    {
        nostalgiaSlider.value = num;
    }
}
