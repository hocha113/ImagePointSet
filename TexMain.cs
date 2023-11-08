using Gtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

namespace TexDate
{
    public class TexMain
    {
        /// <summary>
        /// 当前文件的运行目录
        /// </summary>
        public static string CurrentDirectory => Directory.GetCurrentDirectory();

        public static XmlDocument Doc = new XmlDocument();

        public static TexMain instance;

        public void Load()
        {
            instance = this;
        }

        public static void Main()
        {
            //Doc.Load(CurrentDirectory + "/Config.xml");
            //XmlNode setting1Node = Doc.SelectSingleNode("/Config/OutPath");
            TexMain texMain = new TexMain();
            texMain.Load();//加载实例

            List<Bitmap> textureList = new List<Bitmap>();
            List<Microsoft.Xna.Framework.Color> colorList = new List<Microsoft.Xna.Framework.Color>();
            List<Vector4> pointLists = new List<Vector4>();

            string[] pngFiles = Directory.GetFiles(CurrentDirectory, "*.png");//匹配PNG后缀的文件的路径
            instance.FindPNG(pngFiles, textureList);
            instance.InprotTexture(textureList, colorList);
        }

        public void FindPNG(string[] pngFiles, List<Bitmap> inputList)
        {
            for(int i = 0; i < pngFiles.Length; i++)
            {
                using (FileStream stream = new FileStream(pngFiles[i], FileMode.Open, FileAccess.Read))
                {
                    // 使用Bitmap类加载PNG文件
                    Bitmap bitmap = new Bitmap(stream);
                    inputList.Add(bitmap);
                }
            }           
        }

        public void InprotTexture(List<Bitmap> pngs, List<Microsoft.Xna.Framework.Color> inputColors)
        {
            foreach(Bitmap bitmap in pngs)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        System.Drawing.Color color = bitmap.GetPixel(x, y);
                        Microsoft.Xna.Framework.Color xnaColor = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
                        inputColors.Add(xnaColor);
                    }
                }
            }
        }

        public void InprotVmongs(List<Microsoft.Xna.Framework.Color> colors, List<Vector4> inprotPoints)
        {

        }
    }
}
