using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DuckDefense
{
    class Projectile : GameObject
    {
        private Vector2 enemyPosition;
        private int damage = 2;


        public Projectile(Texture2D sprite, Vector2 position, Vector2 enemyPosition)
        {
            this.sprite = sprite;
            Position = position;
            this.enemyPosition = new Vector2(enemyPosition.X + 20 , enemyPosition.Y + 20);

            speed = 1400;
            color = Color.White;
           

        }


        public override void LoadContent(ContentManager content)
        {
           
        }


        public override void OnCollision(GameObject other)
        {
            if (other is Enemy enemy)
            {
               enemy.Damage(damage);
               GameWorld.Despawn(this);   
            }
        }

        public override void Update(GameTime gameTime)
        {
            ProjectileShoot(gameTime, enemyPosition);
            if (Vector2.Distance(Position, enemyPosition) < 10)
            {
                GameWorld.Despawn(this);
            }
        }

        /// <summary>
        /// Targeting function
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="enemyPosition"></param>
        public void ProjectileShoot(GameTime gameTime, Vector2 enemyPosition)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 shootDir = enemyPosition - Position;
            shootDir.Normalize();
            Position += shootDir * speed * deltaTime;


        }



    }


}
