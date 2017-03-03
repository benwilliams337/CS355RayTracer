using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS355RayTracer
{
	public class Ray
	{
		public Vector origin { get; set; }
		public Vector direction { get; set; }

		public Ray()
		{
			origin = new Vector();
			direction = new Vector();
		}

		public Ray(Vector origin, Vector direction)
		{
			this.origin = origin;
			this.direction = direction;
		}

		public Vector getParameterizedPoint(double t)
		{
			return origin + (t * direction);
		}

		public Ray getOffsetRay()
		{
			return new Ray(getParameterizedPoint(0.0001), direction);
		}
	}
}
