using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SpaceShooter
{
    public class LevelEditor
    {
        SpaceShooterGame game;
        LevelBlueprint blueprint;
        Camera camera;
        MouseState previousMouse;
        KeyboardState previousKeyboard;

        static int TileSize { get { return AssetManager.TileSize; } }

        public LevelEditor(SpaceShooterGame game, LevelBlueprint blueprint)
        {
            this.game = game;
            this.blueprint = blueprint;
            camera = new Camera(Vector2.Zero, SpaceShooterGame.InternalResolution.Size.ToVector2(), Vector2.Zero);
            previousMouse = Mouse.GetState();
            previousKeyboard = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();
            Controller controller = game.Settings.Controllers["General"];

            Vector2 position = mouse.Position.ToVector2();
            position.X /= Math.Min(game.WidthScale, game.HeightScale);
            position.Y /= Math.Min(game.WidthScale, game.HeightScale);
            position += camera.Position;
            if (mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
            {
                blueprint.Spawns.Add(new SineFighterSpawn(Difficulty.Casual, position));
            }
            if (keyboard.IsKeyDown(Keys.C) && previousKeyboard.IsKeyUp(Keys.O))
            {
                blueprint.Spawns.Clear();
            }
            if (keyboard.IsKeyDown(Keys.F) && previousKeyboard.IsKeyUp(Keys.F))
            {
                SaveBlueprint();
            }
            if (keyboard.IsKeyDown(Keys.O) && previousKeyboard.IsKeyUp(Keys.O))
            {
                LoadBlueprint();
            }
            if (keyboard.IsKeyDown(Keys.T) && previousKeyboard.IsKeyDown(Keys.T))
            {
                game.StartNewSession(Difficulty.Nightmare, 1);
                game.Session.ActiveLevel = new Level(game.Session, blueprint);
                game.State = new LevelGameState(game);
            }

            camera.Velocity = Vector2.Zero;
            if (controller.IsControlDown(Action.MoveLeft))
                camera.Velocity += new Vector2(-512, 0);
            if (controller.IsControlDown(Action.MoveRight))
                camera.Velocity += new Vector2(512, 0);
            if (controller.IsControlDown(Action.MoveUp))
                camera.Velocity += new Vector2(0, -512);
            if (controller.IsControlDown(Action.MoveDown))
                camera.Velocity += new Vector2(0, 512);
            camera.Update(gameTime, blueprint.Bounds);

            previousMouse = mouse;
            previousKeyboard = keyboard;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawGrid(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp);
            foreach (Spawn spawn in blueprint.Spawns)
            {
                Texture2D texture = spawn.GetTexture(game.Assets);
                Vector2 origin = texture.Bounds.Center.ToVector2();
                Vector2 position = spawn.Position - origin - camera.Position;
                spriteBatch.Draw(spawn.GetTexture(game.Assets), position, Color.White);
            }
            spriteBatch.End();
        }

        public void DrawGrid(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicWrap);
            int gridMinX = (int)-camera.Position.X % TileSize;
            int gridMinY = (int)-camera.Position.Y % TileSize;
            int gridMaxX = (int)camera.Size.X + TileSize;
            int gridMaxY = (int)camera.Size.Y + TileSize;
            Rectangle gridDestination = new Rectangle(gridMinX, gridMinY, gridMaxX, gridMaxY);
            spriteBatch.Draw(game.Assets.GridTexture, gridDestination, SpaceShooterGame.InternalResolution, Color.White * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        async void SaveBlueprint()
        {
            FileSavePicker filePicker = new FileSavePicker();
            filePicker.SuggestedStartLocation = PickerLocationId.Desktop;
            filePicker.DefaultFileExtension = ".xml";
            filePicker.FileTypeChoices.Add("Xml", new List<string>(){ ".xml" });
            StorageFile storageFile = await filePicker.PickSaveFileAsync();
            blueprint.SaveToFile(storageFile);
        }

        async void LoadBlueprint()
        {
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.Desktop;
            filePicker.FileTypeFilter.Add(".xml");
            StorageFile storageFile = await filePicker.PickSingleFileAsync();

            XmlDocument xmlDocument = new XmlDocument();
            using (Stream stream = await storageFile.OpenStreamForReadAsync())
                xmlDocument.Load(stream);

            blueprint = new LevelBlueprint(xmlDocument.DocumentElement);
        }
    }
}
