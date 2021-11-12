using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuckDefense
{
    class Tower : GameObject
    {

        protected float attackSpeed;
        private int range = 200;

        Enemy target;
        Enemy oldTarget;

        double timer = 1D;
        public int Range { get => range; set => range = value; }
        internal Enemy Target { get => target; set => target = value; }


    


        public Tower()
        {
            
           
            speed = 0;
            offset = Vector2.Zero;
            color = Color.White;
            origin = Vector2.Zero;
           
            
        }
        public Tower(float attackSpeed, int damage) 
        {
        
        }


        public override void LoadContent(ContentManager content)
        {
            
            sprite = content.Load<Texture2D>("SpritePlaceHolder1");

            this.Position = new Vector2(800, 10);
        }

        public override void Update(GameTime gameTime)
        {
            timer -= gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0)
            {
                    Shoot();
               
                    timer = 1;
           
            }
               

        }

    
        public override void OnCollision(GameObject other)
        {

            if (other is Enemy)
            {
              
            }
        }


        public void Shoot()
        {
            

            if (target != null && target != oldTarget)
            {
                GameWorld.Instantiate(new Projectile(sprite, Position, target.Position));

               
            }
            oldTarget = target;

        }


        
      


    }
}
