using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS355RayTracer
{
	public class MyColor
	{
		public double red { get; set; }
		public double green { get; set; }
		public double blue { get; set; }

		public MyColor()
		{
			//Default to black
			red = 0;
			green = 0;
			blue = 0;
		}

		public MyColor(double red, double green, double blue)
		{
			this.red = red;
			this.green = green;
			this.blue = blue;
		}

		public static System.Drawing.Color ToSystemColor(MyColor c)
		{
			return System.Drawing.Color.FromArgb(Convert.ToByte(c.red * 255), Convert.ToByte(c.green * 255), Convert.ToByte(c.blue * 255));
		}

		public static MyColor operator +(MyColor op1, MyColor op2)
		{
			return new MyColor(Math.Min(op1.red + op2.red, 1.0), Math.Min(op1.green + op2.green, 1.0), Math.Min(op1.blue + op2.blue, 1.0));
		}

		public static MyColor operator *(MyColor op1, MyColor op2)
		{
			return new MyColor(Math.Min(op1.red * op2.red, 1.0), Math.Min(op1.green * op2.green, 1.0), Math.Min(op1.blue * op2.blue, 1.0));
		}

		public static MyColor operator *(MyColor op1, double op2)
		{
			return new MyColor(Math.Min(op1.red * op2, 1.0), Math.Min(op1.green * op2, 1.0), Math.Min(op1.blue * op2, 1.0));
		}
	}
}
