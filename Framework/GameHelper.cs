using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;
using System.Threading;

namespace GalagaFramework.Framework
{
    public abstract class GameHelper : Game
    {
        protected GraphicsDeviceManager Graphics;
        protected SpriteBatch SpriteBatch;

        public GameHelper()
            : base()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
            ApplicationMemory.ScreenController = new ScreenController(Graphics, 1024, 768);
            Content.RootDirectory = "Content";
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            ApplicationMemory.ScreenController.SetResolution(Window.ClientBounds.Width, Window.ClientBounds.Height);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            TextureManager.LoadTextures(Content);
            SoundManager.LoadSounds(Content);
            VideoManager.LoadVideos(Content);
            ApplicationMemory.Font = Content.Load<SpriteFont>("Fonts/Arcade");
            LoadScene();
        }

        protected virtual void LoadScene() { }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void UnloadContent()
        {
        }

        protected sealed override void Update(GameTime gameTime)
        {
            ApplicationMemory.GameTime = gameTime;
            ApplicationMemory.ScreenController.Update();
            ArcadeControl.Update();

            UpdateScene(gameTime);
            base.Update(gameTime);
        }

        protected sealed override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            ApplicationMemory.ScreenController.Draw(SpriteBatch);
            DrawScene(gameTime);
            ApplicationMemory.ScreenController.LateDraw(SpriteBatch);
            SpriteBatch.End();
            base.Draw(gameTime);
            
        }

        protected abstract void UpdateScene(GameTime gameTime);
        protected abstract void DrawScene(GameTime gameTime);
    }
}
