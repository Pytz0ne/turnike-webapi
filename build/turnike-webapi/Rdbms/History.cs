using System;
using System.Collections.Generic;

namespace turnike_webapi.Rdbms
{
    public partial class History
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public DateTime? Revoked { get; set; }

        public virtual User User { get; internal set; }
    }
}
