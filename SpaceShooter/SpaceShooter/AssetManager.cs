using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class AssetManager
    {
        public Texture2D PlayerShipTexture { private set; get; }
        public Texture2D AsteroidTexture { private set; get; }
        public Texture2D BulletTexture { private set; get; }
        public Texture2D ParticleTexture { private set; get; }

        public LevelBlueprint TestLevelBlueprint { private set; get; }

        public AssetManager(ContentManager content)
        {
            PlayerShipTexture = content.Load<Texture2D>("Textures/Ship.png");
            AsteroidTexture = content.Load<Texture2D>("Textures/Asteroid.png");
            BulletTexture = content.Load<Texture2D>("Textures/Bullet.png");
            ParticleTexture = content.Load<Texture2D>("Textures/Particle.png");
        }

        public void CreateTestLevel(SpaceShooterGame game)
        {
            TestLevelBlueprint = new LevelBlueprint(game);
        }
    }
}
