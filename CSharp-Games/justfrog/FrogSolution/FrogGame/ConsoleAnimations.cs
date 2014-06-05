using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;

namespace FrogGame
{
    static class ConsoleAnimations
    {
        public static void AnimateTitle()
        {
            try
            {
                Console.CursorVisible = false;
                Console.Clear();

                string title = @"
  =###:                   *#-  
    -#:                   *#-  
    -#: :#-  *@. *###*  :#####%
    -#: :#-  *@.-#:  =-   *#-  
    -#: :#-  *@.-#=       *#-  
    -#: :#-  *@. :###=    *#-  
:-  :#- :#-  *@.    *#-   *#-  
*#- =@. -#* .@@.:+  *#-   :#:  
 =###-   *###=@. =###:     %##%
                               
                               
 *#####:                       
 *@.                           
 *@.      %%###* -###%   .@##=@
 *@.      %#:    %% .@=  %% .@@
 *####%   %=    -#:  *@.-#:  *@
 *@.      %=    :#-  *@.:#-  *@
 *@.      %=    -#:  *@.-#:  *@
 *@.      %=     %% .@=  %% .@@
 *@.      %=     .@##%   .@##=@
                             *@
                         ** .@=
                          %##% ";

                string[] titleLines = title.Split(new char[] {'\n'});

                //string[] titleLines = ReadFileLines(@"...\...\FrogTitle.txt");

                var maxLength = titleLines.Aggregate(0, (max, line) => Math.Max(max, line.Length));
                var col = Console.BufferWidth / 2 - maxLength / 2;

                int startRow = -titleLines.Length;
                int endRow = (Console.WindowHeight - titleLines.Length) / 2;

                Console.ForegroundColor = ConsoleColor.Green;
                for (int row = startRow; row < endRow; row++)
                {
                    if (Console.KeyAvailable) break;

                    ConsoleDraw(titleLines, col, row);
                    Thread.Sleep(50);
                }

                Console.ReadKey(true);
                Console.ResetColor();
                Console.Clear();
            }
            catch (FileNotFoundException) { Console.WriteLine("File not found!"); }
            catch (DirectoryNotFoundException) { Console.WriteLine("Directory not found!"); }
            catch (SecurityException) { Console.WriteLine("Security error detected!"); }
            catch (IOException e) { Console.WriteLine(e.Message); }
            catch (UnauthorizedAccessException e) { Console.WriteLine(e.Message); }
        }

        private static void ConsoleDraw(IEnumerable<string> lines, int col, int row)
        {
            if (col > Console.WindowWidth) return;
            if (row > Console.WindowHeight) return;

            var trimLeft = col < 0 ? -col : 0;
            int index = row;

            col = col < 0 ? 0 : col;
            row = row < 0 ? 0 : row;

            var linesToPrint =
                from line in lines
                let currentIndex = index++
                where currentIndex > 0 && currentIndex < Console.WindowHeight
                select new
                {
                    Text = new String(line.Skip(trimLeft).Take(Math.Min(Console.WindowWidth - col, line.Length - trimLeft)).ToArray()),
                    X = col,
                    Y = row++
                };

            Console.Clear();
            foreach (var line in linesToPrint)
            {
                Console.SetCursorPosition(line.X, line.Y);
                Console.Write(line.Text);
            }
        }

        private static string[] ReadFileLines(string file)
        {
            List<string> fileLines = new List<string>();
            using (StreamReader reader = new StreamReader(file))
            {
                string fileLine;
                while ((fileLine = reader.ReadLine()) != null)
                {
                    fileLines.Add(fileLine);
                }
            }
            return fileLines.ToArray();
        }   
    }
}
