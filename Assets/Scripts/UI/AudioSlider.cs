using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private string _group;
    AudioManager _audioManager;
    Slider _slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioManager = AudioManager.Instance;
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        _audioManager.SetVolume(_group, _slider.value);
    }
}
