using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Controls
{
    public class RightShipComponentsPanel : Panel
    {
        protected SpriteFont _spriteFontButtons;
        protected HorizonVentureGame _game;
        private int ComponentsPanelIndex { get; set; }

        private static readonly int DEFAULT_WIDTH = 300;
        public int SelectedComponent
        {
            get;
            private set;
        }

        public event ComponentClickHandler Click;
        public delegate void ComponentClickHandler(object sender, ComponentClickArgs e);

        public static readonly int NO_COMPONENT = -1;

        public RightShipComponentsPanel(HorizonVentureGame game)
            : base(null, new Rectangle())
        {
            _game = game;
            _background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background0");
            _spriteFontButtons = _game.GetContent().Load<SpriteFont>(@"Controls\Buttons\Fonts\Button");

            Position = new Rectangle((int)(_game.GetScreenSize().X - DEFAULT_WIDTH), 0, DEFAULT_WIDTH, (int)(_game.GetScreenSize().Y));

            DrawBackgroundColor = new Color(255, 255, 255, 128);

            SelectedComponent = NO_COMPONENT;

            ComponentsPanelIndex = 0;

            RefreshPanel();
        }

        private static readonly float COMPONENTS_IMAGEBUTTON_SIZE = 100;
        private static readonly float COMPONENTS_IMAGEBUTTON_MARGIN = 20;
        private static readonly int COMPONENTS_SHOW_COUNT = 5;

        public void RefreshPanel()
        {
            this.Controls.Clear();

            Texture2D background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background1");
            SpriteFont spriteFont = _game.GetContent().Load<SpriteFont>(@"Controls\Buttons\Fonts\Button");
            Vector2 screenSize = _game.GetScreenSize();

            for (int i = ComponentsPanelIndex; i < COMPONENTS_SHOW_COUNT + ComponentsPanelIndex; i++)
            {
                if (i >= _game.PlayerShip.OwnedComponents.Count)
                    break;

                Texture2D componentImage = _game.PlayerShip.OwnedComponents[i].GetImage();

                ImageButton addComponentButton = new ImageButton(background, componentImage,
                    new Rectangle(
                        (int)(Position.Right - COMPONENTS_IMAGEBUTTON_MARGIN - COMPONENTS_IMAGEBUTTON_SIZE),
                        (int)(Position.Top + COMPONENTS_IMAGEBUTTON_MARGIN +
                            ((COMPONENTS_IMAGEBUTTON_SIZE + COMPONENTS_IMAGEBUTTON_MARGIN) * (i - ComponentsPanelIndex))),
                        (int)COMPONENTS_IMAGEBUTTON_SIZE,
                        (int)COMPONENTS_IMAGEBUTTON_SIZE));

                addComponentButton.DrawBackgroundColor = new Color(50, 255, 50, 128);

                addComponentButton.Tag = i.ToString();

                float maxSize = Math.Max(componentImage.Height, componentImage.Width);
                float curScale = 1;

                while (maxSize * curScale > COMPONENTS_IMAGEBUTTON_SIZE)
                {
                    curScale *= 0.5f;
                }
                addComponentButton.Scale = curScale;

                addComponentButton.Click += ComponentButton_Click;

                this.Controls.Add(addComponentButton);
            }
        }

        void ComponentButton_Click(object sender, Button.ButtonclickArgs e)
        {
            SelectedComponent = int.Parse(e.Tag);

            if (Click != null)
            {
                Click(this, new ComponentClickArgs());
            }
        }

        public class ComponentClickArgs : EventArgs
        {
            public ComponentClickArgs()
            {
            }
        }
    }
}
