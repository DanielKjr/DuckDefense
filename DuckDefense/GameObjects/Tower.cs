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
        private Texture2D cookieProjectile;
        Enemy target;

        protected float attackSpeed;
        private int range = 200;
        private double timer = 2;


        public int Range { get => range; set => range = value; }
        internal Enemy Target { get => target; set => target = value; }


        public Tower(Vector2 mousePosition, float attackSpeed)
        {
            //hvis man ikke laver this.attackSpeed så prøver den at tage sprites fra sprites[] listen af en eller anden grund 
            this.attackSpeed = attackSpeed;
            offset = Vector2.Zero;
            color = Color.Cyan;
            origin = Vector2.Zero;
            Position = mousePosition;

        }

        public Tower(Vector2 mousePosition)
        {
            attackSpeed = 1f;
            offset = Vector2.Zero;
            color = Color.White;
            origin = Vector2.Zero;
            Position = mousePosition;


            
        }


        public override void LoadContent(ContentManager content)
        {

            sprite = content.Load<Texture2D>("Tower2");
            cookieProjectile = content.Load<Texture2D>("Cookie");
            
        }

        public override void Update(GameTime gameTime)
        {
            TowerUpdater(gameTime);
           

        }

        /// <summary>
        /// Updates the towers timer depending on their attack speed to determine their fire rate
        /// </summary>
        /// <param name="gameTime"></param>
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

            if (other is Tower)
            {
                
            }
        }

        /// <summary>
        /// Shoots/instantiates a Projectile if there is a target, and the target is alive
        /// </summary>
        public void Shoot()
        {

            if (target != null && target.IsAlive)
            {
                GameWorld.Instantiate(new Projectile(cookieProjectile, Position, target.Position));

            }

        }






    }
}
