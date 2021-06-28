using System;

namespace NewsAggregator.Core.DataTransferObjects
{
    public class RefreshTokenDto
    {
        public Guid Id { get; set; }

        public DateTime ExpiresUtc { get; set; }

        public string Token { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid UserId { get; set; }
    }
}