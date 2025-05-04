using LineDrawerDemo;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static LineDrawerDemo.MainWindow;

namespace LineDrawerDemo
{

    public partial class MainWindow : Form
    {
        CanvasObject canvasObject;

        ExceptionHandling exception = ExceptionHandling.GetInstance(); //Retrieve a singleton instance of Exception handling
        CanvasHandling canvasHandle;
        GUIExternalEvents guiExternalEvents;
        AddControlDuringRuntimeTestClass AddControlDuringRuntimeTestClass123; //Information sending test class

        public MainWindow()
        {
            InitializeComponent();

            //
            // Initialise all externally callable events
            //
            guiExternalEvents = GUIExternalEvents.GetInstance();

            guiExternalEvents.EventUpdateFormTreeViews += EventUpdateFormTreeViews;
            guiExternalEvents.EventUpdateFormObjects += EventUpdateFormObjects;
            guiExternalEvents.EventResetFormParameters += GuiExternalEvents_EventResetFormParameters;
            guiExternalEvents.EventClearTreeViews += EventClearTreeViews;

            //
            // Initialise new canvas surface's position and size
            //
            canvasObject = new CanvasObject();
            canvasObject.position = new Point(this.CanvasReference.Location.X, this.CanvasReference.Location.Y);
            canvasObject.size = new Size(this.CanvasReference.Size.Width, this.CanvasReference.Size.Height);

            //
            // Create new intermediary data handler a 'canvasHandle Object' and initialise a new drawing surface based on canvasObject's properties
            //
            canvasHandle = new CanvasHandling();
            canvasHandle.CreateCanvas(canvasObject);

            //
            // Add new canvas drawing surface to Control Collection and draw it out
            //
            Controls.Add(canvasHandle.getCanvas());
            canvasHandle.Redraw();
        }

        //
        // Declare externally callable event logic and handling ------------------------------------------
        //

        private void GuiExternalEvents_EventResetFormParameters(object sender, EventArgs e)
        {
            resetFormParameters();
        }

        private void EventUpdateFormObjects(object sender, EventArgs e)
        {
            updateFormObjects();
        }

        private void EventUpdateFormTreeViews(object sender, EventArgs e)
        {
            updateLineTreeView();
            updateSelectedLineTreeView();
        }
        public void EventClearTreeViews(object sender, EventArgs e)
        {
            ClearTreeViews();
        }

        //
        // Necessary miscellaneous public handling ------------------------------------------
        //
        public void ClearTreeViews()
        {
            selectedLinesTreeView.Nodes[0].Nodes.Clear();
            canvasHandle.publicVaribles.selectedLineObjects.Clear();
        }

        public void resetFormParameters()
        {
            this.SuspendLayout();
            canvasHandle.publicVaribles.currentNumClick = numClick.First;
            
            canvasHandle.publicVaribles.ConfirmAction = false;
            canvasHandle.publicVaribles.CancelAction = false;

            canvasHandle.publicVaribles.selectedPointPos = new Point(-20, -20);

            selectedNodeLabel.Text = "Selected node: [None]";
            selectedNodeLabel2.Text = "Selected node: [None]";

            ClearTreeViews();

            //
            // Reset selected radio buttons
            //
            //radioBtnCreateLineMode.Checked = false;
            //radioBtnEditLineMode.Checked = false;
            //radioBtnRemoveLineMode.Checked = false;
            //radioBtnCreatePolygonMode.Checked = false;
        }
        public void clearSelectedLineObjects()
        {
            selectedLinesTreeView.Nodes[0].Nodes.Clear();
            canvasHandle.publicVaribles.selectedLineObjects.Clear();
        }
        public void updateFormObjects()
        {
            if (canvasHandle.publicVaribles.canvasLineMode.Equals(CanvasModes.editLine))
            {
                selectedNodeLabel.Text = "Selected node: [" + canvasHandle.publicVaribles.selectedNode + "]; End: [" + canvasHandle.publicVaribles.selectedCanvasLineEnd + "]";
                selectedNodeLabel2.Text = "Selected node: [" + canvasHandle.publicVaribles.selectedNode + "]; End: [" + canvasHandle.publicVaribles.selectedCanvasLineEnd + "]";
            }
            else if (canvasHandle.publicVaribles.canvasLineMode.Equals(CanvasModes.removeLine))
            {
                selectedNodeLabel.Text = "Selected node: [" + canvasHandle.publicVaribles.selectedNode + "]";
                selectedNodeLabel2.Text = "Selected node: [" + canvasHandle.publicVaribles.selectedNode + "]";
            }
            else if (canvasHandle.publicVaribles.canvasLineMode.Equals(CanvasModes.createLine) && canvasHandle.publicVaribles.currentNumClick == numClick.Second)
            {
                selectedNodeLabel.Text = "Selected node: [...]";
                selectedNodeLabel2.Text = "Selected node: [...]";
            }
            else
            {
                selectedNodeLabel.Text = "Selected node: [None]";
                selectedNodeLabel2.Text = "Selected node: [None]";
            }

            btnCancelCanvasLineAction.Enabled = canvasHandle.publicVaribles.CancelAction;
            btnConfirmCanvasLineAction.Enabled = canvasHandle.publicVaribles.ConfirmAction;
        }
        public void updateLineTreeView() //Updates the treeView to correspond to the updated dictionary's contents
        {
            linesTreeView.Nodes[0].Nodes.Clear();
            Dictionary<int, LineObject> LineObjects = canvasHandle.lineHandle.GetLineObjects();

            foreach (var lineObject in LineObjects.Keys)
            {
                TreeNode node = new TreeNode();
                node.Text = "Line" + lineObject.ToString();
                node.Name = "line" + lineObject.ToString();
                node.Tag = lineObject;
                linesTreeView.Nodes[0].Nodes.Add(node);

            }
            linesTreeView.Nodes[0].Expand();
        }
        
        public void updateSelectedLineTreeView()
        {
            selectedLinesTreeView.Nodes[0].Nodes.Clear();
            if (canvasHandle.publicVaribles.selectedLineObjects.Count > 0) {
                foreach (var line in canvasHandle.publicVaribles.selectedLineObjects)
                {
                    int lineEnd = line.Value;
                    TreeNode node = new TreeNode();
                    node.Tag = line.Key;

                    if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.editLine) // line edit mode
                    {
                        node.Text = "Line" + line.Key.ToString() + ", End:" + line.Value;
                        node.Name = "line:" + line.Key.ToString() + ";end:" + line.Value;
                    }
                    else if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.removeLine) // line remove mode
                    {
                        node.Text = "Line" + line.Key.ToString();
                        node.Name = "line:" + line.Key.ToString();
                    }

                    selectedLinesTreeView.Nodes[0].Nodes.Add(node);
                }
                selectedLinesTreeView.Nodes[0].Expand();
            }
        }

        //
        // Selecting line in treeView handling ------------------------------------------
        //
        private void linesTreeView_AfterSelect(object sender, TreeViewEventArgs e) //After tree view selecting node handling ------------------------------------------
        {
            if (linesTreeView.SelectedNode.Tag.ToString() != "main") {

                resetFormParameters();
                clearSelectedLineObjects();

                canvasHandle.setSelectedNode((int)linesTreeView.SelectedNode.Tag);
                lineKeyBox.Value = canvasHandle.publicVaribles.selectedNode;

                if (!canvasHandle.lineHandle.checkKeyAvailability(canvasHandle.publicVaribles.selectedNode)) //Checks if selected node exists so it can write to from existing LineObject
                //if (true)
                {
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx1 != 0) { x1Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx1.ToString(); }
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy1 != 0) { y1Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy1.ToString(); }
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx2 != 0) { x2Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx2.ToString(); }
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy2 != 0) { y2Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy2.ToString(); }
                }
                else { exception.generateException(CustomExceptions.not_existing_key); }

                selectedNodeLabel.Text = "Selected node: [" + canvasHandle.publicVaribles.selectedNode + "]";
                selectedNodeLabel2.Text = "Selected node: [" + canvasHandle.publicVaribles.selectedNode + "]";
            }
            else {exception.generateException(CustomExceptions.invalid_selected_node); } // invalid selected node
        }
        private void selectedLinesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (selectedLinesTreeView.SelectedNode.Tag.ToString() != "main") 
            {
                if (canvasHandle.publicVaribles.lineMultiLocking == false)
                {

                    canvasHandle.setSelectedNode((int)selectedLinesTreeView.SelectedNode.Tag);
                    lineKeyBox.Value = canvasHandle.publicVaribles.selectedNode;

                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx1 != 0) { x1Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx1.ToString(); }
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy1 != 0) { y1Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy1.ToString(); }
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx2 != 0) { x2Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realx2.ToString(); }
                    if (canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy2 != 0) { y2Box.Text = canvasHandle.lineHandle.LineObjects[canvasHandle.publicVaribles.selectedNode].Realy2.ToString(); }

                    if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.editLine 
                        || canvasHandle.publicVaribles.canvasLineMode == CanvasModes.createLine) // line edit mode or line create mode
                    {
                        if (canvasHandle.publicVaribles.lineMultiLocking == false)
                        {
                            canvasHandle.publicVaribles.selectedCanvasLineEnd = canvasHandle.publicVaribles.selectedLineObjects[canvasHandle.publicVaribles.selectedNode];

                            Point endPoint = canvasHandle.getLineEndCoordinates(canvasHandle.publicVaribles.selectedNode, canvasHandle.publicVaribles.selectedCanvasLineEnd);
                            canvasHandle.publicVaribles.selectedPointPos = new Point(endPoint.X, endPoint.Y);
                            //Switch with comments to these code bits to require confirm button press, maybe add an if statement here for a public option varible
                            //canvasHandle.publicVaribles.currentNumClick = numClick.Second;
                            canvasHandle.publicVaribles.ConfirmAction = true;
                        }
                        //else if (lineMultiLocking == true)
                        //{

                        //}
                    }
                    else if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.removeLine)
                    {
                        canvasHandle.publicVaribles.ConfirmAction = true;
                        //resetSelectedLine();
                    }

                    canvasHandle.Redraw();
                    updateFormObjects();
                }
            }
            else { exception.generateException(CustomExceptions.invalid_selected_node); } // invalid selected node
        }

        //
        // Event Handling ------------------------------------------------------------------------------------
        //
        private void MainWindow_Load(object sender, EventArgs e)
        {
            //InitializeLineObject(10, 20, 120, 80);
            //BeginGraphics();
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            int tempX1; int tempY1; int tempX2; int tempY2; int tempLineKeyBox; //Temporary varibles

            if (int.TryParse(x1Box.Text, out tempX1)) { } else { tempX1 = 1; }
            if (int.TryParse(y1Box.Text, out tempY1)) { } else { tempY1 = 1; }
            if (int.TryParse(x2Box.Text, out tempX2)) { } else { tempX2 = 2; }
            if (int.TryParse(y2Box.Text, out tempY2)) { } else { tempY2 = 2; }
            tempLineKeyBox = ((int)lineKeyBox.Value);

            canvasHandle.createLine(tempLineKeyBox, tempX1, tempY1, tempX2, tempY2);

            //
            // GUI Redraw
            //
            resetFormParameters();
            updateLineTreeView();
            updateSelectedLineTreeView();
            canvasHandle.setSelectedNode(tempLineKeyBox);
            updateFormObjects();

            canvasHandle.Redraw();
            //
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            canvasHandle.removeLine(canvasHandle.publicVaribles.selectedNode);

            //
            // GUI Redraw
            //
            resetFormParameters();
            updateLineTreeView();
            updateSelectedLineTreeView();
            updateFormObjects();

            canvasHandle.Redraw();
            //
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int tempX1; int tempY1; int tempX2; int tempY2; int tempLineKeyBox; //Temporary varibles

            if (int.TryParse(x1Box.Text, out tempX1)) { } else { tempX1 = 0; }
            if (int.TryParse(y1Box.Text, out tempY1)) { } else { tempY1 = 0; }
            if (int.TryParse(x2Box.Text, out tempX2)) { } else { tempX2 = 1; }
            if (int.TryParse(y2Box.Text, out tempY2)) { } else { tempY2 = 1; }

            tempLineKeyBox = ((int)lineKeyBox.Value);

            canvasHandle.saveLine(tempLineKeyBox, tempX1, tempY1, tempX2, tempY2);

            //
            // GUI Redraw
            //
            resetFormParameters();
            updateLineTreeView();
            updateSelectedLineTreeView();
            canvasHandle.setSelectedNode(tempLineKeyBox);
            updateFormObjects();

            canvasHandle.Redraw();
            //

        }
        private void btnDraw_Click(object sender, EventArgs e)
        {
            //BeginGraphics();
            canvasHandle.Redraw();
        }
        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            canvasHandle.fileHandle.viewSaveFileDialog();
            fileLocationBox.Text = canvasHandle.fileHandle.fileLocation;
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            canvasHandle.fileHandle.viewOpenFileDialog();
            fileLocationBox.Text = canvasHandle.fileHandle.fileLocation;

            //
            // GUI Redraw
            //
            resetFormParameters();
            updateLineTreeView();
            updateSelectedLineTreeView();
            updateFormObjects();

            canvasHandle.Redraw();
            //
        }
        private void btnReloadFile_Click(object sender, EventArgs e)
        {
            canvasHandle.fileHandle.readLineFile(canvasHandle.fileHandle.fileLocation);

            //
            // GUI Redraw
            //
            resetFormParameters();
            updateLineTreeView();
            updateSelectedLineTreeView();
            updateFormObjects();

            canvasHandle.Redraw();
            //
        }
        private void checkBoxLabelLines_CheckedChanged(object sender, EventArgs e)
        {
            canvasHandle.publicVaribles.DebugMode = checkBoxDebugMode.Checked;
            canvasHandle.Redraw();
        }
        private void checkBoxLockInToLineEnds_CheckedChanged(object sender, EventArgs e)
        {
            //lockInToLineEnds = checkBoxDebugMode.Checked;
            if (checkBoxDebugMode.CheckState == CheckState.Checked) { canvasHandle.publicVaribles.lockInToLineEnds = true; }
            else if (checkBoxDebugMode.CheckState == CheckState.Unchecked) { canvasHandle.publicVaribles.lockInToLineEnds = false; }
            
        }
        private void radioBtnEditLineMode_CheckedChanged(object sender, EventArgs e)
        {
            resetFormParameters();
            if (radioBtnEditLineMode.Checked) { canvasHandle.setCanvasMode(CanvasModes.editLine); }
        }
        private void radioBtnCreateLineMode_CheckedChanged(object sender, EventArgs e)
        {
            resetFormParameters();
            if (radioBtnCreateLineMode.Checked) { canvasHandle.setCanvasMode(CanvasModes.createLine); }
        }
        private void radioBtnRemoveLineMode_CheckedChanged(object sender, EventArgs e)
        {
            resetFormParameters();
            if (radioBtnRemoveLineMode.Checked) { canvasHandle.setCanvasMode(CanvasModes.removeLine); }
        }
        private void minSelectDistanceBox_ValueChanged(object sender, EventArgs e)
        {
            canvasHandle.publicVaribles.minSelectDistance = (int)minSelectDistanceBox.Value;
            canvasHandle.Redraw();
        }
        private void btnCancelCanvasLineAction_Click(object sender, EventArgs e)
        {
            resetFormParameters();
            updateFormObjects();

            canvasHandle.Redraw();
        }
        private void btnConfirmCanvasLineAction_Click(object sender, EventArgs e)
        {
            if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.editLine)
            {
                canvasHandle.publicVaribles.currentNumClick = numClick.Second;

                canvasHandle.publicVaribles.ConfirmAction = false;
                selectedLinesTreeView.Nodes[0].Nodes.Clear();
            }
            else if (canvasHandle.publicVaribles.canvasLineMode == CanvasModes.removeLine)
            {
                bool tempSuccess = false;
                canvasHandle.removeLineByDialog(canvasHandle.publicVaribles.selectedNode, out tempSuccess);
                if (tempSuccess) { canvasHandle.publicVaribles.CancelAction = false; canvasHandle.publicVaribles.ConfirmAction = false; }

                resetFormParameters();
                ClearTreeViews();
                updateLineTreeView();
                updateSelectedLineTreeView();
            }

            //
            // GUI Redraw
            //
            updateFormObjects();
            canvasHandle.Redraw();
            //
        }
        private void lineMultiLockingBox_CheckedChanged(object sender, EventArgs e)
        {
            canvasHandle.publicVaribles.lineMultiLocking = lineMultiLockingBox.Checked;
        }

        private void toolStripMenuItemForceRedraw_Click(object sender, EventArgs e)
        {
            resetFormParameters();
            canvasHandle.Redraw();
        }

        private void createLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasHandle.setCanvasMode(CanvasModes.createLine);
        }

        private void editLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasHandle.setCanvasMode(CanvasModes.createLine);
        }

        private void removeLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasHandle.setCanvasMode(CanvasModes.removeLine);
        }

        private void createPolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasHandle.setCanvasMode(CanvasModes.createPolygon);
        }

        private void enableToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            canvasHandle.publicVaribles.DebugMode = true;
            canvasHandle.Redraw();
        }

        private void disableToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            canvasHandle.publicVaribles.DebugMode = false;
            canvasHandle.Redraw();
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasHandle.publicVaribles.lockInToLineEnds = true;
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasHandle.publicVaribles.lockInToLineEnds = false;
        }

        private void enableToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            canvasHandle.publicVaribles.lineMultiLocking = true;
        }

        private void disableToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            canvasHandle.publicVaribles.lineMultiLocking = false;
        }

        private void radioBtnCreatePolygonMode_CheckedChanged(object sender, EventArgs e)
        {
            canvasHandle.setCanvasMode(CanvasModes.createPolygon);
        }

        private void numPolygonCornersBox_ValueChanged(object sender, EventArgs e)
        {
            canvasHandle.publicVaribles.numPolygonCorners = (int)numPolygonCornersBox.Value;

        }

        private void resetCanvasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<int> lines = new List<int>();
            foreach (var line in canvasHandle.lineHandle.LineObjects.Keys)
            {
                lines.Add(line);
            }

            bool tempSuccess = false;
            canvasHandle.removeLinesByDialog(lines, out tempSuccess);

            //
            // GUI Redraw
            //
            resetFormParameters();
            ClearTreeViews();
            updateLineTreeView();
            updateSelectedLineTreeView();
            updateFormObjects();

            canvasHandle.Redraw();
            //
        }

        private void showExceptionLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> log = new List<string>();
            //{ " " };
            log = exception.RetrieveLog();

            if (log != null)
            {
                StringBuilder logString = new StringBuilder();

                foreach (var line in log)
                {
                    logString.AppendLine(line + ",");
                }

                MessageBox.Show(logString.ToString());
            }
            else { exception.generateException(CustomExceptions.null_list_contents); }
        }

        private void Canvas_SizeChanged(object sender, EventArgs e)
        {
            canvasHandle.Redraw();
        }

        public void button1_Click(object sender, EventArgs e)
        {

            AddControlDuringRuntimeTestClass test123;
            test123 = new AddControlDuringRuntimeTestClass();
            Control control = test123.getControl();

            Controls.Add(control);
            this.Invalidate();
        }
        
        private void labelTextSizeBox_ValueChanged(object sender, EventArgs e)
        {
            canvasHandle.publicVaribles.labelFontSize = (int)labelTextSizeBox.Value;
            canvasHandle.Redraw();
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasHandle.fileHandle.viewSaveFileDialog();
            fileLocationBox.Text = canvasHandle.fileHandle.fileLocation;
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasHandle.fileHandle.viewOpenFileDialog();
            fileLocationBox.Text = canvasHandle.fileHandle.fileLocation;

            //
            // GUI Redraw
            //
            resetFormParameters();
            updateLineTreeView();
            updateSelectedLineTreeView();
            updateFormObjects();

            canvasHandle.Redraw();
            //
        }

        private void reloadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasHandle.fileHandle.readLineFile(canvasHandle.fileHandle.fileLocation);

            //
            // GUI Redraw
            //
            resetFormParameters();
            updateLineTreeView();
            updateSelectedLineTreeView();
            updateFormObjects();

            canvasHandle.Redraw();
            //
        }
    }
}
