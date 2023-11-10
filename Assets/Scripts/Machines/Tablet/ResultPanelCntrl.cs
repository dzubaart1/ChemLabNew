using Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanelCntrl : MonoBehaviour
{
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _errorsCount;
    [SerializeField] private Text _errorsDescription;
    private void Start()
    {
        var date = (TasksCntrl.EndTime - TasksCntrl.StartTime);
        
        _timerText.text = date.Hours + ":" + date.Minutes + ":" +date.Seconds;
        _errorsCount.text = TasksCntrl.ErrorTasks.Count.ToString();
        if (TasksCntrl.ErrorTasks.Count == 0)
        {
            _errorsDescription.text = "";
            return;
        }
        
        _errorsDescription.text = "Вы ошиблись в следующих заданиях:\n";
        foreach (var error in TasksCntrl.ErrorTasks)
        {
            _errorsDescription.text += error.Number + ". " + error.Title + "\n";
        }
    }
}
