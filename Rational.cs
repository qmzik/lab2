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
        public int Numerator { get; set; }

        public int Denominator { get; set; }
        
        public int Base
        {
            get { return Numerator / Denominator; }
        }

        //Z.N:D N:D
        public Rational Fraction
        {
            get
            {
                var fraction = new Rational
                {
                    Denominator = Denominator,
                    Numerator = Numerator % Denominator
                };
                return fraction;
            }
        }

        public override string ToString()
        {
            int rightNum;
            try
            {
                rightNum = Math.Abs(Numerator) % Denominator;
            }
            catch (DivideByZeroException)
            {
                throw new RationalOperationException("Делить на ноль нельзя");
                
            }

            string sign = Numerator < 0 ? "-" : "";
            
            return sign + Math.Abs(Base) + (rightNum == 0 ? "" : "." + rightNum + ":" + Denominator);
        }

        public static bool TryParse(string input, out Rational result)
        {
            result = new Rational();
            string[] fullNumber = input.Split('.');
            string stringZ, stringFraction;

            if (fullNumber.Length == 1 && !fullNumber[0].Contains(":"))
            {
                result.Denominator = 1;
                try
                {
                    result.Numerator = int.Parse(fullNumber[0]);
                }
                catch (FormatException)
                {
                    Console.WriteLine(RationalOperationException.FormatExceptionMessage);
                    return false;
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Введено слишком большое число! Попробуйте меньше, чем " + int.MaxValue);
                    return false;
                }

                return true;
            }
            
            // Если нет целой части
            if (fullNumber[0] == input)
            {
                stringZ = "+0";
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
                int numerator = int.Parse(fraction[0]);
                int denumerator = int.Parse(fraction[1]);
                int sign = z >= 0 && stringZ[0] != '-' ? 1 : -1;

                if ((stringZ[0] != '+' || stringZ[0] == '-') && stringFraction[0] == '-' || denumerator < 0)
                {
                    Console.WriteLine("Ставить минус можно только в начале");
                    return false;
                }

                result.Denominator = denumerator;
                result.Numerator = z * denumerator + sign * numerator;

                return true;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine(RationalOperationException.FormatExceptionMessage);
                return false;
            }
            catch (FormatException)
            {
                Console.WriteLine(RationalOperationException.FormatExceptionMessage);
                return false;
            }
            catch (OverflowException)
            {
                Console.WriteLine("Целая часть, числитель и знаменатель не должны быть больше чем " + int.MaxValue);
                return false;
            }
        }
        
        public static Rational operator +(Rational x, Rational y)
        {
            Rational result = new Rational();
            if (y.Denominator != x.Denominator)
            {
                int cDenominator = x.Denominator;
                x.Denominator *= cDenominator;
                x.Numerator *= y.Denominator;
                y.Denominator *= cDenominator;
                y.Numerator *= cDenominator;
                result.Denominator = y.Denominator;
            }
            else
            {
                result.Denominator = y.Denominator;
            }
            result.Numerator = y.Numerator + x.Numerator;
            result.Even();
            return result;
        }

        public static Rational operator *(Rational x, Rational y)
        {
            Rational result = new Rational();
            result.Denominator = x.Denominator * y.Denominator;
            result.Numerator = x.Numerator * y. Numerator;
            result.Even();
            return result;
        }

        public static Rational operator /(Rational x, Rational y)
        {
            if (y.Numerator == 0)
            {
                throw new RationalOperationException("Делить на ноль нельзя");
            }
            Rational result = new Rational();
            try
            {
                result.Numerator = x.Numerator * y.Denominator;
                result.Denominator = x.Denominator * y.Numerator;
                result.Even();
            }
            catch (DivideByZeroException )
            {
                throw new RationalOperationException("Делить на ноль нельзя");
            }

            return result;
        }

        public static Rational operator -(Rational x, Rational y)
        {
            return x + y.Negate();
        }
        
        // рациональное число с противоположным знаком
        private Rational Negate()
        {
            Rational negate = new Rational();
            negate.Numerator = 0 - Numerator;
            negate.Denominator = Denominator;
            return negate;
        }

        private int getBiggestDivider(int number1, int number2)
        {
            number1 = Math.Abs(number1);
            number2 = Math.Abs(number2);
            return number2 == 0 ? number1 : getBiggestDivider(number2, number1 % number2);
        }

        // приведение к правеильной дроби
        private void Even()
        {
            var divider = getBiggestDivider(Numerator, Denominator);
            try
            {
                Numerator /= divider;
                Denominator /= divider;
            }
            catch (DivideByZeroException)
            {
                throw new RationalOperationException("Делить на ноль нельзя");
            }
        }

        public static explicit operator Rational(int x)
        {
            return new Rational
            {
                Numerator = x,
                Denominator = x
            };
        }
        public static explicit operator int(Rational x)
        {
            return x.Base;
        }
    }
}