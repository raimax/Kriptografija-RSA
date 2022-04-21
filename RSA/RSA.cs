using System;
using System.Collections.Generic;

namespace RSA
{
    public class Rsa
    {
        public List<ulong> Encrypted { get; set; } = new List<ulong>();
        public byte[]? Decrypted { get; set; }

        public List<ulong> Encrypt(byte[] plainText, ulong e, ulong n)
        {
            List<ulong> encrypted = new();

            for (int i = 0; i < plainText.Length; i++)
            {
                encrypted.Add((ulong)(Math.Pow(plainText[i], e) % n));
            }

            return encrypted;
        }

        public byte[] Decrypt(List<ulong> encryptedText, ulong d, ulong n)
        {
            byte[] decrypted = new byte[encryptedText.Count];

            for (int i = 0; i < encryptedText.Count; i++)
            {
                decrypted[i] = (byte)(Math.Pow(encryptedText[i], d) % n);
            }

            return decrypted;
        }

        public ulong FindN(ulong p, ulong q)
        {
            return p * q;
        }

        public ulong FindPhi(ulong p, ulong q)
        {
            return ((p - 1) * (q - 1));
        }

        public ulong FindGcd(ulong a, ulong b)
        {
            if (b == 0)
            {
                return a;
            }

            return FindGcd(b, a % b);
        }

        public ulong FindE(ulong n, ulong phi)
        {
            ulong e = 2;
            while (FindGcd(e, phi) != 1)
            {
                e++;
            }

            return e;
        }

        private bool IsPrime(ulong number)
        {
            if (number == 1) return false;
            if (number == 2) return true;

            var limit = Math.Ceiling(Math.Sqrt(number));

            for (ulong i = 2; i <= limit; ++i)
                if (number % i == 0)
                    return false;
            return true;

        }

        public ulong GeneratePrivateKey(ulong exponent, ulong phi)
        {
            ulong d = 1;

            while (d * exponent % phi != 1)
            {
                d++;
            }

            return d;
        }

        public Dictionary<string, int> FindPrimeNumbers(int n)
        {
            int num = 33;
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
