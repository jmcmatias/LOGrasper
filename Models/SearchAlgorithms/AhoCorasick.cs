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
            _root = new(null, '\0');
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

            currentNode.KeywordEnd.End = true;
            currentNode.KeywordEnd.keyword = Keyword;
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

                        if (childNode.Failure.KeywordEnd.End)
                        {
                            childNode.Output.AddRange(childNode.Failure.Output);
                            childNode.Output.Add(childNode.Failure);
                        }
                    }
                }
            }
        }

        public List<Tuple<int, string>>? Search(string line)
        {
            List<Tuple<int,string>>? matchesFound = new List<Tuple<int, string>>();
      

            TrieNode currentNode = _root;

            

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                while (currentNode != _root && !currentNode.Children.ContainsKey(c))
                {
                    currentNode = currentNode.Failure;
                }

                if (currentNode.Children.ContainsKey(c))
                {
                    currentNode = currentNode.Children[c];

                    if (currentNode.KeywordEnd.End)
                    {/*
                        foreach (TrieNode node in currentNode.Output)
                        {
                            //matches.Add(match.newMatch((i - node.Depth + 1),currentNode.KeywordEnd.keyword));
                        }*/


                        matchesFound.Add(new Tuple<int, string>(i - currentNode.Depth + 1, currentNode.KeywordEnd.keyword));
                       
    
                    }
                }
            }

            return matchesFound;
        }


    }


    public class TrieNode
    {
        public TrieNode? Parent { get; private set; }
        public Dictionary<char, TrieNode> Children { get; private set; }
        public char Character { get; private set; }
        public TrieNode? Failure { get; set; }
        public List<TrieNode> Output { get; set; }
        public FinalNode KeywordEnd { get; set; }

        public class FinalNode
        {
            public bool End { get; set; }
         
            public bool AllKeywordsMatched { get; set; }
            public string? keyword { get; set; }

         

        }

        public int Depth { get { return Parent == null ? 0 : Parent.Depth + 1; } }

        public TrieNode(TrieNode? parent, char character)
        {
            Parent = parent;
            Character = character;
            Children = new Dictionary<char, TrieNode>();
            Output = new List<TrieNode>();
            KeywordEnd = new FinalNode
            {
                End = false
            };


        }
    }

}


