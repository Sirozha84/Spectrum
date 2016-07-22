using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Spectrum
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D Dysplay;
        Rectangle DysplaySorce = new Rectangle(0, 0, 640, 512); //Потом сделать регулировку видимости бордюра
        Rectangle ScreenSize;
        bool Monitor = false;

        /*int FPS = 0;
        DateTime lastTime;*/

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 960; //640;
            graphics.PreferredBackBufferHeight = 768; //512;
            ScreenSize = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Dysplay = new Texture2D(GraphicsDevice, 640, 512, false, SurfaceFormat.Color);
            Spectrum.Init();
            base.Initialize();


            Monitor form = new Monitor();
            form.Show();
            Monitor = true;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        int str = 0;
        int takt = 0;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            int BreakPoint = 4639;
            //4591 - конец теста памяти
            //4618 - LDDR

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) & !Monitor)
            {
                Monitor form = new Monitor();
                form.Show();
                Monitor = true;
            }

            if (Spectrum.Mode == Spectrum.Modes.Normal)
            {
                do
                {
                    do
                    {
                        takt += Z80.Run();
                        if (Z80.PC == BreakPoint)
                        {
                            Spectrum.Mode = Spectrum.Modes.Stop;
                            return;
                        }
                    } while (takt < 224);
                    NextString();
                } while (str < Spectrum.Strings);
                str = 0;
            }
            if (Spectrum.Mode == Spectrum.Modes.Step)
            {
                takt += Z80.Run();
                if (takt >= 224) NextString();
                if (str >= Spectrum.Strings) str = 0;
                Spectrum.Mode = Spectrum.Modes.Stop;
            }
            if (Spectrum.Mode == Spectrum.Modes.Frame)
            {
                do
                {
                    do
                    {
                        takt += Z80.Run();
                    } while (takt < 224);
                    NextString();
                } while (str < Spectrum.Strings);
                str = 0;
                Spectrum.Mode = Spectrum.Modes.Stop;
            }

            //for (int i = 0; i < Spectrum.Strings; i++)
            //Screen.DrawString(i);

            /*FPS++;
            if (lastTime.Second != DateTime.Now.Second)
            {
                lastTime = DateTime.Now;
                Window.Title = FPS.ToString("FPS: 0");
                FPS = 0;
            }*/

            base.Update(gameTime);
        }
        void NextString()
        {
            takt -= 224;
            Screen.DrawString(str++);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Dysplay.SetData(Screen.Pixels, 0, 0);
            spriteBatch.Begin();
            spriteBatch.Draw(Dysplay, ScreenSize, DysplaySorce, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
