using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Models
{
    public class Keyword
    {
        private string _keyword;

        public Keyword(string keyword)
        {
            _keyword = keyword;
        }

        public string Value { get => _keyword; set => _keyword = value; }
    }
}
