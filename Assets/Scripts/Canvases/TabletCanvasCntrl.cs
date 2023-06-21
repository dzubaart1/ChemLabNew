using Data;
using Installers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Canvases
{
    public class TabletCanvasCntrl : CanvasBase
    {
        [SerializeField] private Text _taskNumber;
        [SerializeField] private Text _taskTitle;
        
        private SignalBus _signalBus;
        private void Start()
        {
            _signalBus.Subscribe<CheckTasksSignal>(UpdateText);
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
        }

        public void SaveSceneState()
        {
            _signalBus.Fire<SaveSignal>();
        }

        public void LoadSceneState()
        {
            _signalBus.Fire<LoadSignal>();
        }
    }
}