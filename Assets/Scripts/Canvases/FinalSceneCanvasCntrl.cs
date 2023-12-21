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

    private const string EMAIL_SENDER = "infochemvr@gmail.com";
    private const string EMAIL_SENDER_PASSWORD = "GMyZS6afH6";
    private const string EMAIL_SUBJECT = "Лабораторная работа по формированию колец Лизеганга";
    private const string SMTP_CLIENT = "smtp.office365.com";

    private string _paperPath, _resultsPath;

    public void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void ShowResultsPanel()
    {
        _mainPanel.SetActive(false);
        _resultsPanel.SetActive(true);
        _vrKeyboard.SetActive(false);
    }
    
    public void ShowSendPanel()
    {
        _mainPanel.SetActive(false);
        _sendPanel.SetActive(true);
        _vrKeyboard.SetActive(true);
    }
    public void ShowMainPanel()
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
        
        var paperLoading = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, "Paper.pdf"));
        paperLoading.SendWebRequest();

        var resultsLoading = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, "Results.pdf"));
        resultsLoading.SendWebRequest();

        while (!paperLoading.isDone && !resultsLoading.isDone) {
            if (paperLoading.isNetworkError || paperLoading.isHttpError || resultsLoading.isNetworkError || resultsLoading.isHttpError) {
                // TODO: Выводить ошибку
                break;
            }

            yield return null;
        }

        _paperPath = Path.Combine(Application.persistentDataPath, "Paper.pdf");
        File.WriteAllBytes(_paperPath, paperLoading.downloadHandler.data);

        _resultsPath = Path.Combine(Application.persistentDataPath, "Results.pdf");
        File.WriteAllBytes(_resultsPath, resultsLoading.downloadHandler.data);

        using(MailMessage mail = new MailMessage(EMAIL_SENDER, _inputText.text, EMAIL_SUBJECT, GenerateMessageBody()))
        {
            mail.Attachments.Add(new Attachment(_paperPath));
            mail.Attachments.Add(new Attachment(_resultsPath));
            using (SmtpClient smtp = new SmtpClient(SMTP_CLIENT, 587))
            {
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(EMAIL_SENDER, EMAIL_SENDER_PASSWORD);
                smtp.Send(mail);
                _successPanel.SetActive(true);
                _send1Panel.SetActive(false);
            }
        }
    }

    private string GenerateMessageBody()
    {
        return "Здравствуй! Это Лаборатория инфохимии Университета ИТМО и Центр юзабилити и смешанной реальности Университета ИТМО. Далее ваши результаты по лабораторной работе по формированию колец Лизеганга.\n" +
               "Время:" + _resPanelCntrl.GetTimerText() + "\nОшибки:" + _resPanelCntrl.GetErrorsCount() + "\n " + _resPanelCntrl.GetErrorsDescription() + "\n " +
               "В прикрепленном файле отчёт по проделанной работе. Надеемся, время, проведенное с нами, было увлекательным. До скорых встреч!";
    }
}
