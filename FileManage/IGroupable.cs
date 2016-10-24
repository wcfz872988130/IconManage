using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileManage
{
    public interface IGroupable
    {
        Guid ID { get; }
        Guid ParentID { get; set; }
        bool IsGroup { get; set; }
    }
}
