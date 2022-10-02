using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Core
{

	public class HashCalc
	{
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
			if (hasher == HashType.Sha1)
				return new SHA1CryptoServiceProvider();
			else if (hasher == HashType.Sha512)
				return new SHA512CryptoServiceProvider(); 
			else
				return new MD5CryptoServiceProvider(); 
		}
	}
}
