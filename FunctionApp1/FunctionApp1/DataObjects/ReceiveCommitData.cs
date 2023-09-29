using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubToFileShareApp.DataObjects
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class ReceiveCommitData
    {
        [JsonProperty("repository.owner.name")]
        public string RepoOwner { get; set; }

        [JsonProperty("repository.name")]
        public string RepoName { get; set; }

        [JsonProperty("commits[0].id")]
        public string CommitId { get; set; }

        [JsonProperty("commits[0].message")]
        public string CommitMessage { get; set; }

        [JsonProperty("commits[0].committer.name")]
        public string CommiterName { get; set; }
    }
}
