using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Manager
{
    public class AnimationController
    {
        private Dictionary<string, SpriteSheet> animations = new Dictionary<string, SpriteSheet>();

        public SpriteSheet currentAnimation;
        private string currentAnimationName;



        public void Update(float deltaTime) => currentAnimation?.Update(deltaTime);

        public void AddAnimation(string name, SpriteSheet animation)
        {
            animations[name] = animation;
            if (currentAnimation == null)
            {
                Play(name);
            }
        }

        public void Play(string name)
        {
            if (animations.ContainsKey(name) && currentAnimationName != name)
            {
                currentAnimation = animations[name];
                currentAnimationName = name;
                //currentAnimation.Reset();
            }
        }
    }
}
