using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BearGame
{
    class World
    {
        public const int TileSize = 64;

        Texture2D _tilesTexture;
        Layer _tilesLayer = new Layer();

        Layer _collisionLayer = new Layer();


        public void Load(Texture2D tilesTexture, int worldNumber)
        {
            _tilesTexture = tilesTexture;

            _tilesLayer.LoadTiles("Content\\Maps\\Tiles" + worldNumber + ".txt");
            _collisionLayer.LoadTiles("Content\\Maps\\Collisions" + worldNumber + ".txt");
        }

        public bool IsPassable(int column, int row)
        {
            return _collisionLayer.GetTile(column, row) != '0';
        }

        public Rectangle GetTileRectangle(int c, int r)
        {
            return new Rectangle(c * TileSize, r * TileSize, TileSize, TileSize);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var c = 0; c < _tilesLayer.NumColumns; c++)
            {
                for (var r = 0; r < _tilesLayer.NumRows; r++)
                {
                    spriteBatch.Draw(_tilesTexture, GetTileRectangle(c, r), Color.White);
                }
            }
        }
    }
}
