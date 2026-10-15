using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine
{
    public class SpriteSheet
    {
        Image spriteSheet;
        int frameWidth, frameHeight;
        public int totalFrames;
        public int currentFrame = 0;
        private Dictionary<int, Action> frameEvents = new Dictionary<int, Action>();
        private bool eventFiredThisFrame = false;

        float elapsedTime = 0f;
        float targetTime;

        public SpriteSheet(Image spriteSheet, int frameWidth, int frameHeight, float targetTime, int totalFrames)
        {
            this.spriteSheet = spriteSheet;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.targetTime = targetTime;
            this.totalFrames = totalFrames;
        }

        public void Update(float deltaTime)
        {
            elapsedTime += deltaTime;
            if (elapsedTime >= targetTime)
            {
                currentFrame = (currentFrame + 1) % totalFrames;
                elapsedTime = 0f;
                eventFiredThisFrame = false;
            }
            if (!eventFiredThisFrame && frameEvents.ContainsKey(currentFrame))
            {
                frameEvents[currentFrame].Invoke();
                eventFiredThisFrame = true;
            }
        }

        public void AddEvent(int frameIndex, Action action) => frameEvents[frameIndex] = action;


        public Image GetCurrentFrame()
        {
            RectangleF sourceRect = GetCurrentSourceRect();
            Bitmap frame = new Bitmap(frameWidth, frameHeight);
            if (frame == null) Console.WriteLine("hola");
            using (Graphics g = Graphics.FromImage(frame))
            {
                g.DrawImage(spriteSheet, new RectangleF(0, 0, frameWidth, frameHeight), sourceRect, GraphicsUnit.Pixel);
            }
            return frame;
        }

        public RectangleF GetCurrentSourceRect()
        {
            int cols = spriteSheet.Width / frameWidth;
            int x = (currentFrame % cols) * frameWidth;
            int y = (currentFrame / cols) * frameHeight;
            return new RectangleF(x, y, frameWidth, frameHeight);
        }

        public void Reset()
        {
            currentFrame = 0;
            elapsedTime = 0;
        }
    }
}
