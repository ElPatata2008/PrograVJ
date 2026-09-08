using PrograVJ.Engine.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Forms;
using PrograVJ.Engine.Manager;
using PrograVJ.Engine;

namespace PrograVJ.Games
{
    public class Test : Game
    {
        Square square;
        float speed;
        float rotationSpeed;

        bool up, down, left, right;
        bool a, d, w, s;
        bool q, e;
        bool space;

        public Test(int w, int h, float fps, CameraType type) : base(w, h, fps, type)
        {
            speed = 1f;
            rotationSpeed = 2f;
            TextureManager.Load("image.jpg", "img");
            AudioManager.LoadSFX("fah.mp3", "fah");
            AudioManager.LoadMusic("TempleBattle.mp3", "temple");
            FontManager.Load("upheavtt.ttf", "uphea");

            square = new Square(
                new Vector3(0f, 0f, 0f),
                new Vector3(1f, 1f, 1f),
                new Vector3(10f, 10f, 10f),
                Color.Black,
                true,
                Color.White,
                2f, TextureManager.Get("img")
            );
            Instantiate(square);

            AudioManager.PlayMusic("temple");
        }

        protected override void ProcessInput()
        {
            up = InputManager.IsKeyPressed(Keys.Up);
            down = InputManager.IsKeyPressed(Keys.Down);
            left = InputManager.IsKeyPressed(Keys.Left);
            right = InputManager.IsKeyPressed(Keys.Right);

            a = InputManager.IsKeyPressed(Keys.A);
            d = InputManager.IsKeyPressed(Keys.D);
            w = InputManager.IsKeyPressed(Keys.W);
            s = InputManager.IsKeyPressed(Keys.S);

            q = InputManager.IsKeyPressed(Keys.Q);
            e = InputManager.IsKeyPressed(Keys.E);

            space = InputManager.IsKeyPressed(Keys.Space);
        }

        protected override void Update()
        {
            if (w) square.position.Z += speed;
            if (s) square.position.Z -= speed;
            if (a) square.position.X -= speed;
            if (d) square.position.X += speed;

            if (left) square.rotation.Y -= rotationSpeed;
            if (right) square.rotation.Y += rotationSpeed;
            if (up) square.rotation.X += rotationSpeed;
            if (down) square.rotation.X -= rotationSpeed;

            if (q) square.rotation.Z -= rotationSpeed;
            if (e) square.rotation.Z += rotationSpeed;



            //if (left)
            //{
            //    //square.size.X -= 0.1f;
            //    //square.size.Y -= 0.1f;
            //}
            //if (right)
            //{
            //    //square.size.X += 0.1f;
            //    //square.size.Y += 0.1f;
            //}

            if (space) AudioManager.PlaySFX("fah");

            Console.WriteLine($"Square: {square.position}, Camera: {c.position}, w: {w}");
        }

        protected override void Render(Graphics g)
        {
            g.Clear(Color.White);

            g.DrawString("Hola Mundo!", FontManager.Get("uphea", 20), new SolidBrush(Color.Black), 0, 0);

            //var img = TextureManager.Get("img");
            //g.DrawImage(img, 0, 0, window.ClientSize.Width, window.ClientSize.Height);

            square.Draw(g, c);
        }
    }
}
