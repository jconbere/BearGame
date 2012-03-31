using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BearGame
{
    class World
    {
        public const int TileSize = 64;

        SpriteBatch spriteBatch;

        Texture2D _tilesTexture;
        Layer _tilesLayer = new Layer();

        Layer _collisionLayer = new Layer();

        public Camera Camera { get; private set; }
        public Bear Bear { get; private set; }
        public List<Actor> AllActors { get; private set; }

        public World(GameSetting settings)
        {
            Camera = new BearGame.Camera();

            Bear = new Bear(settings);

            AllActors = new List<Actor>();
            AllActors.Add(Bear);
        }

        public void LoadContent(GraphicsDevice device, ContentManager content, int worldNumber)
        {
            spriteBatch = new SpriteBatch(device);

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

        Rectangle GetWorldTileRectangle(char tileType)
        {
            switch (tileType)
            {
                case '.':
                    return new Rectangle(0, 0, TileSize, TileSize);
                case 'T':
                    return new Rectangle(TileSize, 0, TileSize, TileSize);
                case 'W':
                    return new Rectangle(2*TileSize, 0, TileSize, TileSize);
                default:
                    return new Rectangle(3*TileSize, 0, TileSize, TileSize);
            }
        }

        public void Draw(Rectangle frame)
        {
            var scale = Matrix.CreateScale((float)frame.Width / (float)(9 * 64));
            var translate = Matrix.CreateTranslation(frame.X, frame.Y, 0);

            var tx = Matrix.Multiply(scale, translate);

            var raster = new RasterizerState();
            raster.ScissorTestEnable = true;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, raster, null, tx);

            spriteBatch.GraphicsDevice.ScissorRectangle = frame;

            for (var c = 0; c < _tilesLayer.NumColumns; c++)
            {
                for (var r = 0; r < _tilesLayer.NumRows; r++)
                {
                    spriteBatch.Draw(_tilesTexture, GetTileRectangle(c, r), GetWorldTileRectangle(_tilesLayer.GetTile(c,r)), Color.White);
                }
            }

            foreach (var a in AllActors)
            {
                a.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
