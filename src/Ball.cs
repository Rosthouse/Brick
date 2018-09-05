using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brick
{
    class Ball : Rectangle
    {
        private Vector2 speed;

        public Ball(Vector2 position, Vector2 size, Vector2 speed) : base(position, size)
        {
            this.speed = speed;
        }

        public override void Update(Game game, GameTime gameTime)
        {
            base.Update(game, gameTime);

            this.Position += this.speed;

            var posRect = new Microsoft.Xna.Framework.Rectangle(this.Position.ToPoint(), this.Size.ToPoint());
            var window = game.GraphicsDevice.Viewport.Bounds;

            if (posRect.Top < window.Top)
            {
                this.Position = new Vector2(posRect.Left, window.Top);
                this.speed.Y *= -1;
            }
            if (posRect.Bottom > window.Bottom)
            {
                this.Position = new Vector2(posRect.Left, window.Bottom - posRect.Height);
                this.speed.Y *= -1;
            }

            if (posRect.Left < window.Left)
            {
                this.Position = new Vector2(window.Left, posRect.Top);
                this.speed.X *= -1;
            }

            if (posRect.Right > window.Right)
            {
                this.Position = new Vector2(window.Right - posRect.Width, posRect.Top);
                this.speed.X *= -1;
            }


        }

        public void Reflect(bool horizontal, bool vertical)
        {
            this.speed.X = horizontal ? this.speed.X *= -1 : this.speed.X;
            this.speed.Y = vertical ? this.speed.Y *= -1 : this.speed.Y;
        }
    }
}
