using HorizonVenture.HorizonVenture.Blocks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Controls
{
    public class RightShipMaterialsPanel : Panel
    {
        protected HorizonVentureGame _game;
        private int ComponentsPanelIndex { get; set; }

        public static readonly int DEFAULT_WIDTH = 300;

        public string SelectedMaterial
        {
            get;
            private set;
        }

        public string SelectedPattern
        {
            get;
            private set;
        }

        public event MaterialClickHandler Click;
        public delegate void MaterialClickHandler(object sender, MaterialClickArgs e);


        public event PatternChangedHandler PatternChanged;
        public delegate void PatternChangedHandler(object sender, MaterialPatternChangedArgs e);

        private RadioButtons _patternRb;

        public static readonly string NO_MATERIAL = "";

        public RightShipMaterialsPanel(HorizonVentureGame game)
            : base(null, new Rectangle())
        {
            _game = game;
            _background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background0");

            Position = new Rectangle((int)(_game.GetScreenSize().X - DEFAULT_WIDTH), 0, DEFAULT_WIDTH, (int)(_game.GetScreenSize().Y));

            DrawBackgroundColor = new Color(255, 255, 255, 128);

            SelectedMaterial = NO_MATERIAL;

            ComponentsPanelIndex = 0;

            RefreshPanel();
        }

        private static readonly float MATERIAL_IMAGEBUTTON_SIZE = 50;
        private static readonly float MATERIAL_BUTTON_MARGIN = 20;
        private static readonly int MATERIAL_SHOW_COUNT = 20;

        public void RefreshPanel()
        {
            this.Controls.Clear();

            AddComponentsButtons();
            AddMaterialPatternRadio();
        }

        private void AddMaterialPatternRadio()
        {
            Texture2D background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background0");

            _patternRb = new RadioButtons(new Vector2(Position.Right - MATERIAL_IMAGEBUTTON_SIZE - (MATERIAL_BUTTON_MARGIN * 2)
                - (RadioButtons.DEFAULT_BUTTON_SIZE + (2 * RadioButtons.DEFAULT_BUTTON_MARGIN)),
                Position.Top + MATERIAL_BUTTON_MARGIN), background);

            _patternRb.ButtonChanged += MaterialPatter_Changed;

            _patternRb.DrawBackgroundColor = Color.White;
            _patternRb.DrawBackgroundButtonColor = Color.WhiteSmoke;
            _patternRb.DrawForegroundButtonColor = Color.White;
            _patternRb.DrawBackgroundSelectedButtonColor = Color.Blue;
            _patternRb.DrawForegroundSelectedButtonColor = Color.White;

           // Texture2D buttonBackground = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background1");

            Texture2D squareImg = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\square");
            ImageButton square = new ImageButton(background, squareImg, new Rectangle());
            square.Tag = "square";

            _patternRb.AddButton(square);

            Texture2D squareRotatedImg = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\squareRotated");
            ImageButton squareRotated = new ImageButton(background, squareRotatedImg, new Rectangle());
            square.Tag = "squareRotated";

            _patternRb.AddButton(squareRotated);

            this.Controls.Add(_patternRb);
        }

        private void AddComponentsButtons()
        {
            Texture2D background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background1");
            SpriteFont spriteFont = _game.GetContent().Load<SpriteFont>(@"Controls\Buttons\Fonts\Button");
            Vector2 screenSize = _game.GetScreenSize();

            for (int i = ComponentsPanelIndex; i < MATERIAL_SHOW_COUNT + ComponentsPanelIndex; i++)
            {
                if (i >= _game.PlayerShip.OwnedMaterial.Count)
                    break;

                AbstractBlock b = new Block(_game.PlayerShip.OwnedMaterial.Keys.ElementAt(i));

                Texture2D componentImage = b.getTexture(_game);// _game.PlayerShip.OwnedComponents[i].GetImage();

                ImageButton addComponentButton = new ImageButton(background, componentImage,
                    new Rectangle(
                        (int)(Position.Right - MATERIAL_BUTTON_MARGIN - MATERIAL_IMAGEBUTTON_SIZE),
                        (int)(Position.Top + MATERIAL_BUTTON_MARGIN +
                            ((MATERIAL_IMAGEBUTTON_SIZE + MATERIAL_BUTTON_MARGIN) * (i - ComponentsPanelIndex))),
                        (int)MATERIAL_IMAGEBUTTON_SIZE,
                        (int)MATERIAL_IMAGEBUTTON_SIZE));

                addComponentButton.DrawBackgroundColor = new Color(50, 255, 50, 128);

                addComponentButton.Tag = _game.PlayerShip.OwnedMaterial.Keys.ElementAt(i);

                addComponentButton.Click += MaterialButton_Click;

                this.Controls.Add(addComponentButton);
            }
        }

        void MaterialButton_Click(object sender, Button.ButtonclickArgs e)
        {
            SelectedMaterial = e.Tag;

            if (Click != null)
            {
                Click(this, new MaterialClickArgs());
            }
        }

        public class MaterialClickArgs : EventArgs
        {
            public MaterialClickArgs()
            {
            }
        }

        void MaterialPatter_Changed(object sender, HorizonVenture.Controls.RadioButtons.SelectedButtonChangedArgs e)
        {
            SelectedPattern = _patternRb.SelectedButtonTag;
            if (PatternChanged != null)
            {
                PatternChanged(this, new MaterialPatternChangedArgs());
            }
        }

        public class MaterialPatternChangedArgs : EventArgs
        {
            public MaterialPatternChangedArgs()
            {
            }
        }
    }
}
