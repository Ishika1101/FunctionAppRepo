using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubToFileShareApp
{
    public interface IGithub
    {
        public Task<string> GetCommitDetails(string repositoryOwner, string repositoryName, string commitId);
    }
}
