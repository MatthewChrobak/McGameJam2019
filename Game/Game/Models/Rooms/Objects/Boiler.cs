﻿using Game.Patterns.Singleton;
using Game.Sounds;
using SFML.Audio;
using System;
using System.IO;


namespace Game.Models.Rooms.Objects
{
    [Serializable]
    public class Boiler : CollisionObject
    {
        public override bool Probe(float x, float y) {
            if (base.Probe(x, y)) {
                var data = Singleton.Get<DataManager>();
                var sound = Singleton.Get<SoundManager>();

                if (data.Player.HasBucket && data.Player.isBucketFilled) {
                    data.CurrentRoom.AddFloatingMessage("The boiler is working again...", x - 100, y - 100, 2500);
                    data.CurrentRoom.BackgroundImage = "graphics/Room2_with light.png";
                    data.Player.isBoilerOn = true;
                    //TODO: add boiler sound
                    SoundBuffer boiler = new SoundBuffer(File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + sound.windStrong));
                    Sound m = new Sound(boiler);
                    sound.PlaySound(m, 20f);
                    sound.StopSound(m, 2000);
                } else {
                    data.CurrentRoom.AddFloatingMessage("This house feel so cold...", x - 100, y - 100, 2500);
                }
                return true;
            }
            return false;
        }
    }
}
