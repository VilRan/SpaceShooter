using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            game.Editor.Update();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            game.Editor.Draw(spriteBatch, game);
        }
    }

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
    }
}
