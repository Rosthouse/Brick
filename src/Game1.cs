﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using PrimRect = Microsoft.Xna.Framework.Rectangle;

namespace Brick
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private List<Rectangle> rectangles;
        private Ball ball;

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
            // TODO: Add your initialization logic here

            base.Initialize();

            var offSet = new Vector2(20, 20);
            var size = new Vector2(20, 10);
            var margin = new Vector2(5, 5);

            this.rectangles = new List<Rectangle>();


            var pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });


            Vector2 currentPos = offSet;

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    var rectangle = new Rectangle(currentPos, size);
                    rectangle.init(pixel);
                    this.rectangles.Add(rectangle);

                    currentPos.X += size.X + margin.X;
                }
                currentPos.X = offSet.X;
                currentPos.Y += size.Y + margin.Y;
            }

            this.ball = new Ball(Vector2.Zero, Vector2.One * 10, Vector2.One * 5);
            this.ball.init(pixel);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            // TODO: Add your update logic here

            base.Update(gameTime);

            this.ball.Update(this, gameTime);
            var ballBounds = ball.Bounds;

            this.rectangles.RemoveAll(brick =>
            {
                var brickBounds = brick.Bounds;
                if (brickBounds.Intersects(ballBounds))
                {
                    ball.Reflect();
                    rectangles.Remove(brick);
                    return true;
                }
                else
                {
                    return false;
                }
            });
            foreach (Rectangle brick in this.rectangles)
            {

            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            this.spriteBatch.Begin();

            foreach (Rectangle rectangle in this.rectangles)
            {
                rectangle.Draw(spriteBatch);
            }
            this.ball.Draw(spriteBatch);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
