using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using FileManage.Controls;
using System.Windows.Documents;
using System;
using System.Diagnostics;

namespace FileManage
{
    //These attributes identify the types of the named parts that are used for templating
    [TemplatePart(Name = "PART_DragThumb", Type = typeof(DragThumb))]
    [TemplatePart(Name = "PART_ResizeDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ConnectorDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    public partial class DesignerItem : ContentControl, ISelectable
    {
        #region IsSelected Property
        DateTime mStartHoverTime = DateTime.MinValue;
        private bool Is_Trigger=false;
        public bool canvas_real_album = true;
        public bool Has_Selected = false;
        //TreeViewItem mHoveredItem = null;
        //AdornerLayer mAdornerLayer = null;
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
          DependencyProperty.Register("IsSelected",
                                       typeof(bool),
                                       typeof(DesignerItem),
                                       new FrameworkPropertyMetadata(false));
        #endregion

        #region DragThumbTemplate Property

        // can be used to replace the default template for the DragThumb
        public static readonly DependencyProperty DragThumbTemplateProperty =
            DependencyProperty.RegisterAttached("DragThumbTemplate", typeof(ControlTemplate), typeof(DesignerItem));

        public static ControlTemplate GetDragThumbTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(DragThumbTemplateProperty);
        }

        public static void SetDragThumbTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(DragThumbTemplateProperty, value);
        }

        #endregion

        #region ConnectorDecoratorTemplate Property

        // can be used to replace the default template for the ConnectorDecorator
        public static readonly DependencyProperty ConnectorDecoratorTemplateProperty =
            DependencyProperty.RegisterAttached("ConnectorDecoratorTemplate", typeof(ControlTemplate), typeof(DesignerItem));

        public static ControlTemplate GetConnectorDecoratorTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(ConnectorDecoratorTemplateProperty);
        }

        public static void SetConnectorDecoratorTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(ConnectorDecoratorTemplateProperty, value);
        }

        #endregion

        #region IsDragConnectionOver

        // while drag connection procedure is ongoing and the mouse moves over 
        // this item this value is true; if true the ConnectorDecorator is triggered
        // to be visible, see template
        public bool IsDragConnectionOver
        {
            get { return (bool)GetValue(IsDragConnectionOverProperty); }
            set { SetValue(IsDragConnectionOverProperty, value); }
        }
        public static readonly DependencyProperty IsDragConnectionOverProperty =
            DependencyProperty.Register("IsDragConnectionOver",
                                         typeof(bool),
                                         typeof(DesignerItem),
                                         new FrameworkPropertyMetadata(false));

        #endregion

        public bool Is_folder = false;
        public bool Is_Virtual_Folder = false;
        public List<DesignerItem> my_album = new List<DesignerItem>();
        public DesignerItem myParentDesignerItem=null;
        public string realityAddress = null;
        public int MyAlbumID = 0;
        public delegate void doubleClickDelegate();
        public static event doubleClickDelegate doubleclickEvent;
        static DesignerItem()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DesignerItem), new FrameworkPropertyMetadata(typeof(DesignerItem)));
        }
        //protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        //{
        //    if (!Is_Virtual_Folder)
        //    {
        //        base.OnMouseRightButtonDown(e);
        //        ContextMenu aMenu = new ContextMenu();
        //        MenuItem AddComment = new MenuItem();
        //        AddComment.Header = "添加备注";
        //        AddComment.Click += AddComment_Click;
        //        aMenu.Items.Add(AddComment);
        //        this.ContextMenu = aMenu;
        //        e.Handled = true;
        //    }
        //}
        //private void AddComment_Click(object sender, RoutedEventArgs e)
        //{
        //    HeaderedContentControl it = this.Content as HeaderedContentControl;
        //    HeaderedContentControl iit = it.Header as HeaderedContentControl;
        //    TextBox iiit = iit.Content as TextBox;
        //    iiit.IsHitTestVisible = true;
        //    iiit.IsEnabled = true;
        //}
        public DesignerItem()
        {
            this.AllowDrop = true;
            this.Loaded += new RoutedEventHandler(DesignerItem_Loaded);
        }
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            Is_Trigger = false;
            DesignerCanvas designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;
            //string path=designer.IconDictionary[this];
            if (designer != null)
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                {
                    if (this.IsSelected)
                    {
                        designer.SelectionService.RemoveFromSelection(this);
                    }
                    else if(!Has_Selected)
                    {
                        designer.SelectionService.AddToSelection(this);
                    }
                }
                else if (!this.IsSelected&&!Has_Selected)
                {
                    designer.SelectionService.SelectItem(this);
                }
            e.Handled = false;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);            
            DesignerCanvas designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                return;
            if (!Is_Trigger && canvas_real_album)
            {
                Debug.Write("222222222\n");
                DataObject dataObject = new DataObject(designer);
                //SelectionService.CurrentSelection.OfType<DesignerItem>();
                // Here, we should notice that dragsource param will specify on which 
                // control the drag&drop event will be fired
                Is_Trigger = true;
                System.Windows.DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);                
            }
            e.Handled = false;
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (Is_folder)
            {
                DesignerCanvas designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;
                string path=designer.albumDictionary[this];
                designer.Children.Clear();
                pathStack.albumpath.Push(path);
                designer.IconDictionary.Clear();
                designer.leftdistance = 0;
                designer.topdistance = 0;
                designer.id = 0;
                designer.SetRealityCanvasView(path);
            }
            pathStack.virtualalbumpath.Push(this);
            if(Is_Virtual_Folder)
            {
                if (doubleclickEvent != null)
                {
                    doubleclickEvent();
                }
                VirtualDesignerCanvas virtualdesigner = VisualTreeHelper.GetParent(this) as VirtualDesignerCanvas;                
                virtualdesigner.Children.Clear();
                virtualdesigner.virtualalbum.Clear();
                //virtualdesigner.canvas_environment_album = true;
                virtualdesigner.SetVirtualAlbumView(this);
            }
        }

        void DesignerItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (base.Template != null)
            {
                ContentPresenter contentPresenter =
                    this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;
                if (contentPresenter != null)
                {
                    UIElement contentVisual = VisualTreeHelper.GetChild(contentPresenter, 0) as UIElement;
                    if (contentVisual != null)
                    {
                        DragThumb thumb = this.Template.FindName("PART_DragThumb", this) as DragThumb;
                        if (thumb != null)
                        {
                            ControlTemplate template =
                                DesignerItem.GetDragThumbTemplate(contentVisual) as ControlTemplate;
                            if (template != null)
                                thumb.Template = template;
                        }
                    }
                }
            }
        }
    }
}

