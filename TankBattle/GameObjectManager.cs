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
    enum Direction { Up, Down, Left, Right }
    internal class GameObjectManager
    {
        private static List<ImmovableThing> wallList = new List<ImmovableThing>();
        private static List<ImmovableThing> steelList = new List<ImmovableThing>();
        private static ImmovableThing boss;
        private static MyTank myTank;
        private static List<Bullet> bulletList = new List<Bullet>();
        private static List<Explosion> explosionList = new List<Explosion>();

        private static Bitmap Wall = Resources.wall;
        private static Bitmap Steel = Resources.steel;
        private static Bitmap Boss = Resources.Boss;

        private static int enemyBornSpeed = 60;
        private static int enemyBornCount = -120; //计时器
        private static int maxEnemyTank = 8;

        private static Point[] points = new Point[3];
        private static List<EnemyTank> enemyTankList = new List<EnemyTank>();

        public static void Update()
        {
            foreach (var item in wallList)
            {
                item.Update();
            }
            foreach (var item in steelList)
            {
                item.Update();
            }
            foreach (var item in enemyTankList)
            {
                item.Update();
            }
            CheckAndDestroyBulllet();
            //foreach (var item in bulletList)
            //{
            //    item.Update();
            //}
            for (int i = 0; i < bulletList.Count; i++)
            {
                bulletList[i].Update();
            }
            CheckAndDestroyEXP();
            foreach (var item in explosionList)
            {
                item.Update();
            }
            boss.Update();
            myTank.Update();
            EnemyBorn();
        }

        private static void EnemyBorn()
        {
            enemyBornCount++;
            if (enemyBornCount < enemyBornSpeed)
                return;

            SoundManager.PlayAdd();
            //随机生成
            if (enemyTankList.Count < maxEnemyTank)
            {
                Random random = new Random();
                int index = random.Next(0, 3);
                Point position = points[index];
                int enemyType = random.Next(1, 5);
                switch (enemyType)
                {
                    case 1:
                        CreateEnemyTank1(position.X, position.Y);
                        break;
                    case 2:
                        CreateEnemyTank2(position.X, position.Y);
                        break;
                    case 3:
                        CreateEnemyTank3(position.X, position.Y);
                        break;
                    case 4:
                        CreateEnemyTank4(position.X, position.Y);
                        break;
                }
            }
            enemyBornCount = 0;
        }

        /// <summary>
        /// 生成子弹，并添加进子弹列表
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="speed"></param>
        /// <param name="tag"></param>
        /// <param name="dir"></param>
        public static void CreatBullet(int x, int y, int speed, Tag tag, Direction dir)
        {
            Bullet bullet = new Bullet(x, y, speed * 2, dir, tag);
            bulletList.Add(bullet);
        }

        /// <summary>
        /// 检测是否需要删除子弹
        /// </summary>
        private static void CheckAndDestroyBulllet()
        {
            List<Bullet> needToDestroy = new List<Bullet>();
            foreach (var item in bulletList)
            {
                if (item.IsBulletDestroy == true)
                {
                    needToDestroy.Add(item);
                }
            }
            foreach (var item in needToDestroy)
            {
                bulletList.Remove(item);
            }
        }

        /// <summary>
        /// 摧毁墙
        /// </summary>
        /// <param name="wall"></param>
        public static void DestroyWall(ImmovableThing wall)
        {
            wallList.Remove(wall);
        }

        #region 爆炸特效
        public static void CreatExplosion(int x, int y)
        {
            Explosion exp = new Explosion(x, y);
            explosionList.Add(exp);
        }
        private static void CheckAndDestroyEXP()
        {
            List<Explosion> needToDestroy = new List<Explosion>();
            foreach (var item in explosionList)
            {
                if (item.IsEXPNeedDestroy == true)
                {
                    needToDestroy.Add(item);
                }
            }
            foreach (var item in needToDestroy)
            {
                explosionList.Remove(item);
            }
        } 
        #endregion

        #region 碰撞检测机制
        /// <summary>
        /// 检测是否与墙体图片发生接触，如果发生接触，则返回true
        /// </summary>
        /// <param name="rct"></param>
        /// <returns></returns>
        public static ImmovableThing IsCollidedWall(Rectangle rct)
        {
            foreach (var item in wallList)
            {
                if (item.GetRectangle().IntersectsWith(rct))
                {
                    return item;
                }
            }
            return null;
        }
        public static ImmovableThing IsCollidedSteel(Rectangle rct)
        {
            foreach (var item in steelList)
            {
                if (item.GetRectangle().IntersectsWith(rct))
                {
                    return item;
                }
            }
            return null;
        }
        public static EnemyTank IsCollidedEnemyTank(Rectangle rt)
        {
            foreach (var item in enemyTankList)
            {
                if (item.GetRectangle().IntersectsWith(rt))
                {
                    return item;
                }
            }
            return null;
        }
        internal static MyTank IsCollidedMyTank(Rectangle rt)
        {
            if (myTank.GetRectangle().IntersectsWith(rt))
            {
                return myTank;
            }
            else
            {
                return null;
            }
        }
        public static bool IsCollidedBoss(Rectangle rct)
        {
            return boss.GetRectangle().IntersectsWith(rct);
        } 
        internal static void DestroyMyTank(MyTank myTank)
        {
            
        }
        public static void DestroyEnemyTank(EnemyTank item)
        {
            enemyTankList.Remove(item);
        }

        #endregion

        #region 废案
        //public static void DrawMap()
        //{
        //    foreach (var item in wallList)
        //    {
        //        item.DrawSelf();
        //    }
        //    foreach (var item in steelList)
        //    {
        //        item.DrawSelf();
        //    }
        //    boss.DrawSelf();
        //}

        //public static void DrawMyTank()
        //{
        //    myTank.DrawSelf();
        //} 
        #endregion

        #region 初始地图设置
        public static void CreatMap()
        {
            CreatYWall(1, 1, 5, Wall, wallList);
            CreatYWall(3, 1, 5, Wall, wallList);
            CreatYWall(5, 1, 5, Wall, wallList);
            CreatYWall(7, 1, 4, Wall, wallList);
            CreatYWall(9, 1, 5, Wall, wallList);
            CreatYWall(11, 1, 5, Wall, wallList);
            CreatYWall(13, 1, 5, Wall, wallList);
            CreatYWall(1, 9, 5, Wall, wallList);
            CreatYWall(13, 9, 5, Wall, wallList);
            CreatYWall(4, 7, 4, Wall, wallList);
            CreatYWall(10, 7, 4, Wall, wallList);
            CreatXWall(1, 7, 2, Wall, wallList);
            CreatXWall(6, 7, 1, Wall, wallList);
            CreatXWall(8, 7, 1, Wall, wallList);
            CreatXWall(6, 8, 3, Wall, wallList);
            CreatXWall(6, 9, 1, Wall, wallList);
            CreatXWall(8, 9, 1, Wall, wallList);
            CreatXWall(12, 7, 2, Wall, wallList);
            CreatXWall(3, 11, 3, Wall, wallList);
            CreatXWall(7, 11, 1, Steel, steelList);
            CreatXWall(9, 11, 3, Wall, wallList);
            CreatXWall(3, 13, 2, Wall, wallList);
            CreatXWall(10, 13, 2, Wall, wallList);
            CreatXWall(6, 5, 3, Steel, steelList);
            CreatXWall(0, 7, 1, Steel, steelList);
            CreatXWall(14, 7, 1, Steel, steelList);

            //基地
            CreatYWall(6, 13, 2, Wall, wallList);
            CreatXWall(7, 13, 1, Wall, wallList);
            CreatYWall(8, 13, 2, Wall, wallList);

            CreatBoss(7, 14, Boss);
            //boss = new ImmovableThing(210, 420, Boss);

            #region 敌方坦克生成点
            points[0].X = 0;
            points[0].Y = 0;

            points[1].X = 7 * 30;
            points[1].Y = 0;

            points[2].X = 14 * 30;
            points[2].Y = 0; 
            #endregion
        }
        #endregion

        #region 创建墙体方法
        public static void CreatYWall(int x, int y, int count, Image img, List<ImmovableThing> wallList)
        {
            int xPosition = x * 30;
            int yPosition = y * 30;

            //按列创建墙
            for (int i = yPosition; i < yPosition + count * 30; i += 15)
            {
                ImmovableThing wall1 = new ImmovableThing(xPosition, i, img);
                ImmovableThing wall2 = new ImmovableThing(xPosition + 15, i, img);

                wallList.Add(wall1);
                wallList.Add(wall2);
            }
        }
        public static void CreatXWall(int x, int y, int count, Image img, List<ImmovableThing> wallList)
        {
            int xPosition = x * 30;
            int yPosition = y * 30;

            //按行创建墙
            for (int i = xPosition; i < xPosition + count * 30; i += 15)
            {
                ImmovableThing wall1 = new ImmovableThing(i, yPosition, img);
                ImmovableThing wall2 = new ImmovableThing(i, yPosition + 15, img);

                wallList.Add(wall1);
                wallList.Add(wall2);
            }
        }
        #endregion

        #region 创建Boss及Tank
        private static void CreatBoss(int x, int y, Image img)
        {
            int xPosition = x * 30;
            int yPosition = y * 30;
            boss = new ImmovableThing(xPosition, yPosition, img);
        }
        public static void CreateMyTank()
        {
            int x = 150;
            int y = 420;
            myTank = new MyTank(x, y, 2);
        }
        #endregion

        #region 键盘事件方法
        public static void KeyDown(KeyEventArgs args)
        {
            myTank.KeyDown(args);
        }
        public static void KeyUp(KeyEventArgs args)
        {
            myTank.KeyUp(args);
        }
        #endregion

        #region 生成敌方坦克
        private static void CreateEnemyTank1(int x, int y)
        {
            List<Bitmap> imgList = new List<Bitmap>();
            imgList.Add(Resources.GrayUp);
            imgList.Add(Resources.GrayDown);
            imgList.Add(Resources.GrayLeft);
            imgList.Add(Resources.GrayRight);

            EnemyTank eTank = new EnemyTank(x, y, 2, imgList);
            enemyTankList.Add(eTank);
        }

        private static void CreateEnemyTank2(int x, int y)
        {
            List<Bitmap> imgList = new List<Bitmap>();
            imgList.Add(Resources.GreenUp);
            imgList.Add(Resources.GreenDown);
            imgList.Add(Resources.GreenLeft);
            imgList.Add(Resources.GreenRight);

            EnemyTank eTank = new EnemyTank(x, y, 2, imgList);
            enemyTankList.Add(eTank);
        }

        private static void CreateEnemyTank3(int x, int y)
        {
            List<Bitmap> imgList = new List<Bitmap>();
            imgList.Add(Resources.QuickUp);
            imgList.Add(Resources.QuickDown);
            imgList.Add(Resources.QuickLeft);
            imgList.Add(Resources.QuickRight);

            EnemyTank eTank = new EnemyTank(x, y, 4, imgList);
            enemyTankList.Add(eTank);
        }

        private static void CreateEnemyTank4(int x, int y)
        {
            List<Bitmap> imgList = new List<Bitmap>();
            imgList.Add(Resources.SlowUp);
            imgList.Add(Resources.SlowDown);
            imgList.Add(Resources.SlowLeft);
            imgList.Add(Resources.SlowRight);

            EnemyTank eTank = new EnemyTank(x, y, 1, imgList);
            enemyTankList.Add(eTank);
        }
        #endregion
    }
}
