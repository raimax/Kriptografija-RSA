using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RSA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Rsa _rsa = new Rsa();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            ulong p = uint.Parse(Prime1.Text);
            ulong q = uint.Parse(Prime2.Text);
            ulong n = _rsa.FindN(p, q);
            ulong phi = _rsa.FindPhi(p, q);
            ulong exponent = _rsa.FindE(n, phi);

            LabelN.Content = $"n: {n}";
            LabelPhi.Content = $"phi: {phi}";
            LabelE.Content = $"e: {exponent}";

            byte[] plainTextBytes = Encoding.ASCII.GetBytes(EncryptionPlainText.Text);

            _rsa.Encrypted = _rsa.Encrypt(plainTextBytes, exponent, n);

            ulong d = _rsa.GeneratePrivateKey(exponent, phi);
            LabelD.Content = $"d: {d}";

            EncryptionCypherText.Text = ListToString(_rsa.Encrypted);
        }

        private string ListToString<T>(List<T> list)
        {
            string encryptedText = "";
            foreach (T number in list)
            {
                encryptedText += number?.ToString() + " ";
            }

            return encryptedText.Trim();
        }

        private void BtnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            int n = int.Parse(DecryptionN.Text);
            Dictionary<string, int> primeNumbers = _rsa.FindPrimeNumbers(n);

            ulong p = (ulong)primeNumbers["p"];
            ulong q = (ulong)primeNumbers["q"];

            ulong phi = _rsa.FindPhi(p, q);
            ulong exponent = _rsa.FindE((ulong)n, phi);

            ulong d = _rsa.GeneratePrivateKey(exponent, phi);

            List<ulong> encryptedText = StringToList(DecryptionCypherText.Text);
            byte[] decryptedText = _rsa.Decrypt(encryptedText, d, (ulong)n);

            string decryptedTextString = string.Empty;

            foreach (byte item in decryptedText)
            {
                decryptedTextString += item + " ";
            }

            DecryptionPlainText.Text = decryptedTextString.Trim();


        }

        private async void BtnSaveCypherText(object sender, RoutedEventArgs e)
        {
            if (_rsa.Encrypted.Count > 0)
            {
                await SaveFileToDisk("CypherText.txt", ListToString(_rsa.Encrypted));
            }
        }

        private void BtnSavePublicKey(object sender, RoutedEventArgs e)
        {
            //
        }

        private List<ulong> StringToList(string s)
        {
            string[] stringArr = s.Split(" ");
            List<ulong> list = new List<ulong>();

            foreach (string str in stringArr)
            {
                list.Add(ulong.Parse(str));
            }

            return list;
        }

        private async void BtnSaveDecryptedText(object sender, RoutedEventArgs e)
        {
            if (_rsa.Decrypted is not null)
            {
                await SaveFileToDisk("DecryptedText.txt", Encoding.UTF8.GetString(_rsa.Decrypted));
            }
        }

        private async Task SaveFileToDisk(string filePath, string content)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.FileName = filePath;
            saveFileDialog.Filter = "Text Files | *.txt";
            saveFileDialog.DefaultExt = "txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                if (saveFileDialog.FileName != "")
                {
                    await File.WriteAllTextAsync(saveFileDialog.FileName, content);
                }
            }
        }

        private async Task OpenFileFromDisk()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Text Files | *.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileName != "")
                {
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new(fileStream))
                    {
                        DecryptionCypherText.Text = await reader.ReadToEndAsync();
                    }
                }
            }
        }

        private async void BtnLoadCypherText(object sender, RoutedEventArgs e)
        {
            await OpenFileFromDisk();
        }
    }
}
