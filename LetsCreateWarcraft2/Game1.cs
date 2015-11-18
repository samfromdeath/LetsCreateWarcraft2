#region Using Statements
using LetsCreateWarcraft2.Common;
using LetsCreateWarcraft2.Manager;
using LetsCreateWarcraft2.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace LetsCreateWarcraft2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private ManagerMouse _managerMouse;
        private ManagerTiles _managerTiles;
        private ManagerUnits _managerUnits;
        private Camera _camera;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _camera = new Camera();            
            _camera.ViewportWidth = graphics.GraphicsDevice.Viewport.Width;
            _camera.ViewportHeight = graphics.GraphicsDevice.Viewport.Height;

            _camera.MoveCamera(_camera.ViewportCenter);
            
            _managerMouse = new ManagerMouse(_camera);
            _managerTiles = new ManagerTiles(2000, 2000);
            _managerUnits = new ManagerUnits(_managerMouse, _managerTiles);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _managerUnits.LoadContent(Content);
            SelectRectangle.LoadContent(Content);
            _managerTiles.LoadContent(Content);
            // TODO: use this.Content to load your game content here
          //  graphics.SynchronizeWithVerticalRetrace = false;
            
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _camera.HandleInput(Keyboard.GetState(), null);

            _managerMouse.Update();
            _managerUnits.Update();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
    null, null, null, null, _camera.TranslationMatrix);

            _managerTiles.Draw(spriteBatch);
            _managerUnits.Draw(spriteBatch);
            _managerMouse.Draw(spriteBatch);

            
         
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
