namespace Assets.Scripts
{
	public class PlayerRoundScoreResult
	{
		public string PlayerId { get; set; }
		public RoundScoreResult RoundResult {get; set;}
	}

	public class RoundScoreResult
	{
		public RoundScoreResultEnum? Result { get; set; } = null;
		public string SelectedCardTag { get; set; }
	}

	public enum RoundScoreResultEnum
    {
		Win,
		Loss,
		Draw
	}
}