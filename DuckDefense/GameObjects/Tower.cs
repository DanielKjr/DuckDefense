using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace DuckDefense
{
    class Tower : GameObject
    {

        protected float attackSpeed;
        private int range = 200;
        Enemy target;

        double timer = 2d;
        public int Range { get => range; set => range = value; }
        internal Enemy Target { get => target; set => target = value; }


        public Tower(Vector2 mousePosition, float attackSpeed)
        {
            //hvis man ikke laver this.attackSpeed så prøver den at tage sprites fra sprites[] listen af en eller anden grund 
            this.attackSpeed = attackSpeed;
            offset = Vector2.Zero;
            color = Color.Pink;
            origin = Vector2.Zero;
            Position = mousePosition;

        }

        public Tower(Vector2 mousePosition)
        {
            this.attackSpeed = 1f;
            offset = Vector2.Zero;
            color = Color.Blue;
            origin = Vector2.Zero;
            Position = mousePosition;
            
        }


        public override void LoadContent(ContentManager content)
        {

            sprite = content.Load<Texture2D>("SpritePlaceHolder1");
            

        }

        public override void Update(GameTime gameTime)
        {
            TowerUpdater(gameTime);
           

        }

        public void TowerUpdater(GameTime gameTime)
        {

            timer -= gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0 )
            {
                Shoot();
                timer = 2- attackSpeed;

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

            if (target != null && target.IsAlive)
            {
                GameWorld.Instantiate(new Projectile(sprite, Position, target.Position));

            }

        }






    }
}
