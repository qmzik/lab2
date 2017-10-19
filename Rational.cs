using System;
using System.Numerics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Xml.Schema;

namespace lab2
{
    public class Rational
    {
        /// Числитель дроби
        public int Numerator { get; set; }

        /// Знаменатель дроби
        public int Denominator { get; set; }

        /// Целая часть числа Z.N:D, Z. получается делением числителя на знаменатель и
        /// отбрасыванием остатка
        public int Base
        {
            get { return Numerator / Denominator; }
        }

        /// Дробная часть числа Z.N:D, N:D
        public double Fraction
        {
            get { return (double)Numerator / (double)Denominator - Base; }
        }

        public static Rational Error()
        {
            Console.WriteLine("Неверный формат записи");
            Rational nullRational = new Rational();
            nullRational.Denominator = 1;
            nullRational.Numerator = 0;
            return nullRational;
        }

        public override string ToString()
        {
            int rightNum = Numerator;
            while (rightNum >= Denominator)
            {
                rightNum -= Denominator;
            }

            return Base + "." + (rightNum == 0 ? "" : rightNum + ":" + Denominator);
        }
        
        // Переводит строку в объект Rational
        public static Rational ToRation(string number)
        {
            Rational parsedRational = new Rational();
            string[] fullNumber = number.Split('.');
            string stringZ, stringFraction;

            // Есть ли целая часть
            if (fullNumber[0] == number)
            {
                stringZ = "0";
                stringFraction = number;
            }
            else
            {
                stringZ = fullNumber[0];
                stringFraction = fullNumber[1];
            }
            
            string[] fraction = stringFraction.Split(':');

            try
            {
                int z = int.Parse(stringZ);
                parsedRational.Denominator = int.Parse(fraction[1]);
                parsedRational.Numerator = z * parsedRational.Denominator + int.Parse(fraction[0]);
                return parsedRational;
            }
            catch (Exception e)
            {
                    return Rational.Error();
            }
        }

        /// Создание экземпляра рационального числа из строкового представления Z.N:D
        /// допускается N > D, также допускается
        /// Строковое представления рационального числа
        /// Результат конвертации строкового представления в рациональное
        /// число
        /// true, если конвертация из строки в число была успешной,
        /// false если строка не соответствует формату
        public static bool TryParse(string input, out Rational result)
        {
            result = new Rational();
            string[] fullNumber = input.Split('.');
            string stringZ, stringFraction;

            // Есть ли целая часть
            if (fullNumber[0] == input)
            {
                stringZ = "0";
                stringFraction = input;
            }
            else
            {
                stringZ = fullNumber[0];
                stringFraction = fullNumber[1];
            }
            
            string[] fraction = stringFraction.Split(':');

            try
            {
                int z = int.Parse(stringZ);
                result.Denominator = int.Parse(fraction[1]);
                result.Numerator = z * result.Denominator + int.Parse(fraction[0]);
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        /// Операция сложения, возвращает новый объект - рациональное число,
        /// которое является суммой чисел c и this
        public Rational Add(Rational c)
        {
            Rational result = new Rational();
            if (Denominator != c.Denominator)
            {
                int cDenominator = c.Denominator;
                c.Denominator *= cDenominator;
                c.Numerator *= Denominator;
                Denominator *= cDenominator;
                Numerator *= cDenominator;
                result.Denominator = Denominator;
            }
            else
            {
                result.Denominator = Denominator;
            }
            result.Numerator = Numerator + c.Numerator;
            result.Even();
            return result;
        }

        /// Операция умножения, возвращает новый объект - рациональное число,
        /// которое является результатом умножения чисел x и this
        public Rational Multiply(Rational x)
        {
            Rational result = new Rational();
            result.Denominator = x.Denominator * Denominator;
            result.Numerator = x.Numerator * Numerator;
            result.Even();
            return result;
        }

        /// Операция деления, возвращает новый объект - рациональное число,
        /// которое является результатом деления this на x
        public Rational DivideBy(Rational x)
        {
            Rational result = new Rational();
            result.Numerator = Numerator * x.Denominator;
            result.Denominator = Denominator * x.Numerator;
            result.Even();
            return result;
        }
        
        /// Операция смены знака, возвращает новый объект - рациональное число,
        /// которое являтеся разностью числа 0 и this
        public Rational Negate()
        {
            Rational negate = new Rational();
            negate.Numerator = 0 - Numerator;
            negate.Denominator = Denominator;
            return negate;
        }

        private int getBiggestDivider(int number1, int number2)
        {
            if (number2 == 0)
                return number1;
            return getBiggestDivider(number2, number1 % number2);
        }
        
        /// Приведение дроби - сокращаем дробь на общие делители числителя
        /// и знаменателя. Вызывается реализацией после каждой арифметической операции
        private void Even()
        {
            int divider = getBiggestDivider(Numerator, Denominator);
            Numerator /= divider;
            Denominator /= divider;
        }
    }
}