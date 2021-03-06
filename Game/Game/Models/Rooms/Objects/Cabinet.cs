﻿using Game.Patterns.Singleton;
using System;

namespace Game.Models.Rooms.Objects
{
    [Serializable]
    public class Cabinet : CollisionObject
    {
        public override bool Probe(float x, float y) {
            if (base.Probe(x, y)) {
                var player = Singleton.Get<DataManager>().Player;
                var globals = Singleton.Get<Globals>();

                if (player.Hiding) {
                    player.Hiding = false;
                    globals.DisableMovement = false;
                } else {
                    player.Hiding = true;
                    globals.DisableMovement = true;
                }
                return true;
            }
            return false;
        }
    }
}
