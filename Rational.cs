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
        public int Numerator { get; private set; }

        public int Denominator { get; private set; }
        
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
            if (Fraction.Numerator == 0)
            {
                return Base.ToString();
            }
            if (Base == 0)
            {
                return string.Format("{0}:{1}", Fraction.Numerator, Fraction.Denominator);
            }
            return string.Format("{0}.{1}:{2}", Base, Math.Abs(Fraction.Numerator), Fraction.Denominator);
        }

        public static bool TryParse(string input, out Rational result)
        {
            result = new Rational();
            if (input.LastIndexOf('-') > 0)
            {
                return false;
            }
           
            string[] fullNumber = input.Split('.');

            if (fullNumber.Length > 2)
            {
                return false;
            }
            
            // если ввели целое число
            if (fullNumber.Length == 1 && !fullNumber[0].Contains(":"))
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
            if (fullNumber.Length == 1)
            {
                try
                {
                    var fraction = fullNumber[0].Split(':');
                    result.Numerator = int.Parse(fraction[0]);
                    result.Denominator = int.Parse(fraction[1]);
                
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            
            try
            {   
                var fraction = fullNumber[1].Split(':');

                if (fraction.Length > 2)
                {
                    return false;
                }
                int z = int.Parse(fullNumber[0]);
                int numerator = int.Parse(fraction[0]);
                int denumerator = int.Parse(fraction[1]);
                int sign = z >= 0 ? 1 : -1;
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
                throw new DivideByZeroException();
            }
            Rational result = new Rational
            {
                Numerator = x.Numerator * y.Denominator,
                Denominator = x.Denominator * y.Numerator
            };

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
            Rational negate = new Rational
            {
                Numerator = -Numerator,
                Denominator = Denominator
            };
            
            return negate;
        }

        private int GetBiggestDivider(int number1, int number2)
        {
            number1 = Math.Abs(number1);
            number2 = Math.Abs(number2);
            
            return number2 == 0 ? number1 : GetBiggestDivider(number2, number1 % number2);
        }

        // приведение к правильной дроби
        private void Even()
        {
            var divider = GetBiggestDivider(Numerator, Denominator);
            Numerator /= divider;
            Denominator /= divider;
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