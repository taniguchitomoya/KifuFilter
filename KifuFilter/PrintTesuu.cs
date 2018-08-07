using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KifuFilter
{
    class PrintTesuu
    {
        /// <summary>
        /// やねうら王形式の学習用棋譜バイナリの1局面当たりのバイト数
        /// </summary>
        private const int bytesPerPosition = 40;

        /// <summary>
        /// やねうら王形式のバイナリの1局面40バイトの何バイト目にgamePly(開始からの手数）が存在するか
        /// </summary>
        private const int gamePlyOffset = 36;

        internal static void Print(string v)
        {
            using (FileStream fs = new FileStream(v, FileMode.Open, FileAccess.Read))
            {
                byte[] buf = new byte[bytesPerPosition]; // データ格納用配列

                while (true)
                {
                    int readSize = fs.Read(buf, 0, bytesPerPosition);

                    if (readSize != bytesPerPosition)
                    {
                        //バイト数不足
                        break;
                    }

                    int gamePly = BitConverter.ToInt16(buf, gamePlyOffset);
                    Console.WriteLine(gamePly);
                }
            }
        }
    }
}
