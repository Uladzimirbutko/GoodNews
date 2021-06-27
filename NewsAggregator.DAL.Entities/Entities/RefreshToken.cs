using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAggregator.DAL.Core.Entities
{
    public class RefreshToken : IBaseEntity
    {
        public Guid Id { get; set; }

        public DateTime ExpiresUtc { get; set; }

        [Required]
        public string Token { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}