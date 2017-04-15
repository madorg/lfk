using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkClient.Repository
{
    class Repository
    {
        private static Repository instance = null;

        private Repository()
        {
        }
        public static Repository GetInstance()
        {
            lock (instance)
            {
                if (instance == null)
                {
                    instance = new Repository();
                }
            }
            return instance;
        }


    }
}
