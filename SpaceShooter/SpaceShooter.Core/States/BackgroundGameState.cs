using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.States
{
    public abstract class BackgroundGameState : GameState
    {
        protected Session backgroundSession;

        public BackgroundGameState(SpaceShooterGame game)
            : base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            backgroundSession.ActiveLevel.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            backgroundSession.ActiveLevel.Draw(gameTime, spriteBatch);
        }
    }
}
