using Microsoft.Xna.Framework;

namespace TexDate
{
    public static class ExampleDate
    {
        public static float exampleNum = 0;
        public static Vector4[,] exampleVr = new Vector4[,]
        {
            //y行
            {new Vector4(exampleNum, exampleNum, exampleNum, exampleNum), new Vector4(exampleNum, exampleNum, exampleNum, exampleNum),},//x个向量
            {new Vector4(exampleNum, exampleNum, exampleNum, exampleNum), new Vector4(exampleNum, exampleNum, exampleNum, exampleNum),},
            {new Vector4(exampleNum, exampleNum, exampleNum, exampleNum), new Vector4(exampleNum, exampleNum, exampleNum, exampleNum),}
        };
    }
}
