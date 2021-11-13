using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuckDefense
{
    class Enemy : GameObject
    {

        // private int speed;
        //to be used





        public Enemy()
        {
            IsAlive = true;
            Health = 3;
            speed = 300;

            //fps = animation speed, seizure warning
            fps = 0;
            this.Position = new Vector2(1260, 115);
            this.origin = Vector2.Zero;
            color = Color.White;
        }

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
            sprites = new Texture2D[2];
            sprites[0] = content.Load<Texture2D>("SpritePlaceHolder1");
            sprites[1] = content.Load<Texture2D>("SpritePlaceHolder2");



            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = content.Load<Texture2D>("SpritePlaceHolder" + (i + 1));
            }

            sprite = sprites[0];
            /*
             * det her sætter hit boksen nede under ham
           this.origin = new Vector2(sprite.Width /2, sprite.Height/2);
           this.offset.X = sprite.Width / 2;
           this.offset.Y = sprite.Height / 2;
            */
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
