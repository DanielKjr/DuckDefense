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

        private Texture2D placeHolder;
        private Texture2D background;

        private static Vector2 screensize;

        private List<GameObject> gameObjects;
        private static List<GameObject> newObjects;
        private static List<GameObject> deleteObjects;


        //slet meme når vi begynder at loade ting der betyder noget
        private Vector2 meme = new Vector2(45,525);


       private List<Vector2> path = new List<Vector2>();

        public static Vector2 Screensize { get => screensize; set => screensize = value; }

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            screensize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

        }

        protected override void Initialize()
        {
            placeHolder = Content.Load<Texture2D>("SpritePlaceHolder1");
            background = Content.Load<Texture2D>("BackGroundPlaceHolder");

            gameObjects = new List<GameObject>();
            gameObjects.Add(new Enemy());
            
            //path liste, ved ikke om den skal beholdes her
            path.Add(new Vector2(1260, 80));
            path.Add(new Vector2(140,80));
            path.Add(new Vector2(140, 330));
            path.Add(new Vector2(1150, 330));
            path.Add(new Vector2(1150, 525));
            path.Add(new Vector2(45, 525));

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


            /*
            _spriteBatch.Draw(placeHolder, meme, Color.White);
            _spriteBatch.Draw(placeHolder, path[0], Color.White);
            _spriteBatch.Draw(placeHolder, path[1], Color.White);
            _spriteBatch.Draw(placeHolder, path[2], Color.White);
            _spriteBatch.Draw(placeHolder, path[3], Color.White);
            _spriteBatch.Draw(placeHolder, path[4], Color.White);
            _spriteBatch.Draw(placeHolder, path[5], Color.White);
            */
            _spriteBatch.End();

          

            base.Draw(gameTime);
        }
    }
}
