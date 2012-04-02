using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace BearGame
{
    public class WorldLoveSystem
    {
        World _world;

        /// <summary>
        /// 0 is no love, 1 is ambivalence, > 1 is lots of love
        /// </summary>
        public float LovePotential { get; private set; }

        public float AverageLove { get; private set; }

        GameSetting Settings { get { return _world.Settings; } }

        SoundEffect _whistlingWind1, _whistlingWind2, _whistlingWind3, _whistlingWind4, _whistlingWind5;
        SoundEffect _birdChirp1, _birdChirp2, _birdChirp3, _birdChirp4, _birdChirp5;

        public WorldLoveSystem(World world)
        {
            _world = world;
        }

        public void LoadContent(ContentManager content)
        {
            _birdChirp1 = content.Load<SoundEffect>("Audio\\bird_chirp_01");
            _birdChirp2 = content.Load<SoundEffect>("Audio\\bird_chirp_02");
            _birdChirp3 = content.Load<SoundEffect>("Audio\\bird_chirp_03");
            _birdChirp4 = content.Load<SoundEffect>("Audio\\bird_chirp_04");
            _birdChirp5 = content.Load<SoundEffect>("Audio\\bird_chirp_05");

            _whistlingWind1 = content.Load<SoundEffect>("Audio\\whistling_wind_01");
            _whistlingWind2 = content.Load<SoundEffect>("Audio\\whistling_wind_02");
            _whistlingWind3 = content.Load<SoundEffect>("Audio\\whistling_wind_03");
            _whistlingWind4 = content.Load<SoundEffect>("Audio\\whistling_wind_04");
            _whistlingWind5 = content.Load<SoundEffect>("Audio\\whistling_wind_05");
        }

        public void UnloadContent(ContentManager content)
        {
            if (_currentAmbientEffectInstance != null)
            {
                _currentAmbientEffectInstance.Stop();
            }
        }

        SoundEffect _currentAmbientEffect;
        SoundEffectInstance _currentAmbientEffectInstance;

        void SetAmbientSound(SoundEffect effect, float volume)
        {
            if (_currentAmbientEffect == null || _currentAmbientEffect != effect)
            {
                var inst = effect.CreateInstance();
                inst.Volume = volume;
                inst.IsLooped = true;
                inst.Play();

                if (_currentAmbientEffectInstance != null)
                {
                    _currentAmbientEffectInstance.Stop();
                }

                _currentAmbientEffect = effect;
                _currentAmbientEffectInstance = inst;
            }
            else
            {
                _currentAmbientEffectInstance.Volume = volume;
            }
        }

        public void Update(GameTime time)
        {
            //
            // Calculate the stats
            //
            var totalLove = 0;
            foreach (var v in _world.AllVillagers)
            {
                totalLove += v.Love;
            }
            var totalAmbivalentLove = _world.AllVillagers.Count * Settings.Person_InitialLove;

            var newLovePotential = totalLove / (float)(totalAmbivalentLove);
            var newAverageLove = totalLove / (float)(_world.AllVillagers.Count);

            //
            // Update ambient sounds
            //
            if (0 <= newAverageLove && newAverageLove < 1)
            {
                SetAmbientSound(_whistlingWind5, 1);
            }
            else if (1 <= newAverageLove && newAverageLove < 2)
            {
                SetAmbientSound(_whistlingWind2, 0.85f);
            }
            else if (2 <= newAverageLove && newAverageLove < 2.9)
            {
                SetAmbientSound(_whistlingWind1, 0.75f);
            }
            else if (2.9 <= newAverageLove && newAverageLove < 3.1)
            {
                SetAmbientSound(_birdChirp1, 0.03f);
            }
            else if (3.1 <= newAverageLove && newAverageLove < 4)
            {
                SetAmbientSound(_birdChirp1, 0.25f);
            }
            else if (4 <= newAverageLove && newAverageLove < 5)
            {
                SetAmbientSound(_birdChirp2, 0.75f);
            }
            else if (5 <= newAverageLove && newAverageLove < 7)
            {
                SetAmbientSound(_birdChirp2, 1.0f);
            }


            LovePotential = newLovePotential;
            AverageLove = newAverageLove;
        }
    }
}
