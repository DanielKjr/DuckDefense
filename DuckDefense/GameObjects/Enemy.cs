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
        private int health;
       // private int speed;

        public Enemy()
        {
           

            velocity = new Vector2(1, 0);
            speed = 100;
            fps = 10;
            this.position.X = 1260;
            this.position.Y = 80;
           
            offset = Vector2.Zero;
            color = Color.White;
        }

        /// <summary>
        /// Loads the position and sprites to go through
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            sprites = new Texture2D[2];
            sprites[0] = content.Load<Texture2D>("SpritePlaceHolder1");
            sprites[1] = content.Load <Texture2D>("SpritePlaceHolder2");

            

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = content.Load<Texture2D>("SpritePlaceHolder"+(i +1));
            }

            sprite = sprites[0];

            this.position = new Vector2(1260,80);
            this.origin = new Vector2(60,0);
            this.offset.X = (-sprite.Width / 2);
            this.offset.Y = -sprite.Height / 2;

        }

        public override void Update(GameTime gameTime)
        {
          //  PathMovement();
            Move(gameTime);
            Animate(gameTime);
            

        }

        public void PathMovement()
        {
            if (position.X != 140)
            {
                position.X--;
            }
          
           
        }
    }
}
