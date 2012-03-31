using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace BearGame
{
    class World : Layer
    {
        public void Load(Texture2D tilesTexture, string path)
        {
            LoadTiles(path);
        }

        public void Draw(Camera camera)
        {

        }
    }
}
