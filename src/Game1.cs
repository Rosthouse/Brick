using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MonoRect = Microsoft.Xna.Framework.Rectangle;

namespace Brick
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont font;
        private List<Rectangle> rectangles;
        private Ball ball;
        private Player player;
        private int score;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            var window = GraphicsDevice.Viewport.Bounds;

            var offSet = new Vector2(0.05f, 0.05f);
            var size = new Vector2(20, 10);
            var margin = new Vector2(5, 5);


            var pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            var brickBounds = window.Scale(0.9f, 0.3f);
            brickBounds.Offset(window.Width * offSet.X, window.Height * offSet.Y);
            this.rectangles = InitializeBricks(brickBounds, size, margin, pixel);

            this.ball = new Ball(window.Center.ToVector2(), Vector2.One * 10, Vector2.One * 5);
            this.ball.init(pixel);


            var playerSize = new Vector2(50, 20);
            var playerPosition = new Vector2(window.Center.X - playerSize.X / 2, window.Bottom - 2 * playerSize.Y);
            this.player = new Player(playerPosition, playerSize, 500);
            this.player.init(pixel);

            this.score = 0;
        }

        internal void Reset()
        {
            Initialize();
        }

        /// <summary>
        /// Generates a grid of <see cref="Rectangle">Rectangles</see>.
        /// </summary>
        /// <param name="bounds">Bricks will be generated inside this rectangle</param>
        /// <param name="rectangleSize">The size for each individual rectangle</param>
        /// <param name="margin">The margin between each brick</param>
        /// <param name="pixel">A <see cref="Texture2D"> defining a single pixel</param>
        /// <returns></returns>
        private List<Rectangle> InitializeBricks(MonoRect bounds, Vector2 rectangleSize, Vector2 margin, Texture2D pixel)
        {
            Debug.Assert(bounds.Contains(new MonoRect(bounds.Location, rectangleSize.ToPoint())));

            var rectangles = new List<Rectangle>();

            Vector2 currentPos = bounds.Location.ToVector2();

            int rows = (int)Math.Round(bounds.Height / (rectangleSize.Y + margin.Y));
            int columns = (int)Math.Round(bounds.Width / (rectangleSize.X + margin.X));

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    var rectangle = new Rectangle(currentPos, rectangleSize);
                    rectangle.init(pixel);
                    rectangles.Add(rectangle);

                    currentPos.X += rectangleSize.X + margin.X;
                }
                currentPos.X = bounds.X;
                currentPos.Y += rectangleSize.Y + margin.Y;
            }

            return rectangles;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            this.ball.Update(this, gameTime);
            this.player.Update(this, gameTime);

            this.rectangles.RemoveAll(brick => HandleCollision(ball, brick));
            this.HandlePlayerCollision(ball, player);
        }

        /// <summary>
        /// Handles the collision between the ball and the player. 
        /// </summary>
        /// <remark>
        /// This logic is a bit different from a collision with a brick, since the ball is always reflected upwards.
        /// </remark>
        /// <param name="ball">The <see cref="Ball"> to reflect.</param>
        /// <param name="player">The <see cref="Player"> to handle collision against.</param>
        private void HandlePlayerCollision(Ball ball, Player player)
        {
            var ballBounds = ball.Bounds;
            var playerBounds = player.Bounds;

            if (playerBounds.Intersects(ballBounds))
            {
                var absoluteSpeed = new Vector2(Math.Abs(ball.Speed.X), Math.Abs(ball.Speed.Y));
                if (ballBounds.Center.X < playerBounds.Center.X)
                {
                    ball.Speed = new Vector2(-1, -1) * absoluteSpeed;

                }
                else
                {
                    ball.Speed = new Vector2(1, -1) * absoluteSpeed;
                }
            }
        }


        /// <summary>
        /// Checks if the ball and a brick are intersecting with each other. If they do, this method will properly reflect the ball.
        /// </summary>
        /// <param name="ball">The ball of the game</param>
        /// <param name="brick">A brick to check collision against</param>
        /// <returns>True, if the ball and the brick are intersecting</returns>
        private bool HandleCollision(Ball ball, Rectangle brick)
        {
            var brickBounds = brick.Bounds;
            var ballBounds = ball.Bounds;

            if (brickBounds.Intersects(ballBounds))
            {
                bool horizontal = false;
                bool vertical = false;
                this.score++;

                if (ballBounds.Y <= brickBounds.Y - brickBounds.Height / 2)
                {
                    //Hit was from below the brick
                    vertical = true;
                }

                if (ballBounds.Y >= brickBounds.Y + brickBounds.Height / 2)
                {
                    //Hit was from above the brick
                    vertical = true;
                }

                if (ballBounds.X < brickBounds.X)
                {
                    //Hit was on left
                    horizontal = true;
                }

                if (ballBounds.X > brickBounds.X)
                {
                    //Hit was on right
                    horizontal = true;
                }

                ball.Reflect(horizontal, vertical);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            this.spriteBatch.Begin();

            var scorePosition = GraphicsDevice.Viewport.Bounds.Center.ToVector2();
            scorePosition.Y += scorePosition.Y / 2;
            this.spriteBatch.DrawString(this.font, score.ToString(), scorePosition, Color.White);

            foreach (Rectangle rectangle in this.rectangles)
            {
                rectangle.Draw(spriteBatch);
            }
            this.ball.Draw(spriteBatch);

            this.player.Draw(spriteBatch);



            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
