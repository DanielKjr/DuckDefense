using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DuckDefense
{


        abstract class GameObject
        {



                public abstract void LoadContent(ContentManager content);

                public void Draw(SpriteBatch spriteBatch)
                {
    

                }

                public abstract void Update(GameTime gameTime);
          


           
        
        }


}
