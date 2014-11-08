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

        public int SelectedPatternSize
        {
            get;
            private set;
        }

        public event MaterialClickHandler Click;
        public delegate void MaterialClickHandler(object sender, MaterialClickArgs e);


        public event PatternChangedHandler PatternChanged;
        public delegate void PatternChangedHandler(object sender, MaterialPatternChangedArgs e);

        public event PatternSizeChangedHandler PatternSizeChanged;
        public delegate void PatternSizeChangedHandler(object sender, PatternSizeChangedArgs e);

        private RadioButtons _patternSizeRb;

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

            SelectedPatternSize = 1;
            SelectedPattern = SQUARE_PATTERN;

            RefreshPanel();
        }

        private static readonly float MATERIAL_IMAGEBUTTON_SIZE = 50;
        private static readonly float MATERIAL_BUTTON_MARGIN = 20;
        private static readonly int MATERIAL_SHOW_COUNT = 20;

        public void RefreshPanel()
        {
            this.Controls.Clear();

            AddPatternSizeRadio();
            AddComponentsButtons();
            AddMaterialPatternRadio();
        }

        private void AddPatternSizeRadio()
        {
            Texture2D background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background0");

            _patternSizeRb = new RadioButtons(new Vector2(Position.Left,
                Position.Top + MATERIAL_BUTTON_MARGIN), background);

            _patternSizeRb.ButtonChanged += MaterialPatterSize_Changed;

            _patternSizeRb.DrawBackgroundColor = Color.White;
            _patternSizeRb.DrawBackgroundButtonColor = Color.WhiteSmoke;
            _patternSizeRb.DrawForegroundButtonColor = Color.White;
            _patternSizeRb.DrawBackgroundSelectedButtonColor = Color.Blue;
            _patternSizeRb.DrawForegroundSelectedButtonColor = Color.White;

            SpriteFont spriteFont = _game.GetContent().Load<SpriteFont>(@"Controls\Buttons\Fonts\Button");

            for (int i = 1; i <= 9; i += 2)
            {
                Button sizeBtn = new Button(background, i.ToString(), spriteFont, new Rectangle());
                sizeBtn.Tag = i.ToString();
                _patternSizeRb.AddButton(sizeBtn);
            }

            _patternSizeRb.SelectedButtonTag = SelectedPatternSize.ToString();

            this.Controls.Add(_patternSizeRb);
        }

        private void MaterialPatterSize_Changed(object sender, RadioButtons.SelectedButtonChangedArgs e)
        {
            SelectedPatternSize = int.Parse(((RadioButtons)sender).SelectedButtonTag);

            if (PatternSizeChanged != null)
            {
                PatternSizeChanged(this, new PatternSizeChangedArgs());
            }
        }

        public static readonly string SQUARE_PATTERN = "square";
        public static readonly string ROTATED_SQUARE_PATTERN = "squareRotated";

        private void AddMaterialPatternRadio()
        {
            Texture2D background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background0");

            _patternSizeRb = new RadioButtons(new Vector2(Position.Right - MATERIAL_IMAGEBUTTON_SIZE - (MATERIAL_BUTTON_MARGIN * 2)
                - (RadioButtons.DEFAULT_BUTTON_SIZE + (2 * RadioButtons.DEFAULT_BUTTON_MARGIN)),
                Position.Top + MATERIAL_BUTTON_MARGIN), background);

            _patternSizeRb.ButtonChanged += MaterialPatter_Changed;

            _patternSizeRb.DrawBackgroundColor = Color.White;
            _patternSizeRb.DrawBackgroundButtonColor = Color.WhiteSmoke;
            _patternSizeRb.DrawForegroundButtonColor = Color.White;
            _patternSizeRb.DrawBackgroundSelectedButtonColor = Color.Blue;
            _patternSizeRb.DrawForegroundSelectedButtonColor = Color.White;

            // Texture2D buttonBackground = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background1");

            Texture2D squareImg = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\square");
            ImageButton square = new ImageButton(background, squareImg, new Rectangle());
            square.Tag = SQUARE_PATTERN;

            _patternSizeRb.AddButton(square);

            Texture2D squareRotatedImg = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\squareRotated");
            ImageButton squareRotated = new ImageButton(background, squareRotatedImg, new Rectangle());
            squareRotated.Tag = ROTATED_SQUARE_PATTERN;

            _patternSizeRb.AddButton(squareRotated);

            _patternSizeRb.SelectedButtonTag = SelectedPattern;

            this.Controls.Add(_patternSizeRb);
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

                if (SelectedMaterial == NO_MATERIAL)
                    SelectedMaterial = addComponentButton.Tag;
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
            SelectedPattern = _patternSizeRb.SelectedButtonTag;
            if (PatternChanged != null)
            {
                PatternChanged(this, new MaterialPatternChangedArgs());
            }
        }



        public class PatternSizeChangedArgs : EventArgs
        {
            public PatternSizeChangedArgs()
            {
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
