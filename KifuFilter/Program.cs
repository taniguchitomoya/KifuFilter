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
        /// 開発者向けの簡易なツールなので異常系の処理はほとんど行いませんし、コードは構造化せずにコピペ中心で作られています。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("引数が足りません。");
                Console.WriteLine();
                Console.WriteLine("KifuFilter SplitByTesuu input_filename output_filename [split_width] [maxPly]");
                Console.WriteLine(" 与えられたファイルを手数で分割します");
                Console.WriteLine();
                Console.WriteLine("KifuFilter TesuuFilter input_file_name output_file_name minPly maxPly");
                Console.WriteLine(" 与えられたファイルから手数が特定の範囲のもののみ抽出します");
                Console.WriteLine();
                Console.WriteLine("KifuFilter TesuuFilterDirectory input_directory_name output_file_name minPly maxPly");
                Console.WriteLine(" 与えられたディレクトリのすべてのファイルから手数が特定の範囲のもののみ抽出します");
                Console.WriteLine();
                Console.WriteLine("KifuFilter PrintValues input_file_name [count]");
                Console.WriteLine(" 与えられたファイルの局面の手数,手番からみた評価値,手番からみた対戦結果を標準出力に出力します。");
                Console.WriteLine();
                Console.WriteLine("KifuFilter SuishouFilter1 input_file_name [output_file_name]");
                Console.WriteLine(" 与えられたファイルのすべての局面について前後の局面を考慮して不適切と思われる場合に評価値を補正します。");
                Console.WriteLine(" 入力ファイルと出力ファイルは同じ大きさになります。");
                Console.WriteLine();
                Console.WriteLine("KifuFilter SuishouFilter2 input_file_name [output_file_name]");
                Console.WriteLine(" 与えられたファイルから評価値と結果の符号が一致するもののみ抽出します");
            }
            else if (args[0] == "SplitByTesuu")
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


                TesuuSplitter.filter(args[1], args[2], splitWidth, maxPly);
            }
            else if (args[0] == "TesuuFilterDirecotry")
            {
                if (args.Length < 5)
                {
                    Console.WriteLine("引数が足りません。");
                }
                else
                {
                    GamePlyFilter.filterDir(args[1], args[2], int.Parse(args[3]), int.Parse(args[4]));
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
                    GamePlyFilter.filter(args[1], args[2], int.Parse(args[3]), int.Parse(args[4]));
                }
            }
            else if (args[0] == "PrintValues")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("引数が足りません。");
                }
                else if (args.Length == 2)
                {
                    PrintValues.Print(args[1], long.MaxValue);
                }
                else
                {
                    PrintValues.Print(args[1], long.Parse(args[2]));
                }
            }
            else if (args[0] == "SuishouFilter1")
            {
                if (args.Length == 3)
                {
                    SuishouFilter.filter1(args[1], args[2]);
                }
                else if (args.Length == 2)
                {
                    SuishouFilter.filter1(args[1], args[1] + ".suishou1");
                }
                else if (args.Length < 2)
                {
                    Console.WriteLine("引数が足りません。");
                }
            }
            else if (args[0] == "SuishouFilter2")
            {
                if (args.Length == 3)
                {
                    SuishouFilter.filter2(args[1], args[2]);
                }
                else if (args.Length == 2)
                {
                    SuishouFilter.filter2(args[1], args[1] + ".suishou2");
                }
                else if (args.Length < 2)
                {
                    Console.WriteLine("引数が足りません。");
                }
            }

        }
    }

}
