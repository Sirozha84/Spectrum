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
            graphics.PreferredBackBufferWidth = 640; //640; 960
            graphics.PreferredBackBufferHeight = 512; //512; 768
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

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) & !Monitor)
            {
                Monitor form = new Monitor();
                form.Show();
                Monitor = true;
            }
            Z80.IN[65278] = 255;
            Z80.IN[65022] = 255;
            Z80.IN[64510] = 255;
            Z80.IN[63486] = 255;
            Z80.IN[61438] = 255;
            Z80.IN[57342] = 255;
            Z80.IN[49150] = 255;
            Z80.IN[32766] = 255;
            if (Keyboard.GetState().IsKeyDown(Keys.D1)) { Z80.IN[63486] &= 254; }
            if (Keyboard.GetState().IsKeyDown(Keys.D2)) { Z80.IN[63486] &= 253; }
            if (Keyboard.GetState().IsKeyDown(Keys.D3)) { Z80.IN[63486] &= 251; }
            if (Keyboard.GetState().IsKeyDown(Keys.D4)) { Z80.IN[63486] &= 247; }
            if (Keyboard.GetState().IsKeyDown(Keys.D5)) { Z80.IN[63486] &= 239; }
            if (Keyboard.GetState().IsKeyDown(Keys.D6)) { Z80.IN[61438] &= 239; }
            if (Keyboard.GetState().IsKeyDown(Keys.D7)) { Z80.IN[61438] &= 247; }
            if (Keyboard.GetState().IsKeyDown(Keys.D8)) { Z80.IN[61438] &= 251; }
            if (Keyboard.GetState().IsKeyDown(Keys.D9)) { Z80.IN[61438] &= 253; }
            if (Keyboard.GetState().IsKeyDown(Keys.D0)) { Z80.IN[61438] &= 254; }
            if (Keyboard.GetState().IsKeyDown(Keys.Q)) { Z80.IN[64510] &= 254; }
            if (Keyboard.GetState().IsKeyDown(Keys.W)) { Z80.IN[64510] &= 253; }
            if (Keyboard.GetState().IsKeyDown(Keys.E)) { Z80.IN[64510] &= 251; }
            if (Keyboard.GetState().IsKeyDown(Keys.R)) { Z80.IN[64510] &= 247; }
            if (Keyboard.GetState().IsKeyDown(Keys.T)) { Z80.IN[64510] &= 239; }
            if (Keyboard.GetState().IsKeyDown(Keys.Y)) { Z80.IN[57342] &= 239; }
            if (Keyboard.GetState().IsKeyDown(Keys.U)) { Z80.IN[57342] &= 247; }
            if (Keyboard.GetState().IsKeyDown(Keys.I)) { Z80.IN[57342] &= 251; }
            if (Keyboard.GetState().IsKeyDown(Keys.O)) { Z80.IN[57342] &= 253; }
            if (Keyboard.GetState().IsKeyDown(Keys.P)) { Z80.IN[57342] &= 254; }
            if (Keyboard.GetState().IsKeyDown(Keys.A)) { Z80.IN[65022] &= 254; }
            if (Keyboard.GetState().IsKeyDown(Keys.S)) { Z80.IN[65022] &= 253; }
            if (Keyboard.GetState().IsKeyDown(Keys.D)) { Z80.IN[65022] &= 251; }
            if (Keyboard.GetState().IsKeyDown(Keys.F)) { Z80.IN[65022] &= 247; }
            if (Keyboard.GetState().IsKeyDown(Keys.G)) { Z80.IN[65022] &= 239; }
            if (Keyboard.GetState().IsKeyDown(Keys.H)) { Z80.IN[49150] &= 239; }
            if (Keyboard.GetState().IsKeyDown(Keys.J)) { Z80.IN[49150] &= 247; }
            if (Keyboard.GetState().IsKeyDown(Keys.K)) { Z80.IN[49150] &= 251; }
            if (Keyboard.GetState().IsKeyDown(Keys.L)) { Z80.IN[49150] &= 253; }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) { Z80.IN[49150] &= 254; }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) { Z80.IN[65278] &= 254; }
            if (Keyboard.GetState().IsKeyDown(Keys.Z)) { Z80.IN[65278] &= 253; }
            if (Keyboard.GetState().IsKeyDown(Keys.X)) { Z80.IN[65278] &= 251; }
            if (Keyboard.GetState().IsKeyDown(Keys.C)) { Z80.IN[65278] &= 247; }
            if (Keyboard.GetState().IsKeyDown(Keys.V)) { Z80.IN[65278] &= 239; }
            if (Keyboard.GetState().IsKeyDown(Keys.B)) { Z80.IN[32766] &= 239; }
            if (Keyboard.GetState().IsKeyDown(Keys.N)) { Z80.IN[32766] &= 247; }
            if (Keyboard.GetState().IsKeyDown(Keys.M)) { Z80.IN[32766] &= 251; }
            if (Keyboard.GetState().IsKeyDown(Keys.RightShift)) { Z80.IN[32766] &= 253; }
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) { Z80.IN[32766] &= 254; }

            if (Spectrum.Mode == Spectrum.Modes.Normal | Spectrum.Mode == Spectrum.Modes.Frame)
            {
                do
                {
                    do
                    {
                        if (Z80.PC == Spectrum.BreakPoint)
                        {
                            Spectrum.Mode = Spectrum.Modes.Stop;
                            return;
                        }
                        takt += Z80.Run();
                    } while (takt < 224);
                } while (!NextString());
                if (Spectrum.Mode == Spectrum.Modes.Frame) Spectrum.Mode = Spectrum.Modes.Stop;
            }
            if (Spectrum.Mode == Spectrum.Modes.Step)
            {
                takt += Z80.Run();
                if (takt >= 224) NextString();
                Spectrum.Mode = Spectrum.Modes.Stop;
            }
            base.Update(gameTime);
        }
        bool NextString()
        {
            takt -= 224;
            Screen.DrawString(str++);
            if (str >= Spectrum.Strings)
            {
                str = 0;
                takt += Z80.Interrupt(); //Генерируем прерывание в каждый новый кадАр
                return true;
            }
            return false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            Dysplay.SetData(Screen.Pixels, 0, 0);
            spriteBatch.Begin();
            spriteBatch.Draw(Dysplay, ScreenSize, DysplaySorce, Color.White);
            spriteBatch.Draw(Dysplay, new Rectangle(-1, 0, 640, 512), DysplaySorce, Color.FromNonPremultiplied(255, 255, 255, 128));
            spriteBatch.Draw(Dysplay, new Rectangle(1, 0, 640, 512), DysplaySorce, Color.FromNonPremultiplied(255, 255, 255, 128));
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
