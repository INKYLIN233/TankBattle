using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TankBattle.Properties;

namespace TankBattle
{
    internal class MyTank:MoveThing
    {
        //移动
        private bool IsMoving { get; set; }
        private int HP { get; set; }
        private int originalX;
        private int originalY;
        public MyTank(int x, int y,int speed)
        {
            IsMoving = false;
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            originalX = x;
            originalY = y;
            BitmapUp = Resources.MyTankUp;
            BitmapDown = Resources.MyTankDown;
            BitmapLeft = Resources.MyTankLeft;
            BitmapRight = Resources.MyTankRight;
            this.Dir = Direction.Up;
            this.HP = 4;
        }
        public override void Update()
        {
            MoveCheek();
            Move();
            base.Update();
        }
        private void MoveCheek()
        {

            #region 检测是否超出边界
            if (Dir == Direction.Up)
            {
                if (Y - Speed < 0)
                {
                    IsMoving = false;
                    return;
                }
            }
            else if (Dir == Direction.Down)
            {
                if (Y + Height + Speed > 450)
                {
                    IsMoving = false;
                    return;
                }
            }
            else if (Dir == Direction.Left)
            {
                if (X - Speed < 0)
                {
                    IsMoving = false;
                    return;
                }
            }
            else if (Dir == Direction.Right)
            {
                if (X + Width + Speed > 450)
                {
                    IsMoving = false;
                    return;
                }
            }
            #endregion
            #region 检测是否与其他元素发生碰撞
            //获取未来的元素位置
            Rectangle rt = GetRectangle();
            switch (Dir)
            {
                case Direction.Up:
                    rt.Y -= Speed;
                    break;
                case Direction.Down:
                    rt.Y += Speed;
                    break;
                case Direction.Left:
                    rt.X -= Speed;
                    break;
                case Direction.Right:
                    rt.X += Speed;
                    break;
                default:
                    break;
            }
            if (GameObjectManager.IsCollidedWall(rt) != null)
            {
                IsMoving = false;
                return;
            }
            if (GameObjectManager.IsCollidedSteel(rt) != null)
            {
                IsMoving = false;
                return;
            }
            //if (GameObjectManager.IsCollidedBoss(rt) != null)
            //{
            //    IsMoving = false;
            //    return;
            //}
            if (GameObjectManager.IsCollidedBoss(rt))
            {
                IsMoving = false;
                return;
            }
            #endregion
        }
        private void Move()
        {
            if (IsMoving == false)
                return;
            switch (Dir)
            {
                case Direction.Up:
                    Y -= Speed;
                    break;
                case Direction.Down:
                    Y += Speed;
                    break;
                case Direction.Left:
                    X -= Speed;
                    break;
                case Direction.Right:
                    X += Speed;
                    break;
                default:
                    break;
            }
        }
        public void KeyDown(KeyEventArgs args)
        {
            switch (args.KeyCode)
            {
                case Keys.W:
                    Dir = Direction.Up;
                    IsMoving = true;
                    break;
                case Keys.S:
                    Dir = Direction.Down;
                    IsMoving = true;
                    break;
                case Keys.D:
                    Dir = Direction.Right;
                    IsMoving = true;
                    break;
                case Keys.A:
                    Dir = Direction.Left;
                    IsMoving = true;
                    break;
            }
        }
        public void KeyUp(KeyEventArgs args)
        {
            switch (args.KeyCode)
            {
                case Keys.W:
                    IsMoving = false;
                    break;
                case Keys.S:
                    IsMoving = false;
                    break;
                case Keys.D:
                    IsMoving = false;
                    break;
                case Keys.A:
                    IsMoving = false;
                    break;
                //将攻击事件放在KeyUp中，使子弹生成无法在KeyDown中持续生成
                case Keys.Space:
                    Attack(Tag.MyTankBullet);
                    break;
            }
        }
        public void TakeDamage()
        {
            HP--;
            X = originalX;
            Y = originalY;
            if (HP == 0)
            {
                GameFramework.ChangeGameState();
            }
        }
    }
}
