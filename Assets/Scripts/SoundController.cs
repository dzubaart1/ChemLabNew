using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    private bool _soundsAreMute = false;
    [SerializeField] private Image _soundButton;
    [SerializeField] private List<Sprite> _soundButtonImgs;
    public void SetAllSounds()
    {
        AudioListener.volume = _soundsAreMute ? 1 : 0;
        _soundsAreMute = !_soundsAreMute;
        _soundButton.sprite = _soundsAreMute ? _soundButtonImgs[0] : _soundButtonImgs[1];
    }
}
