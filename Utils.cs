using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TexDate
{
    public static class Utils
    {
        /// <summary>
        /// 获取指定文件路径下的文件名称
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="qualifierPNG">是否限定PNG后缀文件</param>
        /// <returns></returns>
        public static string[] GetFileNames(string path, bool qualifierPNG = true)
        {
            string[] fileNames = qualifierPNG
                ? Directory.EnumerateFiles(path, "*.png").Select(Path.GetFileNameWithoutExtension).ToArray()
                : Directory.EnumerateFiles(path).Select(Path.GetFileNameWithoutExtension).ToArray();

            return fileNames;
        }
    }
}
