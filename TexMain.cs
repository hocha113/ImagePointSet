using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace TexDate
{
    public class TexMain
    {
        /// <summary>
        /// 当前文件的运行目录
        /// </summary>
        public static string CurrentDirectory => Directory.GetCurrentDirectory();

        string filePath => Path.Combine(CurrentDirectory, "Config.xml");

        XDocument doc;

        XElement outPath;

        XElement inportPath;

        public static XmlDocument Doc = new XmlDocument();

        public static TexMain instance;

        public void ResetConfig()
        {
            XDocument newDoc = new XDocument(
                    new XDeclaration("1.0", "utf-8", null),
                    new XElement("Config",
                    new XElement("OutPath", "-1"),
                    new XElement("InportPath", "-1")
                )
            );
            newDoc.Save(filePath);
            Console.WriteLine($"已经重新生成 Config 文件 ---> {filePath}" + "\n按下任意键继续");
            Console.ReadKey();
            Console.WriteLine("————————————————————————————————————————————————");
        }

        public void Load()
        {
            instance = this;

            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Config 文件不存在，将重新生成");
                    ResetConfig();
                    Load();
                }

                doc = XDocument.Load(filePath);
                outPath = doc.Root.Element("OutPath");
                inportPath = doc.Root.Element("InportPath");

                if (inportPath.Value == "-1")
                    inportPath.Value = CurrentDirectory;

                if (outPath.Value == "-1")
                    outPath.Value = Path.Combine(CurrentDirectory, "Dates\\");

                doc.Save(filePath);

                if (outPath.Value != Path.Combine(CurrentDirectory, "Dates\\") && outPath.Value != "-1")
                {
                    Console.WriteLine("检测到目标文件移动，Config 文件可能已经失效，是否重构Config？\n是-1\n否-0");
                    int ins = int.Parse(Console.ReadLine());
                    if (ins == 1)
                    {
                        ResetConfig();
                        Load();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生异常：{ex.Message}");
                ResetConfig();
                Load();
            }
        }

        public static void Main()
        {
            TexMain texMain = new TexMain();
            texMain.Load();

            string[] pngFiles = Directory.GetFiles(instance.inportPath.Value, "*.png");//匹配PNG后缀的文件的路径
            string[] pngNames = Utils.GetFileNames(instance.inportPath.Value);

            Console.WriteLine($"搜索路径:【{instance.inportPath.Value + "\\"}】");
            Console.WriteLine($"找到{pngNames.Length}个目标文件:");
            foreach (string name in pngNames)
            {
                Console.WriteLine($"{name + ".png"}");
            }
            Console.WriteLine("按下任意键后继续图像转化");
            Console.ReadKey();
            instance.ProcessImage(pngFiles, pngNames);
            Console.WriteLine($"转化完成，目标文件路径:【{instance.outPath.Value}】{pngFiles.Length}个文件转化成功" +
                $"，{pngFiles.Length - pngFiles.Length}个文件转化失败" + "\n按任意键退出...");
            Console.ReadKey();
            instance.doc.Save(instance.filePath);
        }

        public void ProcessImage(string[] pngFiles, string[] pngNames)
        {
            Console.WriteLine("————————————————————————————————————————————————");
            for (int i = 0; i < pngFiles.Length; i++)
            {
                Bitmap png = new Bitmap(new FileStream(pngFiles[i], FileMode.Open, FileAccess.Read));
                Microsoft.Xna.Framework.Color[,] color = new Microsoft.Xna.Framework.Color[png.Width, png.Height];
                Vector4[,] pointSet = new Vector4[png.Width, png.Height];
                for (int x = 0; x < png.Width; x++)
                {
                    for (int y = 0; y < png.Height; y++)
                    {
                        System.Drawing.Color colors = png.GetPixel(x, y);
                        color[x, y] = new Microsoft.Xna.Framework.Color(colors.R, colors.G, colors.B, colors.A);
                        pointSet[x, y] = color[x, y].ToVector4();
                    }
                }
                string inportFilePath = instance.inportPath.Value + $"{pngNames[i]}.png";
                string outFilePath = instance.outPath.Value + $"{pngNames[i]}Date.cs";
                using (FileStream createFile = new FileStream(outFilePath, FileMode.Create))
                using (StreamWriter writerVrsDate = new StreamWriter(createFile))
                {
                    string tabs = "     ";
                    writerVrsDate.WriteLine($"public static class {pngNames[i]}Date" + "{");
                    writerVrsDate.WriteLine(tabs + "public static Vector4[,] vr = new Vector4[,]{");

                    for (int x = 0; x < pointSet.GetLength(0); x++)
                    {
                        writerVrsDate.Write(tabs + tabs);
                        writerVrsDate.Write("{");
                        for (int y = 0; y < pointSet.GetLength(1); y++)
                        {
                            Vector4 vector = pointSet[x, y] * 255;
                            string endchar = y == pointSet.GetLength(1) - 1 ? ",}," : ", ";
                            if (x == pointSet.GetLength(0) - 1 && y == pointSet.GetLength(1) - 1)
                            {
                                endchar = ",}";
                            }
                            string line = $"new Vector4({vector.X}, {vector.Y}, {vector.Z}, {vector.W})" + endchar;
                            writerVrsDate.Write(line);
                        }
                        writerVrsDate.Write("\n");
                    }
                    writerVrsDate.Write(tabs + "};");
                    writerVrsDate.Write("}");
                }

                Console.WriteLine(inportFilePath + "-->" + outFilePath);
            }
            Console.WriteLine("————————————————————————————————————————————————");
        }
    }
}
