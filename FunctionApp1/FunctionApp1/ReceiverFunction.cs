using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using GithubToFileShareApp.DataObjects;
//using System.Threading.Tasks;


namespace GithubToFileShareApp
{
    public class ReceiverFunction
    {
        private readonly ILogger<ReceiverFunction> _logger;
        private readonly GithubAdapter _githubAdapter;
        private readonly FileShareService _fileShareService;

        public ReceiverFunction(ILogger<ReceiverFunction> log)
        {
            _logger = log;
            _githubAdapter = new GithubAdapter(); // Create an instance of GithubAdapter
            _fileShareService = new FileShareService(); // Create an instance of FileShareService
        }

        [FunctionName("ReceiverFunction")]
        public async Task Run([ServiceBusTrigger("nativetopic", "nativesub", Connection = "ServiceBusConn")] string mySbMsg)
        {
            // Call GetCommitDetails method
            _logger.LogInformation(mySbMsg);
            ReceiveCommitData obj = JsonConvert.DeserializeObject<ReceiveCommitData>(mySbMsg);

            string commitDetails = await _githubAdapter.GetCommitDetails(obj.RepoOwner, obj.RepoName, obj.CommitId);
            _logger.LogInformation($"GitHub Commit Details: {commitDetails}");
            GithubCodeChanges fileChangeData = JsonConvert.DeserializeObject<GithubCodeChanges>(commitDetails);

            if (fileChangeData != null)
            {

                List<CodeChanges> list = fileChangeData.filesList.ToList();
                string result = await _fileShareService.uploadCode(list);
                _logger.LogInformation($"FileShare status: {result}");

            }
        }

    }
}
