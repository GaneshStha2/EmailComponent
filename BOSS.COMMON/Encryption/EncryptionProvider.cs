using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace BOSS.COMMON.Encryption
{
    public static class EncryptionProvider
    {

        public static string EncodeValue(string plaintext)
        {
            byte[] rgbIV;
            byte[] key;

            RijndaelManaged rijndael = BuildRigndaelCommon(out rgbIV, out key);

            //convert plaintext into a byte array
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

            int BlockSize;
            BlockSize = 16 * (1 + (plaintext.Length / 16));
            Array.Resize(ref plaintextBytes, BlockSize);

            // fill the remaining space with 0
            for (int i = plaintext.Length; i < BlockSize; i++)
            {
                plaintextBytes[i] = 0;
            }

            byte[] cipherTextBytes = null;
            //create uninitialized Rijndael encryption obj
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                //Call SymmetricAlgorithm.CreateEncryptor to create the Encryptor obj
                var transform = rijndael.CreateEncryptor();

                //Chaining mode
                symmetricKey.Mode = CipherMode.CFB;

                //create encryptor from the key and the IV value
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(key, rgbIV);

                //define memory stream to hold encrypted data
                using (MemoryStream ms = new MemoryStream())
                {
                    //define cryptographic stream - contains the transformation key to be used and the mode
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        //encrypt contents of cryptostream
                        cs.Write(plaintextBytes, 0, BlockSize);
                        cs.FlushFinalBlock();

                        //convert encrypted data from a memory stream into a byte array
                        cipherTextBytes = ms.ToArray();
                    }
                }
            }

            //store result as a hex value
            string hexOutput = BitConverter.ToString(cipherTextBytes).Replace("-", "");
            hexOutput = hexOutput.Substring(0, plaintext.Length * 2);

            //finially return encrypted string
            return hexOutput;
        }

        public static string DecodeValue(string disguisedtext)
        {
            byte[] rgbIV;
            byte[] key;

            BuildRigndaelCommon(out rgbIV, out key);

            byte[] disguishedtextBytes = FromHexString(disguisedtext);

            string visiabletext = "";
            //create uninitialized Rijndael encryption obj
            using (var symmetricKey = new RijndaelManaged())
            {
                //Call SymmetricAlgorithm.CreateEncryptor to create the Encryptor obj
                symmetricKey.Mode = CipherMode.CFB;
                //create encryptor from the key and the IV value

                // ICryptoTransform encryptor = symmetricKey.CreateEncryptor(key, rgbIV);
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(key, rgbIV);

                //define memory stream to hold encrypted data
                using (MemoryStream ms = new MemoryStream(disguishedtextBytes))
                {
                    //define cryptographic stream - contains the transformation to be used and the mode
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {

                        byte[] plaintextBytes = new Byte[disguishedtextBytes.Length];
                        cs.Write(disguishedtextBytes, 0, disguishedtextBytes.Length);
                        cs.FlushFinalBlock();

                        //convert decrypted data from a memory stream into a byte array
                        byte[] visiabletextBytes = ms.ToArray();

                        visiabletext = Encoding.UTF8.GetString(visiabletextBytes);
                    }
                }
            }
            return visiabletext;
        }

        private static RijndaelManaged BuildRigndaelCommon(out byte[] rgbIV, out byte[] key)
        {
            rgbIV = new byte[] { 0x0, 0x1, 0x2, 0x3, 0x5, 0x6, 0x7, 0x8, 0xA, 0xB, 0xC, 0xD, 0xF, 0x10, 0x11, 0x12 };

            key = new byte[] { 0x0, 0x1, 0x2, 0x3, 0x5, 0x6, 0x7, 0x8, 0xA, 0xB, 0xC, 0xD, 0xF, 0x10, 0x11, 0x12 };

            //Specify the algorithms key & IV
            RijndaelManaged rijndael = new RijndaelManaged { BlockSize = 128, IV = rgbIV, KeySize = 128, Key = key, Padding = PaddingMode.None };

            return rijndael;
        }

        public static byte[] FromHexString(string hexString)
        {
            if (hexString == null)
            {
                return new byte[0];
            }

            var numberChars = hexString.Length;
            var bytes = new byte[numberChars / 2];

            for (var i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }
    }

}