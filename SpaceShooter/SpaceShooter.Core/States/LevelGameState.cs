using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SpaceShooter.States
{
    public class LevelGameState : GameState
    {
        protected Session Session { get { return game.Session; } }

        public LevelGameState(SpaceShooterGame game)
            : base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (!game.IsPaused)
            {
                Session.ActiveLevel.Update(gameTime);
                foreach (Player player in Session.Players)
                    player.Controller.Update();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (game.IsPaused)
                Session.ActiveLevel.Draw(new GameTime(), spriteBatch);
            else
                Session.ActiveLevel.Draw(gameTime, spriteBatch);
        }

        public override void OnActivated()
        {
            if (MediaPlayer.Queue.ActiveSong != game.Assets.SomethingMusic)
            {
                MediaPlayer.Play(game.Assets.SomethingMusic);
                MediaPlayer.IsRepeating = true;
            }
        }
    }
}
