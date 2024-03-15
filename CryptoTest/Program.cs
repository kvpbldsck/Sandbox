using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        // Ваш текст для подписи
        string originalText = "123";
        // Ваш приватный ключ в формате PEM PKCS#8
        string privateKeyPem = @"-----BEGIN PRIVATE KEY-----
MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAKjh32jpH3KALmQS
2fLyVSsQHRNiC5ag9kXBhPPDTXRyoL9K1tpN2WrZ+hjaR3V0N+EgWvivt/1gB81J
NwXMHweMtiiIq0NWEHBB5zrPUaTg7gCmA/0eFSgyb6e1rbKapQ8JUoe+VPT5Hbs9
vJPyWihHv1XlzyRSsC0boG5aviYxAgMBAAECgYB26o1IrxZwpqeD2e5T7qVf7Dm9
A0XLq82dsrVue7AXdfuQlx8Qms6kOZasV08H+my6ffDwubKhnDQCLjKrR1706ZAW
Talp/r01wMiG6NWFP8Fxy4Ih/IJz7D9dyyvO6ZV9MWOzv+YhqeGa4depqniSUxZI
lr3xzBdmVXUhwa/0wQJBAN1SrkepjbHO7vrrUbZ8Bz1ZCbEkb0Cv0IDggAwwoV7x
ULej5hsAFXepgHQ3E0meXeivnYY9CSjEyPofKz4cS5kCQQDDV8jAluOUXHaK/Tiw
vSxOy7cgnC1aAbZ5X2QMjs1bhTIYib1k8fPhrHoZJD8ovwDuyyICK5R4ctvE0ghM
YY5ZAkBPMhXQ6NgeJxkWynu729fPWPwB2jLBJ+hpJWxMcF5eFeW3QLvHL5l+7zVa
4JZTQosCB01Lyq1rXDc6YUrc980JAkAQgP0sEFN/+GPnkEdW31S7/4gkUC2guh5R
mNaaHQKKpfE6k4CV479IJMtYDnDrn1+TzUO9TTNJDeM1eM1Mrr1ZAkAxXf0YO6v1
0ScZO/1zc+jh8OITC882+9Zgtl83+Yoe7pZWTzbZ0bBxLlWJvra38itXiwHj90MF
lHj5w/iQVAgs
-----END PRIVATE KEY-----";

        string publicKeyPem = @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCo4d9o6R9ygC5kEtny8lUrEB0T
YguWoPZFwYTzw010cqC/StbaTdlq2foY2kd1dDfhIFr4r7f9YAfNSTcFzB8HjLYo
iKtDVhBwQec6z1Gk4O4ApgP9HhUoMm+nta2ymqUPCVKHvlT0+R27PbyT8looR79V
5c8kUrAtG6BuWr4mMQIDAQAB
-----END PUBLIC KEY-----";

        string sign =
            "ZOGIp/FBW8xO10r2iED33OABlIXSuMgtuEA79qYc45fE3+t7B7A9GJjZ0VaCj92XIcJuYN1kBoPK\nCTgHA34slDXuR9+Bbd8lCThk2SSK3S7LW6idrMFn3uGKxgccProaIKs/aCIhA84vUpDI1ZExc6DR\nOZ6c0yc42GdniUl6T/k=";

        try
        {
            byte[] bytesToSign = Encoding.UTF8.GetBytes(originalText);

            using RSA rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem);

            byte[] signature = rsa.SignData(bytesToSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            string signatureBase64 = Convert.ToBase64String(signature);

            Console.WriteLine(new {signatureBase64});
        }
        catch (Exception ex)
        {
            Console.WriteLine("error: " + ex.Message);
        }
        
        try
        {
            // Преобразование текста в массив байтов
            byte[] bytesToSign = Encoding.UTF8.GetBytes(originalText);
            byte[] bytesToVerify = Convert.FromBase64String(sign);

            // Загрузка приватного ключа
            using RSA rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem);

            // Создание подписи
            var isVerified = rsa.VerifyData(bytesToSign, bytesToVerify, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            // Преобразование подписи в строку Base64

            Console.WriteLine(new {isVerified});
        }
        catch (Exception ex)
        {
            Console.WriteLine("error: " + ex.Message);
        }
    }
}
