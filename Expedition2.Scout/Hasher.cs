using System.Security.Cryptography;

namespace Expedition2.Scout
{
	public enum HashType
	{
		Sha512 = 64,
		Sha1 = 40,
		Md5 = 32,
	}

	public class Hasher
	{
		public static string GetHash(string filePath, HashAlgorithm hasher)
		{
			using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			return GetHash(fs, hasher);
		}

		public static string GetHash(Stream s, HashAlgorithm hasher)
		{
			var hash = hasher.ComputeHash(s);
			return BitConverter.ToString(hash).Replace("-", "");
		}

		public static HashAlgorithm GetHashAlgorithm(HashType hasher)
		{
			if (hasher == HashType.Sha512)
				return SHA512.Create();
			else if (hasher == HashType.Sha1)
				return SHA1.Create();
			else
				return MD5.Create();
		}
	}
}
