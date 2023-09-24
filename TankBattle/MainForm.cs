using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TankBattle.Properties;

namespace TankBattle
{
    public partial class MainForm : Form
    {
        private Thread t;
        private static Graphics windowG;
        private static Bitmap tempBmp;
        public MainForm()
        {
            InitializeComponent();
            //窗口生成在当前屏幕的正中心(CenterScreen)
            //窗口生成在设置的位置(Manual)-this.Location = new Point(x,y); 
            this.StartPosition = FormStartPosition.CenterScreen;

            windowG = this.CreateGraphics();//窗体画布的获得
            //GameFramework.g = g;//将窗体画布赋值给游戏开发框架（Graphics无法继承）

            //临时画布
            tempBmp = new Bitmap(450, 450);
            Graphics bitmapG = Graphics.FromImage(tempBmp);
            GameFramework.g = bitmapG;

            //阻塞
            t = new Thread(new ThreadStart(GameMainThread));
            t.Start();
        }

        public static void GameMainThread()
        {
            //GameFramework
            GameFramework.Start();
            //windowG.DrawImage(tempBmp, 0, 0);//绘制

            //定时器
            int sleepTime = 1000 / 60;

            while (true)
            {
                //涂黑画布
                GameFramework.g.Clear(Color.Black);

                GameFramework.Update();

                //windowG.DrawImage(tempBmp, 30, 25);//绘制
                windowG.DrawImage(tempBmp, 0, 0);

                Thread.Sleep(sleepTime);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //中止子线程
            t.Abort();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            GameFramework.KeyDown(e);
            //GameObjectManager.KeyDown(e);
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            GameFramework.KeyUp(e);
            //GameObjectManager.KeyUp(e);
        }
    }
}
