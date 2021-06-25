using System;

namespace NewsAggregator.Core.DataTransferObjects
{
    public class RoleDto : IDtoModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}