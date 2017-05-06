﻿using System;
using System.Collections.Generic;

namespace LfkSharedResources.Models
{
    public class Index
    {
        public Index()
        {
            RepoObjectIdAndFileName = new Dictionary<Guid, string>();
        }

        public Guid Id { get; set; }
        public Dictionary<Guid, string> RepoObjectIdAndFileName { get; set; }
    }
}