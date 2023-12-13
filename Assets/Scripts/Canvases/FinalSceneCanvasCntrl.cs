using System.Collections;
using System.IO;
using System.Net.Mail;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalSceneCanvasCntrl : MonoBehaviour
{
    [SerializeField] private Text _inputText;
    [SerializeField] private GameObject _sendPanel, _send1Panel, _successPanel, _mainPanel, _resultsPanel, _vrKeyboard;
    [SerializeField] private ResultPanelCntrl _resPanelCntrl;

    private string _senderEmail = "xxxr771@gmail.com";
    private string _paperPath, _resultsPath;

    public void Again()
    {
        SceneManager.LoadScene(0);
    }
    
    public void Exit()
    {
        Application.Quit();
    }
    
    public void Results()
    {
        _mainPanel.SetActive(false);
        _resultsPanel.SetActive(true);
        _vrKeyboard.SetActive(false);
    }
    
    public void Send()
    {
        _mainPanel.SetActive(false);
        _sendPanel.SetActive(true);
        _vrKeyboard.SetActive(true);
    }
    public void Back()
    {
        _resultsPanel.SetActive(false);
        _vrKeyboard.SetActive(false);
        _sendPanel.SetActive(false);
        _mainPanel.SetActive(true);
    }

    public void OnClickSendMailBtn()
    {
        StartCoroutine(SendMail());
    }
    
    private IEnumerator SendMail()
    {
        if (!_inputText.text.Contains("@"))
        {
            yield break;
        }
        
        var loadingRequest = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, "Paper.pdf"));
        loadingRequest.SendWebRequest();
        
        while (!loadingRequest.isDone) {
            if (loadingRequest.isNetworkError || loadingRequest.isHttpError) {
                break;
            }

            yield return null;
        }
        if (loadingRequest.isNetworkError || loadingRequest.isHttpError) {

        } else
        {
            _paperPath = Path.Combine(Application.persistentDataPath, "Paper.pdf");
            File.WriteAllBytes(_paperPath, loadingRequest.downloadHandler.data);
        }
        
        loadingRequest = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, "Results.pdf"));
        loadingRequest.SendWebRequest();
        
        while (!loadingRequest.isDone) {
            if (loadingRequest.isNetworkError || loadingRequest.isHttpError) {
                break;
            }
            
            yield return null;
        }
        if (loadingRequest.isNetworkError || loadingRequest.isHttpError) {

        } else
        {
            _resultsPath = Path.Combine(Application.persistentDataPath, "Results.pdf");
            File.WriteAllBytes(_resultsPath, loadingRequest.downloadHandler.data);
        }
        
        string subject = "Лабораторная работа по формированию колец Лизеганга";
        string body = "Здравствуй! Это Лаборатория инфохимии Университета ИТМО и Центр юзабилити и смешанной реальности Университета ИТМО. Далее ваши результаты по лабораторной работе по формированию колец Лизеганга.\n" +
               "Время:" + _resPanelCntrl.GetTimerText() + "\nОшибки:" + _resPanelCntrl.GetErrorsCount()+ "\n " + _resPanelCntrl.GetErrorsDescription() + "\n " +
               "В прикрепленном файле отчёт по проделанной работе. Надеемся, время, проведенное с нами, было увлекательным. До скорых встреч!";
        using(MailMessage mail = new MailMessage(_senderEmail, _inputText.text, subject, body))
        {
            mail.Attachments.Add(new Attachment(_paperPath));
            mail.Attachments.Add(new Attachment(_resultsPath));
            using (SmtpClient smtp = new SmtpClient("smtp.office365.com", 587))
            {
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(_senderEmail, "Qw122476");
                smtp.Send(mail);
                _successPanel.SetActive(true);
            }
        }
    }
}
