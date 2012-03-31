﻿using System;
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

        Layer _propsLayer = new Layer();

        Layer _actorsLayer = new Layer();

        public Camera Camera { get; private set; }
        public Bear Bear { get; private set; }
        public List<Actor> AllActors { get; private set; }

        //public List<Prop> AllProps { get; private set; }

        public GameSetting Settings { get; private set; }

        public World(GameSetting settings)
        {
            Settings = settings;

            Camera = new BearGame.Camera();

            Bear = new Bear(settings);

            AllActors = new List<Actor>();
            AllActors.Add(Bear);
        }

        public void LoadContent(GraphicsDevice device, ContentManager content, int worldNumber)
        {
            spriteBatch = new SpriteBatch(device);

            //
            // Map tiles
            //
            _tilesTexture = content.Load<Texture2D>("Sprites\\WorldTiles");
            _tilesLayer.LoadTiles("Content\\Maps\\Tiles" + worldNumber + ".txt");
            _collisionLayer.LoadTiles("Content\\Maps\\Collisions" + worldNumber + ".txt");

            //
            // Load props
            //
            /*_propsLayer.LoadTiles("Content\\Maps\\Props" + worldNumber + ".txt");
            for (var c = 0; c < _propsLayer.NumColumns; c++)
            {
                for (var r = 0; r < _propsLayer.NumColumns; r++)
                {
                    switch (_propsLayer.GetTile(c, r))
                    {
                        case 'H':

                    }
                }
            }*/

            //
            // Load actors
            //
            var bearTexture = content.Load<Texture2D>("Sprites\\spritesheet_bear");
            var villagerTexture = content.Load<Texture2D>("Sprites\\spritesheet_bear");
            _actorsLayer.LoadTiles("Content\\Maps\\Actors" + worldNumber + ".txt");
            for (var c = 0; c < _actorsLayer.NumColumns; c++)
            {
                for (var r = 0; r < _actorsLayer.NumColumns; r++)
                {
                    switch (_actorsLayer.GetTile(c, r))
                    {
                        case 'V':
                            {
                                var v = new Villager(Settings);
                                v.LoadContent(villagerTexture, GetTilePosition(c, r));
                                AllActors.Add(v);
                            }
                            break;
                        case 'R':
                            {
                                Bear.LoadContent(bearTexture, GetTilePosition(c, r));
                            }
                            break;
                    }
                }
            }
            
        }

        public bool IsPassable(int column, int row)
        {
            return _collisionLayer.GetTile(column, row) != '0';
        }

        public Rectangle GetTileRectangle(int c, int r)
        {
            return new Rectangle(c * TileSize, r * TileSize, TileSize, TileSize);
        }

        public Vector2 GetTilePosition(int c, int r)
        {
            return new Vector2(c * TileSize, r * TileSize);
        }

        public void Update(GameTime time)
        {
            foreach (var a in AllActors)
            {
                a.Update(time, this);
            }

            Camera.CenterPosition = Bear.Position + new Vector2(TileSize / 2, TileSize / 2);
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
            var ctranslate = Matrix.CreateTranslation(-(Camera.CenterPosition.X - 4.5f*TileSize), -(Camera.CenterPosition.Y - 4.5f*TileSize), 0);
            var fscale = Matrix.CreateScale((float)frame.Width / (float)(9 * 64));
            var ftranslate = Matrix.CreateTranslation(frame.X, frame.Y, 0);

            var tx = Matrix.Multiply(ctranslate, Matrix.Multiply(fscale, ftranslate));

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