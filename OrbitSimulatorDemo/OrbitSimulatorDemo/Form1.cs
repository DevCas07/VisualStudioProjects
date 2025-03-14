using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace OrbitSimulatorDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        double G = (6.674 * Math.Pow(10, -11));

        public class Object
        {
            public int mass { get; set; }
            public int[] velocityV { get; set; }
            public int distance { get; set; }
            public double eccentricity;
            public int parent {  get; set; }

        }

        public Dictionary<int, Object> Objects = new Dictionary<int, Object>();

        Dictionary<string, string> exceptionList = new Dictionary<string, string>
        {
            { "test_id", "test_title, test_message" }, //Test message, old
            { "already_occupied_id", "Id is already occupied" },
            { "not_existing_id", "Id does not exist" },
            { "no_existing_ids", "No ids' exist" },
            { "invalid_selected_node", "Selected node is invalid" },
            { "2", "" },
            { "3", "" },
        }; //Exception list with exception id and exception message
        public void generateException(string id)
        {
            //throw new NotImplementedException();

            //string[] ex = exceptionList[id].Split(',');

            //MessageBox.Show(ex[1], ex[0]);
            MessageBox.Show(exceptionList[id], id);
        }

        public void InitialiseObject(int id, int mass, int distance, int[] velocityV, int eccentricity, int parent, bool hasParent)
        {
            Objects.Add(id, new Object 
            { mass = mass, distance = distance, velocityV = velocityV, eccentricity = eccentricity, parent = parent});
        }
        public void RemoveObject(int id)
        {
            Objects.Remove(id);
        }
        public void AppendObjectVelocityVector(int id, int[] vector)
        {
            Objects[id].velocityV = vector;
        }

        private void timeClock_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //InitialiseObject(0, 5*Math.Pow(10, 6), )
        }
    }
}
