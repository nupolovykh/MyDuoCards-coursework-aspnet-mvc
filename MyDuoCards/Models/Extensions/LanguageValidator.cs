namespace MyDuoCards.Models.Extensions
{
	public static class LanguageValidator
	{
		public static bool IsRussian(string input)
		{
			foreach (char c in input)
			{
				if (c < 0x0400 || c > 0x04FF)
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsEnglish(string input)
		{
			foreach (char c in input)
			{
				if (c > 0x00FF)
				{
					return false;
				}
			}
			return true;
		}
	}
}
