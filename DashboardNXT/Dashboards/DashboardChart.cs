using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Grasshopper.Kernel;
using Rhino.Geometry;
using MyDashboard;
using LiveCharts.Wpf;
using LiveCharts;

namespace DashboardNXT
{
    public class DashboardChart : GH_Component
    {
        MainWindow window = new MainWindow();
        PieChart pieChart = new PieChart();

        /// <summary>
        /// Initializes a new instance of the DashboardChart class.
        /// </summary>
        public DashboardChart()
          : base("Chart", "Chart", "Donut Chart Readout", "Display", "Dashboard")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Toggle", "B", "True to Launch Window", GH_ParamAccess.item, false);
            pManager.AddNumberParameter("Values", "V", "The values to chart", GH_ParamAccess.list);
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
            //Get the component's input values
            bool launch = false;
            List<double> values = new List<double>();
            DA.GetData(0, ref launch);
            DA.GetDataList(1, values);

            //Create a new instance of the window and its objects prior to launch
            if (launch) { BuildWindow(); }

            //Set the charts data
            pieChart.Series.Clear();
            for (int i = 0; i < values.Count; i++)
            {
                PieSeries series = new PieSeries();
                series.Title = values[i].ToString();
                pieChart.Series.Add(series);

                ChartValues<double> vals = new ChartValues<double>();
                vals.Add(values[i]);
                pieChart.Series[i].Values = vals;
            }

            //If launch is true show the newly created window
            if (launch) { window.Show(); }
        }
        
        public void BuildWindow()
        {
            //Create new instances of the window, panels, labels, and slider controls
            window = new MainWindow();
            pieChart = new PieChart();

            //Set the pie chart properties
            pieChart.Width = 300;
            pieChart.Height = 300;
            pieChart.InnerRadius = 25;
            pieChart.LoadLegend();
            pieChart.LegendLocation = LegendLocation.Bottom;
            pieChart.DisableAnimations = true;

            //Set the window properties and add the pie chart
            window.Width = 320;
            window.Height = 320;
            window.Topmost = true;
            window.Content = pieChart;
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
                return Properties.Resources.chart_pie_solid;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9eec8ea1-bcec-4ce5-ab4d-fdf3229c1a4e"); }
        }
    }
}