using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.iZCU.Objects
{
	public class CameraTypeCollection
	{
		public static CameraType GetType(string CameraType)
		{
			return (CameraType)Enum.Parse(typeof(CameraType), CameraType, true);
		}
		public static CameraType GetType(int index)
		{
			return (CameraType)index;
		}
	}
}
