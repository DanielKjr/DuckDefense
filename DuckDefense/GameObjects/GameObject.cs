using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
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
        protected float speed;
        protected int moveIndex = 1;
        private float timeElapsed;
        private int currentIndex;
 
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
        /// Follows the path list by calculating the absolute value of moveDir, GameWorldPath[moveIndex] - position + 1, if it is greater than 0.1 it moves to next point
        /// 
        /// should probably be explained differently before we turn it in
        /// </summary>
        /// <param name="gameTime"></param>
        protected void Move(GameTime gameTime)
        {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 moveDir = GameWorld.path[moveIndex] - position;
            moveDir.Normalize();
            position += moveDir * speed * deltaTime;
            
            //hvis den absolutte værdi af moveDir, path - position + 1 er større end 0.1, og hvis moveindex ikke er 5 (altså slutningen af pathen så den ikke crasher) så moveindex ++
            if (Math.Abs(Vector2.Dot(moveDir, Vector2.Normalize(GameWorld.path[moveIndex] - position)) +1) < 0.1f)
            {
                position = GameWorld.path[moveIndex];

                if (moveIndex < 5)
                {
                    moveIndex++;
                }
                
            }


        }



    }


}
