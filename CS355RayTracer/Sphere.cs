using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS355RayTracer
{
	public class Sphere : Shape
	{
		public Vector center { get; set; }
		public double radius { get; set; }

		public Sphere() : base()
		{
			
		}

		public override double? Intersect(Ray ray)
		{
			//d = ray direction
			//o = ray origin
			//c = sphere center
			//2(xd*xo - xd*xc + yd*yo - yd*yc + zd*zo - zd*zc)
			double B = 2 * (
				ray.direction.x * ray.origin.x - ray.direction.x * center.x +
				ray.direction.y * ray.origin.y - ray.direction.y * center.y +
				ray.direction.z * ray.origin.z - ray.direction.z * center.z);

			//xo2- 2xoxc+ xc2+ yo2- 2yoyc+ yc2+ zo2-2zozc+ zc2 - r2 
			double C = 
				Math.Pow(ray.origin.x, 2) - 2 * ray.origin.x * center.x + Math.Pow(center.x, 2) +
				Math.Pow(ray.origin.y, 2) - 2 * ray.origin.y * center.y + Math.Pow(center.y, 2) +
				Math.Pow(ray.origin.z, 2) - 2 * ray.origin.z * center.z + Math.Pow(center.z, 2) -
				Math.Pow(radius, 2);

			double discriminant = Math.Pow(B, 2) - 4 * C;

			if(discriminant < 0)
				return null; //No intersection

			double t = (-B - Math.Sqrt(discriminant)) / 2;
			if(t <= 0)
			{
				t = (-B + Math.Sqrt(discriminant)) / 2;
				if(t <= 0)
					return null;
			}

			return t;
		}

		public override Vector getNormal(Vector intersectionPoint)
		{
			return ((1 / radius) * (intersectionPoint - center)).normalize();
		}
	}
}
