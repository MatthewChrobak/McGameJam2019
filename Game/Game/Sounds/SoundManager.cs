﻿using Game.Patterns.Singleton;
using SFML.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
  - Unless stated otherwise, all sounds obtained from freeSound.org
  - bird_whistling, wind_chimes, thunder_rain from InspectorJ @ freeSound.org
  - soft_wind_blowing from Aeonemi @ freeSound.org
  - rock_sliding from Jan Schulze @ freeSound.org
*/

namespace Game.Sounds
{
    public class SoundManager: Singleton
    {
        //Get current directory
        public string directory = Directory.GetCurrentDirectory();

        public string mainTheme = "";

        //Character
        public string woodHitting = "/sounds/pebble_hit_wood.wav";
        public string keyPickup = "/sounds/keys_pickup.wav";

        //FootSteps
        public string footstepStoneFloor1 = "/sounds/footstep_stone_1.wav";
        public string footstepDirt1 = "/sounds/footstep_dirt_1.wav";
        public string footstepGrass1 = "/sounds/footstep_grass_1.wav";
        public string footstepGrass2 = "/sounds/footstep_grass_2.wav";
        public string footstepWood1 = "/sounds/footstep_soft_wood1.wav";
        public string footstepWood2 = "/sounds/footstep_soft_wood2.wav";

        //Environment
        public string clothShuffling = "/sounds/cloth_shuffling.wav";
        public string movingRock = "/sounds/rock_sliding.wav";
        public string woodenDoorOpen = "/sounds/creaking_door_opening.wav";
        public string woodCreaking = "/sounds/creaking_wood_floor.wav";
        public string pansClanking = "/sounds/pots_and_pans.wav";
        public string saucepan = "/sounds/saucepan.wav";
        public string windStrong = "/sounds/strong_wind.wav";
        public string windSoft = "/sounds/soft_wind_blowing.wav";
        public string windChimes = "/sounds/wind_chimes.wav";
        public string windChimesSoft = "/sounds/soft_windchimes.wav";
        public string chirpingBirds = "/sounds/bird_whistling.wav";
        public string crowCawing = "/sounds/crow_cawing_1.wav";
        public string rainModerate = "/sounds/moderate_rain.wav";
        public string rainSoftLong = "/sounds/soft_rain_long.wav";
        public string rainThunder = "/sounds/thunder_rain.wav";

        //Enemy - Minion - Monster
        public string laughFemale = "/sounds/female_evil_laugh.wav";
        public string laughEvilWhispered = "/sounds/evil_laugh_whispered.wav";

        //To play music
        public void PlayMusic(string musicInput)
        {
            Music music = new Music(musicInput);
            music.Play();
        }

        //To play a sound
        public void PlaySound(string soundInput, float volume = 0, float offset = 0f)
        {
            SoundBuffer newSound = new SoundBuffer(directory + soundInput);
            Sound sound = new Sound(newSound);

            if(volume > 0)
            {
                sound.Volume = volume;
            }

            //Not working at the moment
            if(offset > 0)
            {
                sound.PlayingOffset.AsSeconds();               
            }

            sound.Play();
        }
        
        
    }
}
