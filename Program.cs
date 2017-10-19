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
            while (true)
            {
                string commandString = Console.ReadLine();
                string[] receivedCommands = commandString.ToLower().Split(' ');

                Rational first = new Rational();
                Rational second = new Rational();
                Rational result = new Rational();

                try
                {
                    bool isFirstCorrect = Rational.TryParse(receivedCommands[1], out first);
                    bool isSecondCorrect = Rational.TryParse(receivedCommands[2], out second);
                    if (!isFirstCorrect || !isSecondCorrect)
                    {
                        throw new FormatException();
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Введите два числа");
                    continue;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Нужно ввести <число>.<число>:<число> <число>.<число>:<число>");
                    continue;
                }
                
                switch (receivedCommands[0])
                {
                    // Сложение рациональных чисел
                    case Commands.Add:
                        result = first.Add(second);
                        break;
                    
                    case Commands.Sub:
                        result = first.Add(second.Negate());
                        break;
                        
                    case Commands.Mul:
                        result = first.Multiply(second);
                        break;
                        
                    case Commands.Div:
                        result = first.DivideBy(second);
                        break;
                    default:
                        Console.WriteLine("Вы ввели некоректную команду");
                        break;
                }
                Console.WriteLine(result);
            }
            
        }
    }
}