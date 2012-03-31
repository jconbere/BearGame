using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BearGame
{
    class World
    {
        public const int TileSize = 64;

        Texture2D _tilesTexture;
        Layer _tilesLayer = new Layer();

        Layer _collisionLayer = new Layer();

        public Bear Bear { get; private set; }
        public List<Actor> AllActors { get; private set; }

        public World(GameSetting settings)
        {
            Bear = new Bear(settings);

            AllActors = new List<Actor>();
            AllActors.Add(Bear);
        }

        public void LoadContent(ContentManager content, int worldNumber)
        {
            Bear.LoadContent(content.Load<Texture2D>("Sprites\\firstsprite"), new Vector2(100,100));

            _tilesTexture = content.Load<Texture2D>("Sprites\\WorldTiles");

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

        public void Update(GameTime time)
        {
            foreach (var a in AllActors)
            {
                a.Update(time);
            }
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

            foreach (var a in AllActors)
            {
                a.Draw(spriteBatch);
            }
        }
    }
}
