using PrograVJ.Engine;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ.Games.SimonSays
{
    public class SimonSays : Game
    {

        bool a, s, k, l;
        bool pa, ps, pk, pl;
        bool anyKeyPressed = false;
        Button ba, bs, bk, bl;
        float offset = 60;

        Square visibleTime;
        float visibleTimeX = 29;
        float roundtime = 0;
        float maxRoundtime = 0;
        int round = 1;
        int highestRound = 0;
        Stopwatch roundTimeSW = new Stopwatch();
        Stopwatch btnPressedDelaySW = new Stopwatch();
        Stopwatch timeBetweenButtonPatternSW = new Stopwatch();
        Stopwatch gameOverDelaySW = new Stopwatch();

        List<int> pattern = new List<int>();
        int currentButtonInPattern = 0;
        int buttonPressed = 4;
        float timeBetweenButtonPattern = 1;
        float btnPressedDelay = 0.25f;
        bool finishedPattern = false;
        bool gameOver = false;
        bool soundPlayed = false;

        Random rnd = new Random();

        public SimonSays(int w, int h, float fps, CameraType type) : base(w, h, fps, type)
        {
            timeBetweenButtonPatternSW.Start();
            roundtime = (round * (int)Math.Log(6 * round) + 10);
            int rndN = rnd.Next(0, 4);
            Console.WriteLine(rndN);
            pattern.Add(rndN);
            Console.WriteLine($"{pattern[0]}");

            pa = ps = pk = pl = false;

            FontManager.Load("ByteBounce.ttf", "bb");

            Instantiate(visibleTime = new Square(
                new Vector3(0, 285, 0),
                new Vector3(0, 0, 0),
                new Vector3(visibleTimeX, 5, 0f),
                Color.Blue, true, Color.Black
            ));

            Instantiate(ba = new Button(
                new Vector3(-offset * 3, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(10, 10, 0),
                Color.Black, true, Color.Yellow,
                0
            ));

            Instantiate(bs = new Button(
                new Vector3(-offset, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(10, 10, 0),
                Color.Black, true, Color.Blue,
                1
            ));

            Instantiate(bk = new Button(
                new Vector3(offset, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(10, 10, 0),
                Color.Black, true, Color.Red,
                2
            ));

            Instantiate(bl = new Button(
                new Vector3(offset * 3, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(10, 10, 0),
                Color.Black, true, Color.Green,
                3
            ));

        }

        protected override void ProcessInput()
        {
            a = InputManager.IsKeyPressed(Keys.A);
            s = InputManager.IsKeyPressed(Keys.S);
            k = InputManager.IsKeyPressed(Keys.K);
            l = InputManager.IsKeyPressed(Keys.L);
        }

        protected override void Render(Graphics g)
        {
            g.Clear(Color.White);

            ba.Draw(g, c);
            bs.Draw(g, c);
            bk.Draw(g, c);
            bl.Draw(g, c);

            if (finishedPattern) visibleTime.Draw(g, c);
            g.DrawString($"Round: {round}", FontManager.Get("bb", 50), new SolidBrush(Color.Black), c.size.X / 2 - 120, 30);
            if (highestRound > 0) g.DrawString($"Highest Round: {round}", FontManager.Get("bb", 50), new SolidBrush(Color.Black), c.size.X / 2 - 220, 60);
            if (gameOver) g.DrawString("Game Over!", FontManager.Get("bb", 50), new SolidBrush(Color.Black), c.size.X / 2 - 220, c.size.Y - 60);
        }

        protected override void Update()
        {
            //if (roundDelaySW.ElapsedMilliseconds / 1000 > 1)

            //new PointF(
            //a.X + (b.X - a.X) * t,
            //a.Y + (b.Y - a.Y) * t)



            if (!gameOver)
            {
                if (roundtime < 1)
                {
                    gameOver = true;
                    gameOverDelaySW.Start();
                }
                else roundtime = (round * (float)Math.Log(6 * round) + 10) - (roundTimeSW.ElapsedMilliseconds / 1000);

                if (finishedPattern)
                {
                    visibleTime.size.X = visibleTimeX * (roundtime / maxRoundtime);
                    if (anyKeyPressed)
                    {
                        if (pattern[currentButtonInPattern] == buttonPressed && buttonPressed != 4)
                        {
                            currentButtonInPattern++;
                            if (!soundPlayed)
                            {
                                Console.WriteLine("Played Sound");
                                soundPlayed = true;
                            }
                            if (pattern.Count() == currentButtonInPattern)
                            {
                                StartNewRound();
                            }
                            buttonPressed = 4;
                            btnPressedDelaySW.Start();
                            anyKeyPressed = false;
                            //Console.WriteLine($"{pattern.Count()}, {currentButtonInPattern}, {pattern[currentButtonInPattern]}");
                        }
                        else if (pattern[currentButtonInPattern] != buttonPressed && buttonPressed != 4)
                        {
                            gameOver = true;
                            gameOverDelaySW.Start();
                        }
                        else
                        {
                            if (btnPressedDelay < btnPressedDelaySW.ElapsedMilliseconds / 1000)
                            {
                                btnPressedDelaySW.Reset();
                                anyKeyPressed = true;
                            }
                        }
                    }
                    else
                    {
                        if (!ps && !pk && !pl && a)
                        {
                            pa = true;
                            buttonPressed = 0;
                            Console.WriteLine(buttonPressed);
                            anyKeyPressed = true;
                        }
                        else pa = false;
                        if (!pa && !pk && !pl && s)
                        {
                            ps = true;
                            buttonPressed = 1;
                            Console.WriteLine(buttonPressed);
                            anyKeyPressed = true;
                        }
                        else ps = false;
                        if (!pa && !ps && !pl && k)
                        {
                            pk = true;
                            buttonPressed = 2;
                            Console.WriteLine(buttonPressed);
                            anyKeyPressed = true;
                        }
                        else pk = false;
                        if (!pa && !ps && !pk && l)
                        {
                            pl = true;
                            buttonPressed = 3;
                            Console.WriteLine(buttonPressed);
                            anyKeyPressed = true;
                        }
                        else pl = false;
                    }
                }
                else
                {
                    if (pattern.Count() > currentButtonInPattern)
                    {
                        switch (pattern[currentButtonInPattern])
                        {
                            case 0: pa = true; soundPlayed = false; break;
                            case 1: ps = true; soundPlayed = false; break;
                            case 2: pk = true; soundPlayed = false; break;
                            case 3: pl = true; soundPlayed = false; break;
                            default: break;
                        }
                    }
                    if (timeBetweenButtonPattern < timeBetweenButtonPatternSW.ElapsedMilliseconds / 1000)
                    {
                        currentButtonInPattern++;
                        if (!soundPlayed)
                        {
                            Console.WriteLine("Played Sound");
                            soundPlayed = true;
                        }
                        if (pattern.Count() == currentButtonInPattern)
                        {
                            StartRound();
                        }
                        timeBetweenButtonPatternSW.Restart();
                        pa = ps = pk = pl = false;
                    }
                }
            }
            else
            {
                if (gameOverDelaySW.ElapsedMilliseconds / 1000 > 5)
                    {
                        StartOver();
                    }
            }


            ba.IsPressed(pa);
            bs.IsPressed(ps);
            bk.IsPressed(pk);
            bl.IsPressed(pl);

        }

        public void StartRound()
        {
            finishedPattern = true;
            anyKeyPressed = false;
            pa = ps = pk = pl = false;
            currentButtonInPattern = 0;

            roundTimeSW.Start();
            maxRoundtime = (round * (int)Math.Log(6 * round) + 10) - (roundTimeSW.ElapsedMilliseconds / 1000);
            roundtime = maxRoundtime;
            visibleTime.size.X = visibleTimeX;
            timeBetweenButtonPatternSW.Reset();
            timeBetweenButtonPatternSW.Stop();
            Console.WriteLine($"{timeBetweenButtonPatternSW.ElapsedMilliseconds / 1000}");
            Console.WriteLine("Pattern finished");
        }

        public void StartNewRound()
        {
            Console.WriteLine("Starting new Round...");
            btnPressedDelaySW.Restart();
            timeBetweenButtonPatternSW.Restart();
            roundTimeSW.Reset();
            maxRoundtime = (round * (int)Math.Log(6 * round) + 10) - (roundTimeSW.ElapsedMilliseconds / 1000);
            roundtime = maxRoundtime;
            visibleTime.size.X = visibleTimeX;
            int rndN = rnd.Next(0, 4);
            pattern.Add(rndN);
            currentButtonInPattern = 0;
            buttonPressed = 4;
            anyKeyPressed = false;
            finishedPattern = false;
            pa = ps = pk = pl = false;
            round++;
            if (round % 10 == 0)
            {
                timeBetweenButtonPattern--;
            }
            //Console.WriteLine($"Pattern Size: {pattern.Count()}, Current Button: {currentButtonInPattern}");
            //for (int i = 0; i < pattern.Count; i++) Console.Write($"{pattern[i]}, ");
        }

        public void StartOver()
        {
            Console.WriteLine("Starting Over...");
            pattern.Clear();
            timeBetweenButtonPatternSW.Restart();
            gameOverDelaySW.Reset();
            gameOverDelaySW.Stop();
            roundTimeSW.Reset();
            maxRoundtime = (round * (int)Math.Log(6 * round) + 10) - (roundTimeSW.ElapsedMilliseconds / 1000);
            roundtime = maxRoundtime;
            visibleTime.size.X = visibleTimeX;
            if (round >= highestRound) highestRound = round;
            round = 1;
            int rndN = rnd.Next(0, 4);
            pattern.Add(rndN);
            currentButtonInPattern = 0;
            buttonPressed = 4;
            anyKeyPressed = false;
            finishedPattern = false;
            pa = ps = pk = pl = false;
            gameOver = false;
            //for (int i = 0; i < pattern.Count; i++) Console.Write($"{pattern[i]}, ");
        }
    }
}
