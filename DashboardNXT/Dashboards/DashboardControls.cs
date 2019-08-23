using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Grasshopper.Kernel;
using MyDashboard;
using Rhino.Geometry;

namespace DashboardNXT
{
    public class DashboardControls : GH_Component
    {
        MainWindow window = new MainWindow();
        double altitude = 0.0;
        double azimuth = 0.0;

        /// <summary>
        /// Initializes a new instance of the Controls class.
        /// </summary>
        public DashboardControls()
          : base("Control Panel", "Controls", "Control Panel", "Display", "Dashboard")
        {
            
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Launch", "B", "Launches the Dashboard", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Azimuth", "AZ", "Azimuth", GH_ParamAccess.item);
            pManager.AddNumberParameter("Altitude", "AL", "Altitude", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Get the component's input values
            bool launch = false;
            DA.GetData(0, ref launch);

            //If launch is true create a new instance of the window and it's controls and show the window
            if (launch) {
                BuildWindow();
                window.Show();
            }

            //Return the numbers from the variables slider values
            DA.SetData(0, azimuth);
            DA.SetData(1, altitude);
        }

        public void BuildWindow()
        {
            //Create new instances of the window, panels, labels, and slider controls
            window = new MainWindow();

            StackPanel panel = new StackPanel();

            Slider sliderAzimuth = new Slider();
            Slider sliderAltitude = new Slider();
            
            Label labelAzimuth = new Label();
            Label labelAltitude = new Label();

            //Set the label's value and properties
            labelAzimuth.FontSize = 18;
            labelAzimuth.Content = "Azimuth";

            labelAltitude.FontSize = 18;
            labelAltitude.Content = "Altitude";

            //Set the panel properties
            panel.Width = 200;
            panel.Height = 300;
            panel.Orientation = Orientation.Vertical;

            //Set slider properties and event 
            sliderAzimuth.Minimum = 0;
            sliderAzimuth.Maximum = 360;
            sliderAzimuth.TickFrequency = 1;
            sliderAzimuth.ValueChanged += (o, e) => { azimuth = sliderAzimuth.Value; ExpireSolution(true); };

            sliderAltitude.Minimum = 0;
            sliderAltitude.Maximum = 90;
            sliderAltitude.TickFrequency = 1;
            sliderAltitude.ValueChanged += (o, e) => { altitude = sliderAltitude.Value; ExpireSolution(true); };

            //Add the alternating labels and sliders
            panel.Children.Add(labelAzimuth);
            panel.Children.Add(sliderAzimuth);
            panel.Children.Add(labelAltitude);
            panel.Children.Add(sliderAltitude);

            //Set the window properties and add the panel
            window.Width = 220;
            window.Height = 320;
            window.Topmost = true;
            window.Content = panel;
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
                return Properties.Resources.sliders_h_solid;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("375e3d8e-aae1-4c2a-9309-20e5ab5773e4"); }
        }
    }
}