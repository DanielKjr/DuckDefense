using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DuckDefense
{
    public partial class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

     
        private Texture2D background;
        private static Vector2 screensize;

        private List<GameObject> gameObjects;
        private static List<GameObject> newObjects;
        private static List<GameObject> deleteObjects;
        //to be used

       
        private double timer = 2D;

       public static List<Vector2> path = new List<Vector2>();


        public static Vector2 Screensize { get => screensize; set => screensize = value; }

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            screensize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

        }

        private void AddGameObject(GameObject gameObject){

            if(gameObject is null)
            throw new System.ArgumentNullException($"{nameof(gameObject)} cannot be null, fucker.");

            gameObject.LoadContent(this.Content);
            gameObjects.Add(gameObject);

        }

        protected override void Initialize()
        {
            background = Content.Load<Texture2D>("BackGroundPlaceHolder");
            gameObjects = new List<GameObject>();
            
            AddGameObject(new Enemy());


            //path liste, ved ikke om den skal beholdes her
            path.Add(new Vector2(1260, 115));
            path.Add(new Vector2(160,115));
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

            foreach (GameObject go in gameObjects)
            {
                go.LoadContent(this.Content);
            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            foreach (GameObject go in gameObjects)
            {
                go.Update(gameTime);
            }


            timer -= gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0)
            {
                AddGameObject(new Enemy());
                timer = 2;
                
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            
            _spriteBatch.Begin();
          
            _spriteBatch.Draw(background, new Vector2(0,0), Color.White);
           

            foreach (GameObject go in gameObjects)
            {
                go.Draw(_spriteBatch);
            }
           

            _spriteBatch.End();

          

            base.Draw(gameTime);
        }
    }
}
