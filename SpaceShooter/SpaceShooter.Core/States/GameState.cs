using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.States
{
    public abstract class GameState
    {
        protected readonly SpaceShooterGame game;

        public GameState(SpaceShooterGame game)
        {
            this.game = game;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void OnActivated();
    }
}
