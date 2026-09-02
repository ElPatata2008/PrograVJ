using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ.Engine.Manager
{
    public static class InputManager
    {
        public static List<Keys> pressedKeys = new List<Keys>();

        public static Dictionary<string, List<Keys>> controls = new Dictionary<string, List<Keys>>();

        public static bool MBL;
        public static bool MBR;
        public static PointF MouseLocation;

        public static void Register(string key, List<Keys> controlList)
        {
            if (!controls.ContainsKey(key))
            {
                controls[key] = controlList;
            }
        }

        public static bool GetInput(string key)
        {
            if (controls.ContainsKey(key))
            {
                var keys = controls[key];
                foreach(var k in keys)
                {
                    if (IsKeyPressed(k))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void KeyDown(Keys key)
        {
            if (!pressedKeys.Contains(key)) { pressedKeys.Add(key); }
        }

        public static void KeyUp(Keys keys)
        {
            if (pressedKeys.Contains(keys)) { pressedKeys.Remove(keys); }
        }

        public static bool IsKeyPressed(Keys key) => pressedKeys.Contains(key);

        public static void MouseDown(MouseButtons buttons)
        {
            if (buttons == MouseButtons.Left && !MBL)
            {
                MBL = true;
            }
            if (buttons == MouseButtons.Right && !MBR)
            {
                MBR = true;
            }
        }

        public static void MouseUp(MouseButtons buttons)
        {
            if (buttons == MouseButtons.Left && MBL)
            {
                MBL = false;
            }
            if (buttons == MouseButtons.Right && MBR)
            {
                MBR = false;
            }
        }

        public static void MouseMove(PointF location) => MouseLocation = location;
    }
}
