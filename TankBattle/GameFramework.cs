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
    enum GameState
    {
        GameStart,
        GameOver
    }
    internal class GameFramework
    {
        public static Graphics g;
        public static GameState state = GameState.GameStart;
        public static void Start()
        {
            SoundManager.InitSound();
            GameObjectManager.CreatMap();
            GameObjectManager.CreateMyTank();
            SoundManager.PlayStar();
        }

        //update调用次数就是帧率
        public static void Update()
        {
            //绘制游戏元素
            //GameObjectManager.DrawMap();
            //GameObjectManager.DrawMyTank();
            if (state == GameState.GameStart)
            {
                GameObjectManager.Update();
            } else if (state == GameState.GameOver)
            {
                GameOver();
            }
        }

        public static void GameOver()
        {
            g.DrawImage(Resources.GameOver, 0, 0);
        }

        public static void ChangeGameState()
        {
            state = GameState.GameOver;
        }

        public static void KeyDown(KeyEventArgs args)
        {
            GameObjectManager.KeyDown(args);
        }
        public static void KeyUp(KeyEventArgs args)
        {
            GameObjectManager.KeyUp(args);
        }
    }
}
