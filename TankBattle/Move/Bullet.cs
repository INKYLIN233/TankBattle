using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankBattle.Properties;

namespace TankBattle
{
    enum Tag
    {
        MyTankBullet,
        EnemyTankBullet
    }
    internal class Bullet:MoveThing
    {
        public Tag Tag {  get; set; }

        public bool IsBulletDestroy { get; set; }

        public Bullet(int x, int y, int speed, Direction dir, Tag tag)
        {
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            //引用资源图片，以及将图片背景设置为透明
            BitmapUp = Resources.BulletUp;
            BitmapUp.MakeTransparent();
            BitmapDown = Resources.BulletDown;
            BitmapDown.MakeTransparent();
            BitmapLeft = Resources.BulletLeft;
            BitmapLeft.MakeTransparent();
            BitmapRight = Resources.BulletRight;
            BitmapRight.MakeTransparent();
            this.Dir = dir;
            this.Tag = tag;
            this.IsBulletDestroy = false;

            this.X -= Width / 2;
            this.Y -= Height / 2;
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
                if (Y + Height / 2 + 3 < 0)
                {
                    //GameObjectManager.DestroyBullet(this);
                    IsBulletDestroy = true;
                    return;
                }
            }
            else if (Dir == Direction.Down)
            {
                if (Y + Height / 2 - 3 > 450)
                {
                    //GameObjectManager.DestroyBullet(this);
                    IsBulletDestroy= true;
                    return;
                }
            }
            else if (Dir == Direction.Left)
            {
                if (X + Width / 2 - 3 < 0)
                {
                    //GameObjectManager.DestroyBullet(this);
                    IsBulletDestroy = true;
                    return;
                }
            }
            else if (Dir == Direction.Right)
            {
                if (X + Width / 2 + 3 > 450)
                {
                    //GameObjectManager.DestroyBullet(this);
                    IsBulletDestroy = true;
                    return;
                }
            }
            #endregion
            #region 检测是否与其他元素发生碰撞
            //获取未来的元素位置
            Rectangle rt = GetRectangle();
            rt.X = X + Width / 2 - 5;
            rt.Y = Y + Height / 2 - 5;

            rt.Width = 5;
            rt.Height = 5;

            int xExplosion = this.X + Width / 2;
            int yExplosion = this.Y + Height / 2;

            ImmovableThing wall = null;
            if (( wall = GameObjectManager.IsCollidedWall(rt)) != null)
            {
                IsBulletDestroy = true;
                GameObjectManager.DestroyWall(wall);
                GameObjectManager.CreatExplosion(xExplosion, yExplosion);
                SoundManager.PlayBlast();
                return;
            }
            if (GameObjectManager.IsCollidedSteel(rt) != null)
            {
                IsBulletDestroy = true;
                //ChangDirection();
                GameObjectManager.CreatExplosion(xExplosion, yExplosion);
                SoundManager.PlayBlast();
                return;
            }
            if (Tag == Tag.MyTankBullet)
            {
                EnemyTank tank = null;
                if ((tank = GameObjectManager.IsCollidedEnemyTank(rt)) != null)
                {
                    IsBulletDestroy = true;
                    GameObjectManager.DestroyEnemyTank(tank);
                    GameObjectManager.CreatExplosion(xExplosion, yExplosion);
                    SoundManager.PlayHit();
                    return;
                }
            }
            else if (Tag == Tag.EnemyTankBullet)
            {
                MyTank myTank = null;
                if ((myTank = GameObjectManager.IsCollidedMyTank(rt)) != null)
                {
                    IsBulletDestroy = true;
                    GameObjectManager.CreatExplosion(xExplosion, yExplosion);
                    myTank.TakeDamage();
                    SoundManager.PlayBlast();
                    return;
                }
            }
            if (GameObjectManager.IsCollidedBoss(rt))
            {
                GameFramework.ChangeGameState();
                return;
            }
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
    }
}
