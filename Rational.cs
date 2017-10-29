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
            int rightNum = Numerator;
            while (rightNum >= Denominator)
            {
                rightNum -= Denominator;
            }

            return Base + "." + (rightNum == 0 ? "" : rightNum + ":" + Denominator);
        }

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

        public Rational Multiply(Rational x)
        {
            Rational result = new Rational();
            result.Denominator = x.Denominator * Denominator;
            result.Numerator = x.Numerator * Numerator;
            result.Even();
            return result;
        }

        public Rational DivideBy(Rational x)
        {
            Rational result = new Rational();
            result.Numerator = Numerator * x.Denominator;
            result.Denominator = Denominator * x.Numerator;
            result.Even();
            return result;
        }
        
        // рациональное число с противоположным знаком
        public Rational Negate()
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
    }
}