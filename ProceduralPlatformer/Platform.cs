using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProceduralPlatformer
{
    public class Platform : DrawableGameComponent
    {
        private Game1 parent;
        private Texture2D tex;
        private Vector2 position;
        private Rectangle srcRect;
        private Vector2 origin;

        public Platform(Game game, string imageName, Vector2 position) : base(game)
        {
            parent = (Game1)game;
            tex = parent.Content.Load<Texture2D>(imageName);
            this.position = position;

            srcRect = new Rectangle(0, 0, tex.Width, tex.Height);
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
        }

        public override void Draw(GameTime gameTime)
        {
            parent.CameraBatchBegin();
            parent.Sprite.Draw(tex,
                position,
                srcRect,
                Color.White,
                0,
                origin,
                1,
                SpriteEffects.None,
                0f);
            parent.CameraBatchEnd();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public Rectangle GetBound()
        {
            return new Rectangle((int)position.X - (tex.Width / 2), (int)position.Y - (tex.Height / 2), tex.Width, tex.Height);
        }
    }
}
