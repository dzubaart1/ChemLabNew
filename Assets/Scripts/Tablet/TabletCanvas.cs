using Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Tablet
{
    public class TabletCanvas : MonoBehaviour
    {
        [SerializeField]
        private Text _taskTitle;
        private TasksCntrl _tasksCollection;
        
        public void Start()
        {
            UpdateText();
            _tasksCollection.Notify += UpdateText;
        }
        

        [Inject]
        public void Construct(TasksCntrl tasksCollection)
        {
            _tasksCollection = tasksCollection;
        }

        private void UpdateText()
        {
            _taskTitle.text = _tasksCollection.CurrentTask().Title;
        }
    }
}