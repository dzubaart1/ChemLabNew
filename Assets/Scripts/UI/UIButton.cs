using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIButton : MonoBehaviour
    {
        public Action ClickBtnEvent;

        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;

        public bool State { get; private set; }

        private void Awake()
        {
            _button.onClick.AddListener(OnClickBtn);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClickBtn);
        }

        private void OnClickBtn()
        {
            ToggleState();
            ClickBtnEvent?.Invoke();
        }

        public void ToggleState()
        {
            State = !State;

            _image.sprite = State ? _onSprite : _offSprite;

            if (_audioSource)
            {
                _audioSource.Play();
            }
        }

        public void LoadState(bool state)
        {
            State = state;

            _image.sprite = State ? _onSprite : _offSprite;
        }
    }
}
