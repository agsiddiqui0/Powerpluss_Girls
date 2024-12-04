using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] SliderJoint2D VolSlider;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void ChangeVolume()
    {
        AudioListener.volume = VolSlider.value;
    }

}
