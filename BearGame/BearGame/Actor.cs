using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BearGame
{
    class Actor
    {
        Texture2D SpriteTexture;
        int NumColumnsInSpriteTexture;       

        protected int spriteIndex = 0;        

        Vector2 position;
        public CellPosition c_position;
        public CellPosition spawn_position;

        public GameSetting Settings { get; private set; }

        Direction _facingDirection;
        public Direction FacingDirection
        {
            get { return _facingDirection; }
            set
            {
                _facingDirection = value;
                UpdateSpriteIndex();
            }
        }

        public Actor(GameSetting settings)
        {
            Settings = settings;
            spriteIndex = 0;
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public virtual void LoadContent(Texture2D texture_IN, CellPosition cellPosition, Vector2 pixelPosition)
        {
            spawn_position = cellPosition;
            c_position = cellPosition;
            position = pixelPosition;

            SpriteTexture = texture_IN;
            NumColumnsInSpriteTexture = SpriteTexture.Width / World.TileSize;
        }

        public virtual void Update(GameTime time, World world)
        {
        }

        public void Draw(SpriteBatch spriteBatch_IN)
        {
            Rectangle sourceRec;

            var row = spriteIndex / NumColumnsInSpriteTexture;
            var col = spriteIndex - row * NumColumnsInSpriteTexture;

            sourceRec = new Rectangle(col * World.TileSize, row * World.TileSize, World.TileSize, World.TileSize);
            spriteBatch_IN.Draw(SpriteTexture, position, sourceRec, Color.White);
        }

        protected int Distance(Actor Actor1, Actor Actor2)
        {
            return Math.Max(Math.Abs(Actor1.c_position.Row - Actor2.c_position.Row), Math.Abs(Actor1.c_position.Col - Actor2.c_position.Col));
        }

        protected virtual void UpdateSpriteIndex()
        {
        }
    }
}
