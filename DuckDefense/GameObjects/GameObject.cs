using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace DuckDefense
{


    public abstract class GameObject
    {

        private Vector2 position;
        protected Vector2 offset;
        protected Vector2 origin;
        protected Vector2 enemy;
        protected Texture2D[] sprites;
        protected Texture2D sprite;

        protected float fps;
        protected float speed;
        protected int moveIndex = 1;
        private float timeElapsed;
        private int currentIndex;
        protected int currency = 5;
        protected int health;
        private bool isAlive;

        protected Color color;

        public Color GetColor
        {
            get { return color; }
            set { color = value; }

        }

        /// <summary>
        /// Rectangle used to define the sprites edges for collisions
        /// </summary>
        public Rectangle Collision
        {
            get
            {
                return new Rectangle(
                    (int)(position.X ),
                    (int)(position.Y ),
                    sprite.Width,
                    sprite.Height
                    );
            }
        }

        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Enemy { get => enemy; set => enemy = value; }
        public int Health { get => health; set => health = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public int Currency { get => currency; set => currency = value; }

        public abstract void LoadContent(ContentManager content);

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, 0, origin, 1, SpriteEffects.None, 0);

        }

        public abstract void Update(GameTime gameTime);

  
        /// <summary>
        /// Animates the object by alternating between sprites from the Texture2D list in the class
        /// </summary>
        /// <param name="gameTime"></param>
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
        /// Follows the path by checking the distance between enemy and point on path, then changing to the next point.
        /// </summary>
        /// <param name="gameTime"></param>
        protected void Move(GameTime gameTime)
        {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 moveDir = GameWorld.path[moveIndex] - position;
            moveDir.Normalize();
            position += moveDir * speed * deltaTime;

            if (Vector2.Distance(position, GameWorld.path[moveIndex]) < 5f)
            {

                position = GameWorld.path[moveIndex];

                if (moveIndex < 5)
                {
                    moveIndex++;
                }
               
 

            }
            


        }

        /// <summary>
        /// OnCollision determines what happens when 2 objects collide, all the objects can collide but not all of them should
        /// so it is determined in the sub class what happens 
        /// </summary>
        /// <param name="other"></param>
        public abstract void OnCollision(GameObject other);

        /// <summary>
        /// Checks if the Rectangle "Collision" on the different objects intersects and what happens or when which collide is determined in the sub-classes
        /// </summary>
        /// <param name="other"></param>
        public void CheckCollision(GameObject other)
        {
            if (Collision.Intersects(other.Collision))
            {
                OnCollision(other);
                

            }
        }


      
        


    }


}
