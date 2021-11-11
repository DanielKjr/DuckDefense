using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DuckDefense
{
    class Projectile : GameObject
    {
        private Vector2 enemyPosition;


        public Projectile(Texture2D sprite, Vector2 position, Vector2 enemyPosition)
        {
            this.sprite = sprite;
            this.Position = position;
            this.enemyPosition = enemyPosition;
            this.speed = 450;
            this.color = Color.White;
           

        }


        public override void LoadContent(ContentManager content)
        {
           
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Enemy)
            {
                GameWorld.Despawn(other );
                
            }
        }

        public override void Update(GameTime gameTime)
        {
            ProjectileShoot(gameTime, enemyPosition);
        }
        public void ProjectileShoot(GameTime gameTime, Vector2 enemyPosition)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 shootDir = enemyPosition - Position;
            shootDir.Normalize();
            Position += shootDir * speed * deltaTime;

          




        }



    }


}
