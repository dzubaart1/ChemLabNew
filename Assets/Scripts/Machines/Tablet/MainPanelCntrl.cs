using Installers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Machines.Tablet
{
    public class MainPanelCntrl : MonoBehaviour
    {
        [SerializeField] private Text _taskNumber;
        [SerializeField] private Text _taskTitle;
        [SerializeField] private Text _taskDescription, _taskWarning;
        [SerializeField] private GameObject _deskPanel, _warnPanel;
        [SerializeField] private GameObject _deskBtn, _warnBtn;
        
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<CheckTasksSignal>(UpdateText);
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
            
        }

        public void ShowDescriptionPanel()
        {
            _deskPanel.SetActive(true);
            _warnPanel.SetActive(false);
        }
        
        public void ShowWarningPanel()
        {
            _deskPanel.SetActive(false);
            _warnPanel.SetActive(true);
        }
    }
}