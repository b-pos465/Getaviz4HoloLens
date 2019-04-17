using UnityEngine;

namespace Model
{
    public class TransformAndColorInformation
    {
        public Vector3 Position { get; private set; }

        public Vector3 Scale { get; private set; }

        public Color Color { get; private set; }

        public TransformAndColorInformation(Vector3 position, Vector3 scale, Color color)
        {
            this.Position = position;
            this.Scale = scale;
            this.Color = color;
        }
    }
}
