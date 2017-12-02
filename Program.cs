using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace lab2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Введите help для помощи, exit для выхода");
            while (true)
            {   Console.Write("Вводите: ");
                string commandString = Console.ReadLine();
                var separator = new[] {' '};
                commandString = commandString.Trim().ToLower();
                if (commandString == "help")
                {
                    Helper.GetHelp();
                    continue;
                }

                if (commandString == "exit")
                {
                    break;
                }
                
                string[] receivedCommands = commandString.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                if (receivedCommands.Length != 3)
                {
                    Console.WriteLine("Неверный формат ввода, введите <команда> <рациональное число> <рациональное число>");
                    continue;
                }

                Rational first;
                Rational second;

                bool isFirstCorrect = Rational.TryParse(receivedCommands[1], out first);
                bool isSecondCorrect = Rational.TryParse(receivedCommands[2], out second);
                if (!isFirstCorrect && !isSecondCorrect)
                {
                    Console.WriteLine("Числа введенены в неверном формате, уточните формат ввода командой help");
                    continue;
                }
                
                if (!isFirstCorrect)
                {
                    Console.WriteLine("Число " + receivedCommands[1] +
                                      " введенено в неверном формате, уточните формат ввода командой help");
                    continue;
                }

                if (!isSecondCorrect)
                {
                    Console.WriteLine("Число " + receivedCommands[2] +
                                      " введенено в неверном формате, уточните формат ввода командой help");
                    continue;
                }

                Rational result = new Rational();
                switch (receivedCommands[0])
                {
                    case Commands.Add:
                        result = first + second;
                        break;
                
                    case Commands.Sub:
                        result = first - second;
                        break;
                    
                    case Commands.Mul:
                        result = first * second;
                        break;
                    
                    case Commands.Div:
                        try
                        {
                            result = first / second;
                        }
                        catch (DivideByZeroException)
                        {
                            Console.WriteLine("Делить на ноль нельзя");
                            continue;
                        }
                        break;
                    default:
                        Console.WriteLine("Вы ввели некоректную команду");
                        continue;
                }
                Console.WriteLine(result);
            }
            
        }
    }
}