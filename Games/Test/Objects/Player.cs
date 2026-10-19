using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using PrograVJ.Engine.Figures;
using System.Drawing.Drawing2D;
using PrograVJ.Engine.AI;

namespace PrograVJ.Games.Test.Objects
{
    
    class MoveCenterState : FSMState
    {
        List<Vector3> targets = new List<Vector3> { 
            new Vector3(100, -100, 0),
            new Vector3(100, 100, 0),
            new Vector3(-100, 100, 0),
            new Vector3(-100, -100, 0),
        };

        Vector3 currentTarget;
        float speed;
        float tempSpeed;

        public MoveCenterState(float speed,
            AIController controller) : base(controller)
        {
            this.speed = speed;
            tempSpeed = speed;
            Random rnd = new Random();
            currentTarget = targets[rnd.Next(0, 5)];
        }

        public override void OnEnter()
        {
            (controller.pawn as Player).fillColor = Color.Red;
        }

        public override void OnExit()
        {
            Console.WriteLine("Moving out of MoveCenterState!");
        }

        public override void Update(float deltaTime)
        {
            //Console.WriteLine("Moving");
            float rad = (float)(controller.pawn.rotation.Z * (Math.PI / 180f));
            Vector3 dir = Vector3.Normalize(currentTarget - controller.pawn.position);
            controller.pawn.position.X += dir.X * speed * deltaTime;
            controller.pawn.position.Y += dir.Y * speed * deltaTime;

            float distance = Vector3.Distance(controller.pawn.position, currentTarget);

            speed = distance > 150f ? tempSpeed * 2 : tempSpeed;

            if (distance < 10f)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i] == currentTarget)
                    {
                        if (i == targets.Count - 1) currentTarget = targets[0];
                        else currentTarget = targets[i + 1];
                        break;
                    }
                }
            }
        }
    }

    class MoveBordersState : FSMState
    {
        List<Vector3> targets = new List<Vector3> {
            new Vector3(300, -200, 0),
            new Vector3(300, 200, 0),
            new Vector3(-300, 200, 0),
            new Vector3(-300, -200, 0),
        };

        Vector3 currentTarget;
        float speed;
        float tempSpeed;

        public MoveBordersState(float speed,
            AIController controller) : base(controller)
        {
            this.speed = speed;
            tempSpeed = speed;
            Random rnd = new Random();
            currentTarget = targets[rnd.Next(0, 5)];
        }

        public override void OnEnter()
        {
            (controller.pawn as Player).fillColor = Color.Blue;
        }

        public override void OnExit()
        {
            Console.WriteLine("Moving out of MoveBordersState!");
        }

        public override void Update(float deltaTime)
        {
            //Console.WriteLine("Moving");
            float rad = (float)(controller.pawn.rotation.Z * (Math.PI / 180f));
            Vector3 dir = Vector3.Normalize(currentTarget - controller.pawn.position);
            controller.pawn.position.X += dir.X * speed * deltaTime;
            controller.pawn.position.Y += dir.Y * speed * deltaTime;

            float distance = Vector3.Distance(controller.pawn.position, currentTarget);

            speed = distance > 150f ? tempSpeed * 2 : tempSpeed;

            if (distance < 10f)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i] == currentTarget)
                    {
                        if (i == targets.Count - 1) currentTarget = targets[0];
                        else currentTarget = targets[i + 1];
                        break;
                    }
                }
            }
        }
    }

    public class Player : Square, ISaveable
    {
        public string SaveID = "Player";
        public int score = 0;
        float speed = 50f;

        public Player(
            Vector3 position,
            Vector3 rotation,
            Vector3 size,
            Color color,
            Color fillColor,
            Collider collider = null,
            Rigidbody body = null,
            AIController controller = null,
            float borderWidth = 2,
            Bitmap fillTexture = null,
            WrapMode mode = WrapMode.Clamp,
            int repeatTileX = 1,
            int repeatTileY = 1,
            AnimationController animationController = null) : base(
                position,
                rotation,
                size,
                color,
                fillColor,
                collider,
                body,
                controller,
                borderWidth,
                fillTexture,
                mode,
                repeatTileX,
                repeatTileY,
                animationController)
        {
            SaveManager.Register(this);
            if (controller != null)
            {
                (controller.brain as FSM).AddState("MoveCenter", new MoveCenterState(speed, controller));
                (controller.brain as FSM).AddState("MoveBorders", new MoveBordersState(speed, controller));
            }
                
        }

        public class PlayerSaveData {
            public Vector3 position { get; set; }
            public Vector3 rotation { get; set; }
            public Vector3 scale { get; set; }
            public int puntaje { get; set; }
        }

        public string ID => SaveID;

        public void LoadState(object state)
        {
            if (state is string jsonString)
            {
                var options = new JsonSerializerOptions { IncludeFields = true };
                var data = JsonSerializer.Deserialize<PlayerSaveData>(jsonString, options);

                if (data == null) return; 

                position = data.position;
                rotation = data.rotation;
                size = data.scale;
                score = data.puntaje;
            }
        }

        public override void OnColiisionStay(GameObject c)
        {
            if (c is Square square) square.fillColor = Color.Red;
            if (c is Polygon circle) circle.fillColor = Color.Blue;
        }

        public override void OnCollisionEnter(GameObject c)
        {
            if (c is Polygon) score++;
        }

        public override void OnCollisionExit(GameObject c)
        {
            if (c is Square square) square.fillColor = Color.White;
            if (c is Polygon circle) circle.fillColor = Color.White;
        }

        public object SaveState()
        {
            return new PlayerSaveData
            {
                position = position,
                rotation = rotation,
                scale = size,
                puntaje = score
            };
        }

        public override void Update(float dt) { controller.Update(dt); }
    }
}
