using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.iZCU.Objects
{
    public class UserCollecttion: CollectionBase
    {
        public UserCollecttion()
        {

        }

        public User this[int index]
        {
            get { return (User)InnerList[index]; }
        }

        // Add
        public void Add(User user)
        {
            InnerList.Add(user);
        }

        // Remove
        public void Remove(User user)
        {
            InnerList.Remove(user);
        }

        public User GetUserByID(int id)
        {
            foreach (User user in InnerList)
            {
                if (user.UerId == id)
                {
                    return user;
                }
            }
            return null;
        }

        public User GetUserByUsername(string username)
        {
            foreach (User user in InnerList)
            {
                if (user.UserUsername == username)
                {
                    return user;
                }
            }
            return null;
        }
    }
}
