using System;
using System.Net.Mail;
using System.Net;
using UnityEngine;

public class EmailSender: MonoBehaviour
{
    string to = "";
    string subject = "Лабораторная работа по формированию колец Лизеганга";
    string body = "Здравствуй! Это Лаборатория инфохимии Университета ИТМО и Центр юзабилити и смешанной реальности Университета ИТМО. В прикрепленном файле отчёт о проделанной работе. Надеемся, время, проведенное с нами было увлекательным.";

    string email = "xxxr771@gmail.com";
    string pwd = "Qw122476";
    string host = "smtp.office365.com";
    int port = 587;

    public void SendMail()
    {
        if (to == "")
        {
            return;
        }
        using(MailMessage mail = new MailMessage(email, to, subject, body))
        {
            mail.Attachments.Add(new Attachment("C:\\Users\\uxr\\Desktop\\Answer.pdf"));
            using (SmtpClient smtp = new SmtpClient(host, port))
            {
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(email, pwd);
                smtp.Send(mail);
                Console.WriteLine("Success");
            }
        }
    }
}








