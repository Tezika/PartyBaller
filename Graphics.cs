﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PartyBall
{
    public static class Graphics
    {
        public static Texture2D StartMenu;
        public static Texture2D GameOverScreen;

        public static Texture2D Player;
        public static Texture2D Laser;
        public static Texture2D Enemy;
        public static Texture2D Explosion;
        public static Texture2D Boss;
        public static Texture2D HealthBar;

        public static Texture2D Background;
        public static Texture2D BackgroundLayer1;
        public static Texture2D BackgroundLayer2;

        public static SpriteFont Font;

        public static void load(ContentManager content)
        {
            StartMenu = content.Load<Texture2D>("Graphics\\startScreen");
            GameOverScreen = content.Load<Texture2D>("Graphics\\endMenu");

            Player = content.Load<Texture2D>("Graphics\\playerAnimation");
            Laser = content.Load<Texture2D>("Graphics\\laser");
            Enemy = content.Load<Texture2D>("Graphics\\shadesPickupAnimation");
            Explosion = content.Load<Texture2D>("Graphics\\explosion");
            Boss = content.Load<Texture2D>("Graphics\\shadesPickup");
            HealthBar = content.Load<Texture2D>("Graphics\\healthBar");

            Background = content.Load<Texture2D>("Graphics\\mainbackground");
            BackgroundLayer1 = content.Load<Texture2D>("Graphics\\bgLayer1");
            BackgroundLayer2 = content.Load<Texture2D>("Graphics\\bgLayer2");

            Font = content.Load<SpriteFont>("Graphics\\gameFont");
        }
    }
}
