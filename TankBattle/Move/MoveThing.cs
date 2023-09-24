using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    internal class MoveThing:GameObject
    {
        //锁
        private Object _Lock = new Object();

        //图片
        public Bitmap BitmapUp {  get; set; }
        public Bitmap BitmapDown { get; set; }
        public Bitmap BitmapLeft { get; set; }
        public Bitmap BitmapRight { get; set; }

        //速度
        public int Speed { get; set; } 

        //方位
        private Direction dir;
        public Direction Dir
        {
            get
            { 
                return dir; 
            }
            set 
            { 
                dir = value;
                Bitmap bmp = null;

                switch (dir)
                {
                    case Direction.Up:
                        bmp = BitmapUp;
                        break;
                    case Direction.Down:
                        bmp = BitmapDown;
                        break;
                    case Direction.Left:
                        bmp = BitmapLeft;
                        break;
                    case Direction.Right:
                        bmp = BitmapRight;
                        break;
                }                
                lock (_Lock)
                {
                    Width = bmp.Width;
                    Height = bmp.Height;
                }
            }
        }
        protected override Image GetImage()
        {
            Bitmap bitmap = null;
            switch (Dir)
            {
                case Direction.Up:
                    bitmap = BitmapUp;
                    break;
                case Direction.Down: 
                    bitmap = BitmapDown;
                    break;
                case Direction.Left: 
                    bitmap = BitmapLeft;
                    break;
                case Direction.Right: 
                    bitmap = BitmapRight;
                    break;
            }
            //设置透明度
            bitmap.MakeTransparent(Color.Black) ;

            return bitmap;
        }
        public void Attack(Tag tag)
        {
            if (tag == Tag.MyTankBullet)
            {
                SoundManager.PlayFire();
            }
            int x = this.X;
            int y = this.Y;
            int speed = this.Speed;
            switch (Dir)
            {
                case Direction.Up:
                    x = x + Width / 2;
                    break;
                case Direction.Down:
                    x = x + Width / 2;
                    y += Height;
                    break;
                case Direction.Left:
                    y = y + Height / 2;
                    break;
                case Direction.Right:
                    x += Width;
                    y = y + Height / 2;
                    break;
            }
            GameObjectManager.CreatBullet(x, y, speed, tag, Dir);
        }
        public override void DrawSelf()
        {
            lock (_Lock)
            {
                base.DrawSelf();
            }
        }
    }
}
