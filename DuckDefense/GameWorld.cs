using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DuckDefense
{
    public partial class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D placeHolder;
        private Texture2D background;

        //slet meme når vi begynder at loade ting der betyder noget
        private Vector2 meme = new Vector2(130,80);

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
            meme.X++;
            if (meme.X == _graphics.PreferredBackBufferWidth)
            {
                meme.X = -40;
            }

           

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Vector2(0,0), Color.White);
            _spriteBatch.Draw(placeHolder, meme, Color.White);
            _spriteBatch.End();



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
