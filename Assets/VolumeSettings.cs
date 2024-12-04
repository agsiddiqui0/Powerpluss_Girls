using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private string scene;
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat(scene, Mathf.Log10(volume) * 20);
    }
}

