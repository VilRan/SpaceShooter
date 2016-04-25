using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SpaceShooter.States
{
    public class EditorGameState : GameState
    {
        public EditorGameState(SpaceShooterGame game)
            : base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            game.Editor.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            game.Editor.Draw(spriteBatch);
        }

        public override void OnActivated()
        {
            if (MediaPlayer.Queue.ActiveSong != game.Assets.RelaxMusic)
            {
                MediaPlayer.Play(game.Assets.RelaxMusic);
                MediaPlayer.IsRepeating = true;
            }
        }
    }
}
