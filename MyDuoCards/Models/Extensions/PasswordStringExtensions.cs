using System.Security.Cryptography;
using System.Text;

namespace MyDuoCards.Models.Extensions
{
	static class PasswordStringExtensions
	{
		public static string ToHash(this string pswd)
		{
			var bytes = Encoding.UTF8.GetBytes(pswd);
			var hashedBytes = SHA256.HashData(bytes);

			StringBuilder sb = new StringBuilder();
			foreach (var item in hashedBytes)
			{
				sb.Append(item);
			}

			return sb.ToString();
		}
	}
}
