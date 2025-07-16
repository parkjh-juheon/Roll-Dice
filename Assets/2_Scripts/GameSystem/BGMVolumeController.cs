using UnityEngine;
using UnityEngine.UI;

public class BGMVolumeController : MonoBehaviour
{
    public AudioSource bgmSource;   // BGM AudioSource
    public Slider volumeSlider;     // UI Slider

    void Start()
    {
        // 슬라이더 값 변경시 VolumeUpdate 함수 호출
        volumeSlider.onValueChanged.AddListener(VolumeUpdate);

        // 슬라이더 초기값 설정 (현재 오디오 볼륨)
        volumeSlider.value = bgmSource.volume;
    }

    void VolumeUpdate(float value)
    {
        bgmSource.volume = value;
    }
}
