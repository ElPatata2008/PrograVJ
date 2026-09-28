using NAudio.Gui;
using NAudio.Wave;
using PrograVJ.Engine;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.MP.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;

namespace PrograVJ.Games.MP.Scenes
{
    public class MPMainGame : Scene
    {
        List<Bullet> bullets = new List<Bullet>();
        List<Enemy> enemies = new List<Enemy>();
        Stopwatch plateSpawnTime = new Stopwatch();
        Stopwatch cartSpawnTime = new Stopwatch();
        Saucer saucer; bool creatingSaucer = false; Stopwatch saucerAttackTimer = new Stopwatch();
        bool intenseMusic = false;
        Player player;
        bool shootAbove = false;
        bool shootForward = false;
        int lives = 10;
        int score = 0;
        float gravity = 0.5f;

        Random rnd;

        bool starting = true;
        bool gameOver = false;
        float moveLimits = 250f;
        Stopwatch sw = new Stopwatch();
        Stopwatch timeAlive = new Stopwatch();
        float startTime = 3f;
        float gameOverTime = 0f;


        Square Fade;
        bool inTransition = true;
        bool fadeIn = false;
        bool fadeOut = false;
        Parallax bgBack;
        Parallax bgMiddle;
        Parallax bgFront;
        Parallax ground;

        public MPMainGame(Camera c) : base(c)
        {
            bgBack = new Parallax(TextureManager.Get("bgBack"), c.position, c.size, -c.size.X);
            bgMiddle = new Parallax(TextureManager.Get("bgMiddle"), new Vector3(c.position.X, c.position.Y - 25, c.position.Z), c.size, -c.size.X);
            bgFront = new Parallax(TextureManager.Get("bgFront"), new Vector3(c.position.X, c.position.Y - 75, c.position.Z), c.size, -c.size.X);
            ground = new Parallax(TextureManager.Get("ground"), new Vector3(c.position.X, c.position.Y - 180, c.position.Z), new Vector3(c.size.X, 200, c.size.Z), -c.size.X);

            Fade = new Square(
                Vector3.Zero,
                Vector3.Zero,
                new Vector3(sceneCamera.size.X, sceneCamera.size.Y, 200),
                Color.Black, Color.Black
            );
        }

        public override void Init()
        {
            AudioManager.StartLayeredMusic(new Dictionary<string, string>() {
                {"game1", "game1.mp3" },
                {"game2", "game2.mp3" }
            }, "game1", true, 1000 );

            rnd = new Random();

            Instantiate(player = new Player(
                new Vector3(0, -100, 0),
                Vector3.Zero,
                new Vector3(80, 40, 5),
                Color.Black, Color.White, fillTexture: TextureManager.Get("player")
            )
            { accel = 1.2f, maxSpeed = 4f, jumpStrenght = 12f, gravity = gravity, invensibleTexture = TextureManager.Get("playerHit") });

            enemies.Clear();
            foreach (var enemy in enemies) Destroy(enemy);
            saucer = null;
            creatingSaucer = false;
            saucerAttackTimer.Reset();

            Fade.size.X = sceneCamera.size.X;
            inTransition = true;
            gameOver = false;
            starting = true;
            startTime = 3f;
            gameOverTime = 0f;
            lives = 3;
            score = 0;
            fadeOut = false;
            fadeIn = true;
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.Gainsboro);
            bgBack.RenderParallax(g, sceneCamera);
            bgMiddle.RenderParallax(g, sceneCamera);
            bgFront.RenderParallax(g, sceneCamera);
            ground.RenderParallax(g, sceneCamera);
            //lock (bulletsLock) foreach (var bullet in bullets) if (bullets.Contains(bullet)) bullet.Draw(g, sceneCamera);
            foreach (var bullet in bullets) if (sceneGameObjects.Contains(bullet)) bullet.Draw(g, sceneCamera);
            foreach (var enemy in enemies) if (sceneGameObjects.Contains(enemy)) enemy.Draw(g, sceneCamera);
            if (sceneGameObjects.Contains(player)) player.Draw(g, sceneCamera);

            if (startTime > 0) MoonPatrol.DrawText(g, $"{startTime.ToString("0")} s", Color.White, FontManager.Get("byte", 36),new PointF(sceneCamera.size.X, sceneCamera.size.Y / 2)); 
            else
            {
                if (!gameOver) {
                    Rectangle UIBox = new Rectangle(0, (int)sceneCamera.size.Y - 120, (int)sceneCamera.size.X, 120);
                    g.FillRectangle(new SolidBrush(Color.Black), UIBox);
                    g.DrawRectangle(new Pen(Color.White, 4), UIBox);

                    if (lives > 0)
                    {
                        for (int i = 0; i < lives; i++)
                        {
                            Rectangle life = new Rectangle((int)sceneCamera.size.X / 5 + i * 100, (int)sceneCamera.size.Y - 105, 80, 40);
                            TextureBrush b = new TextureBrush(TextureManager.Get("player"))
                            {
                                WrapMode = WrapMode.Clamp
                            };
                            g.FillRectangle(b, life);
                        }

                    }

                    MoonPatrol.DrawText(g, $"{score}", Color.White, FontManager.Get("byte", 36), new PointF(sceneCamera.size.X * 1.5f, sceneCamera.size.Y - 105));
                    MoonPatrol.DrawText(g, "Move right: [LEFT]/[S] | Move left: [RIGHT]/[D] ", Color.White, FontManager.Get("byte", 24), new PointF(sceneCamera.size.X, sceneCamera.size.Y - 56));
                    MoonPatrol.DrawText(g, "Jump: [UP]/[W] | Shoot Up: [Z] | Shoot Front: [X]", Color.White, FontManager.Get("byte", 24), new PointF(sceneCamera.size.X, sceneCamera.size.Y - 28));

                }
                else
                {
                    if (gameOverTime > 2) MoonPatrol.DrawText(g, $"Time Alive: {timeAlive.ElapsedMilliseconds / 1000} s", Color.White, FontManager.Get("byte", 36), new PointF(sceneCamera.size.X, sceneCamera.size.Y / 2));
                    if (gameOverTime > 4) MoonPatrol.DrawText(g, $"Score: {score}", Color.White, FontManager.Get("byte", 36), new PointF(sceneCamera.size.X, sceneCamera.size.Y / 2 + 50));
                    if (gameOverTime > 6) MoonPatrol.DrawText(g, "Press [Enter] to return", Color.White, FontManager.Get("byte", 36), new PointF(sceneCamera.size.X, sceneCamera.size.Y / 2 + 100));
                }
            }

            if (inTransition) Fade.Draw(g, sceneCamera);
        }

        public override void ProcessInput()
        {
            if (!inTransition && !starting && !gameOver)
            {
                if (InputManager.GetInput("escape")) GameOver();
                if (InputManager.GetInput("right")) player.Right();
                if (InputManager.GetInput("left")) player.Left();
                if (InputManager.JustPressedInput("up")) player.Jump();
                //if (InputManager.JustPressedInput("down")) AudioManager.CrossfadeToLayer("game2", 1000);
                shootAbove = InputManager.JustPressedInput("z");
                shootForward = InputManager.JustPressedInput("x");
            }
            if (InputManager.JustPressedInput("enter") && gameOverTime > 6 && !inTransition) { inTransition = true; fadeOut = true; AudioManager.PlaySFX("menuButton"); }
        }

        public override void Update()
        {
            if (!inTransition) GameLogic();
            else Transitions();
        }

        private void GameLogic()
        {
            if (starting)
            {
                startTime = 3 - sw.ElapsedMilliseconds / 1000;
                if (startTime <= 0) StartGame();
            }
            else
            {
                if (!gameOver)
                {
                    if (lives == 0) GameOver();

                    if (enemies.Count >= 5 && !intenseMusic)
                    {
                        AudioManager.CrossfadeToLayer("game2", 1000);
                        intenseMusic = true;
                    }
                    else if (enemies.Count < 5 && intenseMusic)
                    {
                        AudioManager.CrossfadeToLayer("game1", 1000);
                        intenseMusic = false;
                    }

                    bgBack.UpdateParallax(0.2f);
                    bgMiddle.UpdateParallax(0.5f);
                    bgFront.UpdateParallax(0.9f);
                    ground.UpdateParallax(2.5f);

                    SpawnPlate();
                    SpawnCart();

                    PlayerLogic();
                    EnemiesLogic();
                    SaucerLogic();
                    Collisions();
                }
                else
                {
                    gameOverTime = sw.ElapsedMilliseconds / 1000;
                }
            }
        }
        private void StartGame()
        {
            starting = false; 
            timeAlive.Start();
            plateSpawnTime.Start();
            cartSpawnTime.Start();
        }
        private void GameOver()
        {
            gameOver = true; sw.Restart();
            Destroy(player);
            enemies.Clear();
            creatingSaucer = false;
            saucerAttackTimer.Reset();
            Collisions();
            timeAlive.Stop();
            AudioManager.PlaySFX("explosionLoud");
        }

        private void SpawnPlate()
        {
            float frequency = timeAlive.ElapsedMilliseconds / 1000 != 0 ? (100 / (timeAlive.ElapsedMilliseconds / 1000)) + 6 : 15; 
            if (plateSpawnTime.ElapsedMilliseconds / 1000 > rnd.Next(5, (int)frequency))
            {
                plateSpawnTime.Restart();
                int type = rnd.Next(1, 4);
                int amount = rnd.Next(3, 6) + ((int)timeAlive.ElapsedMilliseconds / 1000) / 25;
                for (int i = 0; i < amount; i++)
                {
                    bool canShoot = rnd.Next(0, 2) == 0;
                    PointF pos = new PointF();
                    float rot = 0;
                    switch (type)
                    {
                        case 1: pos = new PointF(sceneCamera.position.X - sceneCamera.size.X / 2 - i * 50, 100); rot = 0; break;
                        case 2: pos = new PointF(sceneCamera.position.X + sceneCamera.size.X / 2 + i * 50, -50); rot = 180;  break;
                        case 3: pos = new PointF(sceneCamera.position.X - sceneCamera.size.X / 3, sceneCamera.position.Y + sceneCamera.size.Y / 2 + i * 50); rot = -90; break;
                    }
                    Plate p = new Plate(
                        new Vector3(pos.X, pos.Y, 0),
                        Vector3.Zero,
                        new Vector3(35, 25, 0),
                        Color.Black, Color.Beige, fillTexture: TextureManager.Get("plate")
                    ) { speed = type == 2 ? 2f : 5f, type = type, shot = canShoot };
                    p.SetRotation(rot);
                    enemies.Add(p); Instantiate(p);
                }
            }
        }
        private void SpawnCart()
        {
            float frequency = timeAlive.ElapsedMilliseconds / 1000 != 0 ? (100 / (timeAlive.ElapsedMilliseconds / 1000)) + 20 : 20;
            if (cartSpawnTime.ElapsedMilliseconds / 1000 > rnd.Next(10, (int)frequency))
            {
                cartSpawnTime.Restart();
                Cart c = new Cart(
                    new Vector3(sceneCamera.position.X - sceneCamera.size.X / 2 - 100, -100, 0),
                    Vector3.Zero,
                    new Vector3(100, 50, 0),
                    Color.Black, Color.Brown, fillTexture: TextureManager.Get("cart")
                )
                { speed = 2f };
                enemies.Add(c); Instantiate(c);
            }
        }

        private void EnemiesLogic()
        {
            List<Enemy> removeEnemies = new List<Enemy>();
            foreach( var enemy in enemies )
            {
                if (enemy is Plate plate)
                {
                    switch (plate.type)
                    {
                        case 1:
                            if (plate.position.X > sceneCamera.position.X) plate.Rotate(-5);
                            if (plate.rotation.Z != 0)
                            {
                                if (plate.position.X + plate.size.X / 2 < sceneCamera.position.X - sceneCamera.size.X / 2)
                                {
                                    removeEnemies.Add(plate);
                                }
                            }
                            break;
                        case 2:
                            if (plate.position.X < sceneCamera.position.X) plate.Rotate(-5);
                            if (plate.rotation.Z != 180)
                            {
                                if (plate.position.X - plate.size.X / 2 > sceneCamera.position.X + sceneCamera.size.X / 2)
                                {
                                    removeEnemies.Add(plate);
                                }
                            }
                            break;
                        case 3:
                            if (plate.rotation.Z >= -90 && plate.rotation.Z < 0 && plate.position.Y < sceneCamera.position.Y + 250) plate.Rotate(5);
                            if (plate.rotation.Z >= 0 && plate.rotation.Z < 90 && plate.position.X > sceneCamera.position.X + sceneCamera.size.X / 3) plate.Rotate(5);

                            if (plate.rotation.Z != -90)
                            {
                                if (plate.position.Y - plate.size.Y / 2 > sceneCamera.position.Y + sceneCamera.size.Y / 2)
                                {
                                    removeEnemies.Add(plate);
                                }
                            }
                            break;
                    }
                    if (!plate.shot)
                    {
                        if (enemy.position.X >= player.position.X - 10 && enemy.position.X <= player.position.X + 10 && plate.type != 2)
                        {
                            Bullet b = plate.Attack(0, -4f, 10, 20);
                            plate.shot = true;
                            if (b != null) { bullets.Add(b); Instantiate(b); }
                        }
                        else if (enemy.position.X - 100 >= player.position.X
                              && enemy.position.Y >= player.position.Y - 10
                              && enemy.position.Y <= player.position.Y + 10
                              && plate.type == 2)
                        {
                            Bullet b = plate.Attack(-4f, 0, 20, 10);
                            plate.shot = true;
                            if (b != null) { bullets.Add(b); Instantiate(b); }
                        }
                    }
                }
                
                if (enemy is Cart cart)
                {
                    if (player.position.X + player.size.X / 2 > cart.position.X - cart.size.X / 2
                        && player.position.X - player.size.X / 2 < cart.position.X + cart.size.X / 2
                        && player.position.Y + player.size.Y / 2 > cart.position.Y - cart.size.Y / 2
                        && player.position.Y - player.size.Y / 2 < cart.position.Y + cart.size.Y / 2
                        && player.CanBeDamaged())
                    {
                        cart.hp--;
                        if (cart.hp == 0 && !removeEnemies.Contains(cart)) removeEnemies.Add(cart);
                        lives--;
                        player.TookDamage();
                        var values = new[] { "hit1", "hit2" };
                        AudioManager.PlaySFX(values[rnd.Next(values.Length)]);
                    }
                    if (cart.CanAttack() && player.position.X < cart.position.X - cart.size.X / 2 - 200)
                    {
                        Bullet b = cart.Attack();
                        bullets.Add(b); Instantiate(b);
                    }
                }
            }
            foreach (var enemy in removeEnemies)
            {
                enemies.Remove(enemy);
                Destroy(enemy);
            }
            removeEnemies.Clear();
        }
        private void SaucerLogic()
        {
            if (timeAlive.ElapsedMilliseconds / 1000 % 20 == 0 & !creatingSaucer)
            {
                creatingSaucer = true;
                var values = new[] { -150, 150 };
                saucer = new Saucer(new Vector3(values[rnd.Next(values.Length)], 250, 0),
                        Vector3.Zero,
                        new Vector3(100, 50, 0),
                        Color.Black, Color.Gray, fillTexture: TextureManager.Get("saucer")
                    )
                { hLimits = moveLimits + 100f, speed = 5f };
                enemies.Add(saucer); Instantiate(saucer);
                saucerAttackTimer.Restart();
            }

            if (saucer != null && creatingSaucer)
            {
                if (saucerAttackTimer.ElapsedMilliseconds / 1000 > 5 && !saucer.shot)
                {
                    saucer.shot = true;
                    Bullet b = saucer.Attack();
                    if (b != null) { bullets.Add(b); Instantiate(b); }
                }
                if (saucerAttackTimer.ElapsedMilliseconds / 1000 > 5 && saucer.shot)
                {
                    saucer.shot = false;
                    saucerAttackTimer.Restart();
                }
            }
        }
        private void PlayerLogic()
        {
            if (player.position.X > moveLimits) player.speed -= player.accel * 1.75f;
            if (player.position.X < -moveLimits) player.speed += player.accel * 1.75f;

            if (bullets.Count(b => !b.enemy) < 5)
            {
                if (shootAbove)
                {
                    Bullet b = player.Attack(5, 15, 0, 10);
                    AudioManager.PlaySFX("shoot");
                    bullets.Add(b); Instantiate(b);
                }
                if (shootForward)
                {
                    Bullet b = player.Attack(15, 5, 10, 0);
                    AudioManager.PlaySFX("shoot");
                    bullets.Add(b); Instantiate(b);
                }
            }
        }
        private void Collisions()
        {
            List<Bullet> removeBullets = new List<Bullet>();
            List<Enemy> removeEnemies = new List<Enemy>();

            foreach (var bullet in bullets)
            {
                // Hit
                if (bullet.enemy)
                {
                    bool inBounds = player.position.X + player.size.X / 2 > bullet.position.X - bullet.size.X / 2
                        && player.position.X - player.size.X / 2 < bullet.position.X + bullet.size.X / 2
                        && player.position.Y + player.size.Y / 2 > bullet.position.Y - bullet.size.Y / 2
                        && player.position.Y - player.size.Y / 2 < bullet.position.Y + bullet.size.Y / 2;

                    if (inBounds && player.CanBeDamaged())
                    {
                        if (!removeBullets.Contains(bullet)) removeBullets.Add(bullet);
                        lives--;
                        player.TookDamage();
                        var values = new[] { "hit1", "hit2" };
                        AudioManager.PlaySFX(values[rnd.Next(values.Length)]);
                    }
                }
                else
                {
                    foreach (Enemy e in enemies)
                    {
                        bool inBounds = e.position.X + e.size.X / 2 > bullet.position.X - bullet.size.X / 2
                            && e.position.X - e.size.X / 2 < bullet.position.X + bullet.size.X / 2
                            && e.position.Y + e.size.Y / 2 > bullet.position.Y - bullet.size.Y / 2
                            && e.position.Y - e.size.Y / 2 < bullet.position.Y + bullet.size.Y / 2;

                        if (inBounds)
                        {
                            if (!removeBullets.Contains(bullet)) removeBullets.Add(bullet);
                            e.hp--;
                            if (e is Saucer saucer)
                            {
                                AudioManager.PlaySFX("saucerHit");
                                if (saucer.hp == 0 && !removeEnemies.Contains(saucer))
                                {
                                    removeEnemies.Add(saucer);
                                    score += 200;
                                    creatingSaucer = false;
                                    AudioManager.PlaySFX("saucerExplosion");
                                }
                            }
                            else if (e is Cart cart)
                            {
                                AudioManager.PlaySFX("cartHit");
                                if (cart.hp == 0 && !removeEnemies.Contains(cart))
                                {
                                    removeEnemies.Add(cart);
                                    score += 300;
                                    AudioManager.PlaySFX("cartExplosion");
                                }
                            }
                            else if (e is Plate plate)
                            {
                                if (plate.hp == 0 && !removeEnemies.Contains(plate))
                                {
                                    removeEnemies.Add(plate);
                                    score += 100;
                                    AudioManager.PlaySFX("plateExplosion");
                                }
                            }
                        }
                    }
                }

                // Clean
                if (bullet.position.X + bullet.size.X / 2 > sceneCamera.size.X / 2
                    || bullet.position.X + bullet.size.X / 2 < -sceneCamera.size.X / 2
                    || bullet.position.Y + bullet.size.Y / 2 > sceneCamera.size.Y / 2
                    || bullet.position.Y + bullet.size.Y / 2 < -sceneCamera.size.Y / 2)
                {
                    if (!removeBullets.Contains(bullet)) removeBullets.Add(bullet);
                }
            }

            // Remove
            if (removeBullets.Count > 0)
            {
                foreach (var bullet in removeBullets)
                {
                    bullets.Remove(bullet);
                    Destroy(bullet);
                }
            }
            removeBullets.Clear();

            if (removeEnemies.Count > 0)
            {
                foreach (var enemy in removeEnemies)
                {
                    enemies.Remove(enemy);
                    Destroy(enemy);
                }
            }
            removeEnemies.Clear();

        }

        private void Transitions()
        {
            if (fadeIn)
            {
                Fade.size.X /= 1.5f;
                if (Fade.size.X < 1) {
                    sw.Start();
                    inTransition = false; fadeIn = false; 
                }
            }
            if (fadeOut)
            {
                Fade.size.X *= 1.5f;
                if (Fade.size.X > sceneCamera.size.X) { 
                    inTransition = false;
                    MoonPatrol.scores.Add(new Tuple<int, string>(score, "User"));
                    sw.Reset();
                    timeAlive.Reset();
                    SceneManager.SetActive("menu"); 
                }
            }
        }
    }
}
