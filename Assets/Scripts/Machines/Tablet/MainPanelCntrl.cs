using System;
using Installers;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

namespace Machines.Tablet
{
    public class MainPanelCntrl : MonoBehaviour
    {
        [SerializeField] private Text _taskNumber;
        [SerializeField] private Text _taskTitle;
        [SerializeField] private Text _taskDescription, _taskWarning;
        [SerializeField] private Image _taskImage;
        [SerializeField] private GameObject _tasksPanel, _deskPanel, _warnPanel, _objDeskPanel;
        [SerializeField] private GameObject _deskBtn, _warnBtn, _objDescBtn;
        [SerializeField] private List<Transform> _scrollviews;
        
        [SerializeField] private AudioController _audioController;
        [SerializeField] private AudioSource _music;
        private bool _musicIsOn = true, _audioIsOn = true;
        [SerializeField] private List<Image> _images;
        [SerializeField] private List<Sprite> _sprites;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<CheckTasksSignal>(UpdateText);
        }

        private void Awake()
        {
            _audioController = FindObjectOfType<AudioController>();
        }

        private void UpdateText(CheckTasksSignal checkTasksSignal)
        {
            _taskTitle.text = checkTasksSignal.CurrentTask.Title;
            _taskNumber.text = "Задание " + checkTasksSignal.CurrentTask.Number;
            
            if (string.IsNullOrWhiteSpace(checkTasksSignal.CurrentTask.TaskDescription))
            {
                _deskBtn.SetActive(false);
            }
            else
            {
                _deskBtn.SetActive(true);
                _taskDescription.text = checkTasksSignal.CurrentTask.TaskDescription;
            }
            
            if (string.IsNullOrWhiteSpace(checkTasksSignal.CurrentTask.TaskWarning))
            {
                _warnBtn.SetActive(false);
            }
            else
            {
                _warnBtn.SetActive(true);
                _taskWarning.text = checkTasksSignal.CurrentTask.TaskWarning;
            }
            
            if (IsNull(checkTasksSignal.CurrentTask.TaskImage))
            {
                _objDescBtn.SetActive(false);
            }
            else
            {
                _objDescBtn.SetActive(true);
                _taskImage.sprite = checkTasksSignal.CurrentTask.TaskImage;
            }
            ToogleAllPanelsOff();
            SetAllScrollviewUp();
        }

        public void ToogleDescriptionPanel()
        {
            _deskPanel.SetActive(!_deskPanel.activeSelf);
            _tasksPanel.SetActive(!_deskPanel.activeSelf);
            _warnPanel.SetActive(false);
            _objDeskPanel.SetActive(false);
            _images[0].sprite = _deskPanel.activeSelf?_sprites[1]:_sprites[0];
            _images[1].sprite = _sprites[2];
            _images[2].sprite = _sprites[4];
        }
        
        public void ToogleWarningPanel()
        {
            _warnPanel.SetActive(!_warnPanel.activeSelf);
            _tasksPanel.SetActive(!_warnPanel.activeSelf);
            _deskPanel.SetActive(false);
            _objDeskPanel.SetActive(false);
            _images[1].sprite = _warnPanel.activeSelf?_sprites[3]:_sprites[2];
            _images[0].sprite = _sprites[0];
            _images[2].sprite = _sprites[4];
        }
        
        public void ToogleObjDescriptionPanel()
        {
            _objDeskPanel.SetActive(!_objDeskPanel.activeSelf);
            _tasksPanel.SetActive(!_objDeskPanel.activeSelf);
            _deskPanel.SetActive(false); 
            _warnPanel.SetActive(false);
            _images[2].sprite = _objDeskPanel.activeSelf?_sprites[5]:_sprites[4];
            _images[0].sprite = _sprites[0];
            _images[1].sprite = _sprites[2];
        }

        public void ToogleAllPanelsOff()
        {
            _deskPanel.SetActive(false); 
            _warnPanel.SetActive(false);
            _objDeskPanel.SetActive(false);
            _tasksPanel.SetActive(true);
            _images[0].sprite = _sprites[0];
            _images[1].sprite = _sprites[2];
            _images[2].sprite = _sprites[4];
        }

        public void SetAllScrollviewUp()
        {
            foreach (var scrollView in _scrollviews)
            {
                scrollView.localPosition = new Vector3(scrollView.localPosition.x, 0, scrollView.localPosition.z);
            }
        }

        public void SetAudio()
        {
            if (_audioIsOn)
            {
                foreach (var audio in _audioController.Audios)
                {
                    audio.mute = true;
                }
            }
            else
            {
                foreach (var audio in _audioController.Audios)
                {
                    audio.mute = false;
                }
            }
            _audioIsOn = !_audioIsOn;
            _images[3].sprite = _audioIsOn?_sprites[7]:_sprites[6];
        }

        public void SetMusic()
        {
            _music.mute = _musicIsOn;
            _musicIsOn = !_musicIsOn;
            _images[4].sprite = _musicIsOn?_sprites[9]:_sprites[8];
        }
        
        public static bool IsNull<T>(T myObject, string message = "") where T : class
        {
            switch (myObject)
            {
                case Object obj when !obj:
                    return true;
                case null:
                    return true;
                default:
                    return false;
            }
        }
    }
}