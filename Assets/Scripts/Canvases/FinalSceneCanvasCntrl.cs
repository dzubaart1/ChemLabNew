using System;
using System.Net.Mail;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class FinalSceneCanvasCntrl : MonoBehaviour
{
    [SerializeField] private Text _inputText;
    [SerializeField] private GameObject _sendPanel, _send1Panel, _successPanel, _mainPanel, _resultsPanel;

    [SerializeField] public Object _resultsFile;
    [SerializeField] public Object _papreFile;
    
    string to = "";
    string subject = "Лабораторная работа по формированию колец Лизеганга";
    string body = "Здравствуй! Это Лаборатория инфохимии Университета ИТМО и Центр юзабилити и смешанной реальности Университета ИТМО. В прикрепленном файле отчёт о проделанной работе. Надеемся, время, проведенное с нами, было увлекательным. До скорых встреч!";

    string email = "xxxr771@gmail.com";
    string pwd = "Qw122476";
    string host = "smtp.office365.com";
    int port = 587;

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
    }
    
    public void Send()
    {
        _mainPanel.SetActive(false);
        _sendPanel.SetActive(true);
    }
    public void Back()
    {
        _resultsPanel.SetActive(false);
        _sendPanel.SetActive(false);
        _mainPanel.SetActive(true);
    }
    
    
    public void SendMail()
    {
        to = _inputText.text;
        if (!to.Contains("@"))
        {
            return;
        }
        
        string _fileName = System.IO.Path.Combine(Application.dataPath, "Resources/Paper.pdf");
        string _fileName2 = System.IO.Path.Combine(Application.dataPath, "Resources/Results.pdf");
        
        using(MailMessage mail = new MailMessage(email, to, subject, body))
        {
            mail.Attachments.Add(new Attachment(_fileName));
            mail.Attachments.Add(new Attachment(_fileName2));
            using (SmtpClient smtp = new SmtpClient(host, port))
            {
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(email, pwd);
                smtp.Send(mail);
                Console.WriteLine("Success");
                _send1Panel.SetActive(false);
                _successPanel.SetActive(true);
            }
        }
    }
}
