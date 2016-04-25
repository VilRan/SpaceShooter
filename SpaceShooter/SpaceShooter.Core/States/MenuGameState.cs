using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using SpaceShooter.Particles;

namespace SpaceShooter.States
{
    public class MenuGameState : BackgroundGameState
    {
        public MenuGameState(SpaceShooterGame game)
            : base(game)
        {
            var blueprint = new LevelBlueprint(int.MaxValue, SpaceShooterGame.InternalResolution.Height);
            var earth = new BackgroundParticle(game.Assets.EarthTexture, new Vector2(640, 810), 0.9f);
            blueprint.Background.Add(earth);
            backgroundSession = new Session(game, Difficulty.Casual, 0);
            backgroundSession.ActiveLevel = new Level(backgroundSession, blueprint);
        }

        public override void OnActivated()
        {
            if (MediaPlayer.Queue.ActiveSong != game.Assets.MainMusic)
            {
                MediaPlayer.Play(game.Assets.MainMusic);
                MediaPlayer.IsRepeating = true;
            }
        }
    }
}
