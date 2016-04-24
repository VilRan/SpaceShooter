using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SpaceShooter
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
    }

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
    }

    public class LevelGameState : GameState
    {
        protected Session Session { get { return game.Session; } }

        public LevelGameState(SpaceShooterGame game)
            : base(game)
        {
            if (MediaPlayer.Queue.ActiveSong != game.Assets.SomethingMusic)
            {
                MediaPlayer.Play(game.Assets.SomethingMusic);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.IsMuted = false;
            }
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
    }

    public class BackgroundGameState : GameState
    {
        protected Session BackgroundSession { get { return game.BackgroundSession; } }

        public BackgroundGameState(SpaceShooterGame game)
            : base(game)
        {
            if (MediaPlayer.Queue.ActiveSong != game.Assets.MainMusic)
            {
                MediaPlayer.Play(game.Assets.MainMusic);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.IsMuted = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            BackgroundSession.ActiveLevel.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            BackgroundSession.ActiveLevel.Draw(gameTime, spriteBatch);
        }
    }
}
