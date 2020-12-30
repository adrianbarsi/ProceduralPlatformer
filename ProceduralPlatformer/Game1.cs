using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ProceduralPlatformer
{
    public class Game1 : Game
    {
        private const int STARTING_POSITION_PLATFORM_OFFSET = 10;
        private const int STARTING_POSITION_PLAYER_OFFSET = 11;

        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;

        public SpriteBatch Sprite { get => spriteBatch; }

        private Vector2 stage;
        public Vector2 Stage { get => stage; set => stage = value; }

        private Player player;

        private CollisionDetection cd;
        private Camera camera;

        public Camera Camera { get => camera;  }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void CameraBatchBegin()
        {
            Matrix viewMatrix = camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: viewMatrix);
        }

        public void CameraBatchEnd()
        {
            spriteBatch.End();
        }

        protected override void Initialize()
        {
            camera = new Camera(GraphicsDevice.Viewport);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            stage = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            Texture2D playerTexture = Content.Load<Texture2D>("images/player");
            Vector2 playerPosition = new Vector2(stage.X / 2, stage.Y - (playerTexture.Height / 2) - STARTING_POSITION_PLAYER_OFFSET - 1);
            player = new Player(this, playerTexture, playerPosition);

            cd = new CollisionDetection(this, player);

            Platform platform;

            Texture2D platformTexture = Content.Load<Texture2D>("images/platform");
            platform = new Platform(this, platformTexture, new Vector2(stage.X / 2, stage.Y - (platformTexture.Height / 2) - STARTING_POSITION_PLATFORM_OFFSET));
            cd.addPlatform(platform);
            Components.Add(platform);

            for (int i = 40; i <= 360; i += 60)
            {
                platform = new Platform(this, platformTexture, new Vector2(i, i + 50));

                cd.addPlatform(platform);

                Components.Add(platform);
            }

            Random random = new Random();

            for (int i = 50; i > -1000; i -= 80)
            {
                platform = new Platform(this, platformTexture, new Vector2(random.Next(0, 300), i));

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
