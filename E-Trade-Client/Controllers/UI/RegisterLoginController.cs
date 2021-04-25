using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCexample.Models;
using System.Web.Security;
using MVCexample.Helper;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MVCexample.Controllers
{
    [AllowAnonymous]
    public class RegisterLoginController : Controller
    {
        MVCexampleEntities m = new MVCexampleEntities();

        public int Register(string name, string lastname, string username, string mail, string password, string passwordrep)
        {
            try
            {
                if (password != passwordrep)
                    return 0;
                else
                {
                    registerTable r = new registerTable();
                    r.name = name;
                    r.lastname = lastname;
                    r.username = username;
                    r.mail = mail;
                    r.password = encryption.MD5Olustur(password);
                    m.registerTable.Add(r);
                    m.SaveChanges();
                    SessionInfo.UserName = r.username;
                    SessionInfo.ID = r.ID;
                    SessionInfo.isLogin = true;
                    FormsAuthentication.SetAuthCookie(r.username, false);
                    if (SessionInfo.basketIsFill == true)
                        return 1;
                    else
                        return 2;
                }
            }
            catch (Exception)
            {
                return -1;
            }

        }

        public int Login(string loginMail, string loginPassword)
        {
            if (!string.IsNullOrEmpty(loginMail) && !string.IsNullOrEmpty(loginPassword))
            {
                string passwordMD5 = encryption.MD5Olustur(loginPassword);
                var find = m.registerTable.Where(i => i.mail == loginMail && i.password == passwordMD5).FirstOrDefault();
                if (find != null)
                {
                    FormsAuthentication.SetAuthCookie(find.username, false);
                    SessionInfo.UserName = find.username;
                    SessionInfo.ID = find.ID;
                    SessionInfo.isLogin = true;
                    if (SessionInfo.basketIsFill == true)
                        return 1;
                    else
                        return 2;
                }
                return -1;
            }
            return 0;
        }

        public string MailSender(string mail)
        {
            var controlAccount = m.registerTable.Where(i => i.mail == mail).FirstOrDefault();
            if (controlAccount == null)
            {
                bool kontrol = EmailKontrol(mail);
                if (kontrol)
                {
                    Random rastgele = new Random();
                    string harfler = "ABCDEFGHIJKLMNOPRSTUVYZWX123456789";
                    string OnayKodu = "";
                    for (int i = 0; i < 6; i++)
                    {
                        OnayKodu += harfler[rastgele.Next(harfler.Length)];
                    }

                    MailMessage _mailmsg = new MailMessage();
                    _mailmsg.IsBodyHtml = true;
                    _mailmsg.From = new MailAddress("adimadimproje@gmail.com");
                    _mailmsg.To.Add(mail);
                    _mailmsg.Subject = "Mail Onay";
                    _mailmsg.Body = "<div style=\"width: 600px;text-align: center;\"><div style=\"font -size: 70px;\" > Wave.com</div><div><img src=\"https://media.istockphoto.com/vectors/wave-icon-vector-id857519722?b=1&k=6&m=857519722&s=612x612&w=0&h=NZJzid5Bl0n32z4VPTppAxNcyuBvaEc5h7WDSWOlBxA=\" ></div><div style=\"font -size: 30px;\"> Mail onay kodunuz : " + OnayKodu + "</div></div>"; ;
                    SmtpClient _smtp = new SmtpClient();
                    _smtp.Host = "smtp.gmail.com";
                    _smtp.Port = 587;
                    NetworkCredential _network = new NetworkCredential("adimadimproje@gmail.com", "gumballgame");
                    _smtp.Credentials = _network;
                    _smtp.EnableSsl = true;
                    _smtp.Send(_mailmsg);
                    return OnayKodu;
                }
                else
                    return string.Empty;
            }
            else
                return "false";
        }

        public int ForgotMailSender(string mail)
        {
           
            var controlAccount = m.registerTable.Where(i => i.mail == mail).FirstOrDefault();
            if (controlAccount != null)
            {
                Random rastgele = new Random();
                string harfler = "ABCDEFGHIJKLMNOPRSTUVYZWX123456789";
                string OnayKodu = "";
                for (int i = 0; i < 9; i++)
                {
                    OnayKodu += harfler[rastgele.Next(harfler.Length)];
                }
                controlAccount.password = encryption.MD5Olustur(OnayKodu);

                m.SaveChanges();
                MailMessage _mailmsg = new MailMessage();
                _mailmsg.IsBodyHtml = true;
                _mailmsg.From = new MailAddress("adimadimproje@gmail.com");
                _mailmsg.To.Add(mail);
                _mailmsg.Subject = "Şifre Sıfırlama";
                _mailmsg.Body = "<div style=\"width: 600px;text-align: center;\"><div style=\"font -size: 70px;\" > Wave.com </div><div><img src=\"https://media.istockphoto.com/vectors/wave-icon-vector-id857519722?b=1&k=6&m=857519722&s=612x612&w=0&h=NZJzid5Bl0n32z4VPTppAxNcyuBvaEc5h7WDSWOlBxA=\" ></div><div style=\"font -size: 30px;\"><br/> Şifreniz Sıfırlandı ve benzersiz şekilde oluşturuldu. <br/> Bu şifre ile giriş yapınız ve şifrenizi baştan kendiniz oluşturunuz.<br/> Şifreniz : "+ OnayKodu + "</div></div>"; ;
                SmtpClient _smtp = new SmtpClient();
                _smtp.Host = "smtp.gmail.com";
                _smtp.Port = 587;
                NetworkCredential _network = new NetworkCredential("adimadimproje@gmail.com", "gumballgame");
                _smtp.Credentials = _network;
                _smtp.EnableSsl = true;
                _smtp.Send(_mailmsg);
                return 1;
            }
            else
                return 0;
        }

        private bool EmailKontrol(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}