using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network_Lesson7_Client;
public class Front
{
    public static short ShowMenu(string[] items, string header = "")
    {
        short selected = 0;
        while (true)
        {
            Console.Clear();
            if (header != "")
            {
                Console.WriteLine(header);
            }
            for (short i = 0; i < items.Length; i++)
            {
                if (i == selected)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"=> {items[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"   {items[i]}");
                }
            }

            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.UpArrow && selected > 0)
            {
                selected--;
            }
            if (key.Key == ConsoleKey.DownArrow && selected < items.Length - 1)
            {
                selected++;
            }
            if (key.Key == ConsoleKey.Enter)
            {
                return selected;
            }
        }
    }
}
