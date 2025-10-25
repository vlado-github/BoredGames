namespace Assets.Scripts
{
	public class RoundScoreResult
	{
		public RoundScoreResultEnum Result { get; set; }
		public string SelectedCardTag { get; set; }
	}

	public enum RoundScoreResultEnum
    {
		Win,
		Loss,
		Draw
	}
}