using Core;

namespace Engine.UI
{
    public class UIAction
    {
        public readonly int ID;
        public Vector2 Position;
        public Vector2 RelativePosition;

        public UIAction(int id, Vector2 initialPosition)
        {
            ID = id;
            Position = initialPosition;
            CalculateRelativePosition();
        }

        public void CalculateRelativePosition()
        {
            RelativePosition = new Vector2((Position.X / Window.Size.Width - 0.5f) * 2 * Window.Ratio, (Position.Y / Window.Size.Height - 0.5f) * -2);
        }
    }
}
