using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LabWork2
{
    public enum Color
    {
        Red = 0,
        Black = 1
    }

    class RBNode
    {
        public RBNode(string val)
        {
            value = val;
            count = 0;
        }

        public RBNode(string val, RBNode par)
        {
            parent = par;
            value = val;
        }

        
        public string value;
        public Color color = Color.Red;
        public RBNode parent;
        public RBNode left;
        public RBNode right;
        public int count;//кол-во элементов с этим значением
    }

    class RedBlackTree
    {
        private int iterations;
        public RBNode root;
        public RedBlackTree()
        {
            root = null;
        }
        public RedBlackTree(string r)
        {
            root = new RBNode(r);
            root.color = Color.Black;
        }

        public int GetIerations()
        {
            var res = iterations;
            iterations = 0;
            return res;
        }

        public RBNode Find(string v)
        {
            var x = root;
            while(x != null && x.value != v)
            {
                iterations++;
                if (String.Compare(x.value, v) < 0)
                    x = x.right;
                else
                    x = x.left;
            }
            return x;
        }

        public List<RBNode> ToList()
        {
            var res = new List<RBNode>();
            AddNode(root);

            void AddNode(RBNode n)
            {
                if (n.left != null)
                    AddNode(n.left);

                res.Add(n);

                if (n.right != null)
                    AddNode(n.right);
            }
            return res;
        }

        public string PrintTree()
        {
            var res = "";
            PrintNode(root);

            void PrintNode(RBNode n)
            {
                if (n.left != null)
                    PrintNode(n.left);

                res += n.value;

                if (n.right != null)
                    PrintNode(n.right);
            }
            return res;
        }

        protected RBNode AddNode(RBNode to, string data)
        {
            iterations++;
            if (data == to.value)
            {
                to.count++;
                return to;
            }

            if (String.Compare(data, to.value) < 0) //добавляем влево
            {
                if (to.left != null)
                    return AddNode(to.left, data);
                return to.left = new RBNode(data, to);
            }
            else //добавляем вправо
            {
                if (to.right != null)
                    return AddNode(to.right, data);
                return to.right = new RBNode(data, to);
            }          
        }

        //добавление элемента с соблюд кч деревьев
        public RBNode Add(string data)
        {
            
            if (root == null)
            {
                root = new RBNode(data);
                root.color = Color.Black;
                return root;
            }
            var n = AddNode(root, data);

            //нормализация дерева
            
            Insert(n);
            root.color = Color.Black;
            return n;
        }

        private void Insert(RBNode x)
        {
            var y = new RBNode("");
            x.color = Color.Red;
            while (x != root && x.parent.color == Color.Red)
            {
                if (x.parent == x.parent.parent.left)
                {
                    y = x.parent.parent.right == null? new RBNode("", x.parent.parent.right) { color = Color.Black } : x.parent.parent.right;
                    if (y.color == Color.Red)
                    {
                        x.parent.color = Color.Black;
                        y.color = Color.Black;
                        x.parent.parent.color = Color.Red;
                        x = x.parent.parent;
                    }
                    else
                    {
                        if (x == x.parent.right)
                        {
                            x = x.parent;
                            rotate_left(x);
                        }
                        
                        x.parent.color = Color.Black;
                        x.parent.parent.color = Color.Red;
                        rotate_right(x.parent.parent);
                    }
                    
                }
                //
                else
                {
                    y = x.parent.parent.left == null? new RBNode("", x.parent.parent.left) { color = Color.Black } : x.parent.parent.left;
                    if (y.color == Color.Red)
                    {
                        x.parent.color = Color.Black;
                        y.color = Color.Black;
                        x.parent.parent.color = Color.Red;
                        x = x.parent.parent;
                    }
                    else{
                        if (x == x.parent.left)
                        {
                            x = x.parent;
                            rotate_right(x);
                        }
                    
                        
                        x.parent.color = Color.Black;
                        x.parent.parent.color = Color.Red;
                        rotate_left(x.parent.parent);
                    }

                }
            }
        }

        //повоторот вправо

        protected void rotate_right(RBNode n)
        {
            
            var y = new RBNode("");
            if (n.left != null)
            {
                y = n.left;
                n.left = y.right;



                if (y.right != null)
                    y.right.parent = n;
                y.parent = n.parent;
                if (n.parent == null)
                    root = y;
                else if (n == n.parent.right)
                    n.parent.right = y;
                else
                    n.parent.left = y;
                y.right = n;
                n.parent = y;
            }
            else
            {
                y = n.parent;
                y.parent.right = n;
                n.parent = n;
                n.parent.right = y;
                n.parent = y.parent;
                y.parent = n;
                y.left = null;
            }

         
        }
        //поворот влево

        protected void rotate_left(RBNode n)
        {

            var y = new RBNode("");
            if (n.right != null)
            {
                y = n.right;
                n.right = y.left;

                if (y.left != null)
                    y.left.parent = n;
                y.parent = n.parent;
                if (n.parent == null)
                    root = y;
                else if (n == n.parent.left)
                    n.parent.left = y;
                else
                    n.parent.right = y;
                y.left = n;
                n.parent = y;
            }
            else
            {
                y = n.parent;

                y.parent.left = n;
                n.parent = n;
                n.parent.left = y;
                n.parent = y.parent;
                y.parent = n;
                y.right = null;
            }
            ////if (n.parent == null)
            ////    return;

            //////левый потомок узла ставим в правый родительского
            ////if (n.left != null)
            ////{
            ////    n.parent.right = n.left;
            ////    n.parent.right.parent = n.parent;
            ////}
            ////else
            ////    n.parent.right = null;

            ////n.left = n.parent;//родительский ставим на его место
            ////n.parent = n.parent.parent;//ссылку на нового родителя берем у бывшего родителя

            ////n.left.parent = n;//бывшему родителю ставим ссылку на текущий узел

            ////if (n.parent != null)//если нужно поправить ссылку у дедушки
            ////{
            ////    if (n.parent.left == n.left)
            ////        n.parent.left = n;
            ////    if (n.parent.right == n.left)
            ////        n.parent.right = n;
            ////}
            ////else //корень дерева поменялся
            ////    this.root = n;

        }



        //найти дедушку

        protected RBNode grandparent(RBNode n)
        {
            if ((n != null) && (n.parent != null))
                return n.parent.parent;
            else
                return null;
        }

        //найти дядю

        protected RBNode uncle(RBNode n)
        {
            RBNode g = grandparent(n);
            if (g == null)
                return null;
            if (n.parent == g.left)
                return g.right;
            else
                return g.left;

        }
      
    }
    
}
