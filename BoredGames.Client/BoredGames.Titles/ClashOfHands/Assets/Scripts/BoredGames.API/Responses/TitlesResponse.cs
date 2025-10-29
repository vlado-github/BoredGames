namespace Assets.Scripts.BoredGames.API.Responses
{
    [System.Serializable]
    public class TitleResponse : IResponse
    {
        public long id;
        public string name;
        public string thumbnailImageUrl;
    }

    [System.Serializable]
    public class TitlesResponse : IResponse
    {
        public TitleResponse[] titles;
    }
}
