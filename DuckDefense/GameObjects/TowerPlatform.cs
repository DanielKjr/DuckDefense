using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DuckDefense
{
    class TowerPlatform : GameObject
    {
        int radius = 50;
        bool used = false;
        public TowerPlatform(Vector2 mouseposition)
        {
            radius = 50;
            Position = mouseposition;
            
        }

        public int Radius { get => radius; set => radius = value; }
        public bool Used { get => used; set => used = value; }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Tower2");
        }

        public override void OnCollision(GameObject other)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }

}
