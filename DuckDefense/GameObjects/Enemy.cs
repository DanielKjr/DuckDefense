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
        // private int health;
        // private int speed;
        //to be used
       

        public Enemy()
        {
            velocity = new Vector2(10, 0);
            speed = 200;

            //fps = animation speed, seizure warning
            fps = 0;
           
            offset = Vector2.Zero;
            color = Color.White;
        }

      
        public override void LoadContent(ContentManager content)
        {

            if(sprites != null)
                return;
            sprites = new Texture2D[2];
            sprites[0] = content.Load<Texture2D>("SpritePlaceHolder1");
            sprites[1] = content.Load <Texture2D>("SpritePlaceHolder2");

            

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = content.Load<Texture2D>("SpritePlaceHolder"+(i +1));
            }

            sprite = sprites[0];

            this.position = new Vector2(1260,80);
            this.origin = new Vector2(0,0);
            this.offset.X = (-sprite.Width / 2);
            this.offset.Y = -sprite.Height / 2;

        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Animate(gameTime);
        }

     
    }
}
