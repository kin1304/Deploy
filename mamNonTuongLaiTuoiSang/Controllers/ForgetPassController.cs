using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class ForgetPassController : Controller
    {
        string code = "";
        string countdefault = "0000000";
        private const string baseURL = "http://localhost:5005/api/PhuHuynhs";


        public IActionResult FindEmail()
        {
            return View();
        }

        public async Task<List<PhuHuynh>> FindPH()
        {
            using (var phuhuynh = new HttpClient())
            {
                string path = baseURL;
                phuhuynh.DefaultRequestHeaders.Accept.Clear();
                phuhuynh.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await phuhuynh.GetAsync(path);
                if (getData.IsSuccessStatusCode)
                {
                    var data = getData.Content.ReadAsStringAsync().Result;
                    var phuHuynhResponse = JsonConvert.DeserializeObject<List<PhuHuynh>>(data);
                    if (phuHuynhResponse == null)
                    {
                        return null;
                    }
                    List<PhuHuynh>phuHuynh = phuHuynhResponse;
                    return phuHuynh;
                }
            }
            return null;


        }
        [HttpPost]
        public async Task<IActionResult> FindEmail(string email)
        {
            if (email == null)
            {
                TempData["AlertMessage"] = "Vui lòng điền email";
                TempData["AlertType"] = "alert-warning";
                return View();
            }
            List<PhuHuynh> phuHuynh = await FindPH();
            if (phuHuynh == null)
            {
                TempData["AlertMessage"] = "Không tìm thấy email phù hợp";
                TempData["AlertType"] = "alert-warning";
                return View();
            }
            foreach (var ph in phuHuynh)
            {
                if (ph.Email == email)
                {
                    SendEmail(email);
                    TempData["Key"] = code;
                    TempData["Input"] = countdefault;
                    TempData["email"] = email;
                    return RedirectToAction("FormCode");
                }
            }
            TempData["AlertMessage"] = "Không tìm thấy email";
            TempData["AlertType"] = "alert-warning";
            return View();
        }
        public bool SendEmail(string EMAIL)
        {
            Random rd = new Random();
            code = rd.Next(100000, 999999).ToString();
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("QL Mầm Non", "nguyennha6a6kl@gmail.com"));
                email.To.Add(new MailboxAddress("Người nhận", EMAIL));
                email.Subject = "Testing out email sending";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                {
                    Text = "Mã Số xác nhận của bạn là : " + code
                };
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication

                    smtp.Authenticate("nguyennha6a6kl@gmail.com", "tsol zvsa dswy wtyx");

                    smtp.Send(email);
                    smtp.Disconnect(true);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public ActionResult FormCode()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FormCode(String CODE)
        {
            string count_string = TempData["Input"] as string;
            string code = TempData["Key"] as string;
            if (CODE.Equals(code))
            {
                return RedirectToAction("ChangePass");
            }
            else
            {
                TempData["Key"] = code;

                if (count_string != null)
                {
                    int count = int.Parse(count_string.ToString());
                    if (count < 6)
                    {
                        TempData["Input"] = (count + 1).ToString();
                        TempData["AlertMessage"] = "mã sai nhập lại";
                        TempData["AlertType"] = "alert-warning";
                        return RedirectToAction("FormCode");
                    }
                    else
                    {
                        return RedirectToAction("FindEmail");
                    }
                }
                else
                {
                    return RedirectToAction("FormCode");
                }
            }
        }
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ChangePass(string NewPass, string ReNewPass)
        {
            if (NewPass == null || ReNewPass == null)
            {
                TempData["AlertMessage"] = "xin hãy nhập đầy đủ";
                TempData["AlertType"] = "alert-warning";
                return View();
            }
            else
            {
                if (!NewPass.Equals(ReNewPass))
                {
                    TempData["AlertMessage"] = "nhập lại mật khẩu khác với mật khẩu mới";
                    TempData["AlertType"] = "alert-warning";
                    return View();
                }
                else
                {
                    string email = TempData["email"] as string;
                    List<PhuHuynh> phuHuynh = await FindPH();
                    foreach (var ph in phuHuynh)
                    {
                        if (email == ph.Email)
                        {
                            ph.MatKhau = NewPass;
                            string data = JsonConvert.SerializeObject(ph);
                            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await new HttpClient().PutAsync(baseURL + "/" + ph.IdPh, content);
                            if (response.IsSuccessStatusCode)
                            {
                                TempData["AlertMessage"] = "Đổi mật khẩu thành công";
                                TempData["AlertType"] = "alert-success";
                                return RedirectToAction("Login", "Account");
                            }
                            else
                            {
                                TempData["AlertMessage"] = "Đổi mật khẩu thất bại";
                                TempData["AlertType"] = "alert-warning";
                                return View("FindEmail", "ForgetPass");
                            }
                        }
                    }
                    return RedirectToAction("Login", "Account");
                }
            }
        }

    }

}
