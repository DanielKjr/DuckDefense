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

        //slet meme når vi begynder at loade ting der betyder noget
        private Vector2 meme = new Vector2(45,525);


       private List<Vector2> path = new List<Vector2>();


        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            placeHolder = Content.Load<Texture2D>("SpritePlaceHolder");
            background = Content.Load<Texture2D>("BackGroundPlaceHolder");

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

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //kan bare slettes når vi kommer til at skulle tegne noget andet
            /*
            meme.X++;
            if (meme.X == _graphics.PreferredBackBufferWidth)
            {
                meme.X = -40;
            }

           */

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
           //path hjørner for at se om det var rigtige coordinater, er bare reference og kan/skal bare slettes eventuentuelt 
            _spriteBatch.Draw(background, new Vector2(0,0), Color.White);
            _spriteBatch.Draw(placeHolder, meme, Color.White);
            _spriteBatch.Draw(placeHolder, path[0], Color.White);
            _spriteBatch.Draw(placeHolder, path[1], Color.White);
            _spriteBatch.Draw(placeHolder, path[2], Color.White);
            _spriteBatch.Draw(placeHolder, path[3], Color.White);
            _spriteBatch.Draw(placeHolder, path[4], Color.White);
            _spriteBatch.Draw(placeHolder, path[5], Color.White);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
