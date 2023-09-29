using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubToFileShareApp.DataObjects
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class GithubCodeChanges
    {
        [JsonProperty("files")]
        public List<CodeChanges> filesList { get; set; }

        [JsonProperty("commit.committer.date")]
        public DateTime changeDate { get; set; }
    }



    public class CodeChanges
    {
        public string sha { get; set; }
        public string filename { get; set; }
        public string status { get; set; }
        public int additions { get; set; }
        public int deletions { get; set; }
        public int changes { get; set; }
        public string blob_url { get; set; }
        public string raw_url { get; set; }
        public string contents_url { get; set; }
        public string patch { get; set; }
    }

}
