using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Parameters;
using System.Globalization;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.Pkcs;

namespace BLL.Crypto
{
    public static class RSACrypto
    {
        
        public static byte[] RSAEncrypt(byte[] data, RsaKeyParameters publicKey)
        {
            var cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(true, publicKey);

            return cipher.DoFinal(data);
        }

        public static byte[] RSADecrypt(byte[] encryptedData, RsaPrivateCrtKeyParameters privateKey)
        {
            var cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(false, privateKey);

            return cipher.DoFinal(encryptedData);
        }

        public static void PrintPemKey(AsymmetricKeyParameter key)
        {
            if (key != null)
            {
                using (TextWriter textWriter = new StringWriter())
                {
                    PemWriter pemWriter = new PemWriter(textWriter);
                    pemWriter.WriteObject(key);
                    pemWriter.Writer.Flush();
                    Console.WriteLine(textWriter.ToString());
                }
            }
        }

        public static RsaKeyParameters ConvertPemToPublicKey(string publicKeyPem)
        {
            byte[] keyBytes = Convert.FromBase64String(publicKeyPem);
            Asn1Object publicKeyInfo = Asn1Object.FromByteArray(keyBytes);
            SubjectPublicKeyInfo pkInfo = SubjectPublicKeyInfo.GetInstance(publicKeyInfo);
            RsaKeyParameters publicKey = (RsaKeyParameters)PublicKeyFactory.CreateKey(pkInfo);

            return publicKey;
        }
        public static RsaPrivateCrtKeyParameters ConvertPemToPrivateKey(string privateKeyPem)
        {
            byte[] keyBytes = Convert.FromBase64String(privateKeyPem);
            Asn1Object privateKeyInfo = Asn1Object.FromByteArray(keyBytes);
            PrivateKeyInfo pkInfo = PrivateKeyInfo.GetInstance(privateKeyInfo);
            RsaPrivateCrtKeyParameters privateKey = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(pkInfo);

            return privateKey;
        }
        public static byte[] FromHexString(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

    }
}
