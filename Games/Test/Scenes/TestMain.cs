using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.Test.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Test.Scenes
{
    public class TestMain : Scene
    {
        Square box1;
        Square box2;
        Square point;
        Polygon circle;
        float speed = 10f;

        Player p;

        public TestMain(Camera c) : base(c)
        {
            Instantiate(box1 = new Square(
                new Vector3(-200, 0, 0),
                Vector3.Zero,
                new Vector3(100, 100, 100),
                Color.White, Color.Black, new BoxCollider2D(box1)
            ));

            Instantiate(box2 = new Square(
                Vector3.Zero,
                new Vector3(0, 0, 45),
                new Vector3(100, 200, 100),
                Color.White, Color.White, new BoxCollider2D(box2)
            ));

            Instantiate(point = new Square(
                new Vector3(0, 200, 0),
                Vector3.Zero,
                Vector3.One,
                Color.White, Color.Black, new PointCollider(point)
            ));

            Instantiate(circle = new Polygon(
                new Vector3(200, 0, 0),
                Vector3.Zero,
                new Vector3(100, 100, 100),
                Color.White, Color.Black,
                collider: new CircleCollider2D(circle)
            ));

            Instantiate(p = new Player(
                new Vector3(0, -200, 0),
                Vector3.Zero,
                new Vector3(100, 100, 0),
                Color.White, Color.Black, new BoxCollider2D(p)
            ));

        }

        public override void Init() {
            SaveManager.Load("test.json");
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.Black);
            circle.Draw(g, sceneCamera);
            //box1.Draw(g, sceneCamera);
            box2.Draw(g, sceneCamera);
            //point.Draw(g, sceneCamera);
            p.Draw(g, sceneCamera);

            DrawingUtils.DrawCenterText(g, $"{p.score}", FontManager.Get("byte", 36), Color.White, new PointF(sceneCamera.size.X / 2, 10));
        }

        public override void ProcessInput() 
        {
            if (InputManager.JustPressedInput("escape"))
            {
                SaveManager.Save("test.json");
                Environment.Exit(0);
            }
        }

        public override void Update(float dt)
        {
            //if (InputManager.GetInput("up")) box1.position.Y += speed * dt;
            //if (InputManager.GetInput("down")) box1.position.Y -= speed * dt;
            //if (InputManager.GetInput("left")) box1.position.X -= speed * dt;
            //if (InputManager.GetInput("right")) box1.position.X += speed * dt;

            if (InputManager.GetInput("w")) p.position.Y += speed * dt;
            if (InputManager.GetInput("s")) p.position.Y -= speed * dt;
            if (InputManager.GetInput("a")) p.position.X -= speed * dt;
            if (InputManager.GetInput("d")) p.position.X += speed * dt;

            //if (InputManager.GetInput("np8")) circle.position.Y += speed * dt;
            //if (InputManager.GetInput("np2")) circle.position.Y -= speed * dt;
            //if (InputManager.GetInput("np4")) circle.position.X -= speed * dt;
            //if (InputManager.GetInput("np6")) circle.position.X += speed * dt;

            box2.rotation.Z += speed / 2 * dt;

            //bool col1 = box2.isColliding(box1);
            //bool col2 = box2.isColliding(point);
            //bool col3 = box2.isColliding(circle);

            //if (col1 && !col2 && !col3) box2.fillColor = Color.Red;
            //else if (col1 && col2 && !col3) box2.fillColor = Color.Purple;
            //else if (col1 && !col2 && col3) box2.fillColor = Color.Yellow;

            //else if (!col1 && col2 && !col3) box2.fillColor = Color.Blue;
            //else if (!col1 && col2 && col3) box2.fillColor = Color.Cyan;

            //else if (!col1 && !col2 && col3) box2.fillColor = Color.Green;

            //else if (col1 && col2 && col3) box2.fillColor = Color.Black;
            //else box2.fillColor = Color.White;
        }
    }
}
 