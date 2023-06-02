using Data;
using Installers;
using Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Canvases
{
    public class TabletCanvasCntrl : MonoBehaviour
    {
        [SerializeField] private Text _taskNumber;
        [SerializeField] private Text _taskTitle;
        
        //private SceneSetter _sceneSetter;
        private SignalBus _signalBus;
        private void Start()
        {
            _signalBus.Subscribe<CheckTasksSignal>(UpdateText);
        }
        
        [Inject]
        //public void Construct(SignalBus signalBus, SceneSetter sceneSetter)
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus; 
            //_sceneSetter = sceneSetter;
        }

        private void UpdateText(CheckTasksSignal checkTasksSignal)
        {
            _taskTitle.text = checkTasksSignal.CurrentTask.Title;
            _taskNumber.text = "Задание " + checkTasksSignal.CurrentTask.Id;
        }

        /*public void SaveSceneState()
        {
            _sceneSetter.SaveSceneState();
        }

        public void LoadSceneState()
        {
            _sceneSetter.GetSavedSceneState();
        }*/
    }
}