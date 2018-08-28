﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PartyBall.Scripts.Entities
{
    public enum PlatformType
    {
        Regular,
        Wall
    }

    public class Platform : GameObject
    {
        public virtual PlatformType Type { get; }

        public Platform(Texture2D texture, Vector2 position) : base(texture, position)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}