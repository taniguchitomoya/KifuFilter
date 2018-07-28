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
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("引数が足りません。");
                Console.WriteLine("KifuFilter input_filename output_filename [split_width] [maxPly]");
            }
            else
            {
                int splitWidth = 10;
                int maxPly = 200;

                if (args.Length >= 3)
                {
                    try
                    {
                        splitWidth = int.Parse(args[2]);
                    }
                    catch { }
                }

                if (args.Length >= 4)
                {
                    try
                    {
                        maxPly = int.Parse(args[3]);
                    }
                    catch { }
                }


                TesuuFilter.filter(args[0], args[1],splitWidth,maxPly);
            }

        }
    }
}
