using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Kztek.iZCU.Objects;

namespace Kztek.iZCU
{
    public enum NodeType
    {
        DeviceRoot,
        Device,
        Unknown
    }

    public class DeviceTreeView : TreeView
    {
        public static Configuration configs = new Configuration();
        // Constructor
        public DeviceTreeView()
        {
            this.NodeMouseClick += new TreeNodeMouseClickEventHandler(deviceTreeReader_NodeMouseClick);
        }

        // Node mouse click
        private void deviceTreeReader_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.SelectedNode = e.Node;
        }

        // Device Root Image
        private int deviceRootImage = 0;
        public int DeviceRootImage
        {
            get { return deviceRootImage; }
            set { deviceRootImage = value; }
        }

        private int deviceImage = 1;
        public int DeviceImage
        {
            get { return deviceImage; }
            set { deviceImage = value; }
        }

        private int deviceImageError = 2;
        public int DeviceImageError
        {
            get { return deviceImageError; }
            set { deviceImageError = value; }
        }


        // Get Node Type
        public NodeType GetNodeType(TreeNode node)
        {
            if (node == null)
            {
                return NodeType.Unknown;
            }

            // check image index
            if (node.ImageIndex == deviceRootImage)
            {
                return NodeType.DeviceRoot;
            }
            else if (node.ImageIndex == deviceImage || node.ImageIndex == deviceImageError)
            {
                return NodeType.Device;
            }

            return NodeType.Unknown;
        }

        private TreeNode deviceRootNode = null;

        // Build TreeView
        public void BuildTreeView()
        {
            this.Nodes.Clear();

            // Add Device node
            string rootName = "Danh sách Camera";

            deviceRootNode = new TreeNode(rootName, deviceRootImage, deviceRootImage);
            this.Nodes.Add(deviceRootNode);
            foreach (Camera camera in frmMainForm.configs.cameras)
            {
                if (camera.cameraController != null)
                {
                    if (camera.cameraController.IsConnected)
                    {
                        TreeNode deviceNode = new TreeNode(camera.CameraName, deviceImage, deviceImage);
                        configs.LoadCameraLastEvent(camera);
                        deviceNode.Name = camera.ID + "";
                        deviceRootNode.Nodes.Add(deviceNode);
                        if (camera.lastPlateNum != null)
                        {
                            string[] str = camera.lastPlateNum.Split("|");
                            foreach(string s in str)
                            {
                                TreeNode childNode = new TreeNode();
                                childNode.Text = s;
                                if (childNode.Text.Length > 2)
                                {
                                    childNode.ImageIndex = 3;
                                    childNode.SelectedImageIndex = 3;
                                }
                                else
                                {
                                    childNode.ImageIndex = 4;
                                    childNode.SelectedImageIndex = 4;
                                }
                                deviceNode.Nodes.Add(childNode);
                            }
                        }
                    }
                    else
                    {
                        TreeNode deviceNode = new TreeNode(camera.CameraName, deviceImageError, deviceImageError);
                        configs.LoadCameraLastEvent(camera);
                        deviceNode.Name = camera.ID + "";
                        deviceRootNode.Nodes.Add(deviceNode);
                        if (camera.lastPlateNum != null)
                        {
                            string[] str = camera.lastPlateNum.Split("|");
                            foreach (string s in str)
                            {
                                TreeNode childNode = new TreeNode();
                                childNode.Text = s;
                                childNode.Name ="Zone:"+ camera.IP + "";
                                if (childNode.Text.Length > 2)
                                {
                                    childNode.ImageIndex = 3;
                                    childNode.SelectedImageIndex = 3;
                                }
                                else
                                {
                                    childNode.ImageIndex = 4;
                                    childNode.SelectedImageIndex = 4;
                                }
                                deviceNode.Nodes.Add(childNode);
                            }
                        }

                    }
                }
            }
            deviceRootNode.Expand();
        }

        public void UpdateTreeView()
        {
            foreach (Camera cam in frmMainForm.configs.cameras)
            {
                TreeNode treeNode = GetDeviceNode(cam.CameraName);
                if (treeNode != null)
                {
                    int newImageIndex = (cam.cameraController.IsConnected) ? deviceImage : deviceImageError;

                    if (treeNode.ImageIndex != newImageIndex)
                    {
                        treeNode.ImageIndex = newImageIndex;
                        treeNode.SelectedImageIndex = newImageIndex;
                    }
                }
            }
        }

        // Get Device Node
        public TreeNode GetDeviceNode(string deviceNodeName)
        {
            foreach (TreeNode node in deviceRootNode.Nodes)
            {
                if (node.Text == deviceNodeName)
                {
                    return node;
                }
            }
            return null;
        }
    }
}
