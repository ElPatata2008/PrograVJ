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
        Circle circle;
        float speed = 10f;

        public TestMain(Camera c) : base(c)
        {
            Instantiate(box1 = new Square(
                Vector3.Zero,
                Vector3.Zero,
                new Vector3(100, 100, 100),
                Color.White, Color.White, new BoxCollider2D(box1)
            ));

            Instantiate(box2 = new Square(
                Vector3.Zero,
                Vector3.Zero,
                new Vector3(100, 100, 100),
                Color.White, Color.White, new BoxCollider2D(box2)
            ));

            Instantiate(point = new Square(
                Vector3.Zero,
                Vector3.Zero,
                Vector3.One,
                Color.White, Color.White, new PointCollider(point)
            ));

            Instantiate(circle = new Circle(
                Vector3.Zero,
                Vector3.Zero,
                new Vector3(100, 100, 100),
                Color.White, Color.White, new CircleCollider(circle)
            ));
        }

        public override void Init()
        {
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.Black);
            box1.Draw(g, sceneCamera);
            box2.Draw(g, sceneCamera);
            point.Draw(g, sceneCamera);
            circle.Draw(g, sceneCamera);
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

            if (InputManager.JustPressedInput("enter")) Console.WriteLine($"Box 1: {box1.collider} | {box1.collider.parent.position} | {box1.position}" +
                $"\nBox 2: {box2.collider} | {box2.collider.parent.position} | {box2.position}");
        }

        public override void Update()
        {
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
