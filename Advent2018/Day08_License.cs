using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2018.Day08
{
    public class Node
    {
        private static char[] Letters = InitializeLetters().ToArray();

        private int _Id = 0;

        public char Id { get { return Letters[_Id]; } }
        public int ChildrenCount { get; set; }
        public int MetadataCount { get; set; }
        public List<Node> ChildNodes { get; } = new List<Node>();
        public Node Parent { get; private set; }
        public List<int> Metadata { get; } = new List<int>();

        public Node(Node parent, int id)
        {
            Parent = parent;
            _Id = id;
        }

        public Node(int id)
        {
            _Id = id;
        }

        public Node AddChildNode(int id)
        {
            Node child = new Node(this, id);
            ChildNodes.Add(child);
            return child;
        }

        public bool IsDone()
        {
            return ChildNodes.Count == ChildrenCount && Metadata.Count == MetadataCount;
        }

        public int GetValue()
        {
            if (ChildNodes.Count == 0)
            {
                return Metadata.Sum();
            }
            else
            {
                int sum = 0;
                foreach (var index in Metadata)
                {
                    if (index > 0 && index <= ChildNodes.Count)
                    {
                        sum += ChildNodes[index - 1].GetValue();
                    }
                }
                return sum;
            }
        }

        public static IEnumerable<char> InitializeLetters()
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                yield return c;
            }
            for (char c = 'a'; c <= 'z'; c++)
            {
                yield return c;
            }
        }
    }

    public enum State
    {
        Child,
        MetadataQuantity,
        Metadata
    }

    public class Day
    {
        private int _MetadataTotal = 0;
        private int _MaxId = 0;
        private Stack<Node> _Parents = new Stack<Node>();

        private (Node node, State state) AddChildNode(Node current)
        {
            _MaxId++;
            Node child = current.AddChildNode(_MaxId);
            _Parents.Push(child);
            return (child, State.Child);
        }

        private Node BuildNodes(int[] list)
        {
            _MaxId = 0;
            _Parents.Clear();

            Node root = new Node(_MaxId);
            _Parents.Push(root);

            Node current = root;
            State state = State.Child;

            foreach (int value in list)
            {
                (current, state) = Process(state, current, value);
            }

            return root;
        }

        private (Node node, State state) GetParentState()
        {
            while (_Parents.Count > 0 && _Parents.Peek().IsDone())
            {
                _Parents.Pop();
            }
            if (_Parents.Count == 0)
            {
                return (null, State.Child);
            }
            else
            {
                Node node = _Parents.Peek();
                if (node.ChildrenCount > node.ChildNodes.Count)
                {
                    return AddChildNode(node);
                }
                else
                {
                    return (node, State.Metadata);
                }
            }
        }

        private (Node node, State state) Process(State state, Node current, int value)
        {
            Node nextNode = current;
            State nextState = state;

            switch (state)
            {
                case State.Child:
                    current.ChildrenCount = value;
                    nextState = State.MetadataQuantity;
                    break;

                case State.MetadataQuantity:
                    current.MetadataCount = value;
                    if (current.ChildNodes.Count == current.ChildrenCount)
                    {
                        if (current.Metadata.Count == current.MetadataCount)
                        {
                            (nextNode, nextState) = GetParentState();
                        }
                        else
                        {
                            nextState = State.Metadata;
                        }
                    }
                    else
                    {
                        (nextNode, nextState) = AddChildNode(current);
                    }
                    break;

                case State.Metadata:
                    _MetadataTotal += value;
                    current.Metadata.Add(value);
                    if (current.Metadata.Count == current.MetadataCount)
                    {
                        if (current.ChildNodes.Count == current.ChildrenCount)
                        {
                            (nextNode, nextState) = GetParentState();
                        }
                        else
                        {
                            (nextNode, nextState) = AddChildNode(current);
                        }
                    }
                    break;
            }

            return (nextNode, nextState);
        }

        public int Solve1(int[] list)
        {
            BuildNodes(list);
            return _MetadataTotal;
        }

        public int Solve2(int[] list)
        {
            Node root = BuildNodes(list);
            return root.GetValue();
        }
    }
}
