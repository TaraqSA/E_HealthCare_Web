using E_HealthCare_Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace E_HealthCare_Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        E_HealthCareEntities context = new E_HealthCareEntities();
        
        public ActionResult VerifyEmail(string EmailAddress, string UserName)
        {
            var getUserId = context.SiteUsers.Where(q => q.UserName == UserName).Select(q => q.Id).FirstOrDefault();
            string url = "/E_HealthCare_Web/Manage/EmailVerfied/";
            var encryptedId = Encrypt(getUserId.ToString()); 
            var finalUrl = url + encryptedId;
            string Link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, finalUrl);

            var subject = "Email Verification Link";
            var body = "Hi " + UserName + ", <br/> Please Verify Your Email Address By clicking the <a href = '" + Link + "'>Link</a>" +
                "<br />if you did requested a Email verification, please ignore this message or reply us to let us know. <br/><br/> Thank you.";

            AccountController accountController = new AccountController();
            accountController.SendEmail(EmailAddress, body, subject);
            return View();
        }
       
        public ActionResult EmailVerfied(string id)
        {
            int decryptedId = Convert.ToInt32(Decrypt(id));
            var getUserRole = context.SiteUsers.Where(q => q.Id == decryptedId).Select(q => q.UserRole).FirstOrDefault();
            if (getUserRole == "Patient")
            {
                var getPatient = context.Patients.Where(q => q.UserId == decryptedId).FirstOrDefault();
                getPatient.IsEmailVerified = true;
                context.SaveChanges();
                return RedirectToAction("PatientAccount","Patient", new { id = getPatient.p_id });
            }
            else if (getUserRole == "Doctor")
            {
                var getDoctor = context.Doctors.Where(q => q.D_UserId == decryptedId).FirstOrDefault();
                getDoctor.IsEmailVerified = true;
                context.SaveChanges();
                return RedirectToAction("DoctorAccount","Doctor", new { id = getDoctor.Id });
            }
            return View();
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MS1DW5G5LNS45PU8";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MS1DW5G5LNS45PU8";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}