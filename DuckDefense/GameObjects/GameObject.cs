﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DuckDefense
{


    abstract class GameObject
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

        /// <summary>
        /// Animates by changing the sprites at the speed it is moving by (fps), velocity/speed/fps determines the speed of the animation
        /// </summary>
        /// <param name="gameTime"></param>
        protected void Animate(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentIndex = (int)(timeElapsed * fps);
            sprite = sprites[currentIndex];

            if (currentIndex >= sprites.Length -1)
            {
                timeElapsed = 0;
                currentIndex = 0;
            }

        }

        /// <summary>
        /// Movement depending on the speed we determine 
        /// </summary>
        /// <param name="gameTime"></param>
        protected void Move(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += ((velocity * speed) * deltaTime);
            
        }



    }


}
