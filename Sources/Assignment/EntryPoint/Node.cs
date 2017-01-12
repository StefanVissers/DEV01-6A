using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EntryPoint
{
    class Node

    {
        private Vector2 val;
        private Node left;
        private Node right;

        public Node(Vector2 p)
        {
            val = p;
        }

        public Vector2 Value
        {
            get { return val; }
            set { val = value; }
        }

        public Node Right
        {
            get { return right; }
            set { right = value; }
        }

        public Node Left
        {
            get { return left; }
            set { left = value; }
        }
    }
}