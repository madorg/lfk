using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkClient.UserMessages
{
    public enum InvalidRepositoryOpenReasons
    {
        None = 0,
        FolderDoesNotContainRepository = 1,
        RepositoryDoNotBelongToUser = 2
    }
}
