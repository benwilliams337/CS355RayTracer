using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS355RayTracer
{
	class RayTracer
	{
		private const int IMAGE_SIZE = 514; //Image will be this many pixels in both x and y
		private const int MAX_RAY_DEPTH = 5; //Maximum ray tracing recursion

		public Scene scene { get; set; }

		public void Render()
		{
			Bitmap result = new Bitmap(IMAGE_SIZE, IMAGE_SIZE);
			for(int u = 0; u < IMAGE_SIZE; u++)
			{
				for(int v = 0; v < IMAGE_SIZE; v++)
				{
					Color pixelColor = tracePrimaryRay(u, v);
					result.SetPixel(u, v, pixelColor);
				}
			}
			result.Save("rayTracerOutput_" + DateTime.Now.ToFileTime() + ".bmp", ImageFormat.Bmp);
		}

		private Color tracePrimaryRay(int u, int v)
		{
			//First, find the ray we need to trace
			Vector lookFromPoint = new Vector(0, 0, scene.distanceToViewPlane);
			Vector lookAtPoint = viewportToWindow(u + 0.5, v + 0.5);
			Ray primaryRay = new Ray(lookFromPoint, (lookAtPoint - lookFromPoint).normalize());
			return MyColor.ToSystemColor(traceRay(primaryRay, 0, false));
		}

		private Vector viewportToWindow(double u, double v)
		{
			Vector result = new Vector();
			//No translation needed on viewport
			//Scale to width/height = 1 (divide by IMAGE_SIZE)
			//Scale up to window size (multiply by window size)
			//Translate to window top-left corner (subtract half of window width)
			double ratio = scene.windowSize / IMAGE_SIZE;
			double halfWindowSize = scene.windowSize / 2;
			result.x = u * ratio - halfWindowSize;
			result.y = (IMAGE_SIZE - v) * ratio - halfWindowSize; //Need to flip v-coordinate before converting, moving origin to bottom left
			//z-coordinate of window is always 0
			return result;	
		}

		private MyColor traceRay(Ray ray, int rayDepth, bool isInShape)
		{
			//Determine if we hit anything
			RayIntersectionResult intersectResult = scene.IntersectRay(ray);
			if(intersectResult == null)
				return scene.backgroundColor;

			//Get the primary color (using the illumination model) of what we hit
			bool isInShadow = isPointInShadow(intersectResult.intersectionPoint);
			MyColor primaryColor = getColorAtPoint(intersectResult.intersectedShape, intersectResult.intersectionPoint, isInShadow);

			//If we've hit the max ray depth, stop.
			if(rayDepth >= MAX_RAY_DEPTH)
				return primaryColor;

			MyColor reflectionColor = new MyColor();
			MyColor refractionColor = new MyColor();
			//Trace reflection ray
			if(intersectResult.intersectedShape.reflectionAmount > 0)
			{
				Ray reflectionRay = getReflectionRay(intersectResult.intersectedShape, intersectResult.intersectionPoint, ray);
				reflectionColor = traceRay(reflectionRay, rayDepth + 1, isInShape);
			}

			//Trace refraction ray
			if(intersectResult.intersectedShape.refractionAmount > 0)
			{
				Ray refractionRay = getRefractionRay(intersectResult.intersectedShape, intersectResult.intersectionPoint, ray, isInShape);
				refractionColor = traceRay(refractionRay, rayDepth + 1, !isInShape);
			}

			return primaryColor * intersectResult.intersectedShape.ownColorAmount +
				reflectionColor * intersectResult.intersectedShape.reflectionAmount +
				refractionColor * intersectResult.intersectedShape.refractionAmount;
		}

		private bool isPointInShadow(Vector point)
		{
			Ray rayToLight = new Ray(point, scene.directionToLight);
			rayToLight = rayToLight.getOffsetRay();
			return scene.IntersectRay(rayToLight) != null;
		}

		private MyColor getColorAtPoint(Shape shape, Vector intersectionPoint, bool isInShadow)
		{
			//Start with ambient
			MyColor result = shape.matteColor * scene.ambientColor;
			
			//If we're in shadow, stop here
			if(isInShadow)
				return result;

			//Add diffuse
			Vector normal = shape.getNormal(intersectionPoint);
			result = result + ((shape.matteColor * scene.lightColor) * Math.Max(0, normal.dotProduct(scene.directionToLight)));

			//Add specular highlight if info is provided
			if(shape.specularColor != null)
			{
				Vector lookFromPoint = new Vector(0, 0, scene.distanceToViewPlane);
				Vector surfaceToViewer = (lookFromPoint - intersectionPoint).normalize();
				Vector reflectDirection = ((2 * normal.dotProduct(scene.directionToLight) * normal) - scene.directionToLight).normalize();
				result = result + ((scene.lightColor * shape.specularColor) * 
					Math.Pow(Math.Max(0, surfaceToViewer.dotProduct(reflectDirection)), shape.phongConstant));
			}
			return result;
		}

		private Ray getReflectionRay(Shape shape, Vector intersectionPoint, Ray incomingRay)
		{	
			Vector incomingDirection = incomingRay.direction;
			Vector normal = shape.getNormal(intersectionPoint);
			Vector reflectionDirection = (incomingDirection - (2 * (incomingDirection.dotProduct(normal)) * normal)).normalize();
			return (new Ray(intersectionPoint, reflectionDirection)).getOffsetRay();
		}

		private Ray getRefractionRay(Shape shape, Vector intersectionPoint, Ray incomingRay, bool isInShape)
		{
			double n = isInShape ? shape.refractionIndex / 1.003 : 1.003 / shape.refractionIndex;
			//double n = 1.003 / shape.refractionIndex;
			Vector normal = shape.getNormal(intersectionPoint);
			if(isInShape)
				normal = -1 * normal;
			double c1 = -(normal.dotProduct(incomingRay.direction));
			double c2 = Math.Sqrt(1 - Math.Pow(n, 2) * (1 - Math.Pow(c1, 2)));
			Vector refractionDirection = ((n * incomingRay.direction) + (n * c1 - c2) * normal).normalize();
			return (new Ray(intersectionPoint, refractionDirection)).getOffsetRay();
		}
	}
}
