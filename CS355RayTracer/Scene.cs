using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS355RayTracer
{
	public class Scene
	{
		public List<Shape> shapes { get; set; }
		public MyColor backgroundColor { get; set; }
		public MyColor ambientColor { get; set; }
		public MyColor lightColor { get; set; }
		public Vector directionToLight { get; set; }
		public double distanceToViewPlane { get; set; }
		public double fieldOfView { get; set; }

		private double? _windowSize = null; //Cache
		public double windowSize
		{
			get
			{
				if(!_windowSize.HasValue)
					_windowSize = 2 * distanceToViewPlane * Math.Tan(fieldOfView * Math.PI / 180);
				return _windowSize.Value;
			}
		}

		public Scene()
		{
			shapes = new List<Shape>();
			backgroundColor = new MyColor(); //Constructor defaults to black
		}

		public RayIntersectionResult IntersectRay(Ray ray)
		{
			double curMin = double.MaxValue;
			Shape curIntersectedShape = null;
			foreach(Shape shape in shapes)
			{
				double? intersectResult = shape.Intersect(ray);
				if(intersectResult.HasValue && intersectResult.Value < curMin) {
					curMin = intersectResult.Value;
					curIntersectedShape = shape;
				}
			}
			if(curIntersectedShape != null)
			{
				return new RayIntersectionResult() 
				{
					intersectedShape = curIntersectedShape,
					intersectionPoint = ray.getParameterizedPoint(curMin)
				};
			}
			return null;
		}

		public static Scene InitTestScene()
		{
			Scene result = new Scene();
			result.distanceToViewPlane = 1.0;
			result.fieldOfView = 28.0;

			result.directionToLight = new Vector(1, 1, 0);
			result.lightColor = new MyColor(1, 1, 1);
			result.ambientColor = new MyColor(0.1, 0.1, 0.1);

			result.shapes.Add(new Sphere()
			{
				center = new Vector(),
				radius = 0.2,
				matteColor = new MyColor(0, 0, 1),
				specularColor = new MyColor(0.5, 1, 0.5),
				phongConstant = 32
			});
			//Polygon p = new Polygon();
			//p.points.Add(new Vector(-0.3, -0.2, -0.1));
			//p.points.Add(new Vector(0.3, -0.2, -0.1));
			//p.points.Add(new Vector(0, 0.2, -0.1));
			//result.shapes.Add(p);

			return result;
		}

		public static Scene InitRenderOne()
		{
			Scene result = new Scene();
			result.distanceToViewPlane = 1.0;
			result.fieldOfView = 28.0;
			result.directionToLight = new Vector(1, 0, 0);
			result.lightColor = new MyColor(1, 1, 1);
			result.ambientColor = new MyColor(0.1, 0.1, 0.1);
			result.backgroundColor = new MyColor(0.2, 0.2, 0.2);
			
			Sphere s1 = new Sphere()
			{
				center = new Vector(0.35, 0, -0.1),
				radius = 0.05,
				matteColor = new MyColor(1, 1, 1),
				specularColor = new MyColor(1, 1, 1),
				phongConstant = 4
			};

			Sphere s2 = new Sphere()
			{
				center = new Vector(0.2, 0, -0.1),
				radius = 0.075,
				matteColor = new MyColor(1, 0, 0),
				specularColor = new MyColor(0.5, 1, 0.5),
				phongConstant = 32
			};

			Sphere s3 = new Sphere()
			{
				center = new Vector(-0.6, 0, 0),
				radius = 0.3,
				matteColor = new MyColor(0, 1, 0),
				specularColor = new MyColor(0.5, 1, 0.5),
				phongConstant = 32
			};

			Polygon p1 = new Polygon()
			{
				matteColor = new MyColor(0, 0, 1),
				specularColor = new MyColor(1, 1, 1),
				phongConstant = 32
			};
			p1.points.Add(new Vector(0.3, -0.3, -0.4));
			p1.points.Add(new Vector(0, 0.3, -0.1));
			p1.points.Add(new Vector(-0.3, -0.3, 0.2));
			//p1.points.Add(new Vector(0.3, -0.3, -0.1));
			//p1.points.Add(new Vector(0, 0.3, 0));
			//p1.points.Add(new Vector(-0.3, -0.3, 0.1));

			Polygon p2 = new Polygon()
			{
				matteColor = new MyColor(1, 1, 0),
				specularColor = new MyColor(1, 1, 1),
				phongConstant = 4
			};
			p2.points.Add(new Vector(-0.2, 0.1, 0.1));
			p2.points.Add(new Vector(-0.2, -0.5, 0.2));
			p2.points.Add(new Vector(-0.2, 0.1, -0.3));

			result.shapes.Add(s1);
			result.shapes.Add(s2);
			result.shapes.Add(s3);
			result.shapes.Add(p1);
			result.shapes.Add(p2);

			return result;
		}

		public static Scene InitRenderTwo()
		{
			Scene result = new Scene();
			result.distanceToViewPlane = 1.2;
			result.fieldOfView = 55.0;
			result.directionToLight = new Vector(0, 1, 0);
			result.lightColor = new MyColor(1, 1, 1);
			result.ambientColor = new MyColor(0, 0, 0);
			result.backgroundColor = new MyColor(0.2, 0.2, 0.2);

			Sphere s = new Sphere() {
				center = new Vector(0, 0.3, 0),
				radius = 0.2,
				//matteColor = new MyColor(0.75, 0.75, 0.75),
				matteColor = new MyColor(0, 0, 0),
				reflectionAmount = 0.75
			};

			Polygon p1 = new Polygon()
			{
				matteColor = new MyColor(0, 0, 1),
				specularColor = new MyColor(1, 1, 1),
				phongConstant = 4
			};
			p1.points.Add(new Vector(0, -0.5, 0.5));
			p1.points.Add(new Vector(1, 0.5, 0));
			p1.points.Add(new Vector(0, -0.5, -0.5));

			Polygon p2 = new Polygon()
			{
				matteColor = new MyColor(1, 1, 0),
				specularColor = new MyColor(1, 1, 1),
				phongConstant = 4
			};
			p2.points.Add(new Vector(0, -0.5, 0.5));
			p2.points.Add(new Vector(0, -0.5, -0.5));
			p2.points.Add(new Vector(-1, 0.5, 0));

			result.shapes.Add(s);
			result.shapes.Add(p1);
			result.shapes.Add(p2);

			return result;
		}

		public static Scene InitRenderThree()
		{
			Scene result = new Scene();
			result.distanceToViewPlane = 1.4;
			result.fieldOfView = 35.0;
			result.directionToLight = new Vector(1, 1, 0);
			result.lightColor = new MyColor(0.5, 0.5, 0.5);
			result.ambientColor = new MyColor(0.25, 0.25, 0.25);
			result.backgroundColor = new MyColor(0.3, 0.3, 0.3);

			Sphere s1 = new Sphere()
			{
				matteColor = new MyColor(0, 0, 0),
				specularColor = new MyColor(1, 1, 1),
				phongConstant = 32,
				ownColorAmount = 0.5,
				reflectionAmount = 0.1,
				refractionAmount = 0.9,
				refractionIndex = 0.98,
				center = new Vector(-0.4, 0, 0.1),
				radius = 0.3
			};

			Sphere s3 = new Sphere()
			{
				matteColor = new MyColor(0, 0, 0),
				specularColor = new MyColor(1, 1, 1),
				phongConstant = 32,
				ownColorAmount = 0.5,
				reflectionAmount = 0.1,
				refractionAmount = 0.9,
				refractionIndex = 1.5,
				center = new Vector(0.4, 0, 0.1),
				radius = 0.3
			};

			Sphere s4 = new Sphere()
			{
				matteColor = new MyColor(0, 0, 1),
				specularColor = new MyColor(1, 1, 0),
				phongConstant = 16,
				center = new Vector(1.4, 0, -1),
				radius = 0.1
			};

			Polygon p1 = new Polygon()
			{
				matteColor = new MyColor(1, 1, 0),
				specularColor = new MyColor(1, 1, 1),
				phongConstant = 4
			};
			p1.points.Add(new Vector(-1.5, -0.6, -1.0));
			p1.points.Add(new Vector(-0.8, -0.2, -1.5));
			p1.points.Add(new Vector(-1.3, 0.9, -1.0));

			Polygon floor = new Polygon()
			{
				matteColor = new MyColor(.3, .7, .3),
				specularColor = new MyColor(1, 1, 1),
				phongConstant = 128
			};
			floor.points.Add(new Vector(4, -1, 4));
			floor.points.Add(new Vector(4, -1, -4));
			floor.points.Add(new Vector(-4, -1, -4));
			floor.points.Add(new Vector(-4, -1, 4));

			Sphere s2 = new Sphere()
			{
				matteColor = new MyColor(0.8, 0.1, 0.1),
				specularColor = new MyColor(0.7, 0.5, 0.5),
				phongConstant = 64,
				center = new Vector(0, 0, -1.0),
				radius = 0.7
			};

			result.shapes.Add(s1);
			result.shapes.Add(p1);
			result.shapes.Add(s2);
			result.shapes.Add(s3);
			result.shapes.Add(s4);
			result.shapes.Add(floor);

			return result;
		}
	}

	public class RayIntersectionResult
	{
		public Shape intersectedShape { get; set; }
		public Vector intersectionPoint { get; set; }
	}
}
