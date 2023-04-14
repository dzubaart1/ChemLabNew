using Data;
using Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Tablet
{
    public class TabletCanvas : MonoBehaviour
    {
        [SerializeField] private Text _taskTitle;
        [SerializeField] private Text _taskNumber;

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