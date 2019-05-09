using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KifuFilter
{

    /// <summary>
    /// 水匠（WCSC29）の資料に記載されているフィルターの実装
    /// </summary>
    class SuishouFilter
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
        /// (未テスト状態）
        /// https://tayayan-ts.hatenablog.com/entry/2019/05/07/010728
        /// 上記記事の（２）勝敗と評価値のずれの排除
        /// 
        /// 入力された棋譜ファイルから評価値と試合結果の符号が一致する局面のみを出力します。
        /// 出力されるファイルがすでに存在する場合は追記されます。
        /// </summary>
        /// <param name="inputFileName">入力ファイル名</param>
        /// <param name="outputFileName">出力ファイル名</param>
        public static void filter2(string inputFileName, string outputFileName)
        {
            using (FileStream fs = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            {
                using (FileStream outputStream = new FileStream(outputFileName, FileMode.Append, FileAccess.Write))
                {
                    byte[] buf = new byte[bytesPerPosition];

                    while (true)
                    {
                        int readSize = fs.Read(buf, 0, bytesPerPosition);

                        if (readSize != bytesPerPosition)
                        {
                            //バイト数不足
                            break;
                        }

                        int gamePly = BitConverter.ToInt16(buf, gamePlyOffset);
                        int score = BitConverter.ToInt16(buf, scoreOffset);

                        // [以下のコメント部分はやねうら王のlearn.hから記述をコピーした]
                        // この局面の手番側が、ゲームを最終的に勝っているなら1。負けているなら-1。
                        // 引き分けに至った場合は、0。
                        int result = (sbyte)BitConverter.ToChar(buf, resultOffset);

                        if (result * score > 0) //符号が同じもののみ出力
                        {
                            outputStream.Write(buf, 0, bytesPerPosition);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// (未テスト状態）
        /// https://tayayan-ts.hatenablog.com/entry/2019/05/07/010728
        /// 上記記事の（１）前後2手の評価値を見た評価値の修正
        /// 
        /// 入力された棋譜ファイルについて上記記事を参考に評価値の修正を行います。
        /// 出力されるファイルがすでに存在する場合は追記されます。
        /// </summary>
        /// <param name="inputFileName">入力ファイル名</param>
        /// <param name="outputFileName">出力ファイル名</param>
        public static void filter1(string inputFileName, string outputFileName)
        {
            using (FileStream fs = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            {
                using (FileStream outputStream = new FileStream(outputFileName, FileMode.Append, FileAccess.Write))
                {
                    //同時に読み込む局面数
                    //最初と最後の２局面については処理がなされないので大きな数値を指定する方が好ましい
                    //棋譜ファイルの局面数がこの数値の倍数でない場合は最後の端数部分が利用されない
                    int numberOfPosition = 1000;

                    byte[] buf = new byte[bytesPerPosition * numberOfPosition];

                    while (true)
                    {
                        int readSize = fs.Read(buf, 0, bytesPerPosition * numberOfPosition);


                        if (readSize != bytesPerPosition)
                        {
                            //バイト数不足
                            break;
                        }

                        for (int i = 0; i < numberOfPosition - 5; i++)
                        {
                            //numberOfPosition個の局面のうちi番目の局面からi+4番目までの５局面について
                            //条件を満たしているかを確認して
                            //条件を満たしている場合にはi+2番目の局面の評価値を書き換える

                            //まずは連続する５局面であるかどうかを手数から調べる
                            int[] gamePly = new int[5];
                            for(int j = 0;j < 5;j++)
                                gamePly[j] = BitConverter.ToInt16(buf, (i + j) * bytesPerPosition + gamePlyOffset);

                            bool flag = true;
                            for (int j = 0; j < 4; j++)
                                if (gamePly[j] - 1 != gamePly[j + 1])
                                    flag = true;
                            if(flag)break;

                            //評価値の条件を満たしているか調べる
                            short[] score = new short[5];
                            for (int j = 0; j < 5; j++)
                                score[j] = BitConverter.ToInt16(buf, (i + j) * bytesPerPosition + scoreOffset);

                            if (
                                (score[2] < score[0] &&
                                score[0] < score[4] &&
                                score[1] < score[3])
                                ||
                                (score[2] > score[0] && //記事では詳細は言及されていないものの、逆向きも当然やっているだろう
                                score[0] > score[4] &&
                                score[1] > score[3])
                                )
                            {
                                //i+2番目の局面の評価値をi番目の評価値で上書きする
                                Buffer.BlockCopy( BitConverter.GetBytes(score[0]),0,buf,(i+2)*bytesPerPosition+scoreOffset,2);
                            }
                        }

                        outputStream.Write(buf, 0, bytesPerPosition);
                    }
                }
            }
        }

    }
}
