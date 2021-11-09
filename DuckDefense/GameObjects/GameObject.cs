using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DuckDefense
{


    public abstract class GameObject
    {

        protected Vector2 position;
        protected Vector2 velocity;
        protected Vector2 offset;
        protected Vector2 origin;

        protected Texture2D[] sprites;
        protected Texture2D sprite;

        protected float fps;
        private float timeElapsed;
        protected float speed;
        private int currentIndex;

        private int p = 1;
        //i dunno

        bool opOgNed;
        //don't ask me
       

        protected Color color;

        public Color GetColor
        {
            get { return color; }
            set { color = value; }

        }


        public abstract void LoadContent(ContentManager content);

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, 0, origin, 1, SpriteEffects.None, 0);

        }

        public abstract void Update(GameTime gameTime);

  
        protected void Animate(GameTime gameTime)
        {

            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentIndex = (int)(timeElapsed * fps);
           
                sprite = sprites[currentIndex];

                if (currentIndex >= sprites.Length - 1)
                {
                    timeElapsed = 0;
                    currentIndex = 0;
                }

            
           

        }

        /// <summary>
        /// Follows the path
        /// </summary>
        /// <param name="gameTime"></param>
        protected void Move(GameTime gameTime)
        {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            Vector2 moveDir = GameWorld.path[p] - position;
            moveDir.Normalize();
            position += moveDir * speed * deltaTime;

            if (opOgNed == false)
            {
                if (position.X > GameWorld.path[p].X - 5 && position.X < GameWorld.path[p].X + 5)
                {
                    p++;
                    opOgNed = true;
                }

            }
            if (opOgNed == true)
            {
                if (position.Y > GameWorld.path[p].Y - 5 && position.Y < GameWorld.path[p].Y + 5)
                {
                    p++;
                    opOgNed = false;
                }
            }




        }



    }


}
