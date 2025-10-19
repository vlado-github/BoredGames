using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.BoredGames.API.Responses
{
    [System.Serializable]
    public class TitleResponse
    {
        public long id;
        public string name;
        public string thumbnailImageUrl;
    }

    [System.Serializable]
    public class TitlesResponse
    {
        public TitleResponse[] titles;
    }
}
