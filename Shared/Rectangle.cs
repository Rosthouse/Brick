using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brick
{
    class Rectangle : GameObject
    {
        private Vector2 size;
        private Texture2D pixel;

        public Rectangle(Vector2 position, Vector2 size) : base(position)
        {
            this.Size = size;
        }

        public void init(Texture2D texture)
        {
            this.pixel = texture;
        }

        public Vector2 Size { get => size; set => size = value; }

        public Microsoft.Xna.Framework.Rectangle Bounds => new Microsoft.Xna.Framework.Rectangle(Position.ToPoint(), Size.ToPoint());
        public override void Draw(SpriteBatch spriteBatch)
        {
            var rect = new Microsoft.Xna.Framework.Rectangle(this.Position.ToPoint(), this.Size.ToPoint());
            spriteBatch.Draw(this.pixel, rect, Color.White);
        }
    }
}
