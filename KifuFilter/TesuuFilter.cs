using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KifuFilter
{
    class TesuuFilter
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
        /// 入力された棋譜ファイルを経過手数で分割します。
        /// </summary>
        /// <param name="inputFileName">入力ファイル名</param>
        /// <param name="outputFileName">出力ファイル名のうち先頭部分（手数）を除くもの</param>
        /// <param name="splitWidth">何手ごとに分割するか。10なら初手から10手まで、11手から20手までのように分割される</param>
        /// <param name="maxPly">最大何手まで取得するか。この数がsplitWidthの倍数でない場合はsplitWidthの倍数になるように切り上げたのと同じ処理になる。</param>
        public static void filter(string inputFileName,string outputFileName,int splitWidth,int maxPly)
        {
            using (FileStream fs = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            {
                //用意するストリームの個数
                int streamCount = (maxPly + splitWidth - 1) / splitWidth;

                FileStream[] stream = new FileStream[streamCount];
                byte[] buf = new byte[bytesPerPosition]; // データ格納用配列

                while(true)
                {
                    int readSize = fs.Read(buf, 0, bytesPerPosition);

                    if (readSize != bytesPerPosition)
                    {
                        //バイト数不足
                        break;
                    }

                    int gamePly = BitConverter.ToInt16(buf, gamePlyOffset);
                    int streamNumber = gamePly / splitWidth;

                    //その局面の手数が大きすぎる場合はどこにも保存しない
                    if (streamNumber >= streamCount)
                        continue;


                    if (stream[streamNumber] == null) {

                        string filename = string.Format("{0}_{1}to{2}", outputFileName, streamNumber * splitWidth + 1, streamNumber * splitWidth + splitWidth);

                        //書き込み先のファイルがまだ開かれていないなら開く
                        stream[streamNumber] = new FileStream(filename, FileMode.CreateNew, FileAccess.Write);
                    }

                    stream[streamNumber].Write(buf, 0, bytesPerPosition);
                }


                foreach (FileStream outputStream in stream)
                {
                    if (outputStream == null)
                        continue;

                    outputStream.Dispose();
                }
            }





        }




    }
}
