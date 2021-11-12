using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace DuckDefense
{
    public partial class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        private Texture2D background;
        private Texture2D collisionTexture;
        private static Vector2 screensize;

        private List<GameObject> gameObjects;
        private static List<GameObject> newObjects;
        private static List<GameObject> deleteObjects;



        private double timer = 2D;

        public static List<Vector2> path = new List<Vector2>();

        public static Vector2 Screensize { get => screensize; set => screensize = value; }


        //   public static Vector2 Screensize { get => screensize; set => screensize = value; }

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            Screensize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

        }



        protected override void Initialize()
        {
            background = Content.Load<Texture2D>("BackGroundPlaceHolder");
            gameObjects = new List<GameObject>();
            newObjects = new List<GameObject>();
            deleteObjects = new List<GameObject>();






            AddGameObject(new Enemy());
            AddGameObject(new Tower());

            //path liste, ved ikke om den skal beholdes her
            path.Add(new Vector2(1260, 115));
            path.Add(new Vector2(160, 115));
            path.Add(new Vector2(160, 370));
            path.Add(new Vector2(1170, 370));
            path.Add(new Vector2(1170, 560));
            path.Add(new Vector2(45, 560));

            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            collisionTexture = Content.Load<Texture2D>("CollisionTexture ");

            foreach (GameObject go in gameObjects)
            {
                go.LoadContent(this.Content);
            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            UpdateGameObjects(gameTime);
            SpawnEnemies(gameTime);
            TowerTarget();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            _spriteBatch.Begin();

            _spriteBatch.Draw(background, new Vector2(0, 0), Color.White);


            foreach (GameObject go in gameObjects)
            {
                go.Draw(_spriteBatch);
#if DEBUG
                DrawCollisionBox(go);
#endif

            }


            _spriteBatch.End();



            base.Draw(gameTime);
        }

        /// <summary>
        /// Adds the GameObject to the gameObject list, in order to spawn new enemies their textures must be loaded first, otherwise resulting in a ArgumentNullException
        /// </summary>
        /// <param name="gameObject"></param>
        private void AddGameObject(GameObject gameObject)
        {

            if (gameObject is null)
                throw new System.ArgumentNullException($"{nameof(gameObject)} cannot be null.");

            gameObject.LoadContent(this.Content);
            gameObjects.Add(gameObject);


        }

        /// <summary>
        /// Adds new instantiated objects, e.g. new Enemy, Tower or Projectile, 
        /// updates GameObjects, checks for Collision between Enemy and Projectile,
        /// removes objects that collides from gameObjects list
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateGameObjects(GameTime gameTime)
        {
            gameObjects.AddRange(newObjects);
            newObjects.Clear();

            foreach (GameObject go in gameObjects)
            {
                go.Update(gameTime);

                foreach (GameObject other in gameObjects)
                {
                    go.CheckCollision(other);
                }

            }

            foreach (GameObject go in deleteObjects)
            {
                gameObjects.Remove(go);
            }
            deleteObjects.Clear();
        }

        /// <summary>
        /// Spawns enemies
        /// </summary>
        /// <param name="gameTime"></param>
        public void SpawnEnemies(GameTime gameTime)
        {
            timer -= gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0)
            {
                AddGameObject(new Enemy());
                timer = 2;


            }

        }

        /// <summary>
        /// Tower can target an enemy if the distance between tower and enemy is less than the towers range.
        /// </summary>
        public void TowerTarget()
        {
            foreach (Tower tower in gameObjects.OfType<Tower>())
            {
                foreach (Enemy enemy in gameObjects.OfType<Enemy>())
                {

                    if (Vector2.Distance(tower.Position, enemy.Position) < tower.Range)
                    {
                        tower.Target = enemy;
                        break;
                    }
                }
            }
        }

 

        /// <summary>
        /// Instantiates new objects by adding them to newObjects list.
        /// </summary>
        /// <param name="go"></param>
        public static void Instantiate(GameObject go)
        {

            newObjects.Add(go);

        }

        /// <summary>
        /// Despawns Objects by adding it to deleteObjects that is cleared in the UpdateGameObjects function
        /// </summary>
        /// <param name="go"></param>
        public static void Despawn(GameObject go)
        {

            deleteObjects.Add(go);
        }


        /// <summary>
        /// Draws collision box to make it easier to see if the hitbox is where it should be. 
        /// Won't be visible in the release version.
        /// </summary>
        /// <param name="go"></param>
        private void DrawCollisionBox(GameObject go)
        {

            Rectangle topLine = new Rectangle(go.Collision.X, go.Collision.Y, go.Collision.Width, 1);
            Rectangle bottomLine = new Rectangle(go.Collision.X, go.Collision.Y + go.Collision.Height, go.Collision.Width, 1);
            Rectangle rightLine = new Rectangle(go.Collision.X + go.Collision.Width, go.Collision.Y, 1, go.Collision.Height);
            Rectangle leftLine = new Rectangle(go.Collision.X, go.Collision.Y, 1, go.Collision.Height);

            _spriteBatch.Draw(collisionTexture, topLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, bottomLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, rightLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, leftLine, Color.Red);
        }


    }
}
