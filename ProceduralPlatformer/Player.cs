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
        private Game1 parent;
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
        private Vector2 originalCameraPosition;
        private readonly Vector2 gravity = new Vector2(0, 525);

        public Player(Game game, Texture2D tex, Vector2 position) : base(game)
        {
            parent = (Game1)game;
            this.tex = tex;
            this.position = position;

            srcRect = new Rectangle(0, 0, tex.Width, tex.Height);
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
            originalCameraPosition = parent.Camera.Position;
        }

        public override void Draw(GameTime gameTime)
        {
            parent.CameraBatchBegin();
            parent.Sprite.Draw(tex,
                position,
                srcRect,
                Color.White,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                0f);
            parent.CameraBatchEnd();
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

            if(jumping || falling)
            {
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                velocity.Y += gravity.Y * elapsedTime;
                position.Y += velocity.Y * elapsedTime;
            }

            float playerCenterDelta = position.Y - (parent.Stage.Y / 2);

            if(playerCenterDelta < 0)
            {
                parent.Camera.Position = new Vector2(parent.Camera.Position.X, position.Y - (parent.Stage.Y / 2));
            }
            else
            {
                parent.Camera.Position = originalCameraPosition;
            }

            if(position.X < 0)
            {
                position.X = parent.Stage.X - (tex.Width / 2);
            }

            if(position.X > parent.Stage.X)
            {
                position.X = tex.Width / 2;
            }

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
