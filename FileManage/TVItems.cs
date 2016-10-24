using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace FileManage {
    /// <summary>
    /// 需要加载的ListViewItem项
    /// </summary>
    public class TVItems {
        /// <summary>
        /// TreeView子项
        /// </summary>
        public TreeViewItem ViewItem {
            get; set;
        }

        /// <summary>
        /// 该子项的路径
        /// </summary>
        public DesignerItem path
        {
            get;
            set;
        }

        public TVItems()
        {
        }
    }
}
