using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkClient.UserMessages
{
    public enum InvalidRepositoryCreationReasons
    {
        None = 0,
        DuplicateTitle = 1,
        FolderAlreadyContainsRepository =2
    }
}
