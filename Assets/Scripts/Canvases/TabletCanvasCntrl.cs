using Data;
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

        private TasksCntrl _tasksCntrl;
        private SceneSetter _sceneSetter;
        public void Start()
        {
            UpdateText();
            _tasksCntrl.Notify += UpdateText;
        }
        
        [Inject]
        public void Construct(TasksCntrl tasksCntrl, SceneSetter sceneSetter)
        {
            _tasksCntrl = tasksCntrl;
            _sceneSetter = sceneSetter;
        }

        private void UpdateText()
        {
            _taskTitle.text = _tasksCntrl.CurrentTask().Title;
            _taskNumber.text = "Задание " + _tasksCntrl.CurrentTask().Id;
        }

        public void SaveSceneState()
        {
            _sceneSetter.SaveSceneState();
        }

        public void LoadSceneState()
        {
            _sceneSetter.GetSavedSceneState();
        }
    }
}