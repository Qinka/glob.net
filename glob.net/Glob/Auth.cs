using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace Glob.Auth
{
    public class TransPsk2Token
    {
        private string[] psks;
        private string timestampeStr;
        public  string tokens { get { return flashToken(); } }
        public TransPsk2Token(string[] ps,string timestame)
        {
            psks = ps;
            timestampeStr = timestame;
        }
        public string flashToken()
        {
            var sha1 = HashAlgorithmNames.Sha1;
            var sha256 = HashAlgorithmNames.Sha256;
            var sha224 = "SHA224";
            var sha384 = HashAlgorithmNames.Sha384;
            var sha512 = HashAlgorithmNames.Sha512;
            var hash256 = HashAlgorithmProvider.OpenAlgorithm(sha256);
            var hash1 = HashAlgorithmProvider.OpenAlgorithm(sha1);
            var ch1 = hash1.CreateHash();
            var ch256 = hash256.CreateHash();
            var timestrBuf = CryptographicBuffer.ConvertStringToBinary(timestampeStr, BinaryStringEncoding.Utf8);
            ch1.Append(timestrBuf);
            var timestampeSha = CryptographicBuffer.EncodeToHexString(ch1.GetValueAndReset());
            var refToken = psks[0]+ timestampeSha;
            var refTokenBuf = CryptographicBuffer.ConvertStringToBinary(refToken, BinaryStringEncoding.Utf8);
            ch256.Append(refTokenBuf);
            refToken = CryptographicBuffer.EncodeToHexString(ch256.GetValueAndReset());
            var ca = 0;
            for (var i=0;i<refToken.Length;++i)
            {
                var tmp = (int)refToken[i];
                if (tmp >= 48 && tmp <= 57)
                    ca += tmp - 48;
                else
                    ca += tmp - 87;
            }
            CryptographicHash chdo;
            switch (ca % 4)
            {
                case 0:
                    chdo = hash256.CreateHash();
                    break;
                case 1:
                    var hash512 = HashAlgorithmProvider.OpenAlgorithm(sha512);
                    chdo = hash512.CreateHash();
                    break;
                case 2:
                    chdo = hash1.CreateHash();
                    break;
                case 3:
                    var hash384 = HashAlgorithmProvider.OpenAlgorithm(sha384);
                    chdo = hash384.CreateHash();
                    break;
                default:
                    var hash224 = HashAlgorithmProvider.OpenAlgorithm(sha224);
                    chdo = hash224.CreateHash();
                    break;
            }
            psks = changOrder(psks,ca % psks.Length);
            var finToken = "";
            for (var i=0;i<psks.Length;++i)
            {
                finToken += psks[i];
            }
            finToken += timestampeSha;
            var finTBuf = CryptographicBuffer.ConvertStringToBinary(finToken, BinaryStringEncoding.Utf8);
            ch256.Append(finTBuf);
            finToken = CryptographicBuffer.EncodeToHexString(ch256.GetValueAndReset());
            return finToken.Substring(0, 56);
        }
        private string[] changOrder (string[] xs,int p)
        {
            var ys = new List<string>(xs);
            for (var i = 0; i < xs.Length; ++i)
            {
                ys.Add(xs[i]);
            }
            ys.RemoveRange(0, p);
            return ys.Take(xs.Length).ToArray();
        }
    }
}
