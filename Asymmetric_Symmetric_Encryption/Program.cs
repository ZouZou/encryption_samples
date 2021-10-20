using CertificateManager;
using EncryptDecryptLib;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Asymmetric_Symmetric_Encryption
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = "I have a big dog. You've got a cat. We all love animals!";

            Console.WriteLine("Which kind of encryption would you like to try?");
            Console.WriteLine("");
            Console.WriteLine("1 - Symmetric Encryption");
            Console.WriteLine("2 - Asymmetric Encryption");
            Console.WriteLine("");
            var encryptionOption = Console.ReadLine();
            if (encryptionOption == "1")
            {
                // Call the symmetric encryption
                SymmetricEncryption(text);
            }
            if (encryptionOption == "2")
            {
                // Call the asymmetric encryption
                AsymmetricEncryption(text);
            }
            Console.ReadLine();
        }

        static void SymmetricEncryption(string text)
        {
            Console.WriteLine("-- Encrypt Decrypt symmetric --");
            Console.WriteLine("");

            var symmetricEncryptDecrypt = new SymmetricEncryptDecrypt();
            var (Key, IVBase64) = symmetricEncryptDecrypt.InitSymmetricEncryptionKeyIV();

            var encryptedText = symmetricEncryptDecrypt.Encrypt(text, IVBase64, Key);

            Console.WriteLine("-- Key --");
            Console.WriteLine(Key);
            Console.WriteLine("-- IVBase64 --");
            Console.WriteLine(IVBase64);

            Console.WriteLine("");
            Console.WriteLine("-- Encrypted Text --");
            Console.WriteLine(encryptedText);

            var decryptedText = symmetricEncryptDecrypt.Decrypt(encryptedText, IVBase64, Key);

            Console.WriteLine("-- Decrypted Text --");
            Console.WriteLine(decryptedText);
        }

        static void AsymmetricEncryption(string text)
        {
            var serviceProvider = new ServiceCollection()
                            .AddCertificateManager()
                            .BuildServiceProvider();

            var cc = serviceProvider.GetService<CreateCertificates>();

            var cert3072 = CreateRsaCertificates.CreateRsaCertificate(cc, 3072);

            Console.WriteLine("-- Encrypt Decrypt asymmetric --");
            Console.WriteLine("");

            var asymmetricEncryptDecrypt = new AsymmetricEncryptDecrypt();

            var encryptedText = asymmetricEncryptDecrypt.Encrypt(text,
                Utils.CreateRsaPublicKey(cert3072));

            Console.WriteLine("");
            Console.WriteLine("-- Encrypted Text --");
            Console.WriteLine(encryptedText);

            var decryptedText = asymmetricEncryptDecrypt.Decrypt(encryptedText,
               Utils.CreateRsaPrivateKey(cert3072));

            Console.WriteLine("-- Decrypted Text --");
            Console.WriteLine(decryptedText);
        }
    }
}
