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
        private const int RECTANGLE_SIDE_OFFSET = 1;
        private const int HORIZONTAL_BOUND_OFFSET = 5;

        private Game1 parent;
        private Player player;
        private List<Platform> platforms;

        public CollisionDetection(Game game) : base(game)
        {
            parent = (Game1)game;
            platforms = new List<Platform>();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void AddPlayer(Player player)
        {
            if(this.player == null)
            {
                this.player = player;
            }
        }

        public void AddPlatform(Platform platform)
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
                // platform rectangle above actual platform
                Rectangle platformRect = platform.GetBound();
                if (platformRect.Intersects(playerRect))
                {
                    // Landing on the platform
                    // If it sinks too deep and the bottom rectangle is below the platform but the right and left are still in
                    // If only one side made it inside then we need to check if the player is inside the platform instead of on the edge
                    if(platformRect.Intersects(playerBottomRect) || (!platformRect.Intersects(playerTopRect) && platformRect.Intersects(playerRightRect) && platformRect.Intersects(playerLeftRect)) || (!platformRect.Intersects(playerTopRect) && playerRect.Right > (platformRect.Left + HORIZONTAL_BOUND_OFFSET) && playerRect.Left < (platformRect.Right - HORIZONTAL_BOUND_OFFSET)))
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
                    else if (platformRect.Intersects(playerRightRect))
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
