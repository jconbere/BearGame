using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BearGame
{
    public class Entity
    {
        Texture2D SpriteTexture;
        int NumColumnsInSpriteTexture;

        protected int spriteIndex = 0;

        Vector2 position;
        public CellPosition c_position;
        public CellPosition spawn_position;

        public World World { get; private set; }
        public GameSetting Settings { get { return World.Settings; } }

        public bool IsVisible { get; set; }

        public Entity(World world)
        {
            World = world;
            IsVisible = true;
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

        public virtual void Update(GameTime time)
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
