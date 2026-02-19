using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDISS.UserRebate.API.Domain
{
    public class UserRebate
    {
        [Key]
        public string UserRebateId { get; set; }
        public string UserId { get; set; }
        public float RebateRate { get; set; }
        public int Status { get; set; }
        //public string AccessToken { get; set; }
        public DateTime VerifiedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime ExpiredAt { get; set; }

        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

        public ICollection<RebateEvent> RebateEvents { get; set; } = new List<RebateEvent>();
    }

}