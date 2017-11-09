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

        public double Fraction
        {
            get { return (double)Numerator / (double)Denominator - Base; }
        }

        public override string ToString()
        {
            int rightNum = Math.Abs(Numerator);
            while (rightNum >= Denominator)
            {
                rightNum -= Denominator;
            }

            string sign = Numerator < 0 ? "-" : "";
            
            return sign + Math.Abs(Base) + "." + (rightNum == 0 ? "" : rightNum + ":" + Denominator);
        }

        public static bool TryParse(string input, out Rational result)
        {
            result = new Rational();
            string[] fullNumber = input.Split('.');
            string stringZ, stringFraction;

            // Если нет целой части
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
                int numerator = int.Parse(fraction[0]);
                int denumerator = int.Parse(fraction[1]);
                int sign = z >= 0 ? 1 : -1;
                
                if (sign == -1 && (denumerator < 0 || numerator < 0))
                {
                    throw new FormatException();
                }
                
                if (denumerator < 0 && numerator < 0)
                {
                    throw new FormatException();
                }
                
                result.Denominator = denumerator;
                result.Numerator = z * denumerator + sign * numerator;
                
                return true;
            }
            catch (Exception)
            {
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
            Rational result = new Rational();
            result.Numerator = x.Numerator * y.Denominator;
            result.Denominator = x.Denominator * y.Numerator;
            result.Even();
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
            Numerator /= divider;
            Denominator /= divider;
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