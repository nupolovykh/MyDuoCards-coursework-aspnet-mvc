using System.Security.Cryptography;

namespace MyDuoCards.Models.Extensions
{
	static class PasswordStringExtensions
	{
		private const int SaltSize = 16;
		private const int HashSize = 32;
		private const int Iterations = 100_000;

		public static string ToHash(this string pswd)
		{
			var salt = RandomNumberGenerator.GetBytes(SaltSize);
			var hash = Rfc2898DeriveBytes.Pbkdf2(pswd, salt, Iterations, HashAlgorithmName.SHA256, HashSize);

			return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
		}

		public static bool VerifyHash(this string pswd, string storedHash)
		{
			var parts = storedHash.Split('.', 3);
			if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
			{
				return false;
			}

			var salt = Convert.FromBase64String(parts[1]);
			var expectedHash = Convert.FromBase64String(parts[2]);
			var actualHash = Rfc2898DeriveBytes.Pbkdf2(pswd, salt, iterations, HashAlgorithmName.SHA256, expectedHash.Length);

			return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
		}
	}
}
