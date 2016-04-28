using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.States
{
    public abstract class GameState
    {
        protected readonly SpaceShooterGame Game;

        protected ISpaceShooterUI UI { get { return SpaceShooterGame.UI; } }
        protected Settings Settings { get { return Game.Settings; } }

        public GameState(SpaceShooterGame game)
        {
            this.Game = game;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void OnActivated();
    }
}
