using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public Slider backgroundVolumeSlider;
    public Slider effectVolumeSlider;

    private void Start()
    {
        backgroundVolumeSlider.value = AudioManager.Instance.BackGroundVolume;
        effectVolumeSlider.value = AudioManager.Instance.EffectVolume;

        backgroundVolumeSlider.onValueChanged.AddListener(HandleBackgroundVolumeChanged);
        effectVolumeSlider.onValueChanged.AddListener(HandleEffectVolumeChanged);
    }

    private void HandleBackgroundVolumeChanged(float volume)
    {
        AudioManager.Instance.BackGroundVolume = volume;
        // 필요한 경우 여기에서 AudioManager의 볼륨 변경을 적용할 수 있습니다.
    }

    private void HandleEffectVolumeChanged(float volume)
    {
        AudioManager.Instance.EffectVolume = volume;
        // 필요한 경우 여기에서 AudioManager의 볼륨 변경을 적용할 수 있습니다.
    }
}
