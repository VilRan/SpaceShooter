using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SpaceShooterGame : Game
    {
        public AssetManager Assets { private set; get; }
        public Session Session { private set; get; }
        public Random Random { private set; get; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState previousKeyboardState;
        bool isPaused = false;

        public SpaceShooterGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Assets";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Random = new Random();
            previousKeyboardState = Keyboard.GetState();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Assets = new AssetManager(Content);
            Session = new Session(this);
            Session.PlayNextLevel();
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
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.P) && previousKeyboardState.IsKeyUp(Keys.P))
                isPaused = !isPaused;
            if (keyboard.IsKeyDown(Keys.N) && previousKeyboardState.IsKeyUp(Keys.N))
                Session.PlayNextLevel();
            previousKeyboardState = keyboard;

            if (!isPaused)
                Session.ActiveLevel.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if (isPaused)
                Session.ActiveLevel.Draw(new GameTime(), spriteBatch);
            else
                Session.ActiveLevel.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
