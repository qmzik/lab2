using System;
using System.Numerics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
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

            int z = Math.Abs(Base);
            string sign = Numerator < 0 ? "-" : "";
            
            // ставим точку только в случае "Z.N:M"
            string dot = z != 0 && rightNum != 0 ? "." : "";
            
            // не выводим целую часть только в случае "N:M"
            string outBase = z == 0 && rightNum != 0 ? "" : z.ToString();
            
            // не выводим дробную часть только в случае "Z"
            string outFraction = rightNum == 0 ? "" : rightNum + ":" + Denominator;

            return sign + outBase + dot + outFraction;
        }

        public static bool TryParse(string input, out Rational result)
        {
            result = new Rational();
            if (input.LastIndexOf('-') > 0)
            {
                return false;
            }
           
            string[] fullNumber = input.Split('.');
            string stringZ, stringFraction;
            
            // если ввели целое число
            if (fullNumber[0] == input && !fullNumber[0].Contains(":"))
            {
                result.Denominator = 1;
                try
                {
                    result.Numerator = int.Parse(fullNumber[0]);
                }
                catch (Exception)
                {
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
            Rational result = new Rational
            {
                Numerator = x.Numerator * y.Denominator + y.Numerator * x.Denominator,
                Denominator = x.Denominator * y.Denominator
            };
            result.Even();
            
            return result;
        }

        public static Rational operator *(Rational x, Rational y)
        {
            Rational result = new Rational
            {
                Denominator = x.Denominator * y.Denominator,
                Numerator = x.Numerator * y.Numerator
            };
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
            Rational negate = new Rational
            {
                Numerator = -Numerator,
                Denominator = Denominator
            };
            
            return negate;
        }

        private int getBiggestDivider(int number1, int number2)
        {
            number1 = Math.Abs(number1);
            number2 = Math.Abs(number2);
            
            return number2 == 0 ? number1 : getBiggestDivider(number2, number1 % number2);
        }

        // приведение к правильной дроби
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

        public static implicit operator Rational(int x)
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