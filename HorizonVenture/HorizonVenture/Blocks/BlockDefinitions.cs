using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Blocks
{
    class BlockDefinitions
    {
        private static Dictionary<string, string> _texturesPath;
        private static Dictionary<string, Texture2D> _textures;
        //    private static Dictionary<string, Color4> _colors;

        static BlockDefinitions()
        {
            _texturesPath = new Dictionary<string, string>();
            _texturesPath.Add("metal1", @"Blocks\bunker_galvanized");
            _texturesPath.Add("metal2", @"Blocks\GraniteTechno");

            _texturesPath.Add("gray1", @"Blocks\GraniteTechno");
            //_colors = new Dictionary<string, Color4>();
            _textures = new Dictionary<string, Texture2D>();
        }

        public static Texture2D getTextureById(string id, HorizonVentureGame game)
        {
            if (!_texturesPath.ContainsKey(id))
            {
                throw new KeyNotFoundException("invalid id");
            }

            string path = _texturesPath[id];

            if (!_textures.ContainsKey(path))
            {
                _textures.Add(path, game.GetContent().Load<Texture2D>(path));
            }

            return _textures[path];
        }

       

        /* private static readonly Color4 DEFAULT_COLOR = Color4.White;

       public static Color4 getColorById(string id)
       {
           if (!_colors.ContainsKey(id))
               return DEFAULT_COLOR;

           return _colors[id];
       }*/
    }
}
