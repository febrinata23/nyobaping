using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Timers;
using mshtml;

namespace nyobaping
{
    public partial class Form1 : Form
    {
        public System.Timers.Timer aTimer = new System.Timers.Timer();
        public Uri url;
        public int rto, success;
        public WebBrowser web = new WebBrowser();
        public int th=0;
        public Boolean flag = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                checkking();
            }
            else
            {
                loginspeedy();
                checkking();
            }
        }

        private void checkking()
        {
            try
            {
                aTimer.Stop();
                this.aTimer = new System.Timers.Timer();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                aTimer.Interval = Convert.ToInt16(textBox3.Text);
                aTimer.Enabled = true;
            }
            catch 
            {
                this.aTimer = new System.Timers.Timer();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                aTimer.Interval = Convert.ToInt16(textBox3.Text);
                aTimer.Enabled = true;
            }
            
        }

        private void checking2()
        {
            this.aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = Convert.ToInt16(textBox3.Text);
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            this.url = new Uri(textBox1.Text);
            string pingurl = string.Format("{0}", url.Host);
            string host = pingurl;
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 3000);
                textBox6.Text = reply.Status.ToString();
                if (reply.Status == IPStatus.Success)
                {
                    textBox2.Text = "ip address " + reply.Address.ToString() + " time " + reply.RoundtripTime + " ttl " + reply.Options.Ttl + " size " + reply.Buffer.Length;
                    this.rto = 0;
                    success++;
                    textBox5.Text = Convert.ToString(success);
                    textBox4.Text = Convert.ToString(rto);
                }
                else if (reply.Status == IPStatus.TimedOut)
                {
                    textBox2.Text = IPStatus.TimedOut.ToString();
                    this.rto++; this.success = 0;
                    textBox4.Text = Convert.ToString(rto);
                    textBox5.Text = Convert.ToString(success);
                    if (rto >= th)
                    {
                        aTimer.Stop();
                        loginspeedy();
                        checkking();
                    }
                }
                else
                {
                    textBox6.Text = "diskonek";
                }
            }
            catch(Exception ) 
            {}
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.aTimer.Stop();
        }

        private void loginspeedy()
        {

                aTimer.Stop();
                string url = "http://welcome.indonesiawifi.net/wifi.id/speedy/?switch_url=http://1.1.1.1/login.html&ap_mac=88:75:56:15:a1:f0&client_mac=0c:ee:e6:84:7f:0b&wlan=@wifi.id&redirect=www.msftncsi.com/redirect";
                webBrowser1.ScriptErrorsSuppressed = true;
                webBrowser1.Navigate(url);

                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(OnDocumentCompleted);
            
        }

        private void OnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElement head = webBrowser1.Document.GetElementsByTagName("head")[0];
            HtmlElement scriptEl = webBrowser1.Document.CreateElement("script");
            IHTMLScriptElement element1 = (IHTMLScriptElement)scriptEl.DomElement;
            string alertBlocker = @"window.alert = function () { }; window.confirm=function () { };";
            element1.text = alertBlocker;
            head.AppendChild(scriptEl);
            webBrowser1.ScriptErrorsSuppressed = true;
            ((WebBrowser)sender).Document.Window.Error +=new HtmlElementErrorEventHandler(Window_Error);
            if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            {
                webBrowser1.Document.GetElementById("username").SetAttribute("value", textBox7.Text);
                webBrowser1.Document.GetElementById("username_hidden").SetAttribute("value", textBox7.Text);
                HtmlElementCollection element = this.webBrowser1.Document.GetElementsByTagName("submit");

                HtmlElement hat = webBrowser1.Document.GetElementById("loginForm");
                hat.InvokeMember("click");

                HtmlElement button = this.FindControlByName("Submit", webBrowser1.Document.All);
                button.InvokeMember("submit"); button.InvokeMember("click");

                foreach (HtmlElement currentElement in element)
                {
                    currentElement.InvokeMember("submit");
                }

                System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName("IEXPLORE");
                foreach (System.Diagnostics.Process proc in procs)
                {
                    proc.Kill();
                }

                aTimer.Start();
            }
        }

        private void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }

        private void loadgoo()
        {
            string url = "http://www.google.com";
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(url);
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(OnDocumentCompleted1);
        }

        private void OnDocumentCompleted1(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #region

        private HtmlElement FindControlByName(string name, HtmlElementCollection listOfHtmlControls)
        {
            foreach (HtmlElement element in listOfHtmlControls)
            {
                if (!string.IsNullOrEmpty(element.OuterHtml))
                {
                    if (element.Name == name.Trim())
                    {
                        return element;
                    }
                }
            }

            return null;
        }
        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            this.th = Convert.ToInt16(textBox9.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.th = Convert.ToInt16(textBox9.Text);
            this.url = new Uri(textBox1.Text);
            string pingurl = string.Format("{0}", url.Host);
            string host = pingurl;
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 3000);
                if (reply.Status == IPStatus.Success)
                {
                    this.flag = true;
                }
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "hide")
            {
                webBrowser1.Visible = false;
                button3.Text = "show";
                this.Size = new Size(280, 300);
            }
            else
            {
                button3.Text = "hide";
                webBrowser1.Visible = true;
                webBrowser1.Size = new Size(328, 225);
                this.Size=new Size(627, 300);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }
    }
}
