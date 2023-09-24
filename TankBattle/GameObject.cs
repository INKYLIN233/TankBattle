using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    internal abstract class GameObject
    {
        //位置
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        //图片
        protected abstract Image GetImage();

        public Rectangle GetRectangle()
        {
            Rectangle rectangle = new Rectangle(X, Y, Width, Height);
            return rectangle;
        }

        //绘制
        public virtual void DrawSelf()
        {
            //传递画布
            Graphics g = GameFramework.g;

            g.DrawImage(GetImage(), new Point(X, Y));
        }

        public virtual void Update()
        {
            DrawSelf();
        }
    }
}
