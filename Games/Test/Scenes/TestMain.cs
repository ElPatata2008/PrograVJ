using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
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
        float speed = 5f;

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
        }

        public override void Init()
        {
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.Black);
            circle.Draw(g, sceneCamera);
            box1.Draw(g, sceneCamera);
            box2.Draw(g, sceneCamera);
            point.Draw(g, sceneCamera);
        }

        public override void ProcessInput()
        {
            if (InputManager.GetInput("up")) box1.position.Y += speed;
            if (InputManager.GetInput("down")) box1.position.Y -= speed;
            if (InputManager.GetInput("left")) box1.position.X -= speed;
            if (InputManager.GetInput("right")) box1.position.X += speed;

            if (InputManager.GetInput("w")) point.position.Y += speed;
            if (InputManager.GetInput("s")) point.position.Y -= speed;
            if (InputManager.GetInput("a")) point.position.X -= speed;
            if (InputManager.GetInput("d")) point.position.X += speed;

            if (InputManager.GetInput("np8")) circle.position.Y += speed;
            if (InputManager.GetInput("np2")) circle.position.Y -= speed;
            if (InputManager.GetInput("np4")) circle.position.X -= speed;
            if (InputManager.GetInput("np6")) circle.position.X += speed;

            //if (InputManager.GetInput("z")) box2.rotation.Z -= speed;
            //if (InputManager.GetInput("x")) box2.rotation.Z += speed;
        }

        public override void Update()
        {
            box2.rotation.Z += speed / 2;

            bool col1 = box2.isColliding(box1);
            bool col2 = box2.isColliding(point);
            bool col3 = box2.isColliding(circle);

            if (col1 && !col2 && !col3) box2.fillColor = Color.Red;
            else if (col1 && col2 && !col3) box2.fillColor = Color.Purple;
            else if (col1 && !col2 && col3) box2.fillColor = Color.Yellow;

            else if (!col1 && col2 && !col3) box2.fillColor = Color.Blue;
            else if (!col1 && col2 && col3) box2.fillColor = Color.Cyan;

            else if (!col1 && !col2 && col3) box2.fillColor = Color.Green;

            else if (col1 && col2 && col3) box2.fillColor = Color.Black;
            else box2.fillColor = Color.White;
        }
    }
}
