using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class LevelEditor
    {
        LevelBlueprint blueprint;
        Camera camera;
        MouseState previousMouse;
        KeyboardState previousKeyboard;

        public LevelEditor(LevelBlueprint blueprint)
        {
            this.blueprint = blueprint;
            camera = new Camera();
            previousMouse = Mouse.GetState();
            previousKeyboard = Keyboard.GetState();
        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
                blueprint.Spawns.Add(new FighterSpawn(Difficulty.Casual, mouse.Position.ToVector2()));
            if (keyboard.IsKeyDown(Keys.S) && previousKeyboard.IsKeyUp(Keys.S))
                blueprint.SaveToFile("EditorLevelLevel");

            previousMouse = mouse;
            previousKeyboard = keyboard;
        }

        public void Draw(SpriteBatch spriteBatch, SpaceShooterGame game)
        {
            spriteBatch.Draw(game.Assets.GridTexture, SpaceShooterGame.InternalResolution, SpaceShooterGame.InternalResolution, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);

            foreach (Spawn spawn in blueprint.Spawns)
            {
                Texture2D texture = spawn.GetTexture(game.Assets);
                Vector2 origin = texture.Bounds.Center.ToVector2();
                Vector2 position = spawn.Position - origin + camera.Position;
                spriteBatch.Draw(spawn.GetTexture(game.Assets), position, Color.White);
            }
        }
    }
}
