using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CubeGame.GameWindow;

namespace CubeGame
{
    public partial class GameWindow : Form
    {
        public GameWindow()
        {
            InitializeComponent();
        }

        public double gAcceleration = 9.82; //gravity 9.82 m/s^2
        public List<staticWorldobject> staticWorldobjects = new List<staticWorldobject>();
        public List<dynamicWorldobject> dynamicWorldobjects = new List<dynamicWorldobject>();

        public class staticWorldobject
        {
            public string objectId {  get; set; }
            public int posX { get; set; }
            public int posY { get; set; }
            public int sizeWidth { get; set; }
            public int sizeHeight { get; set; }
            public struct Collider //Collider hitboxe with A, B, C and D as points
            {
                public int colliderA { get; set; }
                public int colliderB { get; set; }
                public int colliderC { get; set; }
                public int colliderD { get; set; }
            }
        }
        public class dynamicWorldobject
        {
            public string objectId { get; set; }
            public int posX { get; set; }
            public int posY { get; set; } 
            public int sizeWidth { get; set; }
            public int sizeHeight { get; set; }
            public int velocityX { get; set; }
            public int velocityY { get; set; }
            public int massWeight { get; set; } //Kilograms
        }

        //public class Velocity
        //{
        //    public int X { get; set; }
        //    public int Y { get; set; }
        //}

        public void InitialisePreStaticObjects() //Adds premade static world objects to list
        {
            staticWorldobjects.Add(new staticWorldobject { objectId = "colliderObject1", posX = 100, posY = 88, sizeHeight = 50, sizeWidth = 50 });
            staticWorldobjects.Add(new staticWorldobject { objectId = "colliderObject2", posX = 200, posY = 108, sizeHeight = 50, sizeWidth = 50 });
            staticWorldobjects.Add(new staticWorldobject { objectId = "colliderObject3", posX = 300, posY = 188, sizeHeight = 50, sizeWidth = 50 });
        }

        public void InitialisePreDynamicObjects() //Adds premade dynamic world objects to list
        {
            dynamicWorldobjects.Add(new dynamicWorldobject { objectId = "playerObject", posX = 0, posY = 0, sizeHeight = 50, sizeWidth = 50, velocityX = 0, velocityY = 0, massWeight = 10 });
        }

        private int fixObjectYCoord(int posY, int sizeHeight) { return ((this.Size.Height - 40) - (sizeHeight + posY)); } //Fixes the object y position to the winforms position system
        public void InitialiseStaticObject(string pId, int pX, int pY, int pHeight, int pWidth) { staticWorldobjects.Add(new staticWorldobject { objectId = pId, posX = pX, posY = pY, sizeHeight = pHeight, sizeWidth = pWidth}); }
        public void InitialiseDynamicObject(string pId, int pX, int pY, int pHeight, int pWidth) { dynamicWorldobjects.Add(new dynamicWorldobject { objectId = pId, posX = pX, posY = pY, sizeHeight = pHeight, sizeWidth = pWidth, velocityX = 0, velocityY = 0 }); }

        public void AddStaticObjectsToWorld() //Adds static world objects to game world
        {
            try
            {
                RemoveStaticObjectsFromWorld();
                foreach (var worldObj in staticWorldobjects)
                {
                    PictureBox obj = new PictureBox();

                    obj.Name = worldObj.objectId;
                    obj.Location = new Point(worldObj.posX, fixObjectYCoord(worldObj.posY, worldObj.sizeHeight));
                    obj.Size = new Size(worldObj.sizeWidth, worldObj.sizeHeight);
                    obj.BackColor = SystemColors.ControlDark;
                    obj.BorderStyle = BorderStyle.FixedSingle;
                    obj.Tag = "world_object";

                    this.Controls.Add(obj);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void AddDynamicObjectsToWorld() //Adds dynamic world objects to game world
        {
            try
            {
                RemoveDynamicObjectsFromWorld();
                foreach (var worldObj in dynamicWorldobjects)
                {
                    PictureBox obj = new PictureBox();

                    obj.Name = worldObj.objectId;
                    obj.Location = new Point(worldObj.posX, fixObjectYCoord(worldObj.posY, worldObj.sizeHeight));
                    obj.Size = new Size(worldObj.sizeWidth, worldObj.sizeHeight);
                    obj.BackColor = SystemColors.ControlDark;
                    obj.BorderStyle = BorderStyle.FixedSingle;
                    obj.Tag = "world_object";

                    this.Controls.Add(obj);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void RemoveStaticObjectsFromWorld() //Removes static world objects from game world
        {
            try
            {
                foreach (var worldObj in staticWorldobjects)
                {
                    if (this.Controls.ContainsKey(worldObj.objectId.ToString()))
                    {
                        this.Controls.RemoveByKey(worldObj.objectId.ToString());
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void RemoveDynamicObjectsFromWorld() //Removes dynamic world objects from game world
        {
            try
            {
                foreach (var worldObj in dynamicWorldobjects)
                {
                    if (this.Controls.ContainsKey(worldObj.objectId.ToString()))
                    {
                        this.Controls.RemoveByKey(worldObj.objectId.ToString());
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void RemoveStaticObjectsFromList() //Removes static world objects from object list
        { 
            try 
            {
                foreach (var worldObj in staticWorldobjects)
                {
                    staticWorldobjects.Remove(worldObj);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
        }
        public void RemoveDynamicObjectsFromList() //Removes dynamic world objects from object list
        {
            try
            {
                foreach (var worldObj in dynamicWorldobjects)
                {
                    dynamicWorldobjects.Remove(worldObj);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void GameTick_Tick(object sender, EventArgs e) //Repeats every tick (once every 100 milliseconds)
        {
           
        }

        private void GameWindow_Load(object sender, EventArgs e) //Initiates upon the appearance of the application window
        {
            this.Controls.Clear();
            //InitialisePreStaticObjects();
            InitialisePreDynamicObjects();
            AddStaticObjectsToWorld();
            AddDynamicObjectsToWorld();
        }
    }
}
