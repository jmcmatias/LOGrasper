using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Models.SearchAlgorithms
{
    public class AhoCorasick
    {
        private TrieNode _root;

        public AhoCorasick()
        {
            _root = new TrieNode(null, '\0');
        }

        public void AddKeyword(string Keyword)
        {
            TrieNode currentNode = _root;

            foreach (char c in Keyword)
            {
                if (!currentNode.Children.ContainsKey(c))
                {
                    currentNode.Children[c] = new TrieNode(currentNode, c);
                }

                currentNode = currentNode.Children[c];
            }

            currentNode.KeywordEnd = true;
        }

        public void BuildAutomaton()
        {
            Queue<TrieNode> queue = new Queue<TrieNode>();

            foreach (TrieNode node in _root.Children.Values)
            {
                queue.Enqueue(node);
                node.Failure = _root;
            }

            while (queue.Count > 0)
            {
                TrieNode currentNode = queue.Dequeue();

                foreach (TrieNode childNode in currentNode.Children.Values)
                {
                    queue.Enqueue(childNode);

                    TrieNode failureNode = currentNode.Failure;

                    while (failureNode != null && !failureNode.Children.ContainsKey(childNode.Character))
                    {
                        failureNode = failureNode.Failure;
                    }

                    if (failureNode == null)
                    {
                        childNode.Failure = _root;
                    }
                    else
                    {
                        childNode.Failure = failureNode.Children[childNode.Character];

                        if (childNode.Failure.KeywordEnd)
                        {
                            childNode.Output.AddRange(childNode.Failure.Output);
                            childNode.Output.Add(childNode.Failure);
                        }
                    }
                }
            }
        }

        public List<int> Search(string text)
        {
            List<int> matches = new List<int>();
            TrieNode currentNode = _root;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                while (currentNode != _root && !currentNode.Children.ContainsKey(c))
                {
                    currentNode = currentNode.Failure;
                }

                if (currentNode.Children.ContainsKey(c))
                {
                    currentNode = currentNode.Children[c];

                    if (currentNode.KeywordEnd)
                    {
                        foreach (TrieNode node in currentNode.Output)
                        {
                            matches.Add(i - node.Depth + 1);
                        }

                        matches.Add(i - currentNode.Depth + 1);
                    }
                }
            }

            return matches;
        }
    }

    public class TrieNode
    {
        public TrieNode Parent { get; private set; }
        public Dictionary<char, TrieNode> Children { get; private set; }
        public char Character { get; private set; }
        public TrieNode Failure { get; set; }
        public List<TrieNode> Output { get; set; }
        public bool KeywordEnd { get; set; }
        public int Depth { get { return Parent == null ? 0 : Parent.Depth + 1; } }

        public TrieNode(TrieNode parent, char character)
        {
            Parent = parent;
            Character = character;
            Children = new Dictionary<char, TrieNode>();
            Output = new List<TrieNode>();
            KeywordEnd = false;
        }
    }
}

