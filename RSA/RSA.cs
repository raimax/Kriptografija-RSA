using System;
using System.Collections.Generic;
using System.Numerics;

namespace RSA
{
    public class Rsa
    {
        public List<BigInteger> Encrypted { get; set; } = new List<BigInteger>();
        public byte[]? Decrypted { get; set; }
        public List<BigInteger> PublicKey { get; set; } = new List<BigInteger>();

        public List<BigInteger> Encrypt(byte[] plainText, BigInteger e, BigInteger n)
        {
            List<BigInteger> encrypted = new();

            for (int i = 0; i < plainText.Length; i++)
            {
                encrypted.Add(BigInteger.Pow(plainText[i], (int)e) % n);
            }

            return encrypted;
        }

        public byte[] Decrypt(List<BigInteger> encryptedText, BigInteger d, BigInteger n)
        {
            byte[] decrypted = new byte[encryptedText.Count];

            for (int i = 0; i < encryptedText.Count; i++)
            {
                decrypted[i] = (byte)BigInteger.ModPow(encryptedText[i], d, n);
            }

            return decrypted;
        }

        public BigInteger FindN(BigInteger p, BigInteger q)
        {
            return p * q;
        }

        public BigInteger FindPhi(BigInteger p, BigInteger q)
        {
            return ((p - 1) * (q - 1));
        }

        public BigInteger FindGcd(BigInteger a, BigInteger b)
        {
            if (b == 0)
            {
                return a;
            }

            return FindGcd(b, a % b);
        }

        public BigInteger FindE(BigInteger n, BigInteger phi)
        {
            BigInteger e = 2;
            while (FindGcd(e, phi) != 1)
            {
                e++;
            }

            return e;
        }

        //private bool IsPrime(BigInteger number)
        //{
        //    if (number == 1) return false;
        //    if (number == 2) return true;

        //    var limit = Math.Ceiling(Math.Sqrt(number));

        //    for (BigInteger i = 2; i <= limit; ++i)
        //        if (number % i == 0)
        //            return false;
        //    return true;

        //}

        public BigInteger GeneratePrivateKey(BigInteger exponent, BigInteger phi)
        {
            BigInteger d = 1;

            while (d * exponent % phi != 1)
            {
                d++;
            }

            return d;
        }

        public Dictionary<string, int> FindPrimeNumbers(int n)
        {
            int num = n;
            int i, j;
            int count = 0;
            List<int> nums = new List<int>();

            for (i = 2; i < num; i++)
            {
                // check for divisibility
                if (num % i == 0)
                {
                    count = 0;
                    // check for prime number
                    for (j = 1; j <= i; j++)
                    {
                        if (i % j == 0)
                            count++;
                    }
                    if (count == 2)
                    {
                        nums.Add(i);
                    }
                }
            }

            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("p", nums[0]);
            result.Add("q", nums[1]);

            return result;
        }
    }
}
