using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Xaml;
using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SpaceShooterGame : Game
    {
        public static Rectangle InternalResolution { get { return new Rectangle(0, 0, 1920, 1080); } }

        public AssetManager Assets { private set; get; }
        public Session Session { private set; get; }
        public Random Random { private set; get; }
        public bool IsPaused = false;
        public bool IsDeactived = false;

        GraphicsDeviceManager graphics;
        RenderTarget2D renderTarget;
        SpriteBatch spriteBatch;
        KeyboardState previousKeyboardState;

        public SpaceShooterGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Assets";
            IsMouseVisible = true;
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
            renderTarget = new RenderTarget2D(GraphicsDevice, InternalResolution.Width, InternalResolution.Height);
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
            Assets.CreateTestLevel(this);
            Session = new Session(this, Difficulty.Nightmare);
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
            if (IsDeactived)
                return;

            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                Windows.UI.Xaml.Window.Current.Content = new ShopPage();
                IsDeactived = true;
            }
            if (keyboard.IsKeyDown(Keys.F) && previousKeyboardState.IsKeyUp(Keys.F))
            {
                ApplicationView view = ApplicationView.GetForCurrentView();
                if (view.IsFullScreenMode)
                    view.ExitFullScreenMode();
                else
                    view.TryEnterFullScreenMode();

            }
            if (keyboard.IsKeyDown(Keys.P) && previousKeyboardState.IsKeyUp(Keys.P))
                IsPaused = !IsPaused;
            if (keyboard.IsKeyDown(Keys.N) && previousKeyboardState.IsKeyUp(Keys.N))
                Session.PlayNextLevel();
            previousKeyboardState = keyboard;

            if (!IsPaused)
            {
                Session.ActiveLevel.Update(gameTime);
                foreach (Player player in Session.Players)
                    player.Controller.Update();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (IsDeactived)
                return;

            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.SetRenderTarget(renderTarget);
            spriteBatch.Begin();
            if (IsPaused)
                Session.ActiveLevel.Draw(new GameTime(), spriteBatch);
            else
                Session.ActiveLevel.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.AnisotropicClamp);
            float widthRatio = Window.ClientBounds.Width / (float)InternalResolution.Width;
            float heightRatio = Window.ClientBounds.Height / (float)InternalResolution.Height;
            float scale = Math.Min(widthRatio, heightRatio);
            spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
