using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    internal class EnemyTank:MoveThing
    {
        private Random random = new Random();
        private Random changeRandom = new Random();
        public int AttackSpeed { get; set; }
        private int AttackCount = 0;

        public EnemyTank(int x, int y, int speed, List<Bitmap> bitmaps)
        {
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            BitmapUp = bitmaps[0];
            BitmapDown = bitmaps[1];
            BitmapLeft = bitmaps[2];
            BitmapRight = bitmaps[3];
            this.Dir = Direction.Down;
            AttackSpeed = 60;
        }
        public override void Update()
        {
            MoveCheek();
            Move();
            AttackCheek();
            base.Update();
        }

        private void ChangeDirection()
        {
            //random.Next(0, 4);
            //switch (random.Next(0, 4))
            //{
            //    case 0:
            //        this.Dir = Direction.Up;
            //        break;
            //    case 1:
            //        this.Dir = Direction.Down;
            //        break;
            //    case 2:
            //        this.Dir = Direction.Left;
            //        break;
            //    case 3:
            //        this.Dir = Direction.Right;
            //        break;
            //}
            while (true)
            {
                Direction dir = (Direction)random.Next(0, 4);
                if (dir == Dir)
                {
                    continue;
                }
                else
                {
                    Dir = dir;
                    break;
                }
            }
            MoveCheek();
        }
        private void MoveCheek()
        {
            #region 检测是否超出边界
            if (Dir == Direction.Up)
            {
                if (Y - Speed < 0)
                {
                    ChangeDirection();
                    return;
                }
            }
            else if (Dir == Direction.Down)
            {
                if (Y + Height + Speed > 450)
                {
                    ChangeDirection();
                    return;
                }
            }
            else if (Dir == Direction.Left)
            {
                if (X - Speed < 0)
                {
                    ChangeDirection();
                    return;
                }
            }
            else if (Dir == Direction.Right)
            {
                if (X + Width + Speed > 450)
                {
                    ChangeDirection();
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
                ChangeDirection();
                return;
            }
            if (GameObjectManager.IsCollidedSteel(rt) != null)
            {
                ChangeDirection();
                return;
            }
            //if (GameObjectManager.IsCollidedBoss(rt) != null)
            //{
            //    IsMoving = false;
            //    return;
            //}
            if (GameObjectManager.IsCollidedBoss(rt))
            {
                ChangeDirection();
                return;
            }
            #endregion
            #region 随机转向
            int cr = changeRandom.Next(0, 100);
            if (cr == 1 || cr == 0)
                ChangeDirection();
            #endregion
        }
        private void Move()
        {
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
        private void AttackCheek()
        {
            AttackCount++;
            if (AttackCount < AttackSpeed)
                return;
            Attack(Tag.EnemyTankBullet);
            AttackCount = 0;
        }
    }
}
