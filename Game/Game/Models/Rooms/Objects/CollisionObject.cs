﻿using Game.Graphics;
using Game.Graphics.Contexts;
using System;

namespace Game.Models.Rooms.Objects
{
    [Serializable]
    public abstract class CollisionObject : IDrawableObject
    {
        public int ItemID;

        public (float x, float y) Position;
        public (float x, float y) Size;

        protected SurfaceContext _ctx;
        public string SurfaceName;

        public virtual bool Probe(float x, float y) {
            if (x >= Position.x && x <= Position.x + Size.x) {
                if (y >= Position.y && y <= Position.y + Size.y) {
                    return true;
                }
            }
            return false;
        }

        public void Draw(IDrawableSurface surface) {
            if (this._ctx == null) {
                RefreshContext();
            }
            surface.Draw(this.SurfaceName, this._ctx);
        }

        public void RefreshContext() {
            this._ctx = new SurfaceContext() {
                Position = this.Position,
                Size = this.Size
            };
        }

        public virtual void HandleAdditionalParamsForCreation(string[] cmd) {

        }
    }
}
