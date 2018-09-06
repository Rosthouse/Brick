
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Brick
{
    class Player : Rectangle
    {
        private float speed;

        public Player(Vector2 position, Vector2 size, float speed) : base(position, size)
        {
            this.speed = speed;
        }

        public override void Update(Game game, GameTime gameTime)
        {
            var inputState = Keyboard.GetState();

            var transform = Vector2.Zero;
            if (inputState.IsKeyDown(Keys.Left))
            {
                transform.X -= this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (inputState.IsKeyDown(Keys.Right))
            {
                transform.X += this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            this.Position += transform;

            var window = game.GraphicsDevice.Viewport.Bounds;
            var playerBounds = this.Bounds;

            if (playerBounds.Left < window.Left)
            {
                this.Position = new Vector2(window.Left, playerBounds.Top);
            }
            else if (playerBounds.Right > window.Right)
            {
                this.Position = new Vector2(window.Right - playerBounds.Width, playerBounds.Top);
            }
        }
    }
}