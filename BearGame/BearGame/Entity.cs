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
        protected Texture2D SpriteTexture;
        public static Texture2D emotes;

        protected int NumColumnsInSpriteTexture;

        protected int spriteIndex = 0;

        protected Vector2 position;
        CellPosition _c_position;
        public CellPosition c_position
        {
            get
            {
                return _c_position;
            }
            set
            {
                _c_position = value;
            }
        }

        public CellPosition spawn_position;

        public World World { get; private set; }
        public GameSetting Settings { get { return World.Settings; } }

        public bool IsVisible { get; set; }
        public bool IsActive { get; set; }

        public Entity(World world)
        {
            World = world;
            IsVisible = true;
            spriteIndex = 0;
            //UpdateSpriteIndex();
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

        public virtual void Draw(SpriteBatch spriteBatch_IN)
        {
            if (!IsVisible) return;

            Rectangle sourceRec;

            var row = spriteIndex / NumColumnsInSpriteTexture;
            var col = spriteIndex - row * NumColumnsInSpriteTexture;

            sourceRec = new Rectangle(col * World.TileSize, row * World.TileSize, World.TileSize, World.TileSize);
            spriteBatch_IN.Draw(SpriteTexture, position, sourceRec, Color.White);

        }

        protected Vector2 targetDirecton(Actor baseActor, Actor targetActor){

            return new Vector2(baseActor.c_position.Col - targetActor.c_position.Col,baseActor.c_position.Row - targetActor.c_position.Row);
        }

        protected int Distance(Actor Actor1, Actor Actor2)
        {
            return Math.Max(Math.Abs(Actor1.c_position.Row - Actor2.c_position.Row), Math.Abs(Actor1.c_position.Col - Actor2.c_position.Col));
        }

        protected int Distance(Entity Actor1, Entity Actor2)
        {
            return Math.Max(Math.Abs(Actor1.c_position.Row - Actor2.c_position.Row), Math.Abs(Actor1.c_position.Col - Actor2.c_position.Col));
        }

        protected virtual void UpdateSpriteIndex()
        {
        }

        protected virtual void UpdateSpriteIndex(GameTime gametime)
        {
        }
    }
}
