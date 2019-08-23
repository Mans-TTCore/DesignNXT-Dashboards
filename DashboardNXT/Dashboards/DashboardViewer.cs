using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Grasshopper.Kernel;
using MyDashboard;
using Rhino.Geometry;

namespace DashboardNXT
{
    public class DashboardViewer : GH_Component
    {
        MainWindow window = new MainWindow();
        ModelVisual3D visual = new ModelVisual3D();
        HelixToolkit.Wpf.HelixViewport3D helixView = new HelixToolkit.Wpf.HelixViewport3D();

        /// <summary>
        /// Initializes a new instance of the DashboardViewer class.
        /// </summary>
        public DashboardViewer()
          : base("Dashboard Viewer", "Viewer", "Helix Model Viewer", "Display", "Dashboard")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Launch", "B", "Launch the viewer", GH_ParamAccess.item, false);
            pManager.AddMeshParameter("Meshes", "M", "The Mesh to Visualize", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Get the component's inputs
            bool launch = false;
            List<Mesh> meshes = new List<Mesh>();
            DA.GetData(0, ref launch);
            DA.GetDataList(1, meshes);

            //Create a new instance of the window and its objects prior to launch
            if (!window.IsLoaded) { BuildWindow();}

            //Conver the mesh and load it into the viewer
            Model3DGroup group = new Model3DGroup();
            foreach (Mesh mesh in meshes)
            {
                group.Children.Add(mesh.ToHelixModel());
            }
            visual.Content = group;

            //Now that there is geometry in the scene, zoom to it's extents
            helixView.ZoomExtents();

            //If launch is true show the newly created window
            if (launch) { window.Show(); }
        }

        public void BuildWindow()
        {
            //Create new instances of the window, helix viewer, and model view
            window = new MainWindow();
            helixView = new HelixToolkit.Wpf.HelixViewport3D();
            visual = new ModelVisual3D();

            //Set the Helix Viewer Properties
            helixView.Width = 300;
            helixView.Height = 300;
            //Add Lights to the Viewer
            helixView.Children.Add(new HelixToolkit.Wpf.DefaultLights());
            //Add the Model Visualization to the viewer (Note: The geometry will be added to the viewer at a later point)
            helixView.Children.Add(visual);

            //Set the Window Properties
            window.Width = 320;
            window.Height = 320;
            window.Topmost = true;
            //Add the Viewer to the Window
            window.Content = helixView;
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.cubes_solid;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("89511982-a59f-4d53-abee-72b60fb86bbf"); }
        }
    }
}