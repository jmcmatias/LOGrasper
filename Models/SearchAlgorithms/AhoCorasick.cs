using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;

namespace LOGrasper.Models.SearchAlgorithms
{
    public class AhoCorasick
    {
        private TrieNode _root;

        public AhoCorasick()
        {
            _root = new(null, '\0');
        }
        /// <summary>
        /// Add keyword to Automaton
        /// </summary>
        /// <param name="Keyword"></param>
        public void AddKeyword(KeywordViewModel KeywordToAdd)
        {
            // Set start as the root node
            TrieNode currentNode = _root;
            string kw = KeywordToAdd.Keyword;
            // For each character in the keyword
            foreach (char c in kw)
            {
                // Check if the current character exists as child of the current node
                if (!currentNode.Children.ContainsKey(c))
                {
                    // If the character doesn't exist, create a new TrieNode and add it as child of the current node
                    currentNode.Children[c] = new TrieNode(currentNode, c);
                }
                // Move to the next node
                currentNode = currentNode.Children[c];
            }
            // Set the current node as the end of a keyword
            currentNode.KeywordEnd.End = true;
            currentNode.KeywordEnd.keyword = KeywordToAdd.Keyword;
            currentNode.KeywordEnd.HasNotClause = KeywordToAdd.IsNot;
        }
        /// <summary>
        /// Method for building AC Automaton from existing tree
        /// </summary>
        public void BuildAutomaton()
        {
            // Create a queue to store TrieNodes
            Queue<TrieNode> queue = new Queue<TrieNode>();

            // Enqueue the children of the root node and set their failure to the root
            foreach (TrieNode node in _root.Children.Values)
            {
                queue.Enqueue(node);
                node.Failure = _root;
            }
            // Process the nodes in the queue
            while (queue.Count > 0)
            {
                // Dequeue a node from the queue
                TrieNode currentNode = queue.Dequeue();

                // Process the children of the current node
                foreach (TrieNode childNode in currentNode.Children.Values)
                {
                    // Enqueue the child node
                    queue.Enqueue(childNode);

                    // Find the failure node for the child node
                    TrieNode failureNode = currentNode.Failure;
                    while (failureNode != null && !failureNode.Children.ContainsKey(childNode.Character))
                    {
                        failureNode = failureNode.Failure;
                    }

                    // Set the failure node of the child node
                    if (failureNode == null)
                    {
                        childNode.Failure = _root;
                    }
                    else
                    {
                        childNode.Failure = failureNode.Children[childNode.Character];

                        // If the failure node represents the end of a keyword, update the output of the child node
                        if (childNode.Failure.KeywordEnd.End)
                        {
                            childNode.Output.AddRange(childNode.Failure.Output);
                            childNode.Output.Add(childNode.Failure);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Method to preform the search in a string using the AC automaton
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public List<Tuple<int, KeywordViewModel>>? Search(string line, ObservableCollection<KeywordViewModel> kwList)
        {
            // Create a list to store the matches found
            List<Tuple<int, KeywordViewModel>>? matchesFound = new List<Tuple<int, KeywordViewModel>>();

            // Start from the root node
            TrieNode currentNode = _root;
            bool ListHasNotClause = false;

            if (kwList.Where(item => item.IsNot).Count()>0)
            {
                ListHasNotClause = true;
            }
            

            // Iterate through each character in the line
            for (int i = 0; i < line.Length; i++)
            {
                // Get the current character
                char c = line[i];

                // Traverse back through the failure links until a valid node or the root is reached
                while (currentNode != _root && !currentNode.Children.ContainsKey(c))
                {
                    currentNode = currentNode.Failure;
                }

                // If the current character exists as a child of the current node, move to that child node
                if (currentNode.Children.ContainsKey(c))
                {
                    currentNode = currentNode.Children[c];

                    // Check if the current node represents the end of a keyword
                    if (currentNode.KeywordEnd.End)
                    {

                        if (!currentNode.KeywordEnd.HasNotClause)
                        {
                            // Add a tuple to the list with the starting position of the match and the keyword itself
                            matchesFound.Add(new Tuple<int, KeywordViewModel>(i - currentNode.Depth + 1, new KeywordViewModel(currentNode.KeywordEnd.keyword)));
                            // Check if all keywords in kwList have been found
                            if (matchesFound != null && kwList.Where(item => !item.IsNot).All(item => matchesFound.Any(tuple => (tuple.Item2.Keyword == item.Keyword))))
                            {
                                
                                // If all keywords have been found, return the matchesFound list
                                if(!ListHasNotClause)
                                    return matchesFound;
                            }
                        }
                        else
                        {
                            matchesFound.Clear();
                            return matchesFound;
                        }                     
                      
                    }

                }
            }
            // Return the list of matches found
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
            public bool HasNotClause { get; set; } 
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


