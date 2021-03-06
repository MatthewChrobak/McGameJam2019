﻿using Game.Events;
using Game.Graphics;
using Game.Graphics.Contexts;
using Game.Patterns.Singleton;
using Game.UserInterface;
using Game.UserInterface.Scenes;
using System;

namespace Game.Models.Entities
{
    public abstract class BabaYaga : IDrawableObject
    {
        protected float _scaling;

        public abstract void Draw(IDrawableSurface surface);

        protected abstract void PerformSummonTasks();

        public static void Summon() {
            var data = Singleton.Get<DataManager>();

            if (data.CurrentRoom.IsBabaYagaFlipped()) {
                data.BabaYaga = new FlippedBabaYaga(data.CurrentRoom.GetBabaYagaStartXConstant(), data.CurrentRoom.GetBabaYagaStartYConstant());
            } else {
                data.BabaYaga = new SimpleBabaYaga(data.CurrentRoom.GetBabaYagaStartXConstant(), data.CurrentRoom.GetBabaYagaStartYConstant());
            }

            data.BabaYaga._scaling = data.CurrentRoom.GetBabaYagaScalingConstant();
            data.BabaYaga.PerformSummonTasks();
        }
    }

    public class FlippedBabaYaga : BabaYaga
    {
        private SurfaceContext _arm_ctx;
        private (float x, float y) _arm_pos;
        private (float x, float y) _arm_size;
        private string _arm_surface;
        private int _arm_frame;

        private SurfaceContext _body_ctx;
        private (float x, float y) _body_pos;
        private (float x, float y) _body_size;
        private string _body_surface;


        public FlippedBabaYaga(float start_x, float start_y) {

            this._body_ctx = new SurfaceContext();
            this._arm_ctx = new SurfaceContext();

            _scaling = Singleton.Get<DataManager>().CurrentRoom.GetBabaYagaScalingConstant();

            this._body_size = (300 / _scaling, 300 / _scaling);
            this._arm_size = (1000 / (4 * _scaling), 700 / (4 * _scaling));

            this._body_pos = (start_x, start_y);
            this._arm_pos = (start_x, start_y + (this._body_size.y / 8));

            this._body_surface = "graphics/Babayagabodynoarm_mirror.png";
            this._arm_surface = "graphics/babarM_mirror.png";
        }

        public override void Draw(IDrawableSurface surface) {
            _body_ctx.Rect = (250, 450, 1300, 1300);
            _body_ctx.Size = (_body_size.x, _body_size.y);
            _body_ctx.Position = (_body_pos.x, _body_pos.y);

            _arm_ctx.Rect = (700 * _arm_frame, 0, 1000, 700);
            _arm_ctx.Size = (_arm_size.x, _arm_size.y);
            _arm_ctx.Position = (_arm_pos.x, _arm_pos.y);


            surface.Draw(_body_surface, _body_ctx);
            surface.Draw(_arm_surface, _arm_ctx);
        }

        protected override void PerformSummonTasks() {
            var queue = Singleton.Get<EventQueue>();
            var player = Singleton.Get<DataManager>().Player;

            (float target_x, float target_y) target;
            float requiredXDistance = 0;
            float deltaX = 0;
            int grabPlayer_Counter = 25;
            int point2 = grabPlayer_Counter / 3;
            int point3 = 2 * point2;
            Func<EVENT_RETURN> grabPlayer = () => {
                grabPlayer_Counter--;
                this._arm_size.x += Math.Abs(deltaX);
                this._arm_pos.x -= Math.Abs(deltaX);

                if (grabPlayer_Counter <= point2) {
                    _arm_frame = 2;
                } else if (grabPlayer_Counter <= point3) {
                    _arm_frame = 1;
                }

                //this._arm_pos.x -= deltaX / 4;
                if (grabPlayer_Counter == 0) {
                    Singleton.Get<UIManager>().LoadScene<GameOver>();
                    Singleton.Get<DataManager>().BabaYaga = null;
                    Singleton.Get<Globals>().DisableUserInput = false;
                    return EVENT_RETURN.REMOVE_FROM_QUEUE;
                } else {
                    return EVENT_RETURN.NONE;
                }
            };

            int waitForPlayer_Counter = 20;
            Func<EVENT_RETURN> waitForPlayer = () => {
                waitForPlayer_Counter--;

                if (!player.Hiding) {
                    queue.AddEvent(PriorityTypes.ANIMATION, grabPlayer, 25, 0);
                    Singleton.Get<Globals>().DisableUserInput = true;
                    target = (player.X + player.Width / 2, player.Y + player.Height / 2 - player.Width);
                    requiredXDistance = (target.target_x - _body_pos.x);
                    deltaX = requiredXDistance / grabPlayer_Counter;
                    return EVENT_RETURN.REMOVE_FROM_QUEUE;
                }

                if (waitForPlayer_Counter == 0) {
                    Singleton.Get<DataManager>().BabaYaga = null;
                    return EVENT_RETURN.REMOVE_FROM_QUEUE;
                } else {
                    return EVENT_RETURN.NONE;
                }
            };
            queue.AddEvent(PriorityTypes.ANIMATION, waitForPlayer, 100, 0);
        }
    }

    public class SimpleBabaYaga : BabaYaga
    {
        private SurfaceContext _arm_ctx;
        private (float x, float y) _arm_pos;
        private (float x, float y) _arm_size;
        private string _arm_surface;
        private int _arm_frame;

        private SurfaceContext _body_ctx;
        private (float x, float y) _body_pos;
        private (float x, float y) _body_size;
        private string _body_surface;


        public SimpleBabaYaga(float start_x, float start_y) {

            this._scaling = Singleton.Get<DataManager>().CurrentRoom.GetBabaYagaScalingConstant();

            this._body_ctx = new SurfaceContext();
            this._arm_ctx = new SurfaceContext();

            this._body_size = (300 / _scaling, 300 / _scaling);
            this._arm_size = (1000 / (4 * _scaling), 700 / (4 * _scaling));

            this._body_pos = (start_x, start_y);
            this._arm_pos = (start_x - 80, start_y + (this._body_size.y / 8));

            this._body_surface = "graphics/Babayagabodynoarm.png";
            this._arm_surface = "graphics/babarM.png";
        }

        public override void Draw(IDrawableSurface surface) {
            _body_ctx.Rect = (250, 450, 1300, 1300);
            _body_ctx.Size = (_body_size.x, _body_size.y);
            _body_ctx.Position = (_body_pos.x, _body_pos.y);

            _arm_ctx.Rect = (700 * _arm_frame, 0, 1000, 700);
            _arm_ctx.Size = (_arm_size.x, _arm_size.y);
            _arm_ctx.Position = (_body_pos.x + _arm_pos.x, _arm_pos.y);


            surface.Draw(_body_surface, _body_ctx);
            surface.Draw(_arm_surface, _arm_ctx);
        }

        protected override void PerformSummonTasks() {
            var queue = Singleton.Get<EventQueue>();
            var player = Singleton.Get<DataManager>().Player;

            (float target_x, float target_y) target;
            float requiredXDistance = 0;
            float deltaX = 0;
            int grabPlayer_Counter = 25;
            int point2 = grabPlayer_Counter / 3;
            int point3 = 2 * point2;
            Func<EVENT_RETURN> grabPlayer = () => {
                grabPlayer_Counter--;
                this._arm_size.x += deltaX;

                if (grabPlayer_Counter <= point2) {
                    _arm_frame = 2;
                } else if (grabPlayer_Counter <= point3) {
                    _arm_frame = 1;
                }

                //this._arm_pos.x -= deltaX / 4;
                if (grabPlayer_Counter == 0) {
                    Singleton.Get<UIManager>().LoadScene<GameOver>();
                    Singleton.Get<DataManager>().BabaYaga = null;
                    Singleton.Get<Globals>().DisableUserInput = false;
                    return EVENT_RETURN.REMOVE_FROM_QUEUE;
                } else {
                    return EVENT_RETURN.NONE;
                }
            };

            int waitForPlayer_Counter = 20;
            Func<EVENT_RETURN> waitForPlayer = () => {
                waitForPlayer_Counter--;

                if (!player.Hiding) {
                    queue.AddEvent(PriorityTypes.ANIMATION, grabPlayer, 25, 0);
                    Singleton.Get<Globals>().DisableUserInput = true;
                    target = (player.X + player.Width / 2, player.Y + player.Height / 2 - player.Width);
                    requiredXDistance = (target.target_x - _body_pos.x);
                    deltaX = requiredXDistance / grabPlayer_Counter;
                    return EVENT_RETURN.REMOVE_FROM_QUEUE;
                }

                if (waitForPlayer_Counter == 0) {
                    Singleton.Get<DataManager>().BabaYaga = null;
                    return EVENT_RETURN.REMOVE_FROM_QUEUE;
                } else {
                    return EVENT_RETURN.NONE;
                }
            };
            queue.AddEvent(PriorityTypes.ANIMATION, waitForPlayer, 100, 0);
        }
    }
}
