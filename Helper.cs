﻿using System;
using System.IO;

namespace lab2
{
    public class Helper
    {
        public static void GetHelp()
        {
            var result = "";
            var reader = new StreamReader("Help.txt");
            while (!reader.EndOfStream)
            {
                result += reader.ReadLine() + "\n";
            }
            reader.Close();
            Console.Write(result);
        }
    }
}