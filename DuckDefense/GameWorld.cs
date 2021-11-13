using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DuckDefense
{
    public partial class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D towerPlaceSprite;
        private Texture2D background;
        private Texture2D collisionTexture;
        private SpriteFont waveCountDown;
        private static Vector2 screensize;

        private List<GameObject> gameObjects;
        private static List<GameObject> newObjects;
        private static List<GameObject> deleteObjects;

        int currentTowers = 1;
        MouseState mState;
        Vector2 mousePosition;
        private bool mRightReleased = true;
        private bool mLeftReleased = true;
        


        private double spawnTimer = 1.2D;
        private double waveTimer = 0.5D;
        private double maxSpawnTimer = 1.2D;
        int spawnedEnemies = 0;
        int maxSpawnedEnemies = 20;
        bool waveInProgress = false;

        private int playerBalance = 5;
        

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
            gameObjects.Add(new Tower(new Vector2(800, 10)));
            // AddGameObject(new Tower(0.5f));

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
            towerPlaceSprite = Content.Load<Texture2D>("SpritePlaceHolder1");
            waveCountDown = Content.Load<SpriteFont>("waveCountDown");

            foreach (GameObject go in gameObjects)
            {
                go.LoadContent(this.Content);
            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            mState = Mouse.GetState();
            mousePosition = new Vector2(mState.X - 22, mState.Y - 20);

            UpdateGameObjects(gameTime);         
            SpawnEnemies(gameTime);
            AddTower();
            TowerTarget();

      

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(towerPlaceSprite, new Vector2(mState.Position.X - 22, mState.Position.Y - 20), Color.Red);

            string currency = playerBalance.ToString();
            _spriteBatch.DrawString(waveCountDown, currency, new Vector2(0, 0), Color.Yellow);
            

#if DEBUG
            //den er her kun for at se at der ikke er towers vi ikke kan se
            string currentPlacedTowers = currentTowers.ToString();
            _spriteBatch.DrawString(waveCountDown, currentPlacedTowers, new Vector2(500, 500), Color.Red);
#endif
            foreach (GameObject go in gameObjects)
            {
                go.Draw(_spriteBatch);
#if DEBUG
                DrawCollisionBox(go);
#endif

            }
            if (waveInProgress == false)
            {
                string waveTimerSec = Math.Floor(waveTimer).ToString();
                // sårn der ikke er en masse decimaler efter
                string wavePauseMessage = $"You Will Feel The GIRTH In {waveTimerSec} Seconds";
                //det beskeden vi bruger når der ikke er en wave igang
                Vector2 sizeOfPauseMessage = waveCountDown.MeasureString(wavePauseMessage);
                // vi bruger en vector2 her til at måle størrelsen på vores string "wavePauseMessage" når den er skrevet med fonten "waveCountDown"
                _spriteBatch.DrawString(waveCountDown, wavePauseMessage, new Vector2(200, 300), Color.DarkOrange);
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

            foreach (Enemy enemy in deleteObjects.OfType<Enemy>())
            {
                playerBalance += 2;
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

            if (waveInProgress == false)
            {
                waveTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (waveTimer <= 0)
                {
                    waveInProgress = true;
                    waveTimer = 6;
                }
            }
            if (waveInProgress == true)
            {
                spawnTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                if (spawnTimer <= 0)
                {
                    AddGameObject(new Enemy());
                    spawnTimer = maxSpawnTimer;
                    spawnedEnemies++;

                }
                if (spawnedEnemies == maxSpawnedEnemies)
                {
                    waveInProgress = false;

                    if (maxSpawnTimer >= 0.5)
                    {
                        maxSpawnTimer -= 0.12;
                    }
                    if (maxSpawnedEnemies >= 50)
                    {
                        maxSpawnedEnemies += 5;
                    }
                    spawnedEnemies = 0;
                }

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
        /// Adds a tower on Left or Right mouseclick
        /// </summary>
        public void AddTower()
        {//TODO gør så at man ikke kan placere towers oven på hinanden

            if (mState.LeftButton == ButtonState.Pressed && mLeftReleased == true && playerBalance >= 5)
            {                
                currentTowers++;
                mLeftReleased = false;
                AddGameObject(new Tower(mousePosition));
                playerBalance -= 5;

            }
            
            if (mState.LeftButton == ButtonState.Released)
            {
                mLeftReleased = true;
            }

            if (mState.RightButton == ButtonState.Pressed && mRightReleased == true && playerBalance >= 10)
            {              
                currentTowers++;
                mRightReleased = false;
                AddGameObject(new Tower(mousePosition, 1.5f));
                playerBalance -= 10;
            }
            if (mState.RightButton == ButtonState.Released)
            {
                mRightReleased = true;
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
