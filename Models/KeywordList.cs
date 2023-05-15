using LOGrasper.Exceptions;
using LOGrasper.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Models
{
    public class KeywordList
    {
        private readonly ObservableCollection<string> _keywordList;

        public KeywordList()
        {
            _keywordList = new ObservableCollection<string>();
        }

        /*
        /// <summary>
        /// Gets keywordList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Keyword> GetAllKeywords()
        {
            return _keywordList;
        }
        */
        /// <summary>
        /// Method for editing a keyword in the List
        /// </summary>
        /// <param name="existingKeyword">The existing keyword</param>
        /// <param name="newKeyword">The new Keyword that will replace de existing one</param>
        /// <exception cref="InvalidKeywordIndexException"></exception>
        public void EditKeyword(string existingKeyword, string newKeyword)
        {
            if (_keywordList.Contains(existingKeyword))
            {
                _keywordList[_keywordList.IndexOf(existingKeyword)] = newKeyword;
            }
            else
            {
                throw new InvalidKeywordIndexException(_keywordList.IndexOf(existingKeyword));
            }
        }
        /// <summary>
        /// Method to remove a keyword from the list
        /// </summary>
        /// <param name="keyword"></param>
        /// <exception cref="InvalidKeywordIndexException"></exception>
        public void RemoveKeyword(string keyword)
        {
            if (_keywordList.Remove(keyword)) { }

            else throw new InvalidKeywordIndexException(_keywordList.IndexOf(keyword));
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"> the keyword to insert</param>
        /// <exception cref="KeywordConflictException"></exception>
        public void AddKeyword(string keyword)
        {
            foreach (string existingKeyword in _keywordList)
            {
                if (_keywordList.Contains(existingKeyword))
                {
                   
                }
            }

            _keywordList.Add(keyword);
        }

    }
}
