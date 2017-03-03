using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS355RayTracer
{
	public class Polygon : Shape
	{
		public const bool ONE_SIDED = false;

		public List<Vector> points;

		public Polygon() : base()
		{
			points = new List<Vector>();
		}

		public override double? Intersect(Ray ray)
		{
			//Get the point of intersection with the polygon's plane
			Vector normal = getNormal();
			double? planeIntersectionResult = getPlaneIntersection(ray, normal);
			if(!planeIntersectionResult.HasValue)
				return null; 
			Vector planeIntersection = ray.getParameterizedPoint(planeIntersectionResult.Value);

			//Determine the dominant axis (the one where the normal is the greatest), and project the polygon/intersection point onto that plane
			List<Vector2D> projectedVertices = new List<Vector2D>();
			Vector2D projectedIntersection = null;
			if(Math.Abs(normal.x) >= Math.Abs(normal.y) && Math.Abs(normal.x) >= Math.Abs(normal.z)) //x is dominant
			{
				foreach(Vector vertex in points)
				{
					projectedVertices.Add(vertex.dropX());
				}
				projectedIntersection = planeIntersection.dropX();
			}
			else if(Math.Abs(normal.y) >= Math.Abs(normal.x) && Math.Abs(normal.y) >= Math.Abs(normal.z)) //y is dominant
			{
				foreach (Vector vertex in points)
				{
					projectedVertices.Add(vertex.dropY());
				}
				projectedIntersection = planeIntersection.dropY();
			}
			else //z is dominant
			{
				foreach (Vector vertex in points)
				{
					projectedVertices.Add(vertex.dropZ());
				}
				projectedIntersection = planeIntersection.dropZ();
			}

			//Translate polygon so the intersection point is at the center
			for(int i = 0; i < projectedVertices.Count; i++)
			{
				projectedVertices[i] = projectedVertices[i] - projectedIntersection;
			}

			//Start calculating crossings
			int numCrossings = 0;
			int signHolder = projectedVertices[0].v < 0 ? -1 : 1;

			for(int i = 0; i < projectedVertices.Count; i++)
			{
				int nextIndex = i + 1;
				if(i + 1 >= projectedVertices.Count)
					nextIndex = 0;
				int nextSignHolder = projectedVertices[nextIndex].v < 0 ? -1 : 1;
				if(signHolder != nextSignHolder) //Current edge crosses u-axis
				{
					if(projectedVertices[i].u > 0 && projectedVertices[nextIndex].u > 0) //Both vertices positive, crosses +u
						numCrossings++;
					else if(projectedVertices[i].u > 0 || projectedVertices[nextIndex].u > 0) //One vertex positive, might cross +u
					{
						//Compute intersection with u-axis
						double uAxisIntersection = projectedVertices[i].u - 
							projectedVertices[i].v *
							(projectedVertices[nextIndex].u - projectedVertices[i].u) /
							(projectedVertices[nextIndex].v - projectedVertices[i].v);
						if(uAxisIntersection > 0) //Check if we intersect in the positive u-axis
							numCrossings++;
					}
					//If both vertices are negative, edge does not cross +u
				}
				signHolder = nextSignHolder;
			}

			if(numCrossings % 2 == 1) //If numCrossings is odd, we're inside. Otherwise, we're outside
				return planeIntersectionResult;	//Return t-value for intersected point
			else
				return null; //Outside polygon, so return null
		}

		private double? getPlaneIntersection(Ray ray, Vector normal) //Normal is a parameter so we can avoid computing it twice
		{
			//Compute d in the plane equation (-(ax + by + cz), where (a,b,c) is the normal, and (x,y,z) is any point in our polygon)
			double d = -(normal.x * points[0].x + normal.y * points[0].y + normal.z * points[0].z);
			double v_d = normal.dotProduct(ray.direction);
			if(v_d >= 0 && ONE_SIDED)
				return null; //Hit back face of plane
			if(v_d == 0)
				return null; //Ray is parallel to plane
			double v_o = -(normal.dotProduct(ray.origin) + d);
			double t = v_o / v_d;
			if(t < 0)
				return null;
			//If  v_d > 0, we should reverse the plane's normal. I'm not sure how to impement this, but it only applies to two-sided planes, so we'll probably be ok.
			return t;
		}

		public Vector getNormal()
		{
			Vector v1 = points[0] - points[1];
			Vector v2 = points[2] - points[1];
			return v2.crossProduct(v1).normalize();
		}

		public override Vector getNormal(Vector intersectionPoint)
		{
			return getNormal();
		}
	}
}
