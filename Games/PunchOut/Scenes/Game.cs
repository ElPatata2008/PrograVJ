using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.PunchOut.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.PunchOut.Scenes
{
    public class POGame : Scene
    {
        Player player;
        Enemy enemy;
        EnemyPunch punch;
        Square bg;
        public POGame(Camera c) : base(c)
        {
            Instantiate(player = new Player(
                new Vector3(0, -50, 0),
                Vector3.Zero,
                new Vector3(50, 178, 0),
                Color.White, Color.White, new BoxCollider2D(player),
                animationController: new AnimationController()
            ));

            Instantiate(enemy = new Enemy(
                Vector3.Zero,
                Vector3.Zero,
                new Vector3(66, 226f, 0),
                Color.White, Color.White, new BoxCollider2D(enemy),
                animationController: new AnimationController()
            ));

            Instantiate(punch = new EnemyPunch(
                new Vector3(enemy.position.X - 5, enemy.position.Y - 30, 0),
                Vector3.Zero,
                new Vector3(25, 25, 0),
                Color.Red, Color.Red, new BoxCollider2D(punch)
            ));

            enemy.punch = punch;

            bg = new Square(
                Vector3.Zero,
                Vector3.Zero,
                new Vector3(sceneCamera.size.X, sceneCamera.size.Y, 0),
                Color.White, Color.White,
                fillTexture: TextureManager.Get("bg")
            );
        }

        public override void Init()
        {
            
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.White);
            bg.Draw(g, sceneCamera);
            enemy.Draw(g, sceneCamera);
            player.Draw(g, sceneCamera);
        }

        public override void ProcessInput()
        {
            player.dl = InputManager.JustPressedInput("left");
            player.dr = InputManager.JustPressedInput("right");
            player.block = InputManager.JustPressedInput("down");
        }

        public override void Update(float dt)
        {
            
        }
    }
}
