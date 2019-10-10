//SimpleAES

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace YtoX.Core.Helpers
{
    public class SimpleAES
    {
        private byte[] Key = { 124, 210, 11, 14, 25, 36, 95, 75, 129, 84, 37, 112, 29, 122, 223, 207, 242, 20, 170, 145, 171, 59, 191, 26, 214, 22, 18, 212, 132, 235, 54, 208 };
        private byte[] Vector = { 136, 44, 121, 131, 21, 2, 117, 15, 201, 41, 52, 233, 49, 32, 114, 255 };

        private ICryptoTransform EncryptorTransform, DecryptorTransform;
        private UTF8Encoding UTFEncoder;

        public SimpleAES()
        {
            RijndaelManaged rm = new RijndaelManaged();
            EncryptorTransform = rm.CreateEncryptor(this.Key, this.Vector);
            DecryptorTransform = rm.CreateDecryptor(this.Key, this.Vector);

            UTFEncoder = new System.Text.UTF8Encoding();
        }

        public string EncryptToString(string text)
        {
            return ByteArrToString(Encrypt(text));
        }

        public byte[] Encrypt(string text)
        {
            Byte[] bytes = UTFEncoder.GetBytes(text);

            MemoryStream memoryStream = new MemoryStream();

            //write decrypted-value to encryption-stream
            CryptoStream cs = new CryptoStream(memoryStream, EncryptorTransform, CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();

            //read encrypted-value back out of the stream
            memoryStream.Position = 0;
            byte[] encrypted = new byte[memoryStream.Length];
            memoryStream.Read(encrypted, 0, encrypted.Length);

            cs.Close();
            memoryStream.Close();

            return encrypted;
        }

        public string DecryptString(string encrypted)
        {
            return Decrypt(StrToByteArray(encrypted));
        }

        public string Decrypt(byte[] encrypted)
        {
            //write encrypted-value to the decryption stream
            MemoryStream encryptedStream = new MemoryStream();
            CryptoStream decryptStream = new CryptoStream(encryptedStream, DecryptorTransform, CryptoStreamMode.Write);
            decryptStream.Write(encrypted, 0, encrypted.Length);
            decryptStream.FlushFinalBlock();

            //read decrypted-value from the stream
            encryptedStream.Position = 0;
            Byte[] decryptedBytes = new Byte[encryptedStream.Length];
            encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
            encryptedStream.Close();

            return UTFEncoder.GetString(decryptedBytes);
        }

        public byte[] StrToByteArray(string str)
        {
            if (str.Length == 0)
                throw new Exception("Invalid string value in StrToByteArray");

            byte val;
            byte[] byteArr = new byte[str.Length / 3];
            int i = 0;
            int j = 0;
            do
            {
                val = byte.Parse(str.Substring(i, 3));
                byteArr[j++] = val;
                i += 3;
            }
            while (i < str.Length);
            return byteArr;
        }

        public string ByteArrToString(byte[] byteArr)
        {
            byte val;
            string tempStr = "";
            for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
            {
                val = byteArr[i];
                if (val < (byte)10)
                    tempStr += "00" + val.ToString();
                else if (val < (byte)100)
                    tempStr += "0" + val.ToString();
                else
                    tempStr += val.ToString();
            }
            return tempStr;
        }
    }
}
