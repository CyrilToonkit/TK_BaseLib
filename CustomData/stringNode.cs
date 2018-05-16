using System;
using System.Collections.Generic;
using System.Drawing;

//COMMENTS OK
namespace TK.BaseLib.CustomData
{
    /// <summary>
    /// Custom Node class for managing a hierarchy of simple data
    /// </summary>
    public class stringNode
    {
        // == CONSTRUCTORS ================================================================

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inName">Name of the node</param>
        public stringNode(string inName)
        {
            name = inName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inName">Name of the node</param>
        public stringNode(string inName, int inIconIndex, Color inBackColor, Color inForeColor  )
        {
            name = inName;
            iconIndex = inIconIndex;
            backColor = inBackColor;
            foreColor = inForeColor;
        }

        //stringNode(rootComp.FullName + " (Compound)", 0, Manager.NodalPrefs.DefaultCompoundColor, Manager.NodalPrefs.NodeLabelFontColor)
        // == MEMBERS =====================================================================

        /// <summary>
        /// Node name
        /// </summary>
        string name = "Name";
        /// <summary>
        /// Accessor for the node name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        bool active = true;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        int iconIndex = -1;
        public int IconIndex
        {
            get { return iconIndex; }
            set { iconIndex = value; }
        }

        Color backColor = Color.Transparent;
        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        Color foreColor = Color.Transparent;
        public Color ForeColor
        {
            get { return foreColor; }
            set { foreColor = value; }
        }

        /// <summary>
        /// Node description
        /// </summary>
        string description = "";
        /// <summary>
        /// Accessor for Node description
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// SubNodes
        /// </summary>
        List<stringNode> nodes = new List<stringNode>();
        /// <summary>
        /// Accessor for SubNodes
        /// </summary>
        public List<stringNode> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }

        stringNode parent = null;
        /// <summary>
        /// Accessor for Parent Node
        /// </summary>
        public stringNode Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        /// <summary>
        /// Accessor for Root Node
        /// </summary>
        public stringNode Root
        {
            get
            {
                stringNode nodeParent = this;

                while(nodeParent.Parent != null)
                {
                    nodeParent = nodeParent.Parent;
                }

                return nodeParent;
            }
        }

        // == METHODS =====================================================================

        public stringNode AddPath(string value)
        {
            string[] splittedValues = value.Split('/');

            stringNode parentNode = this;
            stringNode node = null;
            foreach (string splittedValue in splittedValues)
            {
                node = parentNode.Get(splittedValue);
                if (node == null)
                {
                    node = parentNode.AddNode(splittedValue);
                }

                parentNode = node;
            }

            return node;
        }

        public bool RemovePath(string value)
        {
            string[] splittedValues = value.Split('/');

            stringNode parentNode = this;
            stringNode node = null;
            foreach (string splittedValue in splittedValues)
            {
                node = parentNode.Get(splittedValue);
                if (node != null)
                {
                    parentNode = node;
                }
            }

            if (node != null)
            {
                node.Parent.Nodes.Remove(node);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get a subnode by its name
        /// </summary>
        /// <param name="inName">Name of the Node</param>
        /// <returns>The found node or null</returns>
        public stringNode Get(string inName)
        {
            foreach (stringNode node in Nodes)
            {
                if (node.Name == inName)
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Add a subnode to this node
        /// </summary>
        /// <param name="inName">Name of the new Node</param>
        /// <returns>The newly created node</returns>
        public stringNode AddNode(string inName)
        {
            stringNode newStrNode = new stringNode(inName);
            Nodes.Add(newStrNode);
            newStrNode.Parent = this;
            return newStrNode;
        }

        /// <summary>
        /// Add a subnode to this node
        /// </summary>
        /// <param name="inName">Name of the new Node</param>
        /// <param name="inDesc">Description of the new Node</param>
        /// <returns>The newly created node</returns>
        public stringNode AddNode(string inName, string inDesc)
        {
            stringNode newStrNode = new stringNode(inName);
            newStrNode.Description = inDesc;
            Nodes.Add(newStrNode);
            newStrNode.Parent = this;
            return newStrNode;
        }

        /// <summary>
        /// Add a subnode to this node
        /// </summary>
        /// <param name="inChild">the child to add</param>
        public void AddNode(stringNode inChild)
        {
            if (inChild.Parent != null)
            {
                inChild.Parent.Nodes.Remove(inChild);
            }

            Nodes.Add(inChild);
            inChild.Parent = this;
        }

        /// <summary>
        /// Tag of the node, used to carry any custom object
        /// </summary>
        public object Tag;

        public bool IsChildOf(stringNode realDropped)
        {
            foreach (stringNode node in realDropped.Nodes)
            {
                if (this == node)
                {
                    return true;
                }
                else
                {
                    if (IsChildOf(node))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public string GetFullName()
        {
            return GetFullName(false);
        }

        public string GetFullName(bool useRoot)
        {
            string fullName = Name;

            stringNode parent = Parent;
            while (parent != null && (parent.Parent != null || useRoot))
            {
                fullName = parent.Name + "\\" + fullName;
                parent = parent.Parent;
            }

            return fullName;
        }

        public List<string> GetList()
        {
            List<string> nodesList = new List<string>();
            foreach (stringNode sN in nodes)
            {
                nodesList.Add(sN.Name);

                List<string> subNodesList = sN.GetList();
                if (subNodesList.Count > 0)
                {
                    nodesList.AddRange(subNodesList);
                }
            }

            return nodesList;
        }

        public void Sort(List<string> list)
        {
            nodes.Sort(new stringNodeSorter(list));

            foreach (stringNode node in nodes)
            {
                node.Sort(list);
            }
        }

        public List<stringNode> Collect(bool p)
        {
            List<stringNode> nodes = new List<stringNode>();
            if (p)
            {
                nodes.Add(this);
            }

            foreach (stringNode node in Nodes)
            {
                nodes.Add(node);
                node.Collect(nodes);
            }

            return nodes;
        }

        private void Collect(List<stringNode> nodes)
        {
            foreach (stringNode node in Nodes)
            {
                nodes.Add(node);
                node.Collect(nodes);
            }
        }

    }
}
