using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KifuFilter
{

    /// <summary>
    /// 評価値等を標準出力するためのクラスです
    /// </summary>
    class PrintValues
    {
        /// <summary>
        /// やねうら王形式の学習用棋譜バイナリの1局面当たりのバイト数
        /// </summary>
        private const int bytesPerPosition = 40;

        /// <summary>
        /// やねうら王形式のバイナリの1局面40バイトの何バイト目にscoreが存在するか
        /// </summary>
        private const int scoreOffset = 32;

        /// <summary>
        /// やねうら王形式のバイナリの1局面40バイトの何バイト目にgamePly(開始からの手数）が存在するか
        /// </summary>
        private const int gamePlyOffset = 36;

        /// <summary>
        /// やねうら王形式のバイナリの1局面40バイトの何バイト目にgame_resultが存在するか
        /// </summary>
        private const int resultOffset = 38;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="count">何局面分出力するか</param>
        internal static void Print(string v,long count)
        {
            using (FileStream fs = new FileStream(v, FileMode.Open, FileAccess.Read))
            {
                byte[] buf = new byte[bytesPerPosition]; // データ格納用配列

                while (count-- > 0)
                {
                    int readSize = fs.Read(buf, 0, bytesPerPosition);

                    if (readSize != bytesPerPosition)
                    {
                        //バイト数不足
                        break;
                    }

                    int gamePly = BitConverter.ToInt16(buf, gamePlyOffset);
                    int score = BitConverter.ToInt16(buf, scoreOffset);
                    int result = (sbyte)BitConverter.ToChar(buf, resultOffset);

                    Console.WriteLine("" + gamePly + " " + score + " " + result);
                }
            }
        }
    }
}
