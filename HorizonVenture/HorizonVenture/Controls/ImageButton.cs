using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Controls
{
    public class ImageButton : Button
    {
        public Texture2D Image  {get; set;}
    //    public Color DrawImageColor { get; set; }
        public float Scale { get; set; }
        public float Angle { get; set; }

        public ImageButton(Texture2D background,Texture2D image, Rectangle position)
            :base (background, "", null, position)
        {
            Image = image;
            DrawForegroundColor = Color.White;
            Scale = 1;
            Angle = 0;
        }

        

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, Position, DrawBackgroundColor);

            DrawImage(spriteBatch);
        }

        private Vector2 _imagePosition = new Vector2();
        private Vector2 _imageOrigin = new Vector2();

        private void DrawImage(SpriteBatch spriteBatch)
        {
            _imagePosition.X = Position.Left;
            _imagePosition.Y = Position.Top;

            _imagePosition.X += (Position.Width) / 2.0f;
            _imagePosition.Y += (Position.Height) / 2.0f;

            _imageOrigin.X = (Image.Width) / 2.0f;
            _imageOrigin.Y = (Image.Height) / 2.0f;

       
            spriteBatch.Draw(Image, _imagePosition, null, DrawForegroundColor, Angle, _imageOrigin,
               Scale, SpriteEffects.None, 0);
        }
    }
}
