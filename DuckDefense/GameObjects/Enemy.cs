using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuckDefense
{
    class Enemy : GameObject
    {
        private SoundEffectInstance quack;

        public Enemy()
        {
            IsAlive = true;
            Health = 3;
            speed = 300;

            //fps = animation speed, seizure warning
            fps = 7;
            this.Position = GameWorld.path[0];
            this.origin = Vector2.Zero;
            color = Color.White;
        }

        /// <summary>
        /// Deals damage to the enemy
        /// </summary>
        /// <param name="damage"></param>
        public void Damage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                GameWorld.Despawn(this);

                IsAlive = false;
            }
        }




        public override void LoadContent(ContentManager content)
        {
            

            if (sprites != null)
                return;
            sprites = new Texture2D[3];
            sprites[0] = content.Load<Texture2D>("EnemyDuck1");
            sprites[1] = content.Load<Texture2D>("EnemyDuck2");
            sprites[2] = content.Load<Texture2D>("EnemyDuck3");

          


            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = content.Load<Texture2D>("EnemyDuck" + (i +1));
            }

            sprite = sprites[0];

        }

        public override void Update(GameTime gameTime)
        {

            Move(gameTime);
            Animate(gameTime);

            
        }

        public override void OnCollision(GameObject other)
        {

        }


    }
}
