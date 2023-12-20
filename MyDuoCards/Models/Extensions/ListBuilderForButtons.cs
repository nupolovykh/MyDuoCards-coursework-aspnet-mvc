namespace MyDuoCards.Models.Extensions
{
	public static class ListBuilderForButtons
	{
		public static List<int> GetButtonIndexes(int currentIndex, int totalNumberOfPages)
		{
			List<int> buttonIndexes = new List<int>();

			buttonIndexes.Add(1);
			buttonIndexes.Add(totalNumberOfPages);

			for (int i = currentIndex - 3; i <= currentIndex + 3; i++)
			{
				if (i > 1 && i < totalNumberOfPages)
				{
					buttonIndexes.Add(i);
				}
			}

			buttonIndexes = buttonIndexes.Distinct().OrderBy(x => x).ToList();

			return buttonIndexes;
		}
	}
}
