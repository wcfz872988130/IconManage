   M S B A M L   `   `   `  ���� E  AFileManage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:FileManage
FileManage      FileManage.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:FileManage     -     � Title$  图标资源管理器��$
��1000��$
��1830�� � WindowStartupLocation$ CenterScreen=�#��� �� J� 	AllowDrop .  J� InputBindings �� .  (System.Windows.Input.ApplicationCommands   Delete �� Command# Z 
 �� Key$ Delete�� �� CommandTarget ��  �� ElementName$ MyVirtualDesignerCanvas��.��� 
���   �	 � ColumnDefinitions	 �� $	��10* ��� $��9* �.+�� $��0�����	#FFDEDEDE
 � RowDefinitions
 �� $��1* ��� $	��10* �	 �� $��3* ��� $��7* �.+��� $��0��$��2��
�������$��3q�._�� ����  �� Color$ 	#FFC5C5C5��.+���  �� Orientation$ 
Horizontal=�$��50,0,0,0q�.+��� $	��150��$ 
Horizontal=�.+��� -    ��Name$ button_import_picture��$	��100��$��导入图片��$��35���� $	��150��$ 
Horizontal=�.+��� -   $ button_save��$	��100��$��保存��$��35���� $	��150��$ 
Horizontal=�.+��� -   $��button_delete��$	��100��$��删除��$��35��#��Z ���� $ MyVirtualDesignerCanvas���� $	��150��$ 
Horizontal=�.+��� -   $ VirtualFormat��$	��100��$��返回上一层��$��35��� $��1��$��0�����	#FFDEDEDE.+��� $
��Auto=�$��5q����� # � ��R� -   $��tvMain���� $��1��$��1��._�� 
 �� $��1* ��� $	��20* �.+��� -    ��Name$ txtVirtualPath��$��Center=�$��0��$��
MainAlbum/��{�.  �� -   $��NewDesignerScrollViewer��
������ $��1��$
��Auto=�$
��Auto=�.��&    FileManage.VirtualDesignerCanvas  -	     Name$ MyVirtualDesignerCanvas��$��5,5,5,5q�$	��800��$	��630�� . 
������� � ContextMenu �� .@�m� $��编辑��.@�m�  m� Command$
 Copy��m� $	 Cut��m� $ Paste��m� -
   $��新建相册��� $��1�����	#FFDEDEDE
 �� $��1* ��� $	��10* �.+��� $��0��
�������$��3q�._�� ���� $ 	#FFC5C5C5��.+��� $ 
Horizontal=�.+��� $	��600��$ 
Horizontal=�.+��� $��文件夹路径：��$��Right=�$��Center=��� -   $ txtPath��$	��400��$��35��$��Center=�$�� ��{�.  �� $��10���� -   $��button_path��$	��100��$��选择路径��$��35���� $	��200��$ 
Horizontal=�.+��� -   $ Format��$	��100��$��返回上一层��$��35���� $��1��
�������$��1q�._�� .+��� -   $��DesignerScrollViewer��
������ $
��Auto=�$
��Auto=�.��   FileManage.DesignerCanvas  -     Name$ MyDesignerCanvas��$��5,5,5,5q�$	��800��$	��600�� . 
�������