using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Core
{
	public class Hasher
	{
		private readonly HashAlgorithm _md5;
		private readonly HashAlgorithm _sha1;
		private readonly HashAlgorithm _sha512;

		public Hasher()
		{
			_md5 = MD5.Create();
			_sha1 = SHA1.Create();
			_sha512 = SHA512.Create();
		}

		public string GetHash(string filePath, HashType hasher)
		{
			using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			return GetHash(fs, hasher);
		}

		public string GetHash(Stream s, HashType hasher)
		{
			var hash = GetHashAlgorithm(hasher).ComputeHash(s);
			return BitConverter.ToString(hash).Replace("-", "");
		}

		private HashAlgorithm GetHashAlgorithm(HashType hasher)
		{
			if (hasher == HashType.Sha512)
				return _sha512;
			else if (hasher == HashType.Sha1)
				return _sha1;
			else
				return _md5;
		}

		public void Dispose()
		{
			_md5?.Dispose();
			_sha1?.Dispose();
			_sha512.Dispose();
		}
	}
}
