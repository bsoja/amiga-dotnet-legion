using AmigaNet.Amos.MemoryBanks;
using AmigaNet.Amos.Screens.Amal;
using AmigaNet.IO.Fonts;
using AmigaNet.IO.Graphics.Iff;
using AmigaNet.Types.Graphics;

namespace AmigaNet.Amos.Screens
{
    public class ScreensManager : IScreensManager
    {
        private readonly MemoryBanksManager banks;
        private readonly IGameEngine gameEngine;
        private readonly Display display;
        private readonly List<Screen> screens;
        private FontInfo[] fonts;
        private Pixel penColor = Pixel.White;
        private Pixel paperColor = Pixel.Black;
        private int currentScreen = 0;
        private bool autoView = true;
        private bool updateBobOn = false;
        private bool updateSpriteOn = false;

        private List<AmalInfo> amalInfos = new List<AmalInfo>();
        private object amalLock = new object();

        public ScreensManager(string dataPath, MemoryBanksManager memoryBanksManager, IGameEngine gameEngine)
        {
            this.banks = memoryBanksManager;
            this.gameEngine = gameEngine;

            display = new Display();
            screens = new List<Screen>();
            screens.Add(GetDefaultScreen());

            var fontsReader = new FontsReader();
            fonts = new FontInfo[3];
            fonts[0] = fontsReader.Read(Path.Combine(dataPath, "fonts/garnet/16"));
            fonts[1] = fontsReader.Read(Path.Combine(dataPath, "fonts/defender2/8"));
            fonts[2] = fontsReader.Read(Path.Combine(dataPath, "fonts/Bodacious/8C.Bodacious-Normal42"));
        }

        public List<Screen> Screens => screens;

        public Display Display => display;

        public bool UpdateDisplayRequested { get; set; }

        public bool ShouldUpdateBobs { get; set; }

        public bool AutoUpdateBobs => updateBobOn;

        private void UpdateDisplay(bool force = false)
        {
            if (!force && !autoView) return;
            UpdateDisplayRequested = true;
        }

        private Screen GetScreen(int number)
        {
            if (number < 0) number = currentScreen;
            return screens.FirstOrDefault(s => s.Number == number);
        }

        private Screen GetCurrentScreen()
        {
            return screens.First(s => s.Number == currentScreen);
        }


        /// <summary>
        /// AUTO VIEW ON
        /// AUTO VIEW OFF
        /// instructions: toggle viewing mode on and off
        /// </summary>
        public void AutoViewOn() { autoView = true; }

        /// <summary>
        /// AUTO VIEW ON
        /// AUTO VIEW OFF
        /// instructions: toggle viewing mode on and off
        /// </summary>
        public void AutoViewOff() { autoView = false; }

        /// <summary>
        /// VIEW
        /// instruction: display current screen setting
        /// </summary>
        public void View()
        {
            UpdateDisplay(true);
        }

        public void WaitVbl()
        {
            gameEngine.WaitVbl();
        }

        /// <summary>
        /// CLS
        /// instruction: clear current screen
        /// Cls
        /// Cls colour number
        /// Cls colour number, x1, y1 To x2, y2
        /// </summary>
        public void Cls(int colorNumber = -1)
        {
            var screen = GetCurrentScreen();
            var color = paperColor;
            if (colorNumber >= 0)
            {
                color = screen.Palette[colorNumber];
            }
            Array.Fill(screen.Data, color);
            screen.IsModified = true;
            UpdateDisplay();
        }

        /// <summary>
        /// SCREEN
        /// instruction: set current screen
        /// </summary>
        public void Screen(int number)
        {
            currentScreen = number;
        }

        /// <summary>
        /// SCREEN
        /// function: give current screen number
        /// screen number = Screen
        /// </summary>
        public int Screen()
        {
            return GetCurrentScreen().Number;
        }

        /// <summary>
        /// SCREEN DISPLAY
        /// instruction: position a screen
        /// </summary>
        public void ScreenDisplay(int number, int x, int y, int width, int height)
        {
            var screen = GetScreen(number);
            if (x >= 0) screen.X = x;
            if (y >= 0) screen.Y = y;
            if (width >= 0) screen.DisplayWidth = width;
            if (height >= 0) screen.DisplayHeight = height;
            currentScreen = number;
            screen.IsModified = true;
            UpdateDisplay();
        }

        /// <summary>
        /// SCREEN OFFSET
        /// instruction: offset screen at hardware coordinates
        /// </summary>
        public void ScreenOffset(int number, int x, int y)
        {
            var screen = GetScreen(number);
            screen.OffsetX = x;
            screen.OffsetY = y;
            UpdateDisplay();
        }

        /// <summary>
        /// SCREEN OPEN
        /// instruction: open a new screen
        /// </summary>
        public void ScreenOpen(int number, int width, int height, int colors, PixelMode mode)
        {
            var screen = GetScreen(number);
            if (screen != null) screens.Remove(screen);

            screen = GetDefaultScreen();
            screen.Number = number;
            screen.Width = width;
            screen.DisplayWidth = width;
            screen.Height = height;
            screen.DisplayHeight = height;
            screen.Colors = colors;
            screen.PixelMode = mode;
            screen.Data = new Pixel[width * height];
            screen.Palette = DefaultPalette;//LoadPalette("default");

            Array.Fill(screen.Data, Pixel.Black);

            screens.Add(screen);
            currentScreen = number;
            screen.IsModified = true;
            UpdateDisplay();
        }

        /// <summary>
        /// SCREEN CLOSE
        /// instruction: erase a screen
        /// </summary>
        public void ScreenClose(int number)
        {
            var screen = GetScreen(number);
            if (screen != null) screens.Remove(screen);
            if (number == currentScreen) currentScreen = 0;
            UpdateDisplay();
        }

        /// <summary>
        /// SCREEN HIDE
        /// instruction: hide a screen
        /// </summary>
        public void ScreenHide(int number = -1)
        {
            var screen = GetScreen(number);
            screen.IsVisible = false;
            UpdateDisplay();
        }

        /// <summary>
        /// SCREEN SHOW
        /// instruction: show a screen
        /// </summary>
        public void ScreenShow(int number = -1)
        {
            var screen = GetScreen(number);
            screen.IsVisible = true;
            UpdateDisplay();
        }

        /// <summary>
        /// SCREEN TO FRONT
        /// instruction: move screen to front of display
        /// </summary>
        public void ScreenToFront(int number = -1)
        {
            var screen = GetScreen(number);
            if (screen != null) screens.Remove(screen);
            screens.Add(screen);
            currentScreen = screen.Number;
            UpdateDisplay();
        }

        /// <summary>
        /// SCREEN TO BACK
        /// instruction: move screen to back of display
        /// </summary>
        public void ScreenToBack(int number = -1)
        {
            var screen = GetScreen(number);
            if (screen != null) screens.Remove(screen);
            screens.Insert(0, screen);
            UpdateDisplay();
        }

        /// <summary>
        /// LOAD IFF
        /// instruction: load an IFF screen from disc
        /// </summary>
        public void LoadIff(string fileName, int number = -1)
        {
            var screen = GetScreen(number);
            if (screen == null)
            {
                screen = GetDefaultScreen();
                screen.Number = number;
                screens.Add(screen);
            }

            var iffReader = new IffImagesReader();
            var imagesContainer = iffReader.Read(fileName);
            var imageData = imagesContainer.Images[0];
            imageData.UseMask = false;
            screen.Palette = imagesContainer.Palette;

            if (imageData.Width > screen.Width || imageData.Height > screen.Height)
            {
                //TODO: check transparency here
                screen.Data = imageData.Pixels;
                screen.Width = imageData.Width;
                screen.DisplayWidth = imageData.Width;
                screen.Height = imageData.Height;
                screen.DisplayHeight = imageData.Height;
            }
            else
            {
                PutDataIntoScreen(imageData, 0, 0);
            }

            screen.IsModified = true;
            UpdateDisplay();
        }

        /// <summary>
        /// SPRITE
        /// instruction: display a Sprite on the screen
        /// </summary>
        public void Sprite(int spriteNumber, int hx, int hy, int imageNumber = -1)
        {
            Sprite sprite = null;

            if (imageNumber >= 0)
            {
                var imageData = banks.Bank1.Data[imageNumber - 1];
                sprite = new Sprite();
                sprite.Data = imageData;
                display.Sprites[spriteNumber] = sprite;
                sprite.IsModified = true;
            }
            else
            {
                sprite = display.Sprites[spriteNumber];
            }

            sprite.X = hx - sprite.Data.HotspotX;
            sprite.Y = hy - sprite.Data.HotspotY;

            if (updateSpriteOn)
            {
                UpdateDisplay(true);
            }
        }

        /// <summary>
        /// SPRITE OFF
        /// instruction: remove Sprites from screen
        /// </summary>
        public void SpriteOff(int number = -1)
        {
            if (number < 0)
            {
                for (var i = 0; i < display.Sprites.Length; i++)
                {
                    display.Sprites[i] = null;
                }
            }
            else
            {
                display.Sprites[number] = null;
            }
        }

        /// <summary>
        /// GET SPRITE
        /// instruction: grab screen image into the Object Bank
        /// Get Sprite image number,x1,y1 To x2,y2
        /// Get Sprite screen number,image number, x1, y1 To x2, y2
        /// </summary>
        public void GetSprite(int imageNumber, int x1, int y1, int x2, int y2)
        {
            var screen = GetCurrentScreen();

            var width = x2 - x1;
            var height = y2 - y1;
            var imageData = new ImageData("Sprite "+ imageNumber, width, height);

            for (var sx = 0; sx < width; sx++)
            {
                for (var sy = 0; sy < height; sy++)
                {
                    var pixelX = sx + x1;
                    var pixelY = sy + y1;
                    var pixel = screen.Data[pixelX + screen.Width * pixelY];
                    if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0) continue;
                    imageData.Pixels[sx + width * sy] = pixel;
                }
            }

            //banks.Bank1.Data[imageNumber - 1] = imageData;
            banks.Set(1, imageNumber, imageData);
        }

        /// <summary>
        /// PASTE BOB
        /// instruction: draw an image from Object Bank
        /// </summary>
        public void PasteBob(int x, int y, int imageNumber)
        {
            var imageData = banks.Bank1.Data[imageNumber - 1];
            PutDataIntoScreen(imageData, x, y);
        }

        /// <summary>
        /// DEL BOB
        /// instruction: delete an image from the Object Bank
        /// Del Bob number
        /// Del Bob first To last
        /// </summary>
        public void DelBob(int number)
        {
            var imageData = banks.Bank1.Data[number - 1];
            banks.Bank1.Data.Remove(imageData);
        }

        /// <summary>
        /// BOB
        /// instruction: display a Bob on screen
        /// Bob number,image
        /// Bob number,x,y,image
        /// </summary>
        public void Bob(int bobNumber, int x, int y, int imageNumber)
        {
            var screen = GetCurrentScreen();

            var imageData = banks.Bank1.Data[imageNumber - 1];

            var bob = screen.Bobs.FirstOrDefault(b => b.Number == bobNumber);
            if (bob == null)
            {
                bob = new Bob { Number = bobNumber };
                screen.Bobs.Add(bob);
            }
            if (x >= 0) bob.X = x;
            if (y >= 0) bob.Y = y;
            bob.Data = imageData;

            bob.IsModified = true;

            if (updateBobOn)
            {
                UpdateDisplay(true);
            }
        }

        /// <summary>
        /// BOB OFF
        /// instruction: remove a Bob from display
        /// Bob Off
        /// Bob Off number
        /// </summary>
        public void BobOff(int bobNumber)
        {
            var screen = GetCurrentScreen();
            var bob = screen.Bobs.FirstOrDefault(b => b.Number == bobNumber);
            screen.Bobs.Remove(bob);
            if (updateBobOn)
            {
                UpdateDisplay(true);
            }
        }

        /// <summary>
        /// BOB UPDATE
        /// instruction: move many Bobs simultaneously
        /// Bob Update
        /// Bob Update Off
        /// Bob Update On
        /// </summary>
        public void BobUpdate()
        {
            ShouldUpdateBobs = true;
            UpdateDisplay(true);
        }

        public void BobUpdateOn()
        {
            updateBobOn = true;
        }

        public void BobUpdateOff()
        {
            updateBobOn = false;
        }

        public void SpriteUpdateOn()
        {
            updateSpriteOn = true;
        }

        public void SpriteUpdateOff()
        {
            updateSpriteOn = false;
        }

        /// <summary>
        /// HIDE
        /// instruction: remove the mouse pointer from the screen
        /// Hide
        /// Hide On
        /// </summary>
        public void HideOn()
        {
            gameEngine.HideCursor();
        }

        /// <summary>
        /// SHOW
        /// instruction: reveal the mouse pointer back on screen
        /// Show
        /// Show On
        /// </summary>
        public void ShowOn()
        {
            gameEngine.ShowCursor();
        }

        /// <summary>
        /// CHANGE MOUSE
        /// instruction: change the shape of the mouse pointer
        /// Number Shape of mouse cursor
        /// 1 Arrow pointer(default shape)
        /// 2 Cross-hair
        /// 3 Clock
        /// >= 4 = shape 1, 2, 3, ...
        /// </summary>
        /// <param name="number"></param>
        public void ChangeMouse(int number)
        {
            if (number > 3)
            {
                var mouseImageData = banks.Bank1.Data[number - 1 - 3];
                gameEngine.ChangeMouseCursor(mouseImageData);
            }
        }

        /// <summary>
        /// POINT
        /// function: return the colour of a point
        /// c = Point(x, y)
        /// </summary>
        public int Point(int x, int y)
        {
            var screen = GetCurrentScreen();

            var pixel = screen.Data[x + y * screen.Width];

            for(var i = 0; i < screen.Palette.Length; i++)
            {
                var p = screen.Palette[i];
                if (p.R == pixel.R && p.G == pixel.G && p.B == pixel.B)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// X SCREEN
        /// function: convert hardware x-coordinate to screen x-coordinate
        /// </summary>
        public int XScreen(int xcoordinate)
        {
            var screen = GetCurrentScreen();
            return (xcoordinate - screen.X) + screen.OffsetX;
        }

        /// <summary>
        /// Y SCREEN
        /// function: convert hardware y-coordinate to screen y-coordinate
        /// </summary>
        public int YScreen(int ycoordinate)
        {
            var screen = GetCurrentScreen();
            return (ycoordinate - screen.Y) + screen.OffsetY;
        }

        /// <summary>
        /// RESERVE ZONE
        /// RESERVE memory for a detection zone
        /// </summary>
        public void ReserveZone(int number = -1)
        {
            var screen = GetCurrentScreen();
            if (number == -1)
            {
                screen.Zones = null;
            }
            else
            {
                screen.Zones = new Zone[number];
            }
        }

        /// <summary>
        /// RESET ZONE
        /// instruction: erase screen zone
        /// </summary>
        public void ResetZone(int number = -1)
        {
            var screen = GetCurrentScreen();
            if (number == -1)
            {
                for (var i = 0; i < screen.Zones.Length; i++)
                {
                    screen.Zones[i] = null;
                }
            }
            else
            {
                var zone = screen.Zones.FirstOrDefault(z => z?.Number == number);
                if (zone == null)
                {
                    zone = new Zone() { Number = number };
                    screen.Zones[number] = zone;
                }
                zone.X1 = 0;
                zone.Y1 = 0;
                zone.X2 = 0;
                zone.Y2 = 0;
            }
        }

        /// <summary>
        /// SET ZONE
        /// set a screen zone for testing
        /// </summary>
        public void SetZone(int number, int x1, int y1, int x2, int y2)
        {
            var screen = GetCurrentScreen();
            screen.Zones[number] = new Zone { Number = number, X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 };
        }

        /// <summary>
        /// ZONE
        /// return the zone number under specified screen coordinates
        /// </summary>
        public int Zone(int x, int y)
        {
            var screen = GetCurrentScreen();
            foreach (var zone in screen.Zones)
            {
                if (zone == null) continue;
                if (x >= zone.X1 && x <= zone.X2 && y >= zone.Y1 && y <= zone.Y2)
                {
                    return zone.Number;
                }
            }
            return 0;
        }

        /// <summary>
        /// MOUSE ZONE
        /// function: check if the mouse pointer is in a zone
        /// zone number=Mouse Zone
        /// </summary>
        public int MouseZone()
        {
            var screen = GetCurrentScreen();

            var x = XMouse();
            var y = YMouse();

            x += screen.OffsetX - screen.X;
            y += screen.OffsetY - screen.Y;

            return Zone(x, y);
        }

        /// <summary>
        /// NO MASK
        /// instruction: remove colour zero mask from Bob
        /// No Mask number
        /// </summary>
        public void NoMask(int number)
        {
            var imageData = banks.Bank1.Data[number - 1];
            imageData.UseMask = false;
        }

        /// <summary>
        /// MAKE MASK
        /// instruction: mask an image for collision detection
        /// Make Mask
        /// Make Mask number
        /// </summary>
        public void MakeMask(int number)
        {
            var imageData = banks.Bank1.Data[number - 1];
            imageData.UseMask = true;
        }

        /// <summary>
        /// GET BLOCK
        /// instruction: grab a screen Block into memory
        /// Get Block number,x,y,width,height
        /// Get Block number, x, y, width, height, mask
        /// </summary>
        public void GetBlock(int number, int x, int y, int width, int height, int mask = -1)
        {
            if (x < 0) x = 0;
            if (y < 0) y = 0;

            var screen = GetCurrentScreen();
            var block = screen.Blocks.FirstOrDefault(b => b.Number == number);
            if (block == null)
            {
                block = new Block { Number = number, X = x, Y = y };
                screen.Blocks.Add(block);
            }

            block.Data = new ImageData("Block " + number, width, height);

            for (var sx = 0; sx < width; sx++)
            {
                for (var sy = 0; sy < height; sy++)
                {
                    var pixelX = sx + x;
                    var pixelY = sy + y;
                    var pixel = screen.Data[pixelX + screen.Width * pixelY];
                    //screen.Pixels[pixelX + screen.Width * pixelY] = Pixel.Black;
                    if (!(mask == 1 && pixel.Index == 0))
                    {
                        block.Data.Pixels[sx + width * sy] = pixel;
                    }
                }
            }

            UpdateDisplay(true); //do we really have to force it?
        }

        /// <summary>
        /// PUT BLOCK
        /// instruction: copy Block onto screen
        /// Put Block number
        /// Put Block number, x, y
        /// Put Block number,x,y,bit-planes
        /// Put Block number, x, y, bit-planes,blitter mode
        /// </summary>
        public void PutBlock(int number, int x = -1, int y = -1)
        {
            var screen = GetCurrentScreen();
            var block = screen.Blocks.FirstOrDefault(b => b.Number == number);

            if (x < 0) x = block.X;
            if (y < 0) y = block.Y;

            for (var sx = 0; sx < block.Data.Width; sx++)
            {
                for (var sy = 0; sy < block.Data.Height; sy++)
                {
                    var pixelX = sx + x;
                    var pixelY = sy + y;
                    screen.Data[pixelX + screen.Width * pixelY] = block.Data.Pixels[sx + block.Data.Width * sy];
                }
            }

            screen.IsModified = true;
            UpdateDisplay(true); //do we really have to force it?
        }

        /// <summary>
        /// DEL BLOCK
        /// instruction: delete a screen Block
        /// Del Block
        /// Del Block number
        /// </summary>
        public void DelBlock(int number = -1)
        {
            var screen = GetCurrentScreen();
            
            if (number < 0)
            {
                screen.Blocks.Clear();
            }
            else
            {
                var block = screen.Blocks.FirstOrDefault(b => b.Number == number);
                screen.Blocks.Remove(block);
            }
        }


        /// <summary>
        /// GR WRITING
        /// instruction: change graphic writing mode
        /// Gr Writing bitpattern
        /// 
        /// Bit 0 = 0 only draw graphics using the current ink colour.
        /// Bit 0 = 1 replace any existing graphics with new graphics(default condition).
        /// Bit 1 = 1 change old graphics that overlap with new graphics, using XOR.
        /// Bit 2 = 1 reverse ink and paper colours, creating inverse video effect.
        /// </summary>
        public void GrWriting(int mode)
        {
            var screen = GetCurrentScreen();
            screen.CurrentGraphicWritingMode = mode;
            screen.CurrentShape = null;
            UpdateDisplay(true);
        }

        /// <summary>
        /// INK
        /// instruction: set drawing colour
        /// number - number of the colour register
        /// Ink number
        /// Ink number, pattern, border
        /// 
        /// Ink 3: Rem Set ink colour
        /// ink ,,5: Rem Set border outline only
        /// Ink 0,8,2: Rem Set ink, fill colour and border
        /// Ink 6,13: Rem Set ink and background fill colour
        /// </summary>
        public void Ink(int penColorNumber, int paperColorNumber = -1)
        {
            var screen = GetCurrentScreen();
            penColor = screen.Palette[penColorNumber];
            if (paperColorNumber >= 0)
            {
                paperColor = screen.Palette[paperColorNumber];
            }
        }

        /// <summary>
        /// BOX
        /// instruction: draw a rectangular outline
        /// </summary>
        public void Box(int x1, int y1, int x2, int y2)
        {
            var screen = GetCurrentScreen();

            var width = Math.Abs(x2 - x1) + 1;
            var height = Math.Abs(y2 - y1) + 1;
            var shape = new Shape
            {
                X = x1,
                Y = y1,
                Width = width,
                Height = height,
                Data = new Pixel[width * height]
            };

            for (var sx = 0; sx < width; sx++)
            {
                for (var sy = 0; sy < height; sy++)
                {
                    if (sy == 0 || sy == height - 1 || sx == 0 || sx == width - 1)
                    {
                        PutPixel(shape, penColor, sx, sy);
                    }
                }
            }

            PutDataIntoScreen(shape);

            UpdateDisplay(true); //do we really have to force it?
        }

        /// <summary>
        /// BAR
        /// instruction: draw a filled rectangle
        /// Bar x1, y1 To x2, y2
        /// </summary>
        public void Bar(int x1, int y1, int x2, int y2)
        {
            var screen = GetCurrentScreen();

            var width = Math.Abs(x2 - x1) + 1;
            var height = Math.Abs(y2 - y1) + 1;
            var shape = new Shape
            {
                X = x1,
                Y = y1,
                Width = width,
                Height = height,
                Data = new Pixel[width * height]
            };

            for (var sx = 0; sx < width; sx++)
            {
                for (var sy = 0; sy < height; sy++)
                {
                    PutPixel(shape, penColor, sx, sy);
                }
            }

            PutDataIntoScreen(shape);

            UpdateDisplay(true); //do we really have to force it?
        }

        /// <summary>
        /// DRAW
        /// instruction: draw a line
        /// Draw x1 ,y1 To x2,y2
        /// Draw To x, y
        /// </summary>
        public void Draw(int x1, int y1, int x2, int y2)
        {
            var screen = GetCurrentScreen();

            var width = Math.Max(x1, x2) + 1;
            var height = Math.Max(y1, y2) + 1;
            var shape = new Shape
            {
                X = 0,
                Y = 0,
                Width = width,
                Height = height,
                Data = new Pixel[width * height]
            };

            var data = new ImageData("Shape line", shape.Data, width, height);
            Drawing.PlotLine(data, penColor, x1, y1, x2, y2);

            PutDataIntoScreen(shape);

            UpdateDisplay(true); //do we really have to force it?
        }

        /// <summary>
        /// POLYLINE
        /// instruction: draw multiple line
        /// Polyline x1 ,y1 To x2,y2 To x3,y3
        /// Polyline To x1, y1 To x2, y2
        /// </summary>
        public void Polyline(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            Draw(x1, y1, x2, y2);
            Draw(x2, y2, x3, y3);
        }

        /// <summary>
        /// PLOT
        /// instruction: plot a single point
        /// Plot x, y
        /// Plot x, y, colour
        /// </summary>
        public void Plot(int x, int y, int colour = -1)
        {
            if (x < 0 || y < 0) return;

            var screen = GetCurrentScreen();

            Pixel pixel = null;
            if (colour >= 0)
            {
                pixel = screen.Palette[colour];
            }
            else
            {
                pixel = penColor;
            }

            var idx = x + screen.Width * y;
            if (idx < screen.Data.Length)
            {
                screen.Data[x + screen.Width * y] = pixel;
            }
        }

        /// <summary>
        /// SET FONT
        /// instruction: select font for use by Text command
        /// Set Font font number
        /// </summary>
        public void SetFont(int fontNumber)
        {
            var screen = GetCurrentScreen();
            screen.CurrentFontNr = fontNumber - 1;
        }

        /// <summary>
        /// TEXT
        /// instruction: print graphical text
        /// Text x,y,text$
        /// </summary>
        public void Text(int x, int y, String text)
        {
            if (String.IsNullOrEmpty(text)) return;

            var screen = GetCurrentScreen();
            var font = fonts[screen.CurrentFontNr];
            var cx = x;
            var cy = y - font.Baseline;
            foreach (var c in text)
            {
                int ch = GetCharCode(c);
                if (ch == 0) //space
                {
                    cx += 4;
                    continue;
                }
                var charData = font.GetCharData((char)ch);
                if (charData == null) continue;

                PutDataIntoScreen(charData, cx, cy, Pixel.White, penColor);
                cx += charData.Width + 1;
                if (ch == 0)
                {
                    cx += charData.Width + 1;
                }
            }
            UpdateDisplay(true);
        }

        /// <summary>
        /// TEXT BASE
        /// function: return the text base of the current character set
        /// baseline = Text base
        /// </summary>
        public int TextBase()
        {
            var screen = GetCurrentScreen();
            var font = fonts[screen.CurrentFontNr];
            return font.Baseline;
        }

        int GetCharCode(char c)
        {
            switch (c)
            {
                case 'Ą': return 162; 
                case 'Ć': return 170; 
                case 'Ę': return 171; 
                case 'Ł': return 174; 
                case 'Ń': return 175;
                case 'Ó': return 179; 
                case 'Ś': return 180; 
                case 'Ź': return 186; 
                case 'Ż': return 187; 
                case 'ą': return 194; 
                case 'ć': return 202; 
                case 'ę': return 203; 
                case 'ł': return 206; 
                case 'ń': return 207; 
                case 'ó': return 211; 
                case 'ś': return 212; 
                case 'ź': return 218; 
                case 'ż': return 219;
            }

            var intVal = (int)c;

            if (intVal == 139 || intVal == 155) return intVal; // up/down arrows
            
            return c - 32;
        }

        /// <summary>
        /// TEXT LENGTH
        /// function: return the length of a section of graphical text
        /// width=Text Length(text$)
        /// </summary>
        public int TextLength(String text)
        {
            var screen = GetCurrentScreen();
            var width = 0;
            var font = fonts[screen.CurrentFontNr];
            foreach (var c in text)
            {
                int ch = GetCharCode(c);
                if (ch == 0) //space
                {
                    width += 4;
                    continue;
                }
                var charWidth = font.GetCharWidth((char)ch);
                width += charWidth + 1;
                if (ch == 0)
                {
                    width += charWidth + 1;
                }
            }
            return width;
        }

        /// <summary>
        /// HOT SPOT
        /// instruction: set reference point for all coordinate calculations
        /// Hot Spot image number,x,y
        /// </summary>
        public void HotSpot(int imageNumber, int x, int y)
        {
            var imageData = banks.Bank1.Data[imageNumber - 1];
            imageData.HotspotX = x;
            imageData.HotspotY = y;
        }

        /// <summary>
        /// HOT SPOT
        /// instruction: set reference point for all coordinate calculations
        /// Hot Spot image number,pre-set value
        /// </summary>
        public void HotSpot(int imageNumber, int presetValue)
        {
            var imageData = banks.Bank1.Data[imageNumber - 1];
            switch (presetValue)
            {
                case 0: imageData.HotspotX = 0; imageData.HotspotY = 0; break;
                case 1: imageData.HotspotX = 0; imageData.HotspotY = imageData.Height / 2; break;
                case 2: imageData.HotspotX = 0; imageData.HotspotY = imageData.Height; break;
                case 10: imageData.HotspotX = imageData.Width / 2; imageData.HotspotY = 0; break;
                case 11: imageData.HotspotX = imageData.Width / 2; imageData.HotspotY = imageData.Height / 2; break;
                case 12: imageData.HotspotX = imageData.Width / 2; imageData.HotspotY = imageData.Height; break;
                case 20: imageData.HotspotX = imageData.Width; imageData.HotspotY = 0; break;
                case 21: imageData.HotspotX = imageData.Width; imageData.HotspotY = imageData.Height / 2; break;
                case 22: imageData.HotspotX = imageData.Width; imageData.HotspotY = imageData.Height; break;
            }
        }

        /// <summary>
        /// COLOUR
        /// function: read the colour assignment
        /// c=Colour(index)
        /// </summary>
        /// <returns></returns>
        public Pixel Colour(int index)
        {
            var screen = GetCurrentScreen();
            return screen.Palette[index];
        }

        /// <summary>
        /// COLOUR
        /// instruction: assign a colour to an index
        /// Colour number,$RGB
        /// </summary>
        public void Colour(int number, byte R, byte G, byte B)
        {
            var screen = GetCurrentScreen();
            if (number >= screen.Palette.Length)
            {
                var tempArr = new Pixel[screen.Palette.Length * 2];
                Array.Copy(screen.Palette, tempArr, screen.Palette.Length);
                screen.Palette = tempArr;
            }
            screen.Palette[number] = new Pixel(R, G, B, number);
        }

        /// <summary>
        /// GET PALETTE
        /// instruction: copy palette from a screen
        /// Get Palette number
        /// Get Palette number,mask
        /// </summary>
        public void GetPalette(int screenNumber)
        {
            var screenSrc = GetScreen(screenNumber);
            var screenDest = GetCurrentScreen();
            screenDest.Palette = new Pixel[screenSrc.Palette.Length];
            Array.Copy(screenSrc.Palette, screenDest.Palette, screenSrc.Palette.Length);
        }

        /// <summary>
        /// GET BOB PALETTE
        /// instruction: load image colours into current screen
        /// Get Bob Palette
        /// Get Bob Palette mask
        /// </summary>
        public void GetBobPalette()
        {
            var screen = GetCurrentScreen();
            screen.Palette = banks.BobPalette;
        }

        private void PutPixel(IGraphicElement element, Pixel pixel, int x, int y)
        {
            if (x < 0 || y < 0 || x >= element.Width || y >= element.Height) return;
            element.Data[x + element.Width * y] = pixel;
        }

        private void PutDataIntoScreen(IGraphicElement element)
        {
            var screen = GetCurrentScreen();

            if (screen.CurrentGraphicWritingMode > 1)
            {
                screen.CurrentShape = element;
                element.IsModified = true;
            }
            else
            {
                for (var bx = 0; bx < element.Width; bx++)
                {
                    for (var by = 0; by < element.Height; by++)
                    {
                        var screenPosX = element.X + bx;
                        var screenPosY = element.Y + by;
                        if (screenPosX >= 0 && screenPosX < screen.Width &&
                            screenPosY >= 0 && screenPosY < screen.Height)
                        {
                            var pixel = element.Data[bx + element.Width * by];
                            if (pixel != null)//TODO: is it needed here?: && pixel.Index > 0)
                            {
                                screen.Data[screenPosX + screen.Width * screenPosY] = pixel;
                            }
                        }
                    }
                }
                screen.IsModified = true;
            }
        }

        private void PutDataIntoScreen(ImageData imageData, int x, int y)
        {
            var screen = GetCurrentScreen();

            for (var bx = 0; bx < imageData.Width; bx++)
            {
                for (var by = 0; by < imageData.Height; by++)
                {
                    var screenPosX = x + bx;
                    var screenPosY = y + by;
                    if (screenPosX >= 0 && screenPosX < screen.Width &&
                        screenPosY >= 0 && screenPosY < screen.Height)
                    {
                        var pixel = imageData.Pixels[bx + imageData.Width * by];
                        if (pixel != null && ((pixel.Index == 0 && !imageData.UseMask) || (pixel.Index > 0))) // handling transparency
                        {
                            screen.Data[screenPosX + screen.Width * screenPosY] = pixel;
                        }
                    }
                }
            }

            screen.IsModified = true;
        }

        private void PutDataIntoScreen(ImageData imageData, int x, int y, Pixel filterPixel, Pixel replacePixel)
        {
            var screen = GetCurrentScreen();

            for (var bx = 0; bx < imageData.Width; bx++)
            {
                for (var by = 0; by < imageData.Height; by++)
                {
                    var screenPosX = x + bx - imageData.HotspotX;
                    var screenPosY = y + by - imageData.HotspotY;
                    var pixel = imageData.Pixels[bx + imageData.Width * by];
                    //if (pixel.A == filterPixel.A &&
                    if (pixel.Index == filterPixel.Index &&
                        pixel.R == filterPixel.R &&
                        pixel.G == filterPixel.G &&
                        pixel.B == filterPixel.B)
                    {
                        screen.Data[screenPosX + screen.Width * screenPosY] = replacePixel;
                    }
                }
            }

            screen.IsModified = true;
        }

        /// <summary>
        /// DEFAULT
        /// instruction: re-set to the default screen
        /// </summary>
        public void Default()
        {
            screens.Clear();
            var screen = GetDefaultScreen();
            screens.Add(screen);
            currentScreen = 0;

            UpdateDisplay();
        }

        private Screen GetDefaultScreen()
        {
            return new Screen
            {
                Number = 0,
                Width = Amos.Screens.Screen.DEFAULT_WIDTH,
                Height = Amos.Screens.Screen.DEFAULT_HEIGHT,
                Data = new Pixel[320 * 200],
                Colors = 16,
                PixelMode = PixelMode.Lowres,
                X = Amos.Screens.Screen.DEFAULT_X,
                Y = Amos.Screens.Screen.DEFAULT_Y,
                OffsetX = 0,
                OffsetY = 0,
                DisplayWidth = Amos.Screens.Screen.DEFAULT_WIDTH,
                DisplayHeight = Amos.Screens.Screen.DEFAULT_HEIGHT,
                Palette = DefaultPalette//LoadPalette("default")
            };
        }


        public void AmalRun(int channelNumber, int bobNumber, List<AmalInstruction> instructions)
        {
            var screen = GetCurrentScreen();
            var bob = screen.Bobs.FirstOrDefault(b => b.Number == bobNumber);

            var info = new AmalInfo
            {
                Number = channelNumber,
                Bob = bob,
                Instructions = instructions
            };

            lock (amalLock)
            {
                var exAmal = amalInfos.FirstOrDefault(a => a.Number == channelNumber);
                if (exAmal != null) amalInfos.Remove(exAmal);
                amalInfos.Add(info);
            }

            var amalThread = new Thread(t => { AmalThreadRun(channelNumber); });
            amalThread.Start();
        }

        public void AmalRun(Sprite sprite, List<AmalInstruction> instructions)
        {

        }

        private void AmalThreadRun(int channelNumber)
        {
            int idx = 0;
            while (true)
            {
                AmalInfo info = null;
                lock (amalLock)
                {
                    info = amalInfos.FirstOrDefault(i => i.Number == channelNumber);
                }
                if (info == null)
                {
                    break;
                }

                var instr = info.Instructions[idx];
                if (instr is AmalMove)
                {
                    var move = (AmalMove)instr;
                    var hstep = ((double)move.Horizontal) / move.Step;
                    var vstep = ((double)move.Vertical) / move.Step;
                    double x = info.Bob.X;
                    double y = info.Bob.Y;
                    for (var i = 0; i < move.Step; i++)
                    {
                        x += hstep;
                        y += vstep;
                        info.Bob.X = (int)x;
                        info.Bob.Y = (int)y;
                        UpdateDisplay(true);
                        WaitVbl();
                    }
                }
                if (instr is AmalJump)
                {
                    var label = info.Instructions.FirstOrDefault(i => i is AmalLabel && ((AmalLabel)i).Name == ((AmalJump)instr).Label);
                    idx = info.Instructions.IndexOf(label);
                    continue;
                }
                //if (instr is AmalAnim)
                //{
                ////TODO:
                //    //"Anim 0,(1,3)(2,3)(3,3)(4,3)(5,3)(6,3)(7,3)(8,3)(9,3)(10,3)(11,3)(12,3)(13,3)(14,3)(15,3)(16,3)(17,3)(18,3)(19,3);"
                //    while (true)
                //    {

                //    }
                //    for (var i = 0; i < ((AmalAnim)instr).Times)
                //    {
                //        foreach (var image in ((AmalAnim)instr).Images)
                //        {

                //        }
                //    }
                //}
                idx++;
            }
        }

        public void AmalOff(int number = -1)
        {
            lock (amalLock)
            {
                if (number >= 0)
                {
                    var info = amalInfos.FirstOrDefault(i => i.Number == number);
                    if (info != null) amalInfos.Remove(info);
                }
                else
                {
                    amalInfos.Clear();
                }
            }
        }


        /// <summary>
        /// INKEY$
        /// function: check for a key-press
        /// k$=Inkeys$
        /// </summary>
        public String Inkey_S()
        {
            return gameEngine.GetInkey();
        }

        /// <summary>
        /// SCANCODE
        /// function: return the scancode of a key entered with INKEY$
        /// s=Scancode
        /// </summary>
        public int Scancode()
        {
            return gameEngine.GetScancode();
        }

        public void ClearKey()
        {
            gameEngine.ClearKey();
        }

        /// <summary>
        /// KEY STATE
        /// function: test for a specific key press
        /// k=Key State(scan code)
        /// </summary>
        public bool KeyState(int scancode)
        {
            var key = gameEngine.GetKeyPressed();
            return key == scancode;
        }

        /// <summary>
        /// X MOUSE
        /// reserved variable: report or set the x-co-ordinate of the mouse pointer
        /// </summary>
        public int XMouse()
        {
            var screen = GetCurrentScreen();
            if (screen.PixelMode < PixelMode.Hires)
            {
                return gameEngine.GetMousePosX() / 2 + display.X;
            }
            else
            {
                return gameEngine.GetMousePosX() + display.X;
            }
        }

        /// <summary>
        /// Y MOUSE
        /// reserved variable: report or set the y-coordinate of the mouse pointer
        /// </summary>
        public int YMouse()
        {
            var screen = GetCurrentScreen();
            if (screen.PixelMode < PixelMode.Hires)
            {
                return gameEngine.GetMousePosY() / 2 + display.Y;
            }
            else
            {
                return gameEngine.GetMousePosY() + display.Y;
            }
        }

        /// <summary>
        /// MOUSE KEY
        /// function: read status of mouse buttons
        /// </summary>
        public int MouseKey()
        {
            // 1 = LEFT BUTTON
            // 2 = RIGHT BUTTON
            return gameEngine.GetMouseKey();
        }

        /// <summary>
        /// MOUSE CLICK
        /// function: check for click of mouse button
        /// c=Mouse Click
        /// </summary>
        public int MouseClick()
        {
            return gameEngine.GetMouseClick();
        }


        private readonly Pixel[] DefaultPalette = new Pixel[] 
        {
            new Pixel(0, 0, 0, 0),
            new Pixel(34, 17, 0, 1),
            new Pixel(51, 34, 17, 2),
            new Pixel(0, 170, 0, 3),
            new Pixel(85, 68, 51, 4),
            new Pixel(187, 153, 102, 5),
            new Pixel(119, 102, 85, 6),
            new Pixel(136, 119, 102, 7),
            new Pixel(153, 136, 119, 8),
            new Pixel(0, 0, 0, 9),
            new Pixel(0, 0, 0, 10),
            new Pixel(0, 0, 0, 11),
            new Pixel(0, 0, 0, 12),
            new Pixel(255, 238, 0, 13),
            new Pixel(0, 0, 0, 14),
            new Pixel(12, 63, 12, 15),
            new Pixel(0, 136, 255, 16),
            new Pixel(0, 187, 255, 17),
            new Pixel(221, 153, 0, 18),
            new Pixel(238, 170, 0, 19),
            new Pixel(170, 0, 0, 20),
            new Pixel(0, 0, 0, 21),
            new Pixel(0, 0, 0, 22),
            new Pixel(255, 221, 0, 23),
            new Pixel(102, 102, 102, 24),
            new Pixel(119, 119, 119, 25),
            new Pixel(153, 153, 153, 26),
            new Pixel(0, 0, 0, 27),
            new Pixel(204, 204, 187, 28),
            new Pixel(0, 0, 0, 29),
            new Pixel(170, 170, 170, 30),
            new Pixel(204, 204, 204, 31)
        };
    }


}
