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
            Console.WriteLine("Введите help для помощи");
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
                string[] receivedCommands = commandString.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                Rational first;
                Rational second;

                try
                {
                    bool isFirstCorrect = Rational.TryParse(receivedCommands[1], out first);
                    bool isSecondCorrect = Rational.TryParse(receivedCommands[2], out second);
                    if (!isFirstCorrect || !isSecondCorrect)
                    {
                        continue;
                    }
                }

                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine(RationalOperationException.FormatExceptionMessage);
                    continue;
                }

                try
                {
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
                            result = first / second;
                            break;
                        default:
                            Console.WriteLine("Вы ввели некоректную команду");
                            continue;
                    }
                    Console.WriteLine(result);
                }
                catch (RationalOperationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            
        }
    }
}