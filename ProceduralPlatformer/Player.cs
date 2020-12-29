using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProceduralPlatformer
{
    public class Player : DrawableGameComponent
    {
        Game1 parent;
        private Vector2 position;
        public Vector2 Position { get => position; set => position = value; }
        private Texture2D tex;
        public Texture2D Tex { get => tex; }
        private Rectangle srcRect;
        private Vector2 origin;

        private float scale = 1.0f;
        private float rotation = 0f;

        private Vector2 speed = new Vector2(4, 4);

        private Vector2 velocity;
        private bool jumping = false;
        private bool falling = false;
        private readonly Vector2 gravity = new Vector2(0, 525);

        public Player(Game game, string imageName, Vector2 position) : base(game)
        {
            parent = (Game1)game;
            this.position = position;

            this.tex = parent.Content.Load<Texture2D>(imageName);
            srcRect = new Rectangle(0, 0, tex.Width, tex.Height);
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
        }

        public override void Draw(GameTime gameTime)
        {
            parent.Sprite.Begin();
            parent.Sprite.Draw(tex,
                position,
                srcRect,
                Color.White,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                0f);
            parent.Sprite.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Right))
            {
                position.X += speed.X;
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                position.X -= speed.X;
            }
            if (ks.IsKeyDown(Keys.Up))
            {
                if(!jumping && !falling)
                {
                    jumping = true;
                    velocity = new Vector2(0, -350);
                }
            }

            if(jumping)
            {
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                velocity.Y += gravity.Y * elapsedTime;
                position.Y += velocity.Y * elapsedTime;
                
                if (position.Y >= parent.Stage.Y - (tex.Height / 2))
                {
                    jumping = false;
                }
            }
            else if(falling)
            {
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                velocity.Y += gravity.Y * elapsedTime;
                position.Y += velocity.Y * elapsedTime;
            }

            position.X = MathHelper.Clamp(position.X, tex.Width / 2, parent.Stage.X - (tex.Width / 2));
            position.Y = MathHelper.Clamp(position.Y, tex.Height / 2, parent.Stage.Y - (tex.Height / 2));

            base.Update(gameTime);
        }

        public void StopFalling()
        {
            falling = false;
        }

        public void StartFalling()
        {
            if(!falling && !jumping)
            {
                falling = true;
                velocity = Vector2.Zero;
            }
        }

        public Rectangle GetBound()
        {
            return new Rectangle((int)position.X - (tex.Width / 2), (int)position.Y - (tex.Height / 2), tex.Width, tex.Height);
        }

        public void StopJumping()
        {
            jumping = false;
        }

        public void StopAscent()
        {
            if (jumping)
            {
                velocity = new Vector2(velocity.X, 0);
            }
        }
    }
}
