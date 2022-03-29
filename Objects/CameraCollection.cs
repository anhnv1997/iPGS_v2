using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.iZCU.Objects
{
    public class CameraCollection : CollectionBase
    {
        // Constructor
        public CameraCollection()
        {

        }

        public Camera this[int index]
        {
            get { return (Camera)InnerList[index]; }
        }

        // Add
        public void Add(Camera Camera)
        {
            InnerList.Add(Camera);
        }

        // Remove
        public void Remove(Camera Camera)
        {
            InnerList.Remove(Camera);
        }

        // Get Camera by it's ID
        public Camera GetCameraByID(string id)
        {
            foreach (Camera Camera in InnerList)
            {
                if (Camera.ID == id)
                {
                    return Camera;
                }
            }
            return null;
        }

        // Get Camera by it's name
        public Camera GetCameraByName(string name)
        {
            foreach (Camera Camera in InnerList)
            {
                if (Camera.CameraName == name)
                {
                    return Camera;
                }
            }
            return null;
        }

        public Camera GetCameraByIP(string ipAddress)
        {
            foreach (Camera Camera in InnerList)
            {
                if (Camera.IP == ipAddress)
                {
                    return Camera;
                }
            }
            return null;
        }
    }
}
