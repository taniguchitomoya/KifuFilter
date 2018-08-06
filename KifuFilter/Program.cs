using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KifuFilter
{
    class Program
    {
        /// <summary>
        /// やねうら王形式の学習用棋譜ファイルに対するフィルタ処理を行うプログラムです
        /// 
        /// 開発者用の簡易なツールなので異常系の処理はほとんど行いません。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //今は一種類しかないので特に指定しなくてもそれとして扱うが、そのうちargs[2]あたりでどの処理をするか指定するようになる予定

            if (args.Length < 2)
            {
                Console.WriteLine("引数が足りません。");
                Console.WriteLine("KifuFilter SplitByTesuu input_filename output_filename [split_width] [maxPly]");
                Console.WriteLine(" 与えられたファイルを手数で分割します");
                Console.WriteLine("KifuFilter TesuuFilter input_file_name output_file_name minPly maxPly");
                Console.WriteLine(" 与えられたファイルから手数が特定の範囲のもののみ抽出します");
                Console.WriteLine("KifuFilter TesuuFilterDirectory input_directory_name output_file_name minPly maxPly");
                Console.WriteLine(" 与えられたディレクトリのすべてのファイルから手数が特定の範囲のもののみ抽出します");
            }
            else if(args[0] == "SplitByTesuu")
            {
                int splitWidth = 10;
                int maxPly = 200;

                if (args.Length >= 4)
                {
                    try
                    {
                        splitWidth = int.Parse(args[3]);
                    }
                    catch { }
                }

                if (args.Length >= 5)
                {
                    try
                    {
                        maxPly = int.Parse(args[4]);
                    }
                    catch { }
                }


                TesuuSplitter.filter(args[1], args[2],splitWidth,maxPly);
            }
            else if(args[0] == "TesuuFilterDirecotry")
            {
                if (args.Length < 5)
                {
                    Console.WriteLine("引数が足りません。");
                }
                else
                {
                    GamePlyFIlter.filterDir(args[1], args[2], int.Parse(args[3]), int.Parse(args[4]));
                }
            }
            else if (args[0] == "TesuuFilter")
            {
                if (args.Length < 5)
                {
                    Console.WriteLine("引数が足りません。");
                }
                else
                {
                    GamePlyFIlter.filter(args[1], args[2], int.Parse(args[3]), int.Parse(args[4]));
                }
            }

        }
    }
}
