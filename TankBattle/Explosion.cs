using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankBattle.Properties;

namespace TankBattle
{
    internal class Explosion:GameObject
    {
        public bool IsEXPNeedDestroy {  get; set; }

        private int PlaySpeed = 1;
        private int PlayCount = -1;
        private int index = -1;
        //count = 1 / speed = 2 == 0 播放EXP1


        private Bitmap[] bmpArray = new Bitmap[] {
            Resources.EXP1,
            Resources.EXP2,
            Resources.EXP3,
            Resources.EXP4,
            Resources.EXP5
        };

        public Explosion(int x, int y)
        {
            foreach (var item in bmpArray)
            {
                item.MakeTransparent(Color.Black);
            }
            this.X = x - bmpArray[0].Width / 2;
            this.Y = y - bmpArray[0].Height / 2;
            IsEXPNeedDestroy = false;
        }

        protected override Image GetImage()
        {
            if (index > bmpArray.Length - 1)
                return bmpArray[bmpArray.Length - 1];
            return bmpArray[index];
        }
        public override void Update()
        {
            PlayCount++;
            index = PlayCount / PlaySpeed;
            if (index > bmpArray.Length - 1)
                IsEXPNeedDestroy = true;
            base.Update();
        }
    }
}
