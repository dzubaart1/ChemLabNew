using Data;
using Installers;
using ModestTree;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Canvases
{
    public class TabletCanvasCntrl : CanvasBase
    {
        [SerializeField] private Text _taskNumber;
        [SerializeField] private Text _taskTitle;
        [SerializeField] private Text _taskDescription;
        [SerializeField] private GameObject _deskPanel;
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _deskBtn;
        
        private SignalBus _signalBus;
        private void Start()
        {
            _signalBus.Subscribe<CheckTasksSignal>(UpdateText);
            gameObject.SetActive(false);
        }
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void UpdateText(CheckTasksSignal checkTasksSignal)
        {
            _taskTitle.text = checkTasksSignal.CurrentTask.Title;
            _taskNumber.text = "Задание " + checkTasksSignal.CurrentTask.Number;
            
            if (checkTasksSignal.CurrentTask.TaskDescription == "")
            {
                _deskBtn.SetActive(false);
            }
            else
            {
                _deskBtn.SetActive(true);
                _taskDescription.text = checkTasksSignal.CurrentTask.TaskDescription;
            }
        }

        public void SaveSceneState()
        {
            _signalBus.Fire<SaveSignal>();
        }

        public void LoadSceneState()
        {
            _signalBus.Fire<LoadSignal>();
        }

        public void ToogleDescriptionPanel()
        {
            _deskPanel.SetActive(!_deskPanel.activeSelf);
            _mainPanel.SetActive(!_deskPanel.activeSelf);
        }
    }
}