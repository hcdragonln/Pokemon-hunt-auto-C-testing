using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using WindowsInput;
using WindowsInput.Native;
using KAutoHelper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;
using static KAutoHelper.FindWindow;
using System.Text;
using Tesseract;
using System.Drawing;
using System.Linq;
using System.Drawing.Imaging;
using System.Media;
using Emgu.Util.TypeEnum;
using System.Diagnostics.Eventing.Reader;
using System.CodeDom;
using System.Net.NetworkInformation;
using System.Net.Mail;
using System.Net;
using MailKit.Net.Imap;
using MailKit;
using MimeKit;
using MailKit.Search;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
namespace Auto_Click
{
    public partial class Form1 : Form
    {
        VirtualKeyCode prevousKey = VirtualKeyCode.VK_A;
        //Khoa
        Image screen= CaptureHelper.CaptureScreen();
        //process test
        private Process _process;
        private Task taskRunning = Task.CompletedTask; // Task mặc định
        private bool isTaskRunning = false; // Biến kiểm soát Task
        private object taskLock = new object(); // Lock để tránh chạy trùng Task
        //random
        private Random random = new Random();
        int OFFALL = 0;
        int SL_A = 0;
        int SL_D = 0;
        int SL_Click = 0;
        int repeat = 0;
        string key1;
        int multi;
        int Off = 0;
        int checkspawnAWSD = 0;
        int checkAWSD= 0;
        int teleport = 0;
        int rareformMove = 0;
        int Attack = 0;
        string messageId = "";
        Dictionary<string, int> pokemonCounts = new Dictionary<string, int>();
        Dictionary<string, int> pokemonCatchs = new Dictionary<string, int>();
        Dictionary<string, int> pokemonAbility = new Dictionary<string, int>();
        private static InputSimulator sim = new InputSimulator();
        // Import thư viện user32.dll để thao tác chuột và bàn phím
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        // Import thư viện user32.dll để đăng ký hotkey
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        //Test
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
        [DllImport("User32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        /// <summary>
        /// 
        /// </summary>

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        private const uint WM_KEYDOWN = 0x0100; // Mã sự kiện bấm phím
        private const uint WM_KEYUP = 0x0101;   // Mã sự kiện nhả phím
        private const int HOTKEY_F6 = 1;
        private const int HOTKEY_F5 = 2;
        private const int HOTKEY_F7 = 3;
        private const int MOD_NOREPEAT = 0x4000; // Chống lặp phím
        private const int MOD_ALT = 0x0001;      // Phím Alt
        private const int VK_F6 = 0x75;          // Mã phím F6
        private const int VK_F5 = 0x74;
        private const int VK_F7 = 0x76;
        private const byte VK_A = 0x41;
        private const byte VK_D = 0x44;
        private CancellationTokenSource cts; // Dùng để dừng Task
        /// <summary>
        //
        /// </summary>
        //Bo nho access
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int PROCESS_VM_READ = 0x0010;
        //Image

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern bool CloseHandle(IntPtr hObject);
        private System.Windows.Forms.Timer timer;
        public Form1()
        {

            InitializeComponent();
            richTextBox1.ReadOnly = true;           
            textBox5.ReadOnly = true;
            textBox6.ReadOnly = true;
            // Lấy thông tin chuột
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 100; // Cập nhật mỗi 100ms
            timer.Tick += UpdateMousePosition;
            timer.Start();
            //Nhận button dưới nền global
            RegisterHotKey(this.Handle, HOTKEY_F6, MOD_ALT | MOD_NOREPEAT, VK_F6);
            RegisterHotKey(this.Handle, HOTKEY_F5, MOD_ALT | MOD_NOREPEAT, VK_F5);
            RegisterHotKey(this.Handle, HOTKEY_F7, MOD_ALT | MOD_NOREPEAT, VK_F7);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void UpdateMousePosition(object sender, EventArgs e)
        {
            var mousePos = Cursor.Position;
            textBox1.Text = $"{mousePos.X}";
            textBox2.Text = $"{mousePos.Y}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
            cts = null;
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            OFFALL = 1;
            cts?.Cancel();
            cts = null;
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            if (m.Msg == WM_HOTKEY)
            {
                switch (m.WParam.ToInt32())
                {
                    case HOTKEY_F6:
                        button6.PerformClick(); // Thực hiện hành động khi Alt + F6
                        break;
                    case HOTKEY_F5:
                        button4.PerformClick(); // Thực hiện hành động khi Alt + F5
                        break;
                    default:
                        break;
                }
                return; // Ngừng xử lý phím mặc định
            }
            base.WndProc(ref m);
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(this,"Bạn có chắc chắn muốn thoát?", "Xác nhận thoát",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true; // Hủy đóng form
                return;
            }

            // Hủy đăng ký hotkey trong nền (sử dụng async/await)
            await Task.Run(() =>
            {
                // Mô phỏng việc hủy đăng ký có thể mất thời gian (giả lập với Task.Delay)
                UnregisterHotKey(this.Handle, HOTKEY_F6);
                UnregisterHotKey(this.Handle, HOTKEY_F5);
                UnregisterHotKey(this.Handle, HOTKEY_F7);
            });

            // Đóng MessageBox sau khi hoàn thành công việc hủy đăng ký
            base.OnFormClosing(e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var screen = CaptureHelper.CaptureScreen();
            screen.Save("mainScreen.PNG");
            var subBitmap = ImageScanOpenCV.GetImage(@"G:\\CODE\\Auto_Click\\bin\\Debug\\net8.0-windows\\3.png");
            var resBitmap = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitmap);
            if (resBitmap != null)
            {
                MessageBox.Show(resBitmap.ToString());
            }
        }
       
        private async void button6_Click(object sender, EventArgs e)
        {
            
            textBox3.ReadOnly = true;
            button6.Enabled = false;
            textBox4.ReadOnly = true;
            textBox7.ReadOnly = true;
            textBox8.ReadOnly = true;
            try
            {
                if (!int.TryParse(textBox3.Text, out int minutes) || minutes <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số phút hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Dừng lại nếu nhập sai
                }

                cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(minutes));
                button7.PerformClick();

                // Chạy Task RunMouseAndKeyboard2
                await Task.Run(() => RunMouseAndKeyboard2(cts.Token));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cts?.Dispose();
                cts = null; // Đặt lại để có thể khởi động lại task sau này
                button6.Enabled = true;
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
                textBox7.ReadOnly = false;
                textBox8.ReadOnly = false;
                richTextBox1.Text += "Clear completed";
                if (checkBox18.Checked)
                {
                    int network = 0;
                    bool replycheck = false;
                    //check network
                    for (int i = 0; i < 10; i++)
                    {
                        string address = "google.com";
                        try
                        {
                            using (Ping ping = new Ping())
                            {
                                PingReply reply = ping.Send(address, 1000); // Timeout: 1000ms
                                if (reply.Status == IPStatus.Success)
                                {
                                    network = 1;
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("LOI O NETWORK check");
                            
                        }
                        await Task.Delay(2000);
                    }
                    //check email response
                    if (network == 1)
                    {
                        await SendEmailNotification();
                        await Task.Delay(1000);
                        replycheck= await EmailReplyChecker(messageId);
                    }
                    if (replycheck)
                    {
                        button6.PerformClick();
                    }
                }
            }
        }
        //send email
        private async Task SendEmailNotification()
        {
            DotNetEnv.Env.Load();
            
            string fromEmail = Environment.GetEnvironmentVariable("Email_acc");
            string password = Environment.GetEnvironmentVariable("Email_pass");
            string toEmail = textBox9.Text.Trim();            
            try
            {               
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(toEmail);
                mail.Subject = "Thông báo co Teleport";
                mail.Body = "Hello";
                messageId = $"<{Guid.NewGuid()}@akarework4.com>";
                mail.Headers.Add("Message-ID", messageId);
                if (System.IO.File.Exists("../image/Teleport.png"))
                {
                    Attachment attachment = new Attachment("../image/Teleport.png");
                    mail.Attachments.Add(attachment);
                }
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true
                };

                smtpClient.Send(mail);
                
                this.BeginInvoke((MethodInvoker)delegate
                {
                    richTextBox1.Text = messageId;
                });
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khong the gui mail");
            }
        }
        //check response
        private async Task<bool> EmailReplyChecker(string messageId)
        {
            DotNetEnv.Env.Load();
            string email = Environment.GetEnvironmentVariable("Email_acc");
            string password = Environment.GetEnvironmentVariable("Email_pass");
            string imapServer = "imap.gmail.com";
            int port = 993;
            int delayMilliseconds = 5 * 60 * 1000; // 5 phút

            string fromEmail = textBox9.Text.Trim();
            int maxAttempts = 10;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                AppendTextToRichTextBox($"{DateTime.Now}: 🔎 Lần kiểm tra thứ {attempt}...\n");

                try
                {
                    using (var client = new ImapClient())
                    {
                        await client.ConnectAsync(imapServer, port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                        await client.AuthenticateAsync(email, password);

                        var inbox = client.Inbox;
                        await inbox.OpenAsync(FolderAccess.ReadOnly);

                        var query = SearchQuery.DeliveredAfter(DateTime.UtcNow.AddDays(-1));
                        var uids = await inbox.SearchAsync(query);

                        bool hasReply = false;

                        foreach (var uid in uids.Reverse())
                        {
                            var message = await inbox.GetMessageAsync(uid);

                            if (!string.IsNullOrEmpty(message.InReplyTo) && message.InReplyTo.Contains(messageId.Replace("<", "").Replace(">", "")))
                            {
                                AppendTextToRichTextBox($"📨 InReplyTo nhận được: {message.InReplyTo}\n");
                                hasReply = true;
                                break;
                            }
                        }

                        await client.DisconnectAsync(true);

                        if (hasReply)
                        {
                            AppendTextToRichTextBox("✅ Đã nhận được phản hồi.\n");
                            return true;
                        }
                        else
                        {
                            AppendTextToRichTextBox("⏳ Chưa có phản hồi.\n");
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppendTextToRichTextBox($"❌ Lỗi: {ex.Message}\n");
                }

                if (attempt < maxAttempts)
                {
                    AppendTextToRichTextBox("🕐 Đợi 5 phút rồi kiểm tra lại...\n");
                    await Task.Delay(delayMilliseconds);
                }
            }

            AppendTextToRichTextBox("❌ Hết 10 lần kiểm tra. Không nhận được phản hồi.\n");
            return false;
        }

        // Phương thức này giúp cập nhật RichTextBox an toàn từ bất kỳ thread nào
        private void AppendTextToRichTextBox(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(text)));
            }
            else
            {
                richTextBox1.AppendText(text);
            }
        }
        private void RunMouseAndKeyboard2(CancellationToken token)
        {
            //image
            richTextBox1.Text = "";
            var subBitmap = ImageScanOpenCV.GetImage("test.png");
            //shiny
            var subBitmap1 = ImageScanOpenCV.GetImage("4.png");
            //rare form            
            var subBitmaprareformB = ImageScanOpenCV.GetImage("rare.png");
            var subBitmaprefuse = ImageScanOpenCV.GetImage("refuse.png");
            var subBitmapevole = ImageScanOpenCV.GetImage("checkevole.png");
            var subBitmapyesevole = ImageScanOpenCV.GetImage("yesEvote.png");
            var subBitmapfrisk = ImageScanOpenCV.GetImage("frisk.png");
            //pokemon want
            // Lấy danh sách từ khóa từ textBox4 (cách nhau bởi dấu phẩy)
            string[] keywords = textBox4.Text.Split(',')
                                             .Select(k => k.Trim())  // Loại bỏ khoảng trắng dư
                                             .ToArray();
            string[] keywordAbility = textBox21.Text.Split(',')
                                             .Select(k => k.Trim())  // Loại bỏ khoảng trắng dư
                                             .ToArray();
            string[] keywordAvoid= textBox23.Text.Split(',')
                                             .Select(k => k.Trim())  // Loại bỏ khoảng trắng dư
                                             .ToArray();
            string[] keywordfrisk = textBox25.Text.Split(',')
                                             .Select(k => k.Trim())  // Loại bỏ khoảng trắng dư
                                             .ToArray();
            //var subBitmap2 = ImageScanOpenCV.GetImage(textBox4.Text);
            var subBitmap3 = ImageScanOpenCV.GetImage("10.png");
            var subBitmap4 = ImageScanOpenCV.GetImage("11.png");
            var subBitmapYes = ImageScanOpenCV.GetImage("YesMove.png");
            var subBitmapTrace = ImageScanOpenCV.GetImage("trace.png");
            var subBitmapTrace2 = ImageScanOpenCV.GetImage("trace2.png");
            //Not Have
            var subBitmap5 = ImageScanOpenCV.GetImage("catch.png");
            var subBitmap2 = ImageScanOpenCV.GetImage("ATK.png");

            //clear image
            pokemonCounts = new Dictionary<string, int>();
            pokemonCatchs = new Dictionary<string, int>();
            pokemonAbility = new Dictionary<string, int>();
            int totalDistance = int.Parse(textBox24.Text);
            bool containsKeyword = false;
            bool containsKeyword1=false;
            bool containsAvoid=false;
            bool containsfrisk = false;
            string extractedText="";
            string extractedTextability = "";
            int checkwild = 0;
            int checkability = 0;
            int rareform = 0;
            rareformMove = 0;
            int falseSweep = 0;
            int encountered = -1;
            string previoussendkey = "";
            int Catch = 0;
            int RealCatch = 0;
            int Shiny = 0;
            int Sl = 0;
            int imageTake = 0;
            int type = 0;
            int swap = 0;
            string name;

            int offTime = 0;
            int Loop = 0;
            if (!checkBox20.Checked) Attack = 0;
            teleport = 0;
            int frisk = 0;
            int NoCatch1 = 0;
            checkspawnAWSD = 0;
            checkAWSD = 0;
            OFFALL = 0;
            DateTime checkTeleport = DateTime.Now;
            DateTime checkTrace = DateTime.Now;
            DateTime encounterProblem = DateTime.Now;
            isTaskRunning = false;

            ///
            int z = 0;
            int trace_rare = 0;
            if (checkBox12.Checked) _ = SpamAWSD(token); 
            

            ///Catch swap
            taskRunning = Task.Run(async () =>
            {
                var subBitmaprareformA = ImageScanOpenCV.GetImage("rare.png");
                var subBitmapA = ImageScanOpenCV.GetImage("test.png");
                var subBitmap1A = ImageScanOpenCV.GetImage("4.png");              
                while (!token.IsCancellationRequested&& OFFALL==0)
            {

                    try
                    {
                        //Time check start

                        if (checkBox13.Checked && ((checkwild == 1 && !checkBox7.Checked) || (checkwild == 1 && checkability == 1)))
                        {                           
                            bool keywordCondition = (containsKeyword && checkBox7.Checked && containsKeyword1) ||
                                                        (containsKeyword && !checkBox7.Checked) || (checkBox11.Checked && containsfrisk) || rareform == 1 || (NoCatch1 == 1 & checkBox17.Checked);
                            if (!keywordCondition)
                            {
                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_4);
                                Thread.Sleep(10);
                                sim.Keyboard.KeyUp(VirtualKeyCode.VK_4);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("LOI O VI TRI 2 "+ex.Message);
                    }
                    try
                    {
                        Bitmap screenCapturerare = null;
                        Bitmap screenCapturerare1 = null;
                        //Khoi tao hinh anh
                        try
                        {
                            screenCapturerare = new Bitmap(CaptureHelper.CaptureScreen());
                            screenCapturerare1 = new Bitmap(CaptureHelper.CaptureScreen());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("LOI O VI TRI 3.-1" + ex.Message);
                        }
                        try
                        {
                            bool normalFound = false, rareNotFound = false, wildCondition = false;
                            try
                            {
                                normalFound = ImageScanOpenCV.FindOutPoint(screenCapturerare, subBitmapA) != null;
                                rareNotFound = ImageScanOpenCV.FindOutPoint(screenCapturerare1, subBitmaprareformA) == null;                               
                                wildCondition = (checkwild == 1 && (!checkBox7.Checked || checkability == 1 || checkBox16.Checked && trace_rare==0 )) || rareform == 1;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("LOI O VI TRI 3.0: " + ex.Message);
                            }
                            try
                            {
                                //
                                if (checkwild == 0 || normalFound)
                                {
                                    encounterProblem = DateTime.Now;
                                    
                                }
                                if ((DateTime.Now - encounterProblem).TotalSeconds > 50 && checkBox2.Checked)
                                {
                                    teleport = 1;
                                    OFFALL = 1;
                                }
                                //
                                if (normalFound && wildCondition && rareNotFound)
                                {
                                    richTextBox1.Text = "Check Catch";
                                    using (Bitmap screenCapture2 = new Bitmap(CaptureHelper.CaptureScreen()))
                                    {
                                        bool keywordCondition = (containsKeyword && (checkBox7.Checked ? containsKeyword1 : true)) || rareform == 1||(NoCatch1==1&checkBox17.Checked);
                                        if (keywordCondition) Catch = 1;
                                        else if (containsKeyword && trace_rare == 0 && checkBox16.Checked)
                                        {
                                            this.BeginInvoke((MethodInvoker)delegate
                                            {
                                                SendKeys.Send("2");
                                                Thread.Sleep(random.Next(int.Parse(textBox15.Text), int.Parse(textBox17.Text)));
                                                SendKeys.Send("4");
                                            });

                                            Thread.Sleep(random.Next(int.Parse(textBox15.Text), int.Parse(textBox17.Text)));
                                            checkTrace = DateTime.Now;
                                            trace_rare = 1;
                                            continue;
                                        }
                                        bool targetFound = ImageScanOpenCV.FindOutPoint(screenCapture2, subBitmap1A) != null;
                                        
                                        if (targetFound || keywordCondition)
                                        {
                                            try
                                            {
                                                if (checkBox3.Checked && falseSweep == 0)
                                                {
                                                    if ((checkBox6.Checked || checkBox7.Checked) && swap == 0)
                                                    {
                                                        this.BeginInvoke((MethodInvoker)delegate
                                                        {
                                                            SendKeys.Send("2");
                                                            Thread.Sleep(random.Next(int.Parse(textBox15.Text), int.Parse(textBox17.Text)));
                                                            SendKeys.Send("3");
                                                            swap = 1;
                                                        });
                                                        Thread.Sleep(1000);
                                                        continue;
                                                    }

                                                    this.BeginInvoke((MethodInvoker)delegate
                                                    {
                                                        SendKeys.Send("1");
                                                        Thread.Sleep(random.Next(int.Parse(textBox15.Text), int.Parse(textBox17.Text)));
                                                        SendKeys.Send("1");
                                                        falseSweep = 1;
                                                    });
                                                    Thread.Sleep(int.Parse(textBox17.Text) + 100);
                                                }
                                                RealCatch = 2;
                                                name = extractedText.Trim().Split(' ').LastOrDefault() ?? string.Empty;

                                                this.BeginInvoke((MethodInvoker)delegate
                                                {
                                                    SendKeys.Send("3");
                                                    Thread.Sleep(random.Next(int.Parse(textBox15.Text), int.Parse(textBox17.Text)));
                                                    SendKeys.Send("1");
                                                });

                                                Thread.Sleep(random.Next(int.Parse(textBox15.Text), int.Parse(textBox17.Text)));
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("Lỗi ở vị trí 3.1: " + ex.Message);
                                            }
                                        }
                                        else if (frisk == 1)
                                        {
                                            try
                                            {
                                                frisk = 2;
                                                this.BeginInvoke((MethodInvoker)delegate
                                                {
                                                    SendKeys.Send("1");
                                                    Thread.Sleep(random.Next(int.Parse(textBox15.Text), int.Parse(textBox17.Text)));
                                                    SendKeys.Send("1");
                                                });
                                                Thread.Sleep(int.Parse(textBox17.Text) + 100);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("Lỗi ở vị trí 3.2: " + ex.Message);
                                            }
                                        }
                                        else if (((checkBox6.Checked && Attack < 15) || (checkBox8.Checked && Attack < 10) || (checkBox15.Checked && Attack < 40)) &&
                                                  !containsAvoid &&
                                                  ImageScanOpenCV.FindOutPoint(screenCapture2, subBitmapA) != null)
                                        {
                                            try
                                            {
                                                if ((checkBox8.Checked && Attack >= 9) || (checkBox15.Checked && Attack >= 39)) Off = 1;
                                                this.BeginInvoke((MethodInvoker)delegate
                                                {
                                                    string elixirATK = (Attack / 10 + 1).ToString();
                                                    Attack++;
                                                    textBox22.Text = Attack.ToString();

                                                    SendKeys.Send("1");
                                                    Thread.Sleep(random.Next(int.Parse(textBox15.Text), int.Parse(textBox17.Text)));
                                                    if (checkBox8.Checked || checkBox6.Checked) SendKeys.Send("4");
                                                    else SendKeys.Send(elixirATK);
                                                });

                                                Thread.Sleep(int.Parse(textBox17.Text) + 100);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("Lỗi ở vị trí 3.3: " + ex.Message);
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                this.BeginInvoke((MethodInvoker)delegate
                                                {
                                                    SendKeys.Send("4");
                                                    richTextBox1.AppendText("run  " + DateTime.Now.ToString() + Environment.NewLine);
                                                    richTextBox1.ScrollToCaret();
                                                });
                                                Thread.Sleep(int.Parse(textBox17.Text) + 100);

                                                Thread.Sleep(random.Next(int.Parse(textBox15.Text), int.Parse(textBox17.Text)));
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("Lỗi ở vị trí 3.4: " + ex.Message);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"LOI O VI TRI 3.5: {ex.Message}");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"LOI O VI TRI 3.6: {ex.Message}");
                        }
                        try
                        {
                            this.BeginInvoke((MethodInvoker)delegate
                            {
                                richTextBox1.Text = "Check Argument" + Environment.NewLine;
                                richTextBox1.AppendText($"frisk {frisk}{Environment.NewLine}");
                                richTextBox1.AppendText($"OFF {Off}{Environment.NewLine}");
                                richTextBox1.AppendText($"checkability {containsKeyword1}{Environment.NewLine}");
                                richTextBox1.AppendText($"Encounter {!isTaskRunning}{Environment.NewLine}");
                                richTextBox1.AppendText($"Nocatch {NoCatch1}{Environment.NewLine}");
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"LOI O VI TRI 3.7: {ex.Message}");
                        }
                        Thread.Sleep(100);
                        try
                        {
                            screenCapturerare.Dispose();
                            screenCapturerare1.Dispose();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"LOI O VI TRI 3.7: {ex.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"LOI O VI TRI 3: {ex.Message}");
                    }


                }
                subBitmaprareformA.Dispose();
                subBitmapA.Dispose();
                subBitmap1A.Dispose();               
            });
            //LOOP scan
            while (!token.IsCancellationRequested && OFFALL==0)
            {   
                //Check rare
                using (Bitmap screenBitmap = new Bitmap(CaptureHelper.CaptureScreen()))
                {
                    if (ImageScanOpenCV.FindOutPoint(screenBitmap, subBitmaprareformB) != null)
                    {
                        rareform = 1;
                        rareformMove = 1;
                        isTaskRunning = false;
                        if (!checkBox5.Checked)
                        {
                            using (SoundPlayer player = new SoundPlayer("rare.wav"))
                            {
                                player.PlaySync();
                            }
                        }
                        var screen = CaptureHelper.CaptureScreen();
                        string path = "../image/" + textBox6.Text + ".png";
                        screen.Save(path);
                        screen.Dispose();
                        Catch = 1;
                    } 
                }
                //check name
                if (checkwild == 0)
                {
                    using (Bitmap screenBitmap = new Bitmap(CaptureHelper.CaptureScreen()))
                    {
                        if (ImageScanOpenCV.FindOutPoint(screenBitmap, subBitmap4) != null)
                        {
                            try
                            {
                                
                                Thread.Sleep(100);
                                isTaskRunning = false;
                                if (ImageScanOpenCV.FindOutPoint(screenBitmap, subBitmap5) == null)
                                {
                                    NoCatch1 = 1;
                                }
                                using (Bitmap screenshot = new Bitmap(CaptureHelper.CaptureScreen()))
                                {
                                    int x, y, width = 350, height = 33;
                                    if (!int.TryParse(textBox7.Text, out x) || !int.TryParse(textBox8.Text, out y))
                                    {
                                        MessageBox.Show("Invalid coordinates!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    using (Bitmap croppedImage = CropImage(screenshot, x, y, width, height))
                                    using (Bitmap processedImage = PreprocessImage(croppedImage))
                                    {
                                        processedImage.Save("processed_image.png", System.Drawing.Imaging.ImageFormat.Png);

                                        extractedText = ExtractTextFromImage(processedImage);
                                        if (!extractedText.Contains("ild"))
                                        {
                                            extractedText = ExtractTextFromImage(croppedImage);
                                        }
                                        AddPokemon(pokemonCounts, extractedText);
                                        containsKeyword = keywords.Any(keyword =>
                                            extractedText.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
                                        containsAvoid = keywordAvoid.Any(keyword =>
                                            extractedText.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
                                        containsfrisk = keywordfrisk.Any(keyword =>
                                            extractedText.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);

                                        this.Invoke((MethodInvoker)(() =>
                                        {
                                            textBox28.Text = extractedText;
                                        }));
                                    }
                                }
                                checkTrace = DateTime.Now;
                                checkwild = 1;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("LOI O VI TRI 4 "+ex.Message);
                            }                           
                        }

                    }
                }
                //reset
                using (Bitmap screenBitmap = new Bitmap(CaptureHelper.CaptureScreen()))
                {
                    if (ImageScanOpenCV.FindOutPoint(screenBitmap, subBitmap4) == null && !isTaskRunning)
                    {
                        //Save image
                        //debug
                        try
                        {
                            if (checkBox12.Checked)
                            {

                                isTaskRunning = true;
                                if (Catch == 1)
                                {
                                    if (rareform == 1)
                                    {
                                        Shiny++;
                                        AddPokemon(pokemonCatchs, "Form" + extractedText);
                                    }
                                    else AddPokemon(pokemonCatchs, extractedText);
                                    Sl++;

                                    Catch = 0;
                                    if (checkBox9.Checked)
                                    {
                                        string folderPath = "../image";
                                        Directory.CreateDirectory(folderPath);

                                        // Lấy thời gian hiện tại và tạo tên file
                                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                                        string filePath = Path.Combine(folderPath, $"{timestamp}.png");
                                        var screen = CaptureHelper.CaptureScreen();
                                        screen.Save(filePath);
                                        screen.Dispose();
                                    }
                                }
                                this.BeginInvoke((MethodInvoker)delegate
                                {
                                    textBox6.Text = Shiny.ToString();
                                    textBox5.Text = Sl.ToString();
                                });
                                falseSweep = 0;
                                checkwild = 0;
                                rareform = 0;
                                checkability = 0;
                                swap = 0;
                                trace_rare = 0;
                                NoCatch1 = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("LOI O VI TRI 6 "+ex.Message);
                        }
                        

                        
                        this.Invoke((MethodInvoker)(() =>
                        {
                            richTextBox1.AppendText("Not encounter" + Environment.NewLine);
                            richTextBox1.ScrollToCaret();

                        }));
                        //Check encouter
                        encountered++;
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            textBox18.Text = encountered.ToString();
                        });
                        //Evolution oke
                        try
                        {
                            if (frisk == 2)
                            {
                                SetCursorPos(int.Parse(textBox26.Text), int.Parse(textBox27.Text));
                                Thread.Sleep(300);
                                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                                Thread.Sleep(1000);
                                frisk = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("LOI O VI TRI 7 "+ex.Message);
                        }
                        //Setup Evolve
                        try
                        {
                            while (true && !token.IsCancellationRequested && OFFALL == 0)
                            {
                                Thread.Sleep(2000);
                                using (Bitmap screenBitmap1 = new Bitmap(CaptureHelper.CaptureScreen()))
                                {
                                    Bitmap screenBitrefuse = new Bitmap(CaptureHelper.CaptureScreen());
                                    var b = ImageScanOpenCV.FindOutPoint(screenBitmap1, subBitmapevole);
                                    var a = ImageScanOpenCV.FindOutPoint(screenBitrefuse, subBitmaprefuse);
                                    screenBitrefuse?.Dispose();
                                    if (b == null && a == null)
                                    {
                                        if (Off == 2)
                                        {
                                            Off = 0;
                                        }
                                        break;
                                    }
                                    else if (Off == 0)
                                    {
                                        Off = 2;
                                    }
                                    //Check evol
                                    if (b != null)
                                    {

                                        try
                                        {
                                            SetCursorPos(b.Value.X, b.Value.Y);
                                            Thread.Sleep(500);
                                            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                                            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

                                            Thread.Sleep(2000); // Chờ cho giao diện cập nhật

                                            using (Bitmap screenBitmap2 = new Bitmap(CaptureHelper.CaptureScreen()))
                                            {
                                                var Toado = ImageScanOpenCV.FindOutPoint(screenBitmap2, subBitmapyesevole);

                                                if (Toado != null)
                                                {
                                                    Thread.Sleep(300);
                                                    SetCursorPos(Toado.Value.X, Toado.Value.Y);
                                                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                                                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("LOI O VI TRI 8.1 " + ex.Message);
                                        }
                                    }
                                    Thread.Sleep(2000); // Chờ 1s để tránh spam CPU
                                                        //Check Abi
                                    if (a != null)
                                    {
                                        try
                                        {

                                            // Click vào "refuse"
                                            SetCursorPos(a.Value.X, a.Value.Y);
                                            Thread.Sleep(300);
                                            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                                            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);


                                            Thread.Sleep(2000); // Đợi UI cập nhật

                                            // Capture lại để tìm nút "Yes"
                                            using (Bitmap screenBitmap2 =   new Bitmap(CaptureHelper.CaptureScreen()))
                                            {
                                                var toado = ImageScanOpenCV.FindOutPoint(screenBitmap2, subBitmapYes);
                                                if (toado != null)
                                                {
                                                    SetCursorPos(toado.Value.X, toado.Value.Y);
                                                    Thread.Sleep(300);
                                                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                                                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                                                    Thread.Sleep(1000);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("LOI O VI TRI 8.2 " + ex.Message);

                                        }
                                    }

                                }
                               

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("LOI O VI TRI 8" + ex.Message);
                        }
                        try 
                        {
                            if (Attack > 14 && checkBox6.Checked)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    Attack = 0;
                                    SendKeys.Send("3");
                                    Thread.Sleep(random.Next(800, 1000));
                                    SendKeys.Send("2");
                                    Thread.Sleep(2000);
                                    SetCursorPos(int.Parse(textBox19.Text), int.Parse(textBox20.Text));
                                    Thread.Sleep(1000);
                                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                                    Thread.Sleep(1000);
                                    var Toado = ImageScanOpenCV.FindOutPoint((Bitmap)CaptureHelper.CaptureScreen(), subBitmapYes);
                                    SetCursorPos(Toado.Value.X, Toado.Value.Y);
                                    Thread.Sleep(300);
                                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                                    Thread.Sleep(1000);
                                    //reset
                                    SendKeys.Send("4");
                                    Thread.Sleep(random.Next(800, 1000));
                                    SendKeys.Send("2");
                                    Thread.Sleep(2000);
                                    SetCursorPos(int.Parse(textBox19.Text), int.Parse(textBox20.Text));
                                    Thread.Sleep(300);
                                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                                    Thread.Sleep(1000);
                                    SetCursorPos(Toado.Value.X, Toado.Value.Y);
                                    Thread.Sleep(300);
                                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                                });
                                //if (checkBox12.Checked) _ = SpamAWSD(token);
                            }
                            //Farm LP Reset
                            else if (Attack > 9 && checkBox8.Checked)
                            {

                                Attack = 0;
                                this.BeginInvoke((MethodInvoker)delegate
                                {
                                    textBox22.Text = Attack.ToString();
                                });
                                this.Invoke((MethodInvoker)(() =>
                                {
                                    richTextBox1.Text = "Reset";
                                    SendKeys.Send("3");
                                    Thread.Sleep(random.Next(800, 1000));
                                    SendKeys.Send("2");
                                    Thread.Sleep(random.Next(800, 1000));
                                    SetCursorPos(int.Parse(textBox19.Text), int.Parse(textBox20.Text));
                                    Thread.Sleep(1000);
                                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                                }));

                            }
                            else if (Attack > 39 && checkBox15.Checked)
                            {
                                Attack = 0;
                                this.BeginInvoke((MethodInvoker)delegate
                                {
                                    textBox22.Text = Attack.ToString();
                                });
                                this.Invoke((MethodInvoker)(() =>
                                {
                                    richTextBox1.Text = "Reset";
                                    SendKeys.Send("3");
                                    Thread.Sleep(random.Next(800, 1000));
                                    SendKeys.Send("2");
                                }));
                            }
                            Off = 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("LOI O VI TRI 9" + ex.Message);
                        }
                        lock (taskLock) // Kiểm soát số lượng Task
                        {

                            //if (isTaskRunning) continue; // Nếu Task đang chạy, không tạo mới
                            if (checkBox12.Checked == false)
                            {
                                
                                isTaskRunning = true;
                                checkTeleport = DateTime.Now;
                                taskRunning = Task.Run(async () =>
                                {
                                    try
                                    {
                                        if (checkBox4.Checked)
                                        {
                                            VirtualKeyCode vk1 = prevousKey switch
                                            {
                                                VirtualKeyCode.VK_D => VirtualKeyCode.VK_S,
                                                VirtualKeyCode.VK_W => VirtualKeyCode.VK_S,
                                                VirtualKeyCode.VK_A => VirtualKeyCode.VK_W,
                                                VirtualKeyCode.VK_S => VirtualKeyCode.VK_W,
                                                _ => VirtualKeyCode.SPACE
                                            };
                                            sim.Keyboard.KeyDown(vk1);
                                            prevousKey = vk1;
                                        }
                                        else
                                        {
                                            VirtualKeyCode vk1 = prevousKey switch
                                            {
                                                VirtualKeyCode.VK_S => VirtualKeyCode.VK_D,
                                                VirtualKeyCode.VK_W => VirtualKeyCode.VK_A,
                                                VirtualKeyCode.VK_D => VirtualKeyCode.VK_A,
                                                VirtualKeyCode.VK_A => VirtualKeyCode.VK_D,
                                                _ => VirtualKeyCode.SPACE
                                            };

                                            sim.Keyboard.KeyDown(vk1);
                                            prevousKey = vk1;
                                        }

                                        while (true&&!token.IsCancellationRequested&&OFFALL==0)
                                        {
                                           
                                            Bitmap screenBitmap = null;
                                            try
                                            {
                                                screenBitmap = (Bitmap)CaptureHelper.CaptureScreen();
                                                if (ImageScanOpenCV.FindOutPoint(screenBitmap, subBitmap4) != null || token.IsCancellationRequested || offTime != 0)
                                                {
                                                    break;
                                                }
                                                if ((DateTime.Now - checkTeleport).TotalSeconds > 25 && checkBox10.Checked)
                                                {
                                                    teleport = 1;
                                                    cts?.Cancel();
                                                    cts = null;
                                                    break;
                                                }
                                                // add Log
                                                if (Catch == 1)
                                                {
                                                    if (RealCatch != 2) MessageBox.Show("Co loi trong qua trinh bat");
                                                    if (rareform == 1)
                                                    {
                                                        Shiny++;
                                                    }
                                                    Sl++;

                                                    Catch = 0;
                                                    RealCatch = 0;
                                                    if (checkBox9.Checked)
                                                    {
                                                        string folderPath = "../image";
                                                        Directory.CreateDirectory(folderPath);

                                                        // Lấy thời gian hiện tại và tạo tên file
                                                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                                                        string filePath = Path.Combine(folderPath, $"{timestamp}.png");

                                                        screenBitmap.Save(filePath);
                                                    }
                                                }
                                            }
                                            finally
                                            {
                                                screenBitmap?.Dispose(); // Giải phóng bộ nhớ ngay sau khi dùng xong                                              
                                            }

                                            // Reset trạng thái
                                            previoussendkey = "";
                                            falseSweep = 0;
                                            textBox6.Text = Shiny.ToString();
                                            textBox5.Text = Sl.ToString();

                                            // Di chuyển random
                                            checkwild = 0;
                                            rareform = 0;
                                            checkability = 0;
                                            swap = 0;
                                            frisk = 0;

                                            // Dùng Task.Delay với số đã chọn
                                            await Task.Delay(random.Next(int.Parse(textBox12.Text), int.Parse(textBox13.Text)));

                                            VirtualKeyCode vk = prevousKey switch
                                            {
                                                VirtualKeyCode.VK_D => VirtualKeyCode.VK_A,
                                                VirtualKeyCode.VK_A => VirtualKeyCode.VK_D,
                                                VirtualKeyCode.VK_W => VirtualKeyCode.VK_S,
                                                VirtualKeyCode.VK_S => VirtualKeyCode.VK_W,
                                                _ => VirtualKeyCode.SPACE
                                            };

                                            InputSimulator sim = new InputSimulator();
                                            sim.Keyboard.KeyUp(prevousKey);
                                            sim.Keyboard.KeyDown(vk);
                                            prevousKey = vk;
                                        }
                                    }
                                    finally
                                    {
                                        //isTaskRunning = false; // Đánh dấu Task đã kết thúc
                                        sim.Keyboard.KeyUp(prevousKey);
                                        

                                    }
                                });
                            }
                        }
                    }
                }       
                if (rareform == 0 && checkwild == 0)
                {
                    continue;
                }
                //Frisk
                try
                {
                    if (checkBox11.Checked && containsfrisk && rareform == 0 && frisk == 0)
                    {
                        using (Bitmap screenCapture = new Bitmap(CaptureHelper.CaptureScreen()))
                        {
                            if (ImageScanOpenCV.FindOutPoint(screenCapture, subBitmapfrisk) != null)
                            {
                                frisk = 1;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("LOI O VI TRI 11 " +ex.Message);
                }
                try
                {
                    if (!checkBox16.Checked && checkBox7.Checked && (DateTime.Now - checkTrace).TotalSeconds > 10)
                    {
                        checkability = 1; containsKeyword1 = true;
                    }
                    else if (checkBox16.Checked && trace_rare==1&&checkBox7.Checked && (DateTime.Now - checkTrace).TotalSeconds > 30)
                    {
                        checkability = 1; containsKeyword1 = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("LOI O VI TRI 12"+ex.Message);
                }
                try
                {
                    if (containsKeyword && checkwild == 1 && checkability == 0 && rareform != 1 && checkBox7.Checked)
                    {
                        Point? a = null;
                        Point? a2 = null;

                        using (Bitmap screenForA = new Bitmap(CaptureHelper.CaptureScreen()))
                        {
                            a = ImageScanOpenCV.FindOutPoint(screenForA, subBitmapTrace);
                        }

                        using (Bitmap screenForA2 = new Bitmap(CaptureHelper.CaptureScreen()))
                        {
                            a2 = ImageScanOpenCV.FindOutPoint(screenForA2, subBitmapTrace2);
                        }

                        if ((a != null || a2 != null) && checkability == 0)
                        {
                            using (Bitmap screenshot = new Bitmap(CaptureHelper.CaptureScreen()))
                            {
                                int width = 400, height = 50;
                                int x, y;

                                if (a != null)
                                {
                                    x = a.Value.X + 180;
                                    y = a.Value.Y;
                                }
                                else
                                {
                                    x = a2.Value.X + 100;
                                    y = a2.Value.Y;
                                    height = 20;
                                }

                                using (Bitmap croppedImage = CropImage(screenshot, x, y, width, height))
                                using (Bitmap processedImage = PreprocessImage(croppedImage))
                                {
                                    string extractedText2 = ExtractTextFromImage(processedImage);
                                    if (extractedText2.Contains("ow"))
                                    {
                                        extractedTextability = extractedText2;
                                    }
                                    else
                                    {
                                        string extractedText1 = ExtractTextFromImage(croppedImage);
                                        extractedTextability = extractedText1;
                                    }

                                    containsKeyword1 = keywordAbility.Any(keyword =>
                                        extractedTextability.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);

                                    this.BeginInvoke((MethodInvoker)delegate
                                    {
                                        textBox22.Text = extractedTextability;
                                    });
                                    AddPokemon(pokemonAbility, extractedTextability);
                                    checkability = 1;
                                }
                            }
                        }
                    }

                    else if (checkwild == 1 && !containsKeyword || rareform == 1) { checkability = 1; containsKeyword1 = false; }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("LOI O VI TRI 13"+ex.Message);
                }
                
                
               //Change Catch
                
            }
            try
            {
                if (!checkBox5.Checked && teleport == 1)
                {
                    SoundPlayer player = new SoundPlayer("rare.wav");
                    player.PlaySync();
                    richTextBox1.Text = "Co the da bi teleport"+Environment.NewLine;
                    string address = "google.com";
                    try
                    {
                        using (Ping ping = new Ping())
                        {
                            PingReply reply = ping.Send(address, 1000); // Timeout: 1000ms
                            if (reply.Status == IPStatus.Success)
                                richTextBox1.Text+=$"Kết nối đến mang thành công" + Environment.NewLine;
                            else
                                richTextBox1.Text += $"Không thể kết nối đến mang" + Environment.NewLine;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("LOI O NETWORK check");
                    }
                    var screen = CaptureHelper.CaptureScreen();
                    screen.Save("../image/Teleport.png");
                    screen.Dispose();
                }
                //mem leak fix
                subBitmap.Dispose();
                subBitmap1.Dispose();              
                subBitmaprareformB.Dispose();
                subBitmaprefuse.Dispose();
                subBitmapevole.Dispose();
                subBitmapyesevole.Dispose();
                subBitmapfrisk.Dispose();
                subBitmap3.Dispose();
                subBitmap4.Dispose();
                subBitmapYes.Dispose();
                subBitmapTrace.Dispose();
                subBitmap5.Dispose();
                subBitmap2.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("LOI O VI TRI 14"+ex.Message);
            }
        }
        static void AddPokemon(Dictionary<string, int> dict, string name)
        {
            try
            {
                if (dict.ContainsKey(name))
                    dict[name]++;
                else
                    dict[name] = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("LOI O VI TRI 15 "+ex.Message);
            }
        }
        private async Task SpamAWSD(CancellationToken token)
        {
            try
            {
                int f = 0;
                int z = 0;
                var subBitmap4 = ImageScanOpenCV.GetImage("11.png");
                int w_s_random = -1;
                prevousKey = VirtualKeyCode.VK_S;
                InputSimulator sim = new InputSimulator(); // Chỉ tạo 1 lần
                                                           //isTaskRunning = true; // Đánh dấu Task đang chạy

                DateTime checkteleport = DateTime.Now;
                taskRunning = Task.Run(async () =>
                {
                    try
                    {
                        if (checkBox4.Checked)
                        {
                            VirtualKeyCode vk1 = prevousKey switch
                            {
                                VirtualKeyCode.VK_D => VirtualKeyCode.VK_S,
                                VirtualKeyCode.VK_W => VirtualKeyCode.VK_S,
                                VirtualKeyCode.VK_A => VirtualKeyCode.VK_W,
                                VirtualKeyCode.VK_S => VirtualKeyCode.VK_W,
                                _ => VirtualKeyCode.SPACE
                            };
                            sim.Keyboard.KeyDown(vk1);
                            prevousKey = vk1;
                        }
                        else
                        {
                            VirtualKeyCode vk1 = prevousKey switch
                            {
                                VirtualKeyCode.VK_S => VirtualKeyCode.VK_D,
                                VirtualKeyCode.VK_W => VirtualKeyCode.VK_A,
                                VirtualKeyCode.VK_D => VirtualKeyCode.VK_A,
                                VirtualKeyCode.VK_A => VirtualKeyCode.VK_D,
                                _ => VirtualKeyCode.SPACE
                            };

                            sim.Keyboard.KeyDown(vk1);
                            prevousKey = vk1;
                        }

                        while (!token.IsCancellationRequested && OFFALL == 0)
                        {
                            if ((DateTime.Now - checkteleport).TotalSeconds > 25 && checkBox10.Checked)
                            {
                                teleport = 1;
                                cts?.Cancel();
                                cts = null;
                                break;
                            }

                            // Log nếu cần
                            try
                            {
                                Bitmap screenBitmap;
                                screenBitmap = (Bitmap)CaptureHelper.CaptureScreen();
                                if (ImageScanOpenCV.FindOutPoint(screenBitmap, subBitmap4) != null || Off != 0)
                                {

                                    checkteleport = DateTime.Now;
                                    sim.Keyboard.KeyUp(prevousKey);
                                    f = 1;
                                    Thread.Sleep(100);
                                    screenBitmap?.Dispose();
                                    continue;

                                }
                                else if (f == 1)
                                {
                                    w_s_random++;
                                    if ((checkBox14.Checked && w_s_random >= random.Next(int.Parse(textBox29.Text), int.Parse(textBox30.Text)))||(checkBox19.Checked&&rareformMove==1))
                                    {
                                        if (z == 0 && checkBox4.Checked)
                                        {
                                            sim.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                                            z = 1;
                                            richTextBox1.Text = "A";
                                        }
                                        else if (checkBox4.Checked)
                                        {
                                            z = 0;
                                            sim.Keyboard.KeyPress(VirtualKeyCode.VK_D);
                                            richTextBox1.Text = "D";
                                        }
                                        else if (z == 0)
                                        {
                                            sim.Keyboard.KeyPress(VirtualKeyCode.VK_W);
                                            z = 1;
                                            richTextBox1.Text = "W";
                                        }
                                        else
                                        {
                                            sim.Keyboard.KeyPress(VirtualKeyCode.VK_S);
                                            z = 0;
                                            richTextBox1.Text = "S";
                                        }
                                        w_s_random = 0;
                                        rareformMove = 0;
                                    }
                                    f = 0;

                                    sim.Keyboard.KeyDown(prevousKey);
                                }
                                screenBitmap?.Dispose();
                                await Task.Delay(random.Next(int.Parse(textBox12.Text), int.Parse(textBox13.Text)));
                                VirtualKeyCode vk = prevousKey switch
                                {
                                    VirtualKeyCode.VK_D => VirtualKeyCode.VK_A,
                                    VirtualKeyCode.VK_A => VirtualKeyCode.VK_D,
                                    VirtualKeyCode.VK_W => VirtualKeyCode.VK_S,
                                    VirtualKeyCode.VK_S => VirtualKeyCode.VK_W,
                                    _ => VirtualKeyCode.SPACE
                                };

                                sim.Keyboard.KeyUp(prevousKey);
                                sim.Keyboard.KeyDown(vk);
                                prevousKey = vk;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("LOI O VI TRI 18" + ex.Message);
                            }
                        }
                    }
                    finally
                    {
                        isTaskRunning = false; // Đánh dấu Task đã kết thúc
                        sim.Keyboard.KeyUp(prevousKey);
                        subBitmap4.Dispose();
                        this.Invoke((MethodInvoker)(() =>
                        {
                            richTextBox1.AppendText("SpamADSW-stop" + Environment.NewLine);
                            richTextBox1.ScrollToCaret();

                        }));
                    }
                });

                await taskRunning; // Chờ Task hoàn thành
            }
            catch (Exception ex)
            {
                MessageBox.Show("LOI O VI TRI 555 " + ex.Message);
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            cts?.Cancel();
            cts = null;
        }
        

        private void button7_Click(object sender, EventArgs e)
        {

            IntPtr hwnd = FindWindow("UnityWndClass", null);
            if (hwnd != IntPtr.Zero)
            {
                SetForegroundWindow(hwnd);
                Console.WriteLine("Đã đưa cửa sổ Unity lên foreground!");
            }
            else
            {
                Console.WriteLine("Không tìm thấy cửa sổ Unity.");
            }



            /*
            Process[] procs = Process.GetProcessesByName("PROClient"); // Tìm tiến trình PROClient.exe
            if (procs.Length == 0)
            {
                richTextBox1.AppendText("❌ Không tìm thấy tiến trình PROClient!\n");
                return;
            }

            foreach (var proc in procs)
            {
                if (proc.MainWindowHandle != IntPtr.Zero) // Chỉ lấy cửa sổ chính
                {
                    StringBuilder className = new StringBuilder(256);
                    StringBuilder windowTitle = new StringBuilder(256);

                    GetWindowText(proc.MainWindowHandle, windowTitle, windowTitle.Capacity);
                    GetClassName(proc.MainWindowHandle, className, className.Capacity);

                    richTextBox1.AppendText($"✅ Tìm thấy PROClient - hWnd: {proc.MainWindowHandle}, Title: {windowTitle}, ClassName: {className}\n");
                    return;
                }
            }

            richTextBox1.AppendText("⚠️ PROClient đang chạy nhưng không có cửa sổ hiển thị!\n");*/
        }
/*
        private void button8_Click(object sender, EventArgs e)
        {

            Image img = KAutoHelper.CaptureHelper.CaptureScreen();
            Bitmap screenshot = new Bitmap(img);

            // Tọa độ hình chữ nhật cần cắt
            int x = int.Parse(textBox7.Text), y = int.Parse(textBox8.Text), width = 350, height = 60;
            Bitmap croppedImage = CropImage(screenshot, x, y, width, height);
            Bitmap processedImage = PreprocessImage(croppedImage);

            // Lưu ảnh để kiểm tra (tùy chọn)
            processedImage.Save("processed_image.png", System.Drawing.Imaging.ImageFormat.Png);
           
            // Nhận diện chữ từ ảnh đã xử lý
            string extractedText;
            string extractedText1 = ExtractTextFromImage(processedImage);
            string extractedText2 = ExtractTextFromImage(croppedImage);
            if (extractedText2.Contains("ild"))
            {
                extractedText = extractedText2;
            }
            else { extractedText = extractedText1; }
            richTextBox1.Text =  extractedText;
            var screen = CaptureHelper.CaptureScreen();
            var subBitmaprareform = ImageScanOpenCV.GetImage("rare.png");
            var resBitmaprareform = ImageScanOpenCV.FindOutPoint((Bitmap)screen, subBitmaprareform);
            if (resBitmaprareform != null)
            {
                richTextBox1.Text += "RareForm";
            }
            if (checkBox6.Checked) 
            {
                richTextBox1.Text += "C6";
            }
            if (checkBox8.Checked) {
                richTextBox1.Text += "C8";
            }
        }
*/
        private Bitmap CropImage(Bitmap source, int x, int y, int width, int height)
        {
            Rectangle cropRect = new Rectangle(x, y, width, height);
            Bitmap target = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(source, new Rectangle(0, 0, width, height), cropRect, GraphicsUnit.Pixel);
            }
            return target;
        }

        private Bitmap PreprocessImage(Bitmap image)
        {
            // Resize ảnh lên 3 lần để OCR chính xác hơn
            int newWidth = image.Width * 3;
            int newHeight = image.Height * 3;
            Bitmap resizedImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            // Chuyển sang grayscale
            Bitmap grayImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(grayImage))
            {
                var colorMatrix = new ColorMatrix(new float[][]
                {
                    new float[] { 0.3f, 0.3f, 0.3f, 0, 0 },
                    new float[] { 0.59f, 0.59f, 0.59f, 0, 0 },
                    new float[] { 0.11f, 0.11f, 0.11f, 0, 0 },
                    new float[] { 0, 0, 0, 1, 0 },
                    new float[] { 0, 0, 0, 0, 1 }
                });

                var attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);
                g.DrawImage(resizedImage, new Rectangle(0, 0, newWidth, newHeight),
                            0, 0, newWidth, newHeight, GraphicsUnit.Pixel, attributes);
            }

            // Tăng độ tương phản (Thresholding)
            Bitmap thresholdImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(thresholdImage))
            {
                g.DrawImage(grayImage, 0, 0);
            }

            for (int i = 0; i < thresholdImage.Width; i++)
            {
                for (int j = 0; j < thresholdImage.Height; j++)
                {
                    Color pixel = thresholdImage.GetPixel(i, j);
                    int intensity = (pixel.R + pixel.G + pixel.B) / 3;
                    Color newColor = (intensity > 150) ? Color.White : Color.Black;
                    thresholdImage.SetPixel(i, j, newColor);
                }
            }

            return thresholdImage;
        }

        private string ExtractTextFromImage(Bitmap image)
        {
            string text = "";
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = PixConverter.ToPix(image))
                    {
                        using (var page = engine.Process(img, PageSegMode.SingleLine))
                        {
                            text = page.GetText().Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi OCR: " + ex.Message);
            }
            return text;
        }

        private async void button5_Click_2(object sender, EventArgs e)
        {
            textBox3.ReadOnly = true;
            button5.Enabled = false;
            textBox4.ReadOnly = true;
            textBox7.ReadOnly = true;
            textBox8.ReadOnly = true;
            try
            {
                if (!int.TryParse(textBox3.Text, out int minutes) || minutes <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số phút hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Dừng lại nếu nhập sai
                }

                cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(minutes));
                button7.PerformClick();

                // Chạy Task RunMouseAndKeyboard2
                await Task.Run(() => RunMouseAndKeyboard3(cts.Token));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cts?.Dispose();
                cts = null; // Đặt lại để có thể khởi động lại task sau này
                button5.Enabled = true;
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
                textBox7.ReadOnly = false;
                textBox8.ReadOnly = false;
                richTextBox1.Text += "Clear completed";
            }



        }
        private async void RunMouseAndKeyboard3(CancellationToken token)
        {
            SL_Click = 0;
            isTaskRunning = false;
            VirtualKeyCode prevousKey = VirtualKeyCode.VK_A;
            var screen = CaptureHelper.CaptureScreen();
            screen.Save("mainScreen.PNG");
            var subBitmap = ImageScanOpenCV.GetImage("8.png");
            var subBitmap1 = ImageScanOpenCV.GetImage("4.png");
            var subBitmap2 = ImageScanOpenCV.GetImage("ATK.png");
            var subBitmap4 = ImageScanOpenCV.GetImage("11.png");
            var subBitmap5 = ImageScanOpenCV.GetImage("swap.png");
            DateTime checkteleport=DateTime.Now;
            teleport = 0;
            while (!token.IsCancellationRequested) // Kiểm tra nếu bị hủy
            {                             
                
                
                if (ImageScanOpenCV.FindOutPoint((Bitmap)CaptureHelper.CaptureScreen(), subBitmap1) != null)
                {
                    richTextBox1.Text = "2";
                    cts?.Cancel();
                    cts = null;
                    break;
                }
                else if (ImageScanOpenCV.FindOutPoint((Bitmap)CaptureHelper.CaptureScreen(), subBitmap5) != null)
                {
                    richTextBox1.Text = "3";
                    if (SL_Click++ < int.Parse(textBox11.Text))
                    {
                        this.Invoke((MethodInvoker)(() =>
                        {
                            SendKeys.Send(SL_Click.ToString());
                        }));
                    }
                }
                else if (ImageScanOpenCV.FindOutPoint((Bitmap)CaptureHelper.CaptureScreen(), subBitmap2) != null)
                {
                    richTextBox1.Text = "4";
                    this.Invoke((MethodInvoker)(() =>
                        {
                            SendKeys.Send("1");

                        }));

                        await Task.Delay(3000);
                    
                }
                
                else if (ImageScanOpenCV.FindOutPoint((Bitmap)CaptureHelper.CaptureScreen(), subBitmap) != null)
                {
                    richTextBox1.Text = "5";
                    if (SL_Click < int.Parse(textBox11.Text))
                    {
                        richTextBox1.Text = SL_Click.ToString();
                        this.Invoke((MethodInvoker)(() =>
                        {
                            SendKeys.Send("2");
                        }));

                    }
                    else
                    {
                        this.Invoke((MethodInvoker)(() =>
                        {
                            SendKeys.Send("1");
                        }));

                        await Task.Delay(random.Next(1000, 2000));
                    }
                }
                else if ((ImageScanOpenCV.FindOutPoint((Bitmap)CaptureHelper.CaptureScreen(), subBitmap4) == null))
                {
                    richTextBox1.Text = "6";
                    SL_Click = 1;
                    checkteleport = DateTime.Now;
                    lock (taskLock) // Kiểm soát số lượng Task
                    {
                        if (isTaskRunning) continue; // Nếu Task đang chạy, không tạo mới

                        isTaskRunning = true;
                        taskRunning = Task.Run(() =>
                        {
                            try
                            {

                                while (ImageScanOpenCV.FindOutPoint((Bitmap)CaptureHelper.CaptureScreen(), subBitmap4) == null && !token.IsCancellationRequested)
                                {
                                    if ((DateTime.Now - checkteleport).TotalSeconds > 25)
                                    {
                                        richTextBox1.Text = "1";
                                        teleport = 1;
                                        cts?.Cancel();
                                        cts = null;
                                        break;
                                    }
                                    Task.Delay(random.Next(30, 70));
                                    VirtualKeyCode vk = prevousKey switch
                                    {
                                        VirtualKeyCode.VK_A => VirtualKeyCode.VK_D,
                                        VirtualKeyCode.VK_D => VirtualKeyCode.VK_A,
                                        _ => VirtualKeyCode.SPACE
                                    };

                                    InputSimulator sim = new InputSimulator();
                                    sim.Keyboard.KeyUp(prevousKey);
                                    sim.Keyboard.KeyDown(vk);

                                    prevousKey = vk;

                                }

                            }
                            finally
                            {
                                isTaskRunning = false; // Đánh dấu Task đã kết thúc
                                sim.Keyboard.KeyUp(prevousKey);
                            }
                        });
                    }
                }
            }
            richTextBox1.AppendText("Stop-Oke" + Environment.NewLine);
            subBitmap.Dispose();
            subBitmap1.Dispose();
            subBitmap2.Dispose();
            subBitmap4.Dispose();
            subBitmap5.Dispose();
            if (teleport == 1)
            {
                SoundPlayer player = new SoundPlayer("rare.wav");
                player.PlaySync();
                richTextBox1.Text = "Co the da bi teleport";
                var screen1 = CaptureHelper.CaptureScreen();
                screen1.Save("../image/Teleport.png");
                screen1.Dispose();
            }
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            foreach (var entry in pokemonCounts)
            {
                richTextBox1.AppendText($"{entry.Key}: {entry.Value}"+Environment.NewLine);
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            foreach (var entry in pokemonCatchs)
            {
                richTextBox1.AppendText($"{entry.Key}: {entry.Value}"+ Environment.NewLine);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            foreach (var entry in pokemonAbility)
            {
                richTextBox1.AppendText($"{entry.Key}: {entry.Value}" + Environment.NewLine);
            }
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            button8.Enabled = false;
            await SendEmailNotification();
            await Task.Delay(1000);
            await EmailReplyChecker(messageId);
            button8.Enabled = true;
        }
    }
}
