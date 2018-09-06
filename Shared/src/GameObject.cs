using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brick
{
    class GameObject
    {
        private Vector2 position;

        public Vector2 Position { get => position; set => position = value; }

        public GameObject(Vector2 position)
        {
            this.position = position;
        }

        public virtual void Update(Game1 game, GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
