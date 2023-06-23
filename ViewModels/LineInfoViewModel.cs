using LOGrasper.Models;
using static LOGrasper.Models.OutputObject;

namespace LOGrasper.ViewModels
{
    public class LineInfoViewModel : ViewModelBase
    {
        private readonly FoundInFile.LineInfo _lineinfo;

        public int LineNumber => _lineinfo.Number;
        public string Content => _lineinfo.Content;
        public string LightContent => _lineinfo.LightContent;

        public LineInfoViewModel(FoundInFile.LineInfo lineinfo)
        {
            _lineinfo = lineinfo;
        }

        public string DisplayLine
        {
            get
            {
                return string.Format("Line {0}:\n   {1}", LineNumber, Content);
            }
        }

        public string DisplayLineLight
        {
            get
            {
                return string.Format("Line {0}:\n{1}", LineNumber, LightContent);
            }
        }
    }
}
