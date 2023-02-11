using AmigaNet.Amos;
using AmigaNet.Amos.Screens;
using AmigaNet.Types.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpMod;
using SharpMod.DSP;
using SharpMod.Song;
using System.Collections.Concurrent;

namespace AmigaNet.Legion.DesktopApp
{
    public class LegionGame : Game, IGameEngine
    {
        const int Scale = 1;
        const int WorldWidth = 640;
        const int WorldHeight = 520;
        const float ScreenWidth = WorldWidth;
        const float ScreenHeight = WorldHeight;
        private readonly Matrix scale1Matrix;
        private readonly Matrix scale2Matrix;

        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private readonly FrameCounter frameCounter = new FrameCounter();
        private readonly Legion legion;

        const int MaxScreenCount = 4;
        private readonly Texture2D[] screenTextures = new Texture2D[MaxScreenCount]; // Legion uses max 4 screens
        private readonly Texture2D[] bobTextures = new Texture2D[MaxScreenCount * 64];
        private readonly Texture2D[] spriteTextures = new Texture2D[64];
        private Texture2D currentShapeTexture;

        private ConcurrentQueue<Action> actions = new ConcurrentQueue<Action>();

        private int mouseX = 0;
        private int mouseY = 0;
        private int mouseKey = 0;
        private volatile int mouseClick = 0;
        private bool leftMouseAlreadyPressed = false;
        private bool rightMouseAlreadyPressed = false;

        private int pressedKey = 0;
        private int lastKey = 0;
        private int lastInkey = 0;
        private Queue<KeyInfo> keyboardBuffer = new Queue<KeyInfo>();

        private readonly DynamicSoundEffectInstance soundEffectInstance;
        private XnaSoundRenderer soundRenderer;
        private ModulePlayer modulePlayer;
        private SongModule currentTrack;

        public LegionGame(String resourcesPath, String dataPath)
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = (int)ScreenWidth;
            _graphics.PreferredBackBufferHeight = (int)ScreenHeight;

            soundEffectInstance = new DynamicSoundEffectInstance(48000, AudioChannels.Stereo);

            scale1Matrix = Matrix.CreateScale(1);
            scale2Matrix = Matrix.CreateScale(2);

            legion = new Legion(resourcesPath, dataPath, this);
        }

        public event Action GameLoaded;

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            IsMouseVisible = true;

            GameLoaded?.Invoke();

            new Thread(() => legion.Run()).Start();
        }

        protected override void UnloadContent() { }


        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            //    Keyboard.GetState().IsKeyDown(Keys.Escape))
            //{
            //    Exit();
            //}

            var mouseState = Mouse.GetState();
            mouseX = (int)(mouseState.X / Scale);
            mouseY = (int)(mouseState.Y / Scale);
            var newMouseKey = 0;
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                newMouseKey += 1;
                if (!leftMouseAlreadyPressed)
                {
                    mouseClick = newMouseKey;
                }
                leftMouseAlreadyPressed = true;
            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                newMouseKey += 2;
                if (!rightMouseAlreadyPressed)
                {
                    mouseClick = newMouseKey;
                }
                rightMouseAlreadyPressed = true;
            }
            if (mouseState.LeftButton == ButtonState.Released)
            {
                leftMouseAlreadyPressed = false;
            }
            if (mouseState.RightButton == ButtonState.Released)
            {
                rightMouseAlreadyPressed = false;
            }
            mouseKey = newMouseKey;


            var state = Keyboard.GetState();
            var keys = state.GetPressedKeys();

            if (keys.Length > 0)
            {
                var keyCode = (int)keys[0];
                pressedKey = keyCode;

                if (keyCode != lastKey)
                {
                    lastKey = keyCode;

                    var keyInfo = new KeyInfo
                    {
                        KeyCode = keyCode,
                        CapsLockState = state.CapsLock,
                        ShiftState = state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift),
                        AltState = state.IsKeyDown(Keys.RightAlt),
                    };
                    
                    keyboardBuffer.Enqueue(keyInfo);
                }
            }
            else
            {
                pressedKey = 0;
                lastKey = 0;
            }

            base.Update(gameTime);
        }

        private void DrawFps(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCounter.Update(deltaTime);
            var fps = string.Format("FPS: {0}", frameCounter.AverageFramesPerSecond);
            Console.WriteLine(fps);
            //_spriteBatch.DrawString(spriteFont, fps, new Vector2(1, 1), Color.Black);
        }

        bool shouldCountdown = false;
        int waitMs = 100;
        protected override void Draw(GameTime gameTime)
        {
            if (shouldCountdown)
            {
                waitMs -= gameTime.ElapsedGameTime.Milliseconds;
            }

            GraphicsDevice.Clear(Color.Black);

            //DrawFps(gameTime);

            DrawScreens();
            DrawSprites();

            while (actions.TryDequeue(out var action))
            {
                action.Invoke();
            }

            base.Draw(gameTime);
        }

        private void DrawScreens()
        {
            foreach (var screen in legion.ScreensManager.Screens.ToArray())
            {
                if (screen.IsVisible)
                {
                    if (screenTextures[screen.Number] == null || screen.IsModified)
                    {
                        if (screen.IsModified) screen.IsModified = false;
                        screenTextures[screen.Number] = Texture2DFromImageData(screen.Data, screen.Width, screen.Height);
                    }

                    var matrix = screen.PixelMode < PixelMode.Hires ? scale2Matrix : scale1Matrix;

                    _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, matrix);

                    var screenX = screen.X - legion.ScreensManager.Display.X;
                    var screenY = screen.Y - legion.ScreensManager.Display.Y;

                    var screenTexture = screenTextures[screen.Number];
                    var screenSrcRect = new Rectangle(
                        0 + screen.OffsetX,
                        0 + screen.OffsetY,
                        screen.DisplayWidth,
                        screen.DisplayHeight);
                    var screenDestRect = new Rectangle(
                        screenX,
                        screenY,
                        screen.DisplayWidth,
                        screen.DisplayHeight);

                    _spriteBatch.Draw(screenTexture, screenDestRect, screenSrcRect, Color.White);

                    if (legion.ScreensManager.ShouldUpdateBobs || legion.ScreensManager.AutoUpdateBobs)
                    {
                        foreach (var bob in screen.Bobs.ToArray())
                        {
                            if (bobTextures[(screen.Number * bob.Number) + bob.Number] == null || bob.IsModified)
                            {
                                if (bob.IsModified) bob.IsModified = false;
                                bobTextures[(screen.Number * bob.Number) + bob.Number] = Texture2DFromImageData(bob.Data);
                            }

                            var bobTexture = bobTextures[(screen.Number * bob.Number) + bob.Number];
                            var bobSrcRect = new Rectangle(
                                0,
                                0,
                                bob.Data.Width,
                                bob.Data.Height);
                            var bobDestRect = new Rectangle(
                                screenX + (bob.X - bob.Data.HotspotX) - screen.OffsetX,
                                screenY + (bob.Y - bob.Data.HotspotY) - screen.OffsetY,
                                bob.Data.Width,
                                bob.Data.Height);
                            if (bobDestRect.X < screenX)
                            {
                                var diff = screenX - bobDestRect.X;
                                bobDestRect.X += diff;
                                bobSrcRect.X += diff;
                                bobDestRect.Width -= diff;
                                bobSrcRect.Width -= diff;
                            }
                            if (bobDestRect.Y < screenY)
                            {
                                var diff = screenY - bobDestRect.Y;
                                bobDestRect.Y += diff;
                                bobSrcRect.Y += diff;
                                bobDestRect.Height -= diff;
                                bobSrcRect.Height -= diff;
                            }
                            if (bobDestRect.Width > screenDestRect.Width)
                            {
                                var diff = bobDestRect.Width - screenDestRect.Width;
                                bobDestRect.Width = screenDestRect.Width;
                                bobSrcRect.Width -= diff;
                            }
                            if (bobDestRect.Height > screenDestRect.Height)
                            {
                                var diff = bobDestRect.Height - screenDestRect.Height;
                                bobDestRect.Height = screenDestRect.Height;
                                bobSrcRect.Height -= diff;
                            }

                            _spriteBatch.Draw(bobTexture, bobDestRect, bobSrcRect, Color.White);
                        }
                    }

                    if (screen.CurrentShape != null)
                    {
                        var shape = screen.CurrentShape;
                        currentShapeTexture = Texture2DFromImageData(shape.Data, shape.Width, shape.Height);

                        var shapeSrcRect = new Rectangle(
                            0,
                            0,
                            shape.Width,
                            shape.Height);
                        var shapeDestRect = new Rectangle(
                            screenX + (shape.X) - screen.OffsetX,
                            screenY + (shape.Y) - screen.OffsetY,
                            shape.Width,
                            shape.Height);

                        _spriteBatch.Draw(currentShapeTexture, shapeDestRect, shapeSrcRect, Color.White);
                    }

                    _spriteBatch.End();
                }
            }
        }

        private void DrawSprites()
        {
            for (var i = 0; i < legion.ScreensManager.Display.Sprites.Length; i++)
            {
                var sprite = legion.ScreensManager.Display.Sprites[i];
                if (sprite != null)
                {
                    if (spriteTextures[i] == null || sprite.IsModified)
                    {
                        if (sprite.IsModified) sprite.IsModified = false;
                        spriteTextures[i] = Texture2DFromImageData(sprite.Data);
                    }

                    _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, scale2Matrix);

                    var spriteSrcRect = new Rectangle(0, 0, sprite.Data.Width, sprite.Data.Height);
                    var spriteDestRect = new Rectangle(
                        sprite.X - legion.ScreensManager.Display.X,
                        sprite.Y - legion.ScreensManager.Display.Y,
                        sprite.Data.Width,
                        sprite.Data.Height);

                    _spriteBatch.Draw(spriteTextures[i], spriteDestRect, spriteSrcRect, Color.White);
                    _spriteBatch.End();
                }
            }
        }

        public void Invoke(Action action)
        {
            if (action == null) return;
            actions.Enqueue(action);
        }

        public void LoadTrack(string fileName)
        {
            if (modulePlayer != null)
            {
                modulePlayer.SoundRenderer = null;
                soundRenderer.Dispose();
            }

            currentTrack = ModuleLoader.Instance.LoadModule(fileName);
            soundRenderer = new XnaSoundRenderer(soundEffectInstance);
            modulePlayer = new ModulePlayer(currentTrack);
            modulePlayer.RegisterRenderer(soundRenderer);
            modulePlayer.DspAudioProcessor = new AudioProcessor(8192, 60);
        }

        public bool IsTrackLoop
        {
            get { return modulePlayer.PlayerInstance.mp_loop; }
            set { modulePlayer.PlayerInstance.mp_loop = value; }
        }

        public void PlayTrack()
        {
            modulePlayer.Start();
        }

        public void StopTrack()
        {
            modulePlayer.Stop();
        }

        public void HideCursor()
        {
            IsMouseVisible = false;
        }

        public void ShowCursor()
        {
            IsMouseVisible = true;
        }

        public void ChangeMouseCursor(ImageData cursorImage)
        {
            const int cursorScale = 2;
            Invoke(() =>
            {
                var originX = cursorImage.HotspotX * cursorScale;
                var originY = cursorImage.HotspotY * cursorScale;
                var texture = Texture2DFromImageDataUsingScale(cursorImage, cursorScale);
                Mouse.SetCursor(MouseCursor.FromTexture2D(texture, originX, originY));
            });
        }

        public void WaitVbl()
        {
            var resetEvent = new ManualResetEvent(false);
            Invoke(() =>
            {
                resetEvent.Set();
            });
            resetEvent.WaitOne();
            resetEvent.Reset();
        }

        public int GetKeyPressed()
        {
            return pressedKey;
        }

        public String GetInkey()
        {
            if (keyboardBuffer.Count == 0) return "";
            var keyInfo = keyboardBuffer.Dequeue();
            var key = keyInfo.KeyCode;
            lastInkey = key;
            if (keyInfo.ShiftState)
            {
                switch ((char)key)
                {
                    case '1': return "!";
                    case '2': return "@";
                    case '3': return "#";
                    case '4': return "$";
                    case '5': return "%";
                    case '6': return "^";
                    case '7': return "&";
                    case '8': return "*";
                    case '9': return "(";
                    case '0': return ")";
                    case '-': return "_";
                    case '=': return "+";
                    case '[': return "{";
                    case ']': return "}";
                    case ';': return ":";
                    case '\'': return "\"";
                    case ',': return "<";
                    case '.': return ">";
                    case '/': return "?";
                    case '`': return "~";
                }
            }

            var upcase = (keyInfo.CapsLockState && !keyInfo.ShiftState) || (!keyInfo.CapsLockState && keyInfo.ShiftState);

            if (!upcase)
            {
                if (key >= 65 && key <= 90)
                {
                    key += 32;
                }
            }

            if (keyInfo.AltState)
            {
                switch (key)
                {
                    case 'A': return "Ą";
                    case 'C': return "Ć";
                    case 'E': return "Ę";
                    case 'L': return "Ł";
                    case 'N': return "Ń";
                    case 'O': return "Ó";
                    case 'S': return "Ś";
                    case 'Z': return "Ż";
                    case 'X': return "Ź";

                    case 'a': return "ą";
                    case 'c': return "ć";
                    case 'e': return "ę";
                    case 'l': return "ł";
                    case 'n': return "ń";
                    case 'o': return "ó";
                    case 's': return "ś";
                    case 'z': return "ż";
                    case 'x': return "ź";
                }
            }

            if (key >= 32 && key <= 127)
            {
                return ((char)key).ToString();
            }

            return "";
        }

        public int GetScancode()
        {
            return lastInkey;
        }

        public void ClearKey()
        {
            lastInkey = 0;
            keyboardBuffer.Clear();
        }

        public int GetMousePosX()
        {
            return mouseX;
        }

        public int GetMousePosY()
        {
            return mouseY;
        }

        public int GetMouseKey()
        {
            return mouseKey;
        }

        public int GetMouseClick()
        {
            var result = mouseClick;
            if (mouseClick > 0)
            {
                mouseClick = 0;
            }
            return result;
        }

        private Texture2D Texture2DFromImageData(Pixel[] data, int width, int height)
        {
            return Texture2DFromImageData(new ImageData("", data, width, height));
        }

        private Texture2D Texture2DFromImageData(ImageData imageData)
        {
            var width = imageData.Width;
            var height = imageData.Height;
            var texture = new Texture2D(GraphicsDevice, width, height);
            var textureData = new Color[width * height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var pixel = imageData.Pixels[x + width * y];
                    if (pixel != null && ((pixel.Index == 0 && !imageData.UseMask) || (pixel.Index > 0))) // handling transparency
                    {
                        textureData[x + width * y] = new Color(pixel.R, pixel.G, pixel.B);
                    }
                }
            }

            texture.SetData(textureData);
            return texture;
        }

        private Texture2D Texture2DFromImageDataUsingScale(ImageData imageData, int scale)
        {
            int width = (int)(imageData.Width * scale);
            int height = (int)(imageData.Height * scale);

            var texture = new Texture2D(GraphicsDevice, width, height);
            var textureData = new Color[width * height];

            int targetIdx = 0;
            for (int i = 0; i < height; ++i)
            {
                int iUnscaled = (int)(i / scale);
                for (int j = 0; j < width; ++j)
                {
                    int jUnscaled = (int)(j / scale);
                    var pixel = imageData.Pixels[iUnscaled * imageData.Width + jUnscaled];
                    textureData[targetIdx++] = pixel.Index > 0 ? new Color(pixel.R, pixel.G, pixel.B) : new Color(pixel.R, pixel.G, pixel.B, (byte)0);
                }
            }

            texture.SetData(textureData);
            return texture;
        }

    }
}
