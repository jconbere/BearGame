using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace BearGame
{
    class World : Layer
    {
        public const int TileSize = 64;

        Texture2D _tilesTexture;

        public void Load(Texture2D tilesTexture, string path)
        {
            _tilesTexture = tilesTexture;
            LoadTiles(path);
        }

        public void Draw(Camera camera)
        {
            var leftPosition = camera.Position.X - (Camera.VisibleSize / 2) * TileSize;
            var topPosition = camera.Position.Y - (Camera.VisibleSize / 2) * TileSize;
        }
    }
}
