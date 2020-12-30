using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProceduralPlatformer
{
    public class CollisionDetection : GameComponent
    {
        const int RECTANGLE_SIDE_OFFSET = 1;

        private Game1 parent;
        private Player player;
        private List<Platform> platforms;

        public CollisionDetection(Game game, Player player) : base(game)
        {
            parent = (Game1)game;
            this.player = player;
            platforms = new List<Platform>();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void addPlatform(Platform platform)
        {
            platforms.Add(platform);
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle playerRect = player.GetBound();

            Rectangle playerLeftRect = new Rectangle(playerRect.X - RECTANGLE_SIDE_OFFSET, playerRect.Y, RECTANGLE_SIDE_OFFSET, playerRect.Height);
            Rectangle playerRightRect = new Rectangle(playerRect.X + playerRect.Width, playerRect.Y, RECTANGLE_SIDE_OFFSET, playerRect.Height);
            Rectangle playerTopRect = new Rectangle(playerRect.X, playerRect.Y - RECTANGLE_SIDE_OFFSET, playerRect.Width, RECTANGLE_SIDE_OFFSET);
            Rectangle playerBottomRect = new Rectangle(playerRect.X, playerRect.Y + playerRect.Height, playerRect.Width, RECTANGLE_SIDE_OFFSET);

            bool intersected = false;

            foreach (var platform in platforms)
            {
                Rectangle platformRect = platform.GetBound();
                if (platformRect.Intersects(playerRect))
                {
                    // Landing on the platform
                    if(platformRect.Intersects(playerBottomRect))
                    {
                        player.StopJumping();
                        player.StopFalling();
                        // Make sure player doesn't sink into platform
                        player.Position = new Vector2(player.Position.X, platformRect.Top - (playerRect.Height / 2));
                    }
                    // Hitting the bottom of the platform
                    else if (platformRect.Intersects(playerTopRect))
                    {
                        player.StopAscent();
                        player.Position = new Vector2(player.Position.X, platformRect.Bottom + (playerRect.Height / 2));
                    }
                    // Coming from the left
                    else if(platformRect.Intersects(playerRightRect))
                    {
                        player.StopJumping();
                        player.StartFalling();
                        player.Position = new Vector2(platformRect.Left - (playerRect.Width / 2), player.Position.Y);
                    }
                    // Coming from the right
                    else if(platformRect.Intersects(playerLeftRect))
                    {
                        player.StopJumping();
                        player.StartFalling();
                        player.Position = new Vector2(platformRect.Right + (playerRect.Width / 2), player.Position.Y);
                    }
                    intersected = true;
                }
            }

            if(!intersected)
            {
                player.StartFalling();
            }

            base.Update(gameTime);
        }
    }
}
