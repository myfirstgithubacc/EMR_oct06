using System;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace BaseC
{
    public class EncryptDecrypt
    {
         //private byte[] key = { 101, 102, 113, 114, 225, 226, 227, 228, 229, 110,
         //121, 212, 139, 119, 244, 221, 245, 245, 245, 220,
         //201, 202, 203, 204 };
         //private byte[] iv = { 65, 110, 68, 26, 69, 178, 200, 219 };
        private string sKey = "";
      
        public EncryptDecrypt()
        {

        }

        public EncryptDecrypt(string skey)
        {
            sKey = skey;
        }

        //public string Encrypt(string toEncrypt, string key, bool useHashing, byte[] PrivateKey)
        //{
        //    try
        //    {
        //        byte[] keyArray;
        //        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
        //        byte[] tmpArray;
        //        if (PrivateKey != null)
        //        {
        //            int length = toEncryptArray.Length + PrivateKey.Length;
        //            tmpArray = new byte[length];
        //            toEncryptArray.CopyTo(tmpArray, 0);
        //            PrivateKey.CopyTo(tmpArray, toEncryptArray.Length);
        //        }
        //        else
        //        {
        //            //int length = toEncryptArray.Length + CreateSalt1().Length;
        //            //tmpArray = new byte[length];
        //            //toEncryptArray.CopyTo(tmpArray, 0);
        //            //CreateSalt1().CopyTo(tmpArray, toEncryptArray.Length);

        //            int length = toEncryptArray.Length;
        //            tmpArray = new byte[length];
        //            toEncryptArray.CopyTo(tmpArray, 0);
        //        }

        //        if (useHashing)
        //        {
        //            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
        //            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
        //        }
        //        else
        //            keyArray = UTF8Encoding.UTF8.GetBytes(key);

        //        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        //        tdes.Key = keyArray;
        //        tdes.Mode = CipherMode.ECB;
        //        tdes.Padding = PaddingMode.PKCS7;

        //        ICryptoTransform cTransform = tdes.CreateEncryptor();
        //        byte[] resultArray = cTransform.TransformFinalBlock(tmpArray, 0, tmpArray.Length);
        //        //int length = resultArray.Length + CreateSalt1().Length;
        //        //byte[] tmpArray = new byte[length];
        //        //resultArray.CopyTo(tmpArray, 0);
        //        //CreateSalt1().CopyTo(tmpArray, resultArray.Length);
        //        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public string Encrypt(string toEncrypt, string key, bool useHashing, string PrivateKey)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
                byte[] tmpArray;
                byte[] bPrivateKey;
                if (PrivateKey != "")
                {
                    bPrivateKey = UTF8Encoding.UTF8.GetBytes(PrivateKey);
                    int length = toEncryptArray.Length + PrivateKey.Length;
                    tmpArray = new byte[length];
                    toEncryptArray.CopyTo(tmpArray, 0);
                    bPrivateKey.CopyTo(tmpArray, toEncryptArray.Length);
                }
                else
                {
                    //int length = toEncryptArray.Length + CreateSalt1().Length;
                    //tmpArray = new byte[length];
                    //toEncryptArray.CopyTo(tmpArray, 0);
                    //CreateSalt1().CopyTo(tmpArray, toEncryptArray.Length);

                    int length = toEncryptArray.Length;
                    tmpArray = new byte[length];
                    toEncryptArray.CopyTo(tmpArray, 0);
                }

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(tmpArray, 0, tmpArray.Length);
                //int length = resultArray.Length + CreateSalt1().Length;
                //byte[] tmpArray = new byte[length];
                //resultArray.CopyTo(tmpArray, 0);
                //CreateSalt1().CopyTo(tmpArray, resultArray.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string Decrypt(string toDecrypt, string key, bool useHashing)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string getKey(string sConstring)
        {
            DAL.DAL dL = new DAL.DAL(DAL.DAL.DBType.SqlServer,sConstring);
            string str = (string)dL.ExecuteScalar(CommandType.Text, "Select sKey from EnKeys WITH (NOLOCK)");
            return str; 
        }

        public string CreateSalt()
        {
            byte[] bytSalt = new byte[8];
            RNGCryptoServiceProvider rng;

            rng = new RNGCryptoServiceProvider();

            rng.GetBytes(bytSalt);

            return Convert.ToBase64String(bytSalt);
        }

        public byte[] CreateSalt1()
        {
            byte[] bytSalt = new byte[8];
            RNGCryptoServiceProvider rng;

            rng = new RNGCryptoServiceProvider();

            rng.GetBytes(bytSalt);

            return bytSalt;
        }

        public byte[] GetKeyFromDatabase()
        {
            byte[] bytSalt = new byte[8];
            RNGCryptoServiceProvider rng;

            rng = new RNGCryptoServiceProvider();

            rng.GetBytes(bytSalt);

            return bytSalt;
        }

        //public string Encrypt(string plainText)
        //{
        //    // Declare a UTF8Encoding object so we may use thse GetByte
        //    // method to transform the plainText into a Byte array.
        //    UTF8Encoding utf8encoder = new UTF8Encoding();
        //    byte[] inputInBytes = utf8encoder.GetBytes(plainText);

        //    // Create a new TripleDES service provider
        //    TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();

        //    // The ICryptTransform interface uses the TripleDES
        //    // crypt provider along with encryption key and init vector
        //    // information
        //    ICryptoTransform cryptoTransform = tdesProvider.CreateEncryptor(this.key, this.iv);

        //    // cryptographic functions need a stream to output the
        //    // encrypted information. Here we declare a memory stream
        //    // for this purpose.
        //    MemoryStream encryptedStream = new MemoryStream();
        //    CryptoStream cryptStream = new CryptoStream(encryptedStream, cryptoTransform, CryptoStreamMode.Write);

        //    // Write the encrypted information to the stream. Flush the information
        //    // when done to ensure everything is out of the buffer.
        //    cryptStream.Write(inputInBytes, 0, inputInBytes.Length);
        //    cryptStream.FlushFinalBlock();
        //    encryptedStream.Position = 0;

        //    // Read the stream back into a Byte array and return it to the calling
        //    // method.
        //    byte[] tmpResult = new byte[encryptedStream.Length];
        //    encryptedStream.Read(tmpResult, 0, Convert.ToInt64(encryptedStream.Length));
        //    cryptStream.Close();
        //    string result = System.Text.UTF32Encoding.UTF32.GetString(tmpResult);
        //    return result;
        //}

        //public string Decrypt(string input)
        //{
        //    // UTFEncoding is used to transform the decrypted Byte Array
        //    // information back into a string.
        //    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        //    byte[] inputInBytes = encoding.GetBytes(input);
        //    UTF8Encoding utf8encoder = new UTF8Encoding();
        //    TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();

        //    // As before we must provide the encryption/decryption key along with
        //    // the init vector.
        //    ICryptoTransform cryptoTransform = tdesProvider.CreateDecryptor(this.key, this.iv);

        //    // Provide a memory stream to decrypt information into
        //    MemoryStream decryptedStream = new MemoryStream();
        //    CryptoStream cryptStream = new CryptoStream(decryptedStream, cryptoTransform, CryptoStreamMode.Write);
        //    cryptStream.Write(inputInBytes, 0,inputInBytes.Length);
        //    //cryptStream.FlushFinalBlock();
        //    decryptedStream.Position = 0;

        //    // Read the memory stream and convert it back into a string
        //    byte[] result = new byte[decryptedStream.Length];
        //    decryptedStream.Read(result, 0, Convert.ToInt64(decryptedStream.Length.ToString()));
        //    cryptStream.Close();
        //    UTF8Encoding myutf = new UTF8Encoding();
        //    return myutf.GetString(result);
        //}
    }
}