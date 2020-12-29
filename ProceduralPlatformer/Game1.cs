using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ProceduralPlatformer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        public GraphicsDeviceManager Graphics { get => graphics; }

        SpriteBatch spriteBatch;
        public SpriteBatch Sprite { get => spriteBatch; }

        private Vector2 stage;
        public Vector2 Stage { get => stage; set => stage = value; }

        Player player;

        CollisionDetection cd;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            stage = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            player = new Player(this, "images/player", new Vector2(stage.X / 2, stage.Y));

            cd = new CollisionDetection(this, player);

            Platform platform;

            for (int i = 60; i <= 400; i += 60)
            {
                platform = new Platform(this, "images/platform", new Vector2(i, i + 50));

                cd.addPlatform(platform);

                Components.Add(platform);
            }

            Components.Add(player);

            Components.Add(cd);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            base.Draw(gameTime);
        }
    }
}
