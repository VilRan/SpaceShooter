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
        public GameState State;
        public Settings Settings { private set; get; }
        public AssetManager Assets { private set; get; }
        public HighscoreCollection Highscores { private set; get; }
        public Session Session { private set; get; }
        public LevelEditor Editor { private set; get; }
        public Random Random { private set; get; }
        public bool IsPaused = false;

        GraphicsDeviceManager graphics;
        RenderTarget2D renderTarget;
        SpriteBatch spriteBatch;

        public static Rectangle InternalResolution { get { return new Rectangle(0, 0, 1920, 1080); } }
        public float WidthScale { get { return Window.ClientBounds.Width / (float)InternalResolution.Width; } }
        public float HeightScale { get { return Window.ClientBounds.Height / (float)InternalResolution.Height; } }

        public SpaceShooterGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Assets";
            IsMouseVisible = true;
        }

        public void StartNewSession(Difficulty difficulty, int numberOfPlayers)
        {
            State = null;
            Session = new Session(this, difficulty, numberOfPlayers);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Settings = new Settings();
            Highscores = new HighscoreCollection();
            Random = new Random();
            Editor = new LevelEditor(this, new LevelBlueprint(10240, 1080));
            renderTarget = new RenderTarget2D(GraphicsDevice, InternalResolution.Width, InternalResolution.Height);

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
            Controller controller = Settings.Controllers["General"];
            if (controller.IsControlPressed(Action.MainMenu))
            {
                App.Current.GamePage.NavigateTo(new MainMenu());
            }
            if (controller.IsControlPressed(Action.Fullscreen))
            {
                ToggleFullscreen();
            }
            if (controller.IsControlPressed(Action.Editor))
            {
                App.Current.GamePage.NavigateTo();
                State = new EditorGameState(this);
            }
            if (controller.IsControlPressed(Action.Pause))
            {
                IsPaused = !IsPaused;
            }

            if (State != null)
                State.Update(gameTime);

            controller.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (State == null)
                return;

            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.SetRenderTarget(renderTarget);
            State.Draw(gameTime, spriteBatch);

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.AnisotropicClamp);
            float scale = Math.Min(WidthScale, HeightScale);
            spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void ToggleFullscreen()
        {
            App.ToggleFullscreen();
        }
        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            Highscores.SaveToFile();
        }
    }
}
