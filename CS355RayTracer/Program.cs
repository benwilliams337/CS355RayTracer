using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS355RayTracer
{
	class Program
	{
		static void Main(string[] args)
		{
			RayTracer rayTracer = new RayTracer();
			//rayTracer.scene = Scene.InitTestScene();
			//rayTracer.scene = Scene.InitRenderOne();
			//rayTracer.scene = Scene.InitRenderTwo();
			rayTracer.scene = Scene.InitRenderThree();
			rayTracer.Render();
		}
	}
}
