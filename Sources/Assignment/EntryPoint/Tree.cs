using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Collections;

namespace EntryPoint
{
    class Tree

    {
        private Node rootNode = null;
        List<Vector2> nodeList = new List<Vector2>();

        public Tree(Vector2[] vectors)
        {
            if (vectors != null)
            {
                foreach (Vector2 v in vectors)
                    Insert(v);
            }
        }

        public void Insert(Vector2 p)
        {
            Node current = rootNode;
            Node previous = null;
            bool useX = false;
            bool useLeftSubtree = false;
            while (current != null)
            { 
                useLeftSubtree = useX ? (p.X < current.Value.X) : (p.Y < current.Value.Y);
                previous = current;
                current = useLeftSubtree ? current.Left : current.Right;
                useX = !useX;
            }
            if (rootNode == null)
                rootNode = new Node(p);
            else
            {
                if (useLeftSubtree)
                    previous.Left = new Node(p);
                else
                    previous.Right = new Node(p);
            }
        }

        public static Double get_dist(Vector2 building, Vector2 house)
        {
            return Math.Sqrt(Math.Pow(house.X - building.X, 2) + Math.Pow(house.Y - building.Y, 2));
        }

        public bool InRange(Tuple<Vector2, float> house, Node node)
        {
            double range = get_dist(house.Item1, node.Value);
            return range <= house.Item2;
        }

        public List<Vector2> TraverseRec(Node node, Tuple<Vector2, float> listHousesDistances)
        {
            if (node != null)
            {
                TraverseRec(node.Left, listHousesDistances);
                if (InRange(listHousesDistances, node))
                {
                    nodeList.Add(node.Value);
                }
                TraverseRec(node.Right, listHousesDistances);
                if (InRange(listHousesDistances, node))
                {
                    nodeList.Add(node.Value);
                }
            }
            return nodeList;
        }

        public IEnumerable<IEnumerable<Vector2>> Traverse(Tree root, IEnumerable<Tuple<Vector2, float>> housesAndDistances)
        {
            IEnumerable<IEnumerable<Vector2>> specialNodesArr;
            var specialNodes = new List<List<Vector2>>();
            List<Vector2> nodes = new List<Vector2>();
            if (root != null)
            {
                foreach (var item in housesAndDistances)
                {
                    nodes = TraverseRec(root.rootNode, item);

                    specialNodes.Add(nodes);
                }
            }
            else
            {
                specialNodesArr = specialNodes.ToArray();
                return specialNodesArr;
            }
            specialNodesArr = specialNodes.ToArray();
            return specialNodesArr;
        }

    }

}
