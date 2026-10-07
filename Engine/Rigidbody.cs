using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine
{
    public enum RigidbodyType
    {
        Static, 
        Kinematic,
        Dynamic
    }
    public class Rigidbody
    {
        public GameObject parent;
        public RigidbodyType type;

        public Vector3 velocity;
        public float mass = 1.0f;
        public float gravityScale = 1.0f;

        public Vector3 accumulatedForce;
        public Vector3 accumulatedImpulse;

        public Rigidbody(RigidbodyType type, float mass = 1.0f, float gravityScale = 1.0f) {
            this.type = type;
            this.mass = mass;
            this.gravityScale = gravityScale;
        }

        public void Update(float dt)
        {
            switch(type)
            {
                case RigidbodyType.Static:    break;
                case RigidbodyType.Kinematic:
                    parent.position += velocity * dt;
                break;
                case RigidbodyType.Dynamic:
                    AddForce(0, -9.81f * gravityScale, 0);

                    velocity += accumulatedImpulse * mass;
                    accumulatedImpulse = Vector3.Zero;
                    Vector3 acceleration = accumulatedForce / mass;
                    accumulatedForce = Vector3.Zero;

                    velocity += acceleration * dt;
                    parent.position += velocity * dt;
                break;  
            }
        }

        public void AddForce(float x, float y, float z)
        {
            accumulatedImpulse += new Vector3(x, y, z);
        }
    }
}
