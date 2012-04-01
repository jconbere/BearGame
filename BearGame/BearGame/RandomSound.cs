using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace BearGame
{
    public class RandomSound
    {
        List<SoundEffect> _sounds;
        Random _rand;
        float _volume;

        public RandomSound(ContentManager content, params string[] names)
            : this(content, 1.0f, names)
        {
        }

        public RandomSound(ContentManager content, float volume, params string[] names)
        {
            _sounds = (from n in names
                       select content.Load<SoundEffect>(n)).ToList();
            _rand = new Random();
            _volume = volume;
        }

        public void Play()
        {
            _sounds[_rand.Next(_sounds.Count)].Play();
        }
    }
}
