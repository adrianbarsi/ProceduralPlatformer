﻿using Microsoft.Xna.Framework;
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

        private const float SCORE_HORIZONTAL_OFFSET = 10;
        private const float SCORE_VERTICAL_OFFSET = 10;

        private const int PLATFORM_VERTICAL_OFFSET = 80;

        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;

        public SpriteBatch Sprite { get => spriteBatch; }

        private Vector2 stage;
        private Texture2D platformTexture;
        private Texture2D playerTexture;

        public Vector2 Stage { get => stage; set => stage = value; }

        private Player player;

        private CollisionDetection cd;
        private Camera camera;
        private SpriteFont scoreFont;

        private float nextRenderPosition;
        private Platform startingPlatform;

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

        private void GeneratePlatforms(int startPosition, int endPosition)
        {
            Random random = new Random();

            Platform platform;

            for (int i = startPosition; i > endPosition; i -= PLATFORM_VERTICAL_OFFSET)
            {
                platform = new Platform(this, platformTexture, new Vector2(random.Next(platformTexture.Width / 2, (int)stage.X - (platformTexture.Width / 2)), i));

                cd.AddPlatform(platform);

                Components.Add(platform);
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            stage = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            nextRenderPosition = stage.Y / 2;

            scoreFont = Content.Load<SpriteFont>("fonts/score");
            platformTexture = Content.Load<Texture2D>("images/platform");
            playerTexture = Content.Load<Texture2D>("images/player");

            cd = new CollisionDetection(this);

            Vector2 playerPosition = new Vector2(stage.X / 2, stage.Y - (playerTexture.Height / 2) - platformTexture.Height - STARTING_POSITION_PLAYER_OFFSET);
            player = new Player(this, playerTexture, playerPosition);
            cd.AddPlayer(player);
            Components.Add(player);

            startingPlatform = new Platform(this, platformTexture, new Vector2(stage.X / 2, stage.Y - (platformTexture.Height / 2) - STARTING_POSITION_PLATFORM_OFFSET));
            cd.AddPlatform(startingPlatform);
            Components.Add(startingPlatform);

            GeneratePlatforms((int)stage.Y - PLATFORM_VERTICAL_OFFSET, 0);

            Components.Add(cd);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (player.Position.Y - (player.Tex.Height / 2) > stage.Y)
                Exit();

            if(nextRenderPosition >= (stage.Y - player.MaxDistance))
            {
                int startPosition = (int)nextRenderPosition - (int)(stage.Y / 2);
                int endPosition = startPosition - (int)stage.Y;
                GeneratePlatforms(startPosition, endPosition);
                nextRenderPosition -= stage.Y;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            base.Draw(gameTime);
        }
    }
}
