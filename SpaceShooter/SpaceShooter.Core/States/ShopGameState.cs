using Microsoft.Xna.Framework.Media;

namespace SpaceShooter.States
{
    public class ShopGameState : BackgroundGameState
    {
        public ShopGameState(SpaceShooterGame game)
            : base(game)
        {
            var blueprint = new LevelBlueprint(int.MaxValue, SpaceShooterGame.InternalResolution.Height);
            backgroundSession = new Session(game, Difficulty.Casual, 0);
            backgroundSession.ActiveLevel = new Level(backgroundSession, blueprint);
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
