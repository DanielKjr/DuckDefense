using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
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
     //   private static Vector2 screensize;

        private List<GameObject> gameObjects;
        private static List<GameObject> newObjects;
        private static List<GameObject> deleteObjects;


        MouseState mState;
        Vector2 mousePosition;
        private bool mRightReleased = true;
        private bool mLeftReleased = true;



        private double spawnTimer = 1.2D;
        private double waveTimer = 0.5D;
        private double maxSpawnTimer = 1.2D;
        private double waveCount = -0.1;
        private double gameScore = 0;
        int spawnedEnemies = 0;
        int maxSpawnedEnemies = 20;
        bool waveInProgress = false;

        private int playerBalance = 5;
        private int playerHealth = 10;
        private bool playerIsAlive = true;
        private bool towerBlocked = false;
        int currentTowers = 1;


        public static List<Vector2> path = new List<Vector2>();


     //   public static Vector2 Screensize { get => screensize; set => screensize = value; }




        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

          //  Screensize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

        }



        protected override void Initialize()
        {
            background = Content.Load<Texture2D>("BackGroundPlaceHolder");
            gameObjects = new List<GameObject>();
            newObjects = new List<GameObject>();
            deleteObjects = new List<GameObject>();
            gameObjects.Add(new Tower(new Vector2(800, 10)));

            //path liste, ved ikke om den skal beholdes her
            path.Add(new Vector2(1260, 80));
            path.Add(new Vector2(120, 80));
            path.Add(new Vector2(120, 330));
            path.Add(new Vector2(1140, 330));
            path.Add(new Vector2(1140, 525));
            path.Add(new Vector2(45, 525));

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

            if (playerIsAlive)
            {
                UpdateGameObjects(gameTime);
                SpawnEnemies(gameTime);
                AddTower();
                TowerTarget();
                PlayerDamage();
                

            }
   


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(towerPlaceSprite, new Vector2(mState.Position.X - 22, mState.Position.Y - 20), Color.Red);
            _spriteBatch.Draw(towerPlaceSprite, new Vector2(mState.Position.X - 220, mState.Position.Y - 220), Color.Red);
            _spriteBatch.Draw(towerPlaceSprite, new Vector2(mState.Position.X + 220, mState.Position.Y - 220), Color.Red);
            _spriteBatch.Draw(towerPlaceSprite, new Vector2(mState.Position.X - 220, mState.Position.Y + 220), Color.Red);
            _spriteBatch.Draw(towerPlaceSprite, new Vector2(mState.Position.X + 220, mState.Position.Y + 220), Color.Red);

            InterfaceInfo();

            if (playerIsAlive)
            {

                foreach (GameObject go in gameObjects)
                {
                    go.Draw(_spriteBatch);
#if DEBUG
                    DrawCollisionBox(go);
                    //den er her kun for at se at der ikke er towers vi ikke kan se
                    string currentPlacedTowers = currentTowers.ToString();
                    _spriteBatch.DrawString(waveCountDown, currentPlacedTowers, new Vector2(500, 500), Color.Red);
#endif

                }
             

            }
            if (!playerIsAlive)
            {
                string loseScreen = "YOU LOSE";
                _spriteBatch.DrawString(waveCountDown, loseScreen, new Vector2(_graphics.PreferredBackBufferWidth / 2 -80, _graphics.PreferredBackBufferHeight / 2), Color.Red);

               
                
            }



            _spriteBatch.End();



            base.Draw(gameTime);
        }

        /// <summary>
        /// Shows player balance and health on screen
        /// </summary>
        /// <param name="gameTime"></param>
        public void InterfaceInfo()
        {
            string currency = playerBalance.ToString();
            _spriteBatch.DrawString(waveCountDown, currency, new Vector2(0, 50), Color.Yellow);
            string health = playerHealth.ToString();
            _spriteBatch.DrawString(waveCountDown, health, new Vector2(0, 675), Color.Red);
            string currentgameScore = Math.Floor(gameScore).ToString();
            string currentGameScoreDisplay = $"Score {currentgameScore}";
            _spriteBatch.DrawString(waveCountDown, currentGameScoreDisplay, new Vector2(0, 0), Color.White);
            if (waveInProgress == false)
            {
                string waveTimerSec = Math.Floor(waveTimer).ToString();
                // sårn der ikke er en masse decimaler efter
                string wavePauseMessage = $"You hear a faint quacking noise.. {waveTimerSec}";
                //det beskeden vi bruger når der ikke er en wave igang
                Vector2 sizeOfPauseMessage = waveCountDown.MeasureString(wavePauseMessage);
                // vi bruger en vector2 her til at måle størrelsen på vores string "wavePauseMessage" når den er skrevet med fonten "waveCountDown"
                _spriteBatch.DrawString(waveCountDown, wavePauseMessage, new Vector2(200, 300), Color.Black);
            }

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
                playerBalance += 1;
                double gameScoreIncrease = 10 * (1 + waveCount);
                gameScore += gameScoreIncrease;
            }

            foreach (GameObject go in deleteObjects)
            {
                gameObjects.Remove(go);
            }
            deleteObjects.Clear();
        }

    
        /// <summary>
        /// Spawns enemies when the timer reaches zero
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

                    if (Vector2.Distance(tower.Position, enemy.Position) < tower.Range && IsActive)
                    {
                        tower.Target = enemy;
                        break;
                    }
                    
                    else
                    {
                        tower.Target = null;
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
                AddGameObject(new TowerPlatform(mousePosition));
                towerBlocked = false;
                foreach (TowerPlatform towerplat in gameObjects.OfType<TowerPlatform>())
                {
                    foreach (Tower tower in gameObjects.OfType<Tower>())
                    {
                        int sum = towerplat.Radius + tower.Radius;
                        if (Vector2.Distance(tower.Position, towerplat.Position) < sum)
                        {
                            towerBlocked = true;
                        }
                    }
                    Despawn(towerplat);
                }
                
                if (towerBlocked == false)
                {
                    currentTowers++;
                    mLeftReleased = false;
                    AddGameObject(new Tower(mousePosition));
                    playerBalance -= 5;
                }

                towerBlocked = false;
            }
            
            if (mState.LeftButton == ButtonState.Released)
            {
                mLeftReleased = true;
            }

            if (mState.RightButton == ButtonState.Pressed && mRightReleased == true && playerBalance >= 10)
            {
                AddGameObject(new TowerPlatform(mousePosition));
                towerBlocked = false;
                foreach (TowerPlatform towerplat in gameObjects.OfType<TowerPlatform>())
                {
                    foreach (Tower tower in gameObjects.OfType<Tower>())
                    {
                        int sum = towerplat.Radius + tower.Radius;
                        if (Vector2.Distance(tower.Position, towerplat.Position) < sum)
                        {
                            towerBlocked = true;
                        }
                    }
                    Despawn(towerplat);
                }

                if (towerBlocked == false)
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
        /// Deals damage to the players Health if the Enemy reaches the end of the path
        /// </summary>
        public void PlayerDamage()
        {
            foreach (Enemy enemy in gameObjects.OfType<Enemy>())
            {
                if (Vector2.Distance(enemy.Position, path[5]) < 5)
                {
                    playerHealth--;
                }
                if (playerHealth <= 0)
                {
                    playerIsAlive = false;
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
