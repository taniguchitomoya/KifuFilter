using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KifuFilter
{
    class GamePlyFIlter
    {
        /// <summary>
        /// やねうら王形式の学習用棋譜バイナリの1局面当たりのバイト数
        /// </summary>
        const int bytesPerPosition = 40;

        /// <summary>
        /// やねうら王形式のバイナリの1局面40バイトの何バイト目にgamePly(開始からの手数）が存在するか
        /// </summary>
        private const int gamePlyOffset = 36;

        /// <summary>
        /// 入力されたディレクトリに存在するすべての棋譜ファイルから指定された手数のものだけを出力します。
        /// 
        /// 引数で指定された出力ファイルが既に存在する場合は上書きします。
        /// </summary>
        /// <param name="inputDirectoryName">入力ファイル名</param>
        /// <param name="outputFileName">出力ファイル名</param>
        /// <param name="minPly">何手から取得するか</param>
        /// <param name="maxPly">何手まで取得するか</param>
        public static void filterDir(string inputDirectoryName, string outputFileName, int minPly, int maxPly)
        {
            if (!Directory.Exists(inputDirectoryName))
            {
                //error
                Console.WriteLine("指定されたディレクトリは存在しません。");
                return;
            }

            //新しいファイルを作成する
            using (new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
            {
            }

            foreach (string file in Directory.GetFiles(inputDirectoryName))
            {
                filter(file, outputFileName, minPly, maxPly);
            }
        }

        /// <summary>
        /// 入力された棋譜ファイルから指定された手数のものだけを出力します。
        /// 出力されるファイルがすでに存在する場合は追記されます。
        /// </summary>
        /// <param name="inputFileName">入力ファイル名</param>
        /// <param name="outputFileName">出力ファイル名のうち先頭部分（手数）を除くもの</param>
        /// <param name="minPly">何手から取得するか</param>
        /// <param name="maxPly">何手まで取得するか</param>
        public static void filter(string inputFileName, string outputFileName, int minPly, int maxPly)
        {
            using (FileStream fs = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            {
                using (FileStream outputStream = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.Write))
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
                        if (minPly <= gamePly && gamePly <= maxPly)
                        {
                            outputStream.Write(buf, 0, bytesPerPosition);
                        }
                    }
                }
            }
        }
    }
}
