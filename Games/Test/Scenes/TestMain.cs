using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.Test.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Test.Scenes
{
    public class TestMain : Scene
    {
        //Square box1;
        //Stopwatch stopwatch = new Stopwatch();
        //Square box2;
        //Square point;
        //Polygon circle;
        float speed = 100f;

        Player p;

        public TestMain(Camera c) : base(c)
        {
            //Instantiate(box1 = new Square(
            //    new Vector3(-200, 0, 0),
            //    Vector3.Zero,
            //    new Vector3(100, 100, 100),
            //    Color.White, Color.Black, new BoxCollider2D(box1),
            //    animationController: new AnimationController()
            //));

            //SpriteSheet walkRB = new SpriteSheet(TextureManager.Get("walkRB"), 32, 32, 0.3f, 5);
            //walkRB.AddEvent(2, () => Console.WriteLine("Step"));
            //walkRB.AddEvent(4, () => Console.WriteLine("Step"));
            //box1.animationController.AddAnimation("walk", walkRB);

            //SpriteSheet standRB = new SpriteSheet(TextureManager.Get("standRB"), 32, 32, 0.1f, 19);
            //standRB.AddEvent(18, () => box1.animationController.Play("walk"));
            //box1.animationController.AddAnimation("stand", standRB);

            //SpriteSheet dizzyRB = new SpriteSheet(TextureManager.Get("dizzyRB"), 32, 32, 0.1f, 5);
            //dizzyRB.AddEvent(0, () =>
            //{

            //    if ( >= 2)
            //    {
            //        Console.WriteLine("Standing up!");
            //        box1.animationController.Play("stand");
            //        stopwatch.Stop();
            //        break;
            //    }
            //});
            //dizzyRB.AddEvent(0, () => stopwatch.Start() );
            //box1.animationController.AddAnimation("dizzy", dizzyRB);

            //SpriteSheet rollRB = new SpriteSheet(TextureManager.Get("rollRB"), 32, 32, 0.1f, 5);
            //box1.animationController.AddAnimation("roll", rollRB);

            //box1.animationController.Play("walk");

            //Instantiate(box2 = new Square(
            //    Vector3.Zero,
            //    new Vector3(0, 0, 45),
            //    new Vector3(100, 200, 100),
            //    Color.White, Color.White, new BoxCollider2D(box2)
            //));

            //Instantiate(point = new Square(
            //    new Vector3(0, 200, 0),
            //    Vector3.Zero,
            //    Vector3.One,
            //    Color.White, Color.Black, new PointCollider(point)
            //));

            //Instantiate(circle = new Polygon(
            //    new Vector3(200, 0, 0),
            //    Vector3.Zero,
            //    new Vector3(100, 100, 100),
            //    Color.White, Color.Black,
            //    collider: new CircleCollider2D(circle)
            //));

            Instantiate(p = new Player(
                new Vector3(0, -200, 0),
                Vector3.Zero,
                Vector3.One * 50,
                Color.White, Color.Black, new BoxCollider2D(p),
                controller: new AIController(new Blackboard(), new FSM())
                //animationController: new AnimationController()
            ));

            //SpriteSheet walkP = new SpriteSheet(TextureManager.Get("walkRB"), 32, 32, 0.3f, 5);

        }

        public override void Init() {
            SaveManager.Load("test.json");
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.Black);
            //circle.Draw(g, sceneCamera);
            //box1.Draw(g, sceneCamera);
            //box2.Draw(g, sceneCamera);
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
            if (InputManager.JustPressedInput("enter")) (p.controller.brain as FSM).ChangeState("MoveBorders");
            if (InputManager.JustPressedInput("space")) (p.controller.brain as FSM).ChangeState("MoveCenter");
            //if (InputManager.JustPressedInput("space")) box1.animationController.Play("walk");
            //if (InputManager.JustPressedInput("z")) box1.animationController.Play("roll");
            //if (InputManager.JustPressedInput("x")) box1.animationController.Play("dizzy");
        }

        public override void Update(float dt)
        {
            p.controller.Update(dt);


            //box1.animationController.Update(dt);
            //if (InputManager.GetInput("up")) box1.position.Y += speed * dt;
            //if (InputManager.GetInput("down")) box1.position.Y -= speed * dt;
            //if (InputManager.GetInput("left")) box1.position.X -= speed * dt;
            //if (InputManager.GetInput("right")) box1.position.X += speed * dt;

            //if (InputManager.GetInput("up")) box1.position.Y += speed * dt;
            //if (InputManager.GetInput("down")) box1.position.Y -= speed * dt;
            //if (InputManager.GetInput("left")) box1.position.X -= speed * dt;
            //if (InputManager.GetInput("right")) box1.position.X += speed * dt;

            //if (stopwatch.ElapsedMilliseconds / 1000 >= 2)
            //{
            //    Console.WriteLine("Standing up!");
            //    box1.animationController.Play("stand");
            //    stopwatch.Reset();
            //}

            //if (InputManager.GetInput("np8")) circle.position.Y += speed * dt;
            //if (InputManager.GetInput("np2")) circle.position.Y -= speed * dt;
            //if (InputManager.GetInput("np4")) circle.position.X -= speed * dt;
            //if (InputManager.GetInput("np6")) circle.position.X += speed * dt;

            //box2.rotation.Z += speed / 2 * dt;

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
 