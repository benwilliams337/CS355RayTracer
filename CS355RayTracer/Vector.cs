using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS355RayTracer
{
	public class Vector
	{
		public double x { get; set; }
		public double y { get; set; }
		public double z { get; set; }

		public Vector()
		{
			x = 0;
			y = 0;
			z = 0;
		}

		public Vector (double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public double length()
		{
			return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
		}

		public double dotProduct(Vector other)
		{
			return (this.x * other.x) + (this.y * other.y) + (this.z * other.z);
		}

		public Vector crossProduct(Vector other)
		{
			Vector result = new Vector();
			result.x = this.y * other.z - this.z * other.y;
			result.y = this.z * other.x - this.x * other.z;
			result.z = this.x * other.y - this.y * other.x;
			return result;
		}

		public Vector normalize()
		{
			double length = this.length();
			return new Vector(this.x / length, this.y / length, this.z / length);
		}

		public static Vector operator +(Vector op1, Vector op2)
		{
			return new Vector(op1.x + op2.x, op1.y + op2.y, op1.z + op2.z);
		}

		public static Vector operator -(Vector op1, Vector op2)
		{
			return new Vector(op1.x - op2.x, op1.y - op2.y, op1.z - op2.z);
		}

		public static Vector operator *(double scalar, Vector vector)
		{
			return new Vector(scalar * vector.x, scalar * vector.y, scalar * vector.z);
		}

		public Vector2D dropX()
		{
			return new Vector2D(y, z);
		}	
		
		public Vector2D dropY()
		{
			return new Vector2D(x, z);
		}

		public Vector2D dropZ()
		{
			return new Vector2D(x, y);
		}
	}

	public class Vector2D
	{
		public double u { get; set; }
		public double v { get; set; }

		public Vector2D()
		{
			u = 0;
			v = 0;
		}

		public Vector2D(double u, double v)
		{
			this.u = u;
			this.v = v;
		}

		public static Vector2D operator -(Vector2D op1, Vector2D op2)
		{
			return new Vector2D(op1.u - op2.u, op1.v - op2.v);
		}
	}
}
