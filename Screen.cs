using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum
{
    static class Screen
    {
        public static uint[] Pixels = new uint[640 * 512];
        static uint[] Palette = {
            0xFF000000, 0xFFCB0000, 0xFF0000C0, 0xFFCB00C0,
            0xFF00C000, 0xFFCBC000, 0xFF00C0C0, 0xFFC0C0C0,
            0xFF000000, 0xFFFF0000, 0xFF0000FF, 0xFFFF00FF,
            0xFF00FF00, 0xFFFFFF00, 0xFF00FFFF, 0xFFFFFFFF};
        static int[] AdressesOfStrings = {
            16384, 16640, 16896, 17152, 17408, 17664, 17920, 18176,
            16416, 16672, 16928, 17184, 17440, 17696, 17952, 18208,
            16448, 16704, 16960, 17216, 17472, 17728, 17984, 18240,
            16480, 16736, 16992, 17248, 17504, 17760, 18016, 18272,
            16512, 16768, 17024, 17280, 17536, 17792, 18048, 18304,
            16544, 16800, 17056, 17312, 17568, 17824, 18080, 18336,
            16576, 16800, 17088, 17344, 17600, 17856, 18112, 18368,
            16608, 16864, 17120, 17376, 17632, 17888, 18144, 18400,
            18432, 18688, 18944, 19200, 19456, 19712, 19968, 20224,
            18464, 18720, 18976, 19232, 19488, 19744, 20000, 20256,
            18496, 18752, 19008, 19264, 19520, 19776, 20032, 20288,
            18528, 18784, 19040, 19296, 19552, 19808, 20064, 20320,
            18560, 18816, 19072, 19328, 19584, 19840, 20096, 20352,
            18592, 18848, 19104, 19360, 19616, 19872, 20128, 20384,
            18624, 18880, 18880, 19392, 19648, 19904, 20160, 20416,
            18656, 18912, 19168, 19424, 19680, 19936, 20192, 20448,
            20480, 20736, 20992, 21248, 21504, 21760, 22016, 22272,
            20512, 20768, 21024, 21280, 21536, 21792, 22048, 22304,
            20544, 20800, 21056, 21312, 21568, 21824, 22080, 22336,
            20576, 20832, 21088, 21344, 21600, 21856, 22112, 22368,
            20608, 20864, 21120, 21376, 21632, 21888, 22144, 22400,
            20640, 20896, 21152, 21408, 21664, 21920, 22176, 22432,
            20672, 20928, 21184, 21440, 21696, 21952, 22208, 22464,
            20704, 20960, 21216, 21472, 21728, 21984, 22240, 22496};

        static Random rnd = new Random();
        static int Border = 0;
        static bool flash;
        static int flashtimer;
        static int Byte;
        static int Attr;
        static int Ink;
        static int Paper;
        /// <summary>
        /// Рисование текущей строки телевизора
        /// </summary>
        public static void DrawString(int String)
        {
            //Сдвигаем номер первой строки телека до первой строки экрана для удобства
            String = String - (Spectrum.Strings - 256) / 2;
            //Уходим, если строка не попадает в кадр
            if (String < 0 | String > 255) return;
            for (int i = 0; i < 320; i++)
            {
                if (String - 32 < 0 | String - 32 > 191 | i < 32 | i > 287)
                    SetPixel(i, String, Border);
                else
                {
                    if ((i - 32) % 8 == 0)
                    {
                        int str = String - 32;
                        int tab = (i - 32) / 8;
                        //Надо найти байт точек и байт атрибутов
                        Byte = Spectrum.Memory[AdressesOfStrings[str] + tab];
                        Attr = Spectrum.Memory[22528 + (str / 8) * 32 + tab];
                        //Находим цвета по атрибутам
                        Ink = Attr & 7;
                        Paper = (Attr & 56) / 8;
                        if ((Attr & 64) == 64) //Яркость
                        {
                            Ink += 8;
                            Paper += 8;
                        }
                        if ((Attr & 128) == 128 && flash) //Флеш включен
                        {
                            int swap = Ink;
                            Ink = Paper;
                            Paper = swap;
                        }
                        if ((Byte & 128) == 128) SetPixel(i, String, Ink); else SetPixel(i, String, Paper);
                        if ((Byte & 64) == 64) SetPixel(i + 1, String, Ink); else SetPixel(i + 1, String, Paper);
                        if ((Byte & 32) == 32) SetPixel(i + 2, String, Ink); else SetPixel(i + 2, String, Paper);
                        if ((Byte & 16) == 16) SetPixel(i + 3, String, Ink); else SetPixel(i + 3, String, Paper);
                        if ((Byte & 8) == 8) SetPixel(i + 4, String, Ink); else SetPixel(i + 4, String, Paper);
                        if ((Byte & 4) == 4) SetPixel(i + 5, String, Ink); else SetPixel(i + 5, String, Paper);
                        if ((Byte & 2) == 2) SetPixel(i + 6, String, Ink); else SetPixel(i + 6, String, Paper);
                        if ((Byte & 1) == 1) SetPixel(i + 7, String, Ink); else SetPixel(i + 7, String, Paper);
                    }
                }
            }
            flashtimer++;
            if (flashtimer > 5000)
            {
                flashtimer = 0;
                flash ^= true;
            }
        }

        static void SetPixel(int x, int y, int c)
        {
            int a = y * 1280 + x * 2;
            Pixels[a] = Palette[c];
            Pixels[a + 1] = Palette[c];
            Pixels[a + 640] = Palette[c];
            Pixels[a + 641] = Palette[c];
        }
    }
}
