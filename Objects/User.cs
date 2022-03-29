using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.iZCU.Objects
{
    public class User
    {
        private int userId;
        private string userFullname;
        private string userUsername;
        private string userPassword;
        private string userDescription;

        public int UerId { get; set; }
        public string UserFullname { get; set; }
        public string UserUsername { get; set; }
        public string UserPassword { get; set; }
        public string UserDescription { get; set; }
    }
}
