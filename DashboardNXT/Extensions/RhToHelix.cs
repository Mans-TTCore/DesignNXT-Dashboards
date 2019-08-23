using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace DashboardNXT
{
    public static class RhToHelix
    {
        //Convert a Rhino mesh to a Media 3d Model
        public static GeometryModel3D ToHelixModel(this Rhino.Geometry.Mesh input)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            input.Faces.ConvertQuadsToTriangles();
            foreach(Rhino.Geometry.Point3d pt in input.Vertices)
            {
                mesh.Positions.Add(pt.ToHelixPoint());
            }

            foreach (Rhino.Geometry.Vector3d vc in input.Normals)
            {
                mesh.Normals.Add(vc.ToHelixVector());
            }

            foreach (Rhino.Geometry.MeshFace face in input.Faces)
            {
                mesh.TriangleIndices.Add(face.A);
                mesh.TriangleIndices.Add(face.B);
                mesh.TriangleIndices.Add(face.C);
            }
            
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = mesh;
            model.Material = new DiffuseMaterial(new SolidColorBrush(Colors.LightGray));
            model.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.LightGray));

            return model;
        }
        
        //Convert a rhino point to a windows media point
        public static Point3D ToHelixPoint(this Rhino.Geometry.Point3d input)
        {
            return new Point3D(input.X, input.Y, input.Z);
        }

        //Convert a rhino vector to a windows media vector
        public static Vector3D ToHelixVector(this Rhino.Geometry.Vector3d input)
        {
            return new Vector3D(input.X, input.Y, input.Z);
        }
        
    }
}
