using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.External.Contracts
{
    public class FacebookUserInfoResult
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("picture")]
        public FacebookPicture Picture { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }

        public class FacebookPicture 
        {
            [JsonProperty("data")]
            public FacebookPictureData Data { get; set; }
        }

        public class FacebookPictureData 
        {
            [JsonProperty("height")]
            public long Height { get; set; }
            [JsonProperty("is_silhouette")]
            public bool IsSilHouette { get; set; }
            [JsonProperty("url")]
            public Uri Url { get; set; }
            [JsonProperty("width")]
            public long Width { get; set; }
        }
    }
}
