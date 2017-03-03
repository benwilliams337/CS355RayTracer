using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS355RayTracer
{
	public abstract class Shape
	{
		public MyColor matteColor { get; set; }
		public MyColor specularColor { get; set; }
		public double phongConstant { get; set; }
		public double ownColorAmount { get; set; }
		public double reflectionAmount { get; set; }
		public double refractionAmount { get; set; }
		public double refractionIndex { get; set; }

		protected Shape()
		{
			ownColorAmount = 1;
			reflectionAmount = 0; //Default to no reflection or refraction
			refractionAmount = 0;
		}

		public abstract double? Intersect(Ray ray);
		public abstract Vector getNormal(Vector intersectionPoint);
	}
}
