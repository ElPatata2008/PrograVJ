using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Forms;
using PrograVJ.GameObjects;
using PrograVJ.Engine.Manager;

namespace PrograVJ.Games.Arkanoid
{
    class Arkanoid : Game
    {
        Font scoreFont;
        String gameover = "Game Over!";

        Pad player;
        bool left, right, space;
        int lives = 3;
        int score = 0;

        Ball ball;

        List<Block> blocks = new List<Block>();
        bool allDestroyed = false;

        public Arkanoid(int w, int h, float fps) : base(w, h, fps)
        {

            scoreFont = new Font("Arial", 25, FontStyle.Bold);

            player = new Pad(
                new Vector3(window.ClientSize.Width / 2 - 50f, window.ClientSize.Height - 40f, 1f),
                new Vector3(1, 1, 1),
                new Vector3(100f, 20f, 1f),
                Color.White,
                true,
                12f,
                window.ClientSize.Width
            ); 
            
            ball = new Ball(
                new Vector3(window.ClientSize.Width / 2 - 10f, window.ClientSize.Height / 2 - 10f, 1f),
                new Vector3(1, 1, 1),
                new Vector3(20f, 20f, 20f),
                Color.White,
                true,
                7f,
                window.ClientSize.Width
            );

            for (int x = 0; x < 8; x++)
            {
                for (int y = 3; y > 0; y--)
                    blocks.Add(new Block(
                        new Vector3(80 + x * 80f, 150 - y * 31f, 1),
                        new Vector3(1, 1, 1),
                        new Vector3(75f, 30f, 1f),
                        Color.White,
                        true,
                        y
                    ));
            }

            //blocks.Add(new Block(
            //        new Vector3(window.ClientSize.Width / 2, window.ClientSize.Height / 2 - 75f, 1),
            //        new Vector3(1, 1, 1),
            //        new Vector3(75f, 30f, 1f),
            //        Color.White,
            //        true,
            //        1
            //    ));

            player.SetTarget(ball);


            gameObjects.Add(player);
            gameObjects.Add( ball );
            foreach (Block block in blocks) gameObjects.Add(block);
            foreach (Block block in blocks) Console.WriteLine(block.position);

        }

        protected override void ProcessInput()
        {
            space = InputManager.IsKeyPressed(Keys.Space);
            left = InputManager.IsKeyPressed(Keys.Left) || InputManager.IsKeyPressed(Keys.A);
            right = InputManager.IsKeyPressed(Keys.Right) || InputManager.IsKeyPressed(Keys.D);
        }

        protected override void Update()
        {

            if (!allDestroyed)
            {
                if (lives == 0)
                {
                    ball.Stop();
                    if (space)
                    {
                        ball.Restart();
                        score = 0;
                        lives = 3;
                        foreach (Block block in blocks) block.Restore();
                    }
                }
                else
                {
                    player.Left(left);
                    player.Right(right);

                    if (player.CanBounce())
                    {
                        ball.dirY = -1; player.bounced = true;
                    }
                    if (ball.position.Y + ball.size.Y < player.position.Y - player.size.Y) player.bounced = false;

                    if (ball.position.Y > window.ClientSize.Height + ball.size.Y)
                    {
                        ball.SetInitPosition(); lives--;
                    }

                    foreach (Block block in blocks)
                    {
                        bool inBoundX = ball.position.X + ball.size.X > block.position.X && ball.position.X < block.position.X + block.size.X;
                        bool inBoundY = ball.position.Y + ball.size.Y > block.position.Y && ball.position.Y < block.position.Y + block.size.Y;

                        if (block.isActive)
                        {
                            if (inBoundX)
                            {
                                if (ball.position.Y + ball.size.Y > block.position.Y &&
                                    ball.position.Y + ball.size.Y < block.position.Y + 10f &&
                                    !block.bouncedY)
                                {
                                    ball.dirY = -1;
                                    if (!block.bouncedX && !block.bouncedY)
                                    {
                                        block.hp--;
                                        if (block.hp == 0) score += 10 * block.initHp;
                                    }
                                    block.bouncedY = true;
                                }
                                else if (ball.position.Y + ball.size.Y < block.position.Y - 1f) block.bouncedY = false;

                                if (ball.position.Y < block.position.Y + block.size.Y &&
                                    ball.position.Y > block.position.Y + block.size.Y - 10f &&
                                    !block.bouncedY)
                                {
                                    ball.dirY = 1;
                                    if (!block.bouncedX && !block.bouncedY)
                                    {
                                        block.hp--;
                                        if (block.hp == 0) score += 10 * block.initHp;
                                    }
                                    block.bouncedY = true;
                                }
                                else if (ball.position.Y > block.position.Y + block.size.Y + 1f) block.bouncedY = false;
                            }

                            if (inBoundY)
                            {
                                if (ball.position.X + ball.size.X > block.position.X &&
                                    ball.position.X + ball.size.X < block.position.X + 10f &&
                                    !block.bouncedX)
                                {
                                    ball.dirX = -1;
                                    if (!block.bouncedX && !block.bouncedY)
                                    {
                                        block.hp--;
                                        if (block.hp == 0) score += 10 * block.initHp;
                                    }
                                    block.bouncedX = true;
                                }
                                else if (ball.position.X + ball.size.X < block.position.X + 1f) block.bouncedX = false;

                                if (ball.position.X < block.position.X + block.size.X &&
                                    ball.position.X > block.position.X + block.size.X - 10f &&
                                    !block.bouncedX)
                                {
                                    ball.dirX = 1;
                                    if (!block.bouncedX && !block.bouncedY)
                                    {
                                        block.hp--;
                                        if (block.hp == 0) score += 10 * block.initHp;
                                    }
                                    block.bouncedX = true;
                                }
                                else if (ball.position.X > block.position.X + block.size.X + 1f) block.bouncedX = false;
                            }
                        }
                    };

                    foreach(Block block in blocks)
                    {
                        if (block.isActive) break;
                        allDestroyed = true;
                    }
                }
            } else
            {
                ball.Stop();
                if (space)
                {
                    ball.Restart();
                    foreach (Block block in blocks) block.Restore();
                    allDestroyed = false;
                }
            }
        }

        protected override void Render(Graphics g)
        {
            g.Clear(Color.Black);

            g.DrawString($"{score}", scoreFont, new SolidBrush(Color.White), window.ClientSize.Width / 2 - 10f, window.ClientSize.Height - 100f);
            if (allDestroyed) g.DrawString("You Won!", scoreFont, new SolidBrush(Color.White), window.ClientSize.Width / 2 - 100f, window.ClientSize.Height / 2 + 50f);
            if (lives == 0) g.DrawString($"{gameover}", scoreFont, new SolidBrush(Color.White), window.ClientSize.Width / 2 - 100f, window.ClientSize.Height / 2 + 50f);
            else g.DrawString($"Lives: {lives}", scoreFont, new SolidBrush(Color.White), window.ClientSize.Width / 2 - 50f, window.ClientSize.Height - 155f);

            if (lives == 0 || allDestroyed) g.DrawString("Press [Space] to continue", scoreFont, new SolidBrush(Color.White), window.ClientSize.Width / 2 - 200f, window.ClientSize.Height / 2 + 100f);

                foreach (GameObject obj in  gameObjects) obj.Draw(g, c);
        }
    }
}
