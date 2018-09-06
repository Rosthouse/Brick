using Microsoft.Xna.Framework;
using MonoRect = Microsoft.Xna.Framework.Rectangle;


namespace Brick
{

    static class RectangleExtensions
    {
        public static MonoRect Scale(this MonoRect rect, float horizontal, float vertical)
        {
            Vector2 newSize = rect.Size.ToVector2() * new Microsoft.Xna.Framework.Vector2(horizontal, vertical);
            return new MonoRect(rect.Location, newSize.ToPoint());
        }
    }
}