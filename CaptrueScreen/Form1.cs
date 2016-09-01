using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Threading;
namespace CaptureScreen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.notifyIcon1.DoubleClick += new EventHandler(notifyIcon1_DoubleClick);
            this.notifyIcon1.ShowBalloonTip(500);
        }

        #region 数据
        bool isCapture=false;
        bool isLeftMDown = false;
        Point pDown;
        Point pUp;
        Point pNow;

        Thread thread;
        ThreadStart tStart;

        MouseHookStruct _lParam;
        IntPtr hDCDesktop;
        Graphics g;
        Point topLeft;
        Size size;
        Rectangle rec;
        SolidBrush brush;
        Pen pen;
        MyInvoke mi;
        DialogResult dialogResult;
        #endregion

        #region 重写WndProc
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_HOTKEY: //窗口消息-热键
                    switch (m.WParam.ToInt32())
                    {
                        case 32: //热键ID
                            //MessageBox.Show("Hot Key : Ctrl + Alt + Shift + Space");
                            if (MouseHook() == 0)
                            {
                                MessageBox.Show("鼠标钩子hook失败！");
                                this.Dispose();
                            }
                            isCapture = true;
                            break;
                        default:
                            break;
                    }
                    break;
                case WM_CREATE: //窗口消息-创建
                    RegKey(Handle, Space, MOD_ALT | MOD_CONTROL | MOD_SHIFT, VK_SPACE); //注册热键
                    break;
                case WM_DESTROY: //窗口消息-销毁
                    UnRegKey(Handle, Space); //销毁热键
                    MouseUnhook();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 注册系统热键
        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(IntPtr hwnd, int id);
        int Space = 32; //热键ID
        private const int WM_HOTKEY = 0x312; //窗口消息-热键
        private const int WM_CREATE = 0x1; //窗口消息-创建
        private const int WM_DESTROY = 0x2; //窗口消息-销毁
        private const int MOD_ALT = 0x1; //ALT
        private const int MOD_CONTROL = 0x2; //CTRL
        private const int MOD_SHIFT = 0x4; //SHIFT
        private const int VK_SPACE = 0x20; //SPACE
        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="hotKey_id">热键ID</param>
        /// <param name="fsModifiers">组合键</param>
        /// <param name="vk">热键</param>
        private void RegKey(IntPtr hwnd, int hotKey_id, int fsModifiers, int vk)
        {
            bool result;
            if (RegisterHotKey(hwnd, hotKey_id, fsModifiers, vk) == 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            if (!result)
            {
                MessageBox.Show("注册热键失败！");
                this.Dispose();
            }
        }
        /// <summary>
        /// 注销热键
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="hotKey_id">热键ID</param>
        private void UnRegKey(IntPtr hwnd, int hotKey_id)
        {
            UnregisterHotKey(hwnd, hotKey_id);
        }
        #endregion

        #region 系统事件虚键
        //protected const int WH_MOUSE_LL=14;
        protected const int WH_KEYBOARD_LL=13;
 
        protected const int WH_MOUSE=7;
        protected const int WH_KEYBOARD=2;
        protected const int WM_MOUSEMOVE=0x200;
        protected const int WM_LBUTTONDOWN=0x201;
        protected const int WM_RBUTTONDOWN=0x204;
        protected const int WM_MBUTTONDOWN=0x207;
        protected const int WM_LBUTTONUP=0x202;
        protected const int WM_RBUTTONUP=0x205;
        protected const int WM_MBUTTONUP=0x208;
        protected const int WM_LBUTTONDBLCLK=0x203;
        protected const int WM_RBUTTONDBLCLK=0x206;
        protected const int WM_MBUTTONDBLCLK=0x209;
        protected const int WM_MOUSEWHEEL=0x020A;
        protected const int WM_KEYDOWN=0x100;
        protected const int WM_KEYUP=0x101;
        protected const int WM_SYSKEYDOWN=0x104;
        protected const int WM_SYSKEYUP=0x105;
 
        protected const byte VK_SHIFT=0x10;
        protected const byte VK_CAPITAL=0x14;
        protected const byte VK_NUMLOCK=0x90;
 
        protected const byte VK_LSHIFT=0xA0;
        protected const byte VK_RSHIFT=0xA1;
        protected const byte VK_LCONTROL=0xA2;
        protected const byte VK_RCONTROL=0x3;
        protected const byte VK_LALT=0xA4;
        protected const byte VK_RALT=0xA5;
 
        protected const byte LLKHF_ALTDOWN=0x20;
        #endregion

        #region 鼠标hook
        public int hookID_Mouse;


        private const int WH_MOUSE_LL = 14;
        private IntPtr hMod = IntPtr.Zero;
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }
        private delegate IntPtr HookHandlerDelegate(int nCode, IntPtr wParam, IntPtr lParam);//ref KBDLLHOOKSTRUCT lParam
        ///安装钩子
        //idHook 鼠标为14 ;lpfn 处理键盘事件的委托;
        //hMod 用于设置钩子的应用程序的实例句柄。大多数程序中都把它设为IntPtr.Zero
        //dwThreadId 当前线程的ID。把它设为0表示钩子为全局的，这也是对于低级键盘钩子必须的值。
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SetWindowsHookEx(int idHook, HookHandlerDelegate lpfn, IntPtr hMod, int dwThreadId);
        //下一个钩挂的函数 
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(int hhk, int nCode, IntPtr wParam, IntPtr lParam);//ref KBDLLHOOKSTRUCT lParam
        //卸下钩子的函数 
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(int idHook);


        //GetDesktopWindow的声明
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hwnd, IntPtr rcUpdate, IntPtr hrgnUpdate, int flags);


        //下面定义键盘处理的委托
        private IntPtr HookCaback(int nCode, IntPtr wParam, IntPtr lParam)//ref KBDLLHOOKSTRUCT lParam
        {
            _lParam = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
            if (nCode >= 0 && isCapture)
            {
                //this.lbMouseX.Text=_lParam.pt.x.ToString();
                //this.lbMouseY.Text = _lParam.pt.y.ToString();
                if ((int)wParam == WM_LBUTTONDOWN)//按下鼠标左键
                {
                    pDown = new Point(_lParam.pt.x, _lParam.pt.y);
                    isLeftMDown = true;
                    this.timer1.Start();
                    return (System.IntPtr)1;
                }
                else if ((int)wParam == WM_LBUTTONUP)//松开鼠标左键
                {
                    isLeftMDown = false;
                    this.timer1.Stop();
                    RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0x85);
                    pUp = new Point(_lParam.pt.x, _lParam.pt.y);
                    tStart = new ThreadStart(DoWork);
                    thread = new Thread(tStart);
                    thread.Start();
                    //CapScreen(pDown,pUp);
                    return (System.IntPtr)1;
                }
                else if (isLeftMDown && (int)wParam == WM_MOUSEMOVE)
                {
                    pNow = new Point(_lParam.pt.x, _lParam.pt.y);
                }
            }
            //事件未被处理，把它传递给下一个程序。
            return CallNextHookEx(hookID_Mouse, nCode, wParam, lParam);
        }

        public int MouseHook()
        {
            HookHandlerDelegate lpfn = new HookHandlerDelegate(HookCaback);
            hookID_Mouse = SetWindowsHookEx(WH_MOUSE_LL, lpfn, hMod, 0);
            return hookID_Mouse;
        }
        public void MouseUnhook()
        {
            if (hookID_Mouse != 0)
            {
                UnhookWindowsHookEx(hookID_Mouse);
                hookID_Mouse = 0;
            }
        }
        #endregion

        #region 截屏函数
        public void DoWork()
        {
            mi = new MyInvoke(CapScreen);
            this.BeginInvoke(mi, new Object[] { pDown, pUp });
        }
        public delegate void MyInvoke(Point pDown, Point pUp);
        public void CapScreen(Point pDown,Point pUp)
        {
            try
            {
                isCapture = false;
                topLeft = new Point();
                topLeft.X = pDown.X < pUp.X ? pDown.X : pUp.X;
                topLeft.Y = pDown.Y < pUp.Y ? pDown.Y : pUp.Y;
                size = new Size(Math.Abs(pDown.X - pUp.X), Math.Abs(pDown.Y - pUp.Y));
                //创建一个和截图一样大的Bitmap
                Image imgFullScreen = new Bitmap(size.Width, size.Height);
                //从一个继承自Image类的对象中创建Graphics对象
                g = Graphics.FromImage(imgFullScreen);
                //抓屏并拷贝到myimage里
                g.CopyFromScreen(topLeft, new Point(0, 0), size);
                g.Dispose();

                Clipboard.SetImage(imgFullScreen);
                string localFilePath = System.Environment.CurrentDirectory +@"\"+ System.DateTime.Now.ToString("yyyyMMdd") + ".jpg";
                imgFullScreen.Save(localFilePath);
                /*
                saveFileDialog1.FileName = System.DateTime.Now.ToString("yyyyMMdd")+".jpg";
                dialogResult=saveFileDialog1.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    string localFilePath = saveFileDialog1.FileName.ToString(); //获得文件路径
                    //MessageBox.Show(localFilePath);
                    imgFullScreen.Save(localFilePath);
                }
                */
                imgFullScreen.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show("截屏失败");
            }
            MouseUnhook();

        }
        #endregion

        #region 双击NotifiIcon关闭程序
        //双击NotifiIcon关闭程序
        void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            hDCDesktop = GetWindowDC(GetDesktopWindow());
            g = Graphics.FromHdc(hDCDesktop);//创建Graphics,这样才会有效。
            topLeft = new Point();
            topLeft.X = pDown.X < pNow.X ? pDown.X : pNow.X;
            topLeft.Y = pDown.Y < pNow.Y ? pDown.Y : pNow.Y;
            size = new Size(Math.Abs(pDown.X - pNow.X), Math.Abs(pDown.Y - pNow.Y));

            //从一个继承自Image类的对象中创建Graphics对象
            rec = new Rectangle(topLeft, size);

            brush = new SolidBrush(Color.Red);
            pen = new Pen(brush);
            RedrawWindow(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0x85);
            g.DrawRectangle(pen, rec);
            g.Dispose();
        }
    }
}
