using System;
using System.Collections.Generic;

namespace turnike_webapi.Rdbms
{
    public partial class User
    {
        //public User()
        //{
        //    LoginHistory = new HashSet<LoginHistory>();
        //}

        public int Id { get; internal set; }
        public string Rfid { get; set; }
        public string Name { get; set; }

        protected internal virtual ICollection<History> History { get; internal set; }
    }
}
