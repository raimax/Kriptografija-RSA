namespace RSA
{
    public class Rsa
    {
        public void Encrypt(string plainText, ulong p, ulong q)
        {
            ulong n = FindN(p, q);
            ulong phi = FindPhi(p, q);
            ulong e = FindE(n, phi);
        }

        public void Decrypt(string encryptedText)
        {

        }

        private ulong FindN(ulong p, ulong q)
        {
            return p * q;
        }

        private ulong FindPhi(ulong p, ulong q)
        {
            return p - 1 * q - 1;
        }

        private ulong FindGcd(ulong a, ulong b)
        {
            if (b == 0)
            {
                return a;
            }

            return FindGcd(b, a % b);
        }

        private ulong FindE(ulong n, ulong phi)
        {
            ulong e = 2;
            while (FindGcd(e, n) != 1 || FindGcd(e, phi) != 1)
            {
                e++;
            }

            return e;
        }
    }
}
