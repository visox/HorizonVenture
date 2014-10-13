using HorizonVenture.HorizonVenture.Screens;
using HorizonVenture.HorizonVenture.Space;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HorizonVenture
{
    public class HorizonVentureGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Vector2 _baseScreenSize = new Vector2(1280, 680);

        MainMenuScreen _mainMenuScreen;
        InSpaceScreen _inSpaceScreen;


        HorizonVentureSpace _horizonVentureSpace;
        

        public HorizonVentureGame()
            : base()
        {

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
            };

            Window.AllowUserResizing = false;
            

            Content.RootDirectory = "Content";

   
        }

        protected override void Initialize()
        {
           
            _graphics.PreferredBackBufferWidth = (int)_baseScreenSize.X;
            _graphics.PreferredBackBufferHeight = (int)_baseScreenSize.Y;
            _graphics.ApplyChanges();

            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _mainMenuScreen = new MainMenuScreen(this);
            _mainMenuScreen.Show();

            _inSpaceScreen = new InSpaceScreen(this);


            _horizonVentureSpace = new HorizonVentureSpace(this);
            _inSpaceScreen.HorizonVentureSpace = _horizonVentureSpace;

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {

            InputManager.UpdateInput();
            AbstractScreen.CurrentScreen.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            AbstractScreen.CurrentScreen.Draw(_spriteBatch);

           
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public GraphicsDevice GetGraphicsDevice()
        {
            return GraphicsDevice;
        }

        public ContentManager GetContent()
        {
            return Content;
        }

        public Vector2 GetScreenSize()
        {
            return _baseScreenSize;
           /* return new Vector2(
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);*/
        }


        public void ShowInSpaceScreen()
        {
            _inSpaceScreen.Show();
        }

        public void QuitGame()
        {
            Exit();
        }

       
    }
}
