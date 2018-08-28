﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PartyBall.Scripts.CharacterMovement;
using PartyBall.Scripts.Render;
using PartyBall.Scripts.Singleton;
using System;

namespace PartyBall.Scripts.Entities
{
    public class Character : GameObject
    {
        public float CurrentSpeed { get; internal set; }

        public CharacterMoveState CurrentMoveState { get; private set; }

        public CharacterMoveState[] MoveStates { get; private set; }

        public Platform CurPlatform { get; private set; }

        public Character(Texture2D texture, Vector2 position) : base(texture, position)
        {
        }

        public override void Initialize()
        {
            this.InitMoveStates();
            this.CurPlatform = null;
        }

        //update the player's logic
        public override void Update(GameTime gameTime)
        {
            this.UpdatePosition(Keyboard.GetState());
            this.UpdatePlatform();
            this.UpdatePickups();
            if (this.CurrentMoveState != null)
            {
                this.CurrentMoveState.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void TranslateMoveState(MoveType type)
        {
            if (this.CurrentMoveState != null)
            {
                //if the player has already on that state.
                if (this.CurrentMoveState.Type == type)
                {
                    return;
                }
                this.CurrentMoveState.OnExit();
            }
            this.CurrentMoveState = this.MoveStates[(int)type];
            this.CurrentMoveState.OnEnter();
        }

        public void Respawn()
        {
            Debugger.Instance.Log("The character has already respawned");
            this.CurrentSpeed = CharacterMoveAbilities.RollSpeed;
            this.Scale = 1;
            //Reset the player's 
            this.Position = new Vector2((float)(RenderManager.Instance.Graphics.GraphicsDevice.Viewport.Width * 0.5),
                         (float)(RenderManager.Instance.Graphics.GraphicsDevice.Viewport.Height - this.Height / 2));
            this.TranslateMoveState(MoveType.Roll);
        }

        private void UpdatePickups()
        {
            for (int i = 0; i < Game1.Instance.Pickups.Count; i++)
            {
                var curPickup = Game1.Instance.Pickups[i];
                if (curPickup.BoundingBox.Intersects(this.BoundingBox))
                {
                    curPickup.TakeEffect();
                    break;
                }
            }
        }

        private void UpdatePlatform()
        {
            //When player is jumping or  falling, we dont update the platform
            if (this.CurrentMoveState.Type == MoveType.Jump || this.CurrentMoveState.Type == MoveType.Fall)
            {
                return;
            }
            this.CurPlatform = null;
            for (int i = 0; i < Game1.Instance.Platforms.Count; i++)
            {
                var platform = Game1.Instance.Platforms[i];
                if (platform.BoundingBox.Intersects(this.BoundingBox))
                {
                    this.CurPlatform = platform;
                    break;
                }
            }

            if (this.CurPlatform == null)          
            {
                this.TranslateMoveState(MoveType.Fall);
                return;
            }

            if (this.CurPlatform.Type == PlatformType.Regular && this.CurrentMoveState.Type != MoveType.Roll)
            {
                this.TranslateMoveState(MoveType.Roll);
            }
            else if (this.CurPlatform.Type == PlatformType.Wall && this.CurrentMoveState.Type != MoveType.Slide)
            {
                this.TranslateMoveState(MoveType.Slide);
            }
        }


        private void UpdatePosition(KeyboardState state)
        {
            if (!this.CurrentMoveState.CanControl)
            {
                return;
            }

            if (state.IsKeyDown(Keys.Up))
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y - this.CurrentSpeed);
            }

            if (state.IsKeyDown(Keys.Down))
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y + this.CurrentSpeed);
            }

            if (state.IsKeyDown(Keys.Left) && this.CurrentMoveState.CanMoveLeft)
            {
                this.Position = new Vector2(this.Position.X - this.CurrentSpeed, this.Position.Y);
            }

            if (state.IsKeyDown(Keys.Right) && this.CurrentMoveState.CanMoveRight)
            {
                this.Position = new Vector2(this.Position.X + this.CurrentSpeed, this.Position.Y);
            }

            //Jump
            if (this.CurrentMoveState.CanJump && state.IsKeyDown(Keys.Space))
            {
                this.TranslateMoveState(MoveType.Jump);
                this.CurPlatform = null;
            }
        }

        private void InitMoveStates()
        {
            //set up move states
            if (this.MoveStates == null)
            {
                this.MoveStates = new CharacterMoveState[Enum.GetNames(typeof(MoveType)).Length];
            }

            this.MoveStates[(int)MoveType.Fall] = new CharacterFallState(this);
            this.MoveStates[(int)MoveType.Jump] = new CharacterJumpState(this);
            this.MoveStates[(int)MoveType.Roll] = new CharacterRollState(this);
            this.MoveStates[(int)MoveType.Slide] = new CharacterSlideState(this);
        }
    }
}