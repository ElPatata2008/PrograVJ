using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine
{
    public enum CameraType
    {
        Orthographic,
        Perspective
    }
    public class Camera
    {
        public CameraType type;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 size;

        public float zoom = 1.0f;
        public float focalLength = 100.0f;
        public float nearZ = 0.1f;

        public Camera(CameraType type, Vector3 position, Vector3 rotation, Vector3 size)
        {
            this.type = type;
            this.position = position;
            this.rotation = rotation;
            this.size = size;
        }

        public Vector3 TransformPoint(Vector3 point)
        {
            Vector3 relativePosition = point - position;
            Vector3 invRotation = -rotation;
            return MathUtils.Rotate(relativePosition, invRotation);
            //return RotateInverse(relativePosition, rotation);
        }

        public PointF ProjectPoint(Vector3 point, Vector2 screenResolution)
        {
            switch(type)
            {
                case CameraType.Orthographic: return ProjectPointOrthographic(point, screenResolution); 
                case CameraType.Perspective: return ProjectPointPrespective(point, screenResolution); 
                default: return new PointF(point.X, point.Y);
            }
        }

        public PointF ProjectPointOrthographic(Vector3 point, Vector2 screenResolution) {
            float pixelPerUnitX = (screenResolution.X / size.X) * zoom;
            float pixelPerUnitY = (screenResolution.Y / size.Y) * zoom;

            float projectedX = (screenResolution.X / 2) + (point.X * pixelPerUnitX);
            float projectedY = (screenResolution.Y / 2) - (point.Y * pixelPerUnitY);

            return new PointF(projectedX, projectedY);
        }

        public PointF ProjectPointPrespective(Vector3 point, Vector2 screenResolution) {

            float fovScale = (focalLength * zoom) / point.Z;

            float projectedX = (screenResolution.X / 2) + (point.X * fovScale);
            float projectedY = (screenResolution.Y / 2) - (point.Y * fovScale);

            return new PointF(projectedX, projectedY);
        }

        public Vector3 GetViewDir(Vector3 worldPoints)
        {
            Vector3 viewDir;
            switch(type)
            {
                case CameraType.Orthographic:
                    viewDir = MathUtils.Rotate(new Vector3(0, 0, 1), rotation);
                    break;
                case CameraType.Perspective:
                    //viewDir = MathUtils.Rotate(new Vector3(0, 0, 2), rotation);
                    viewDir = worldPoints;
                    break;
                default: viewDir = new Vector3(0, 0, 0); break;
            }
            return viewDir;
        }

        public Vector3 GetCameraPosition() => TransformPoint(position);
    }
}
