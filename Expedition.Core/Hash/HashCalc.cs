using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Core
{
	public enum HashType
	{
		Sha1 = 40,
		Md5 = 32,
	}

	public class HashCalc
	{
		public static string GetSHA1(string uri)
		{
			using (var sha1 = new SHA1CryptoServiceProvider())
				return GetHash(uri, sha1);
		}

		public static string GetSHA1(Stream stream)
		{
			using (var sha1 = new SHA1CryptoServiceProvider())
				return GetHash(stream, sha1);
		}

		public static string GetMD5(string uri)
		{
			using (var md5 = new MD5CryptoServiceProvider())
				return GetHash(uri, md5);
		}

        public static bool CheckMD5(string uri, string hash)
        {
            try
            {
                var hash2 = GetMD5(uri);
                return String.Compare(hash, hash2, true) == 0;
            }
            catch { return false; }
        }

        public static string GetMD5(Stream stream)
		{
			using (var md5 = new MD5CryptoServiceProvider())
				return GetHash(stream, md5);
		}

		public static string GetHash(string filePath, HashAlgorithm hasher)
		{
			using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				return GetHash(fs, hasher);
		}

		public static string GetHash(Stream s, HashAlgorithm hasher)
		{
			var hash = hasher.ComputeHash(s);
			return BitConverter.ToString(hash).Replace("-", "");
		}

		public static HashAlgorithm GetHashAlgorithm(HashType hasher)
		{
			if (hasher == HashType.Md5)
				return new MD5CryptoServiceProvider();
			else
				return  new SHA1CryptoServiceProvider();
		}
	}
}
