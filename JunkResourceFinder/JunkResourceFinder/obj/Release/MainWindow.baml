   M S B A M L   `   `   `  ���� M  IJunkResourceFinder, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:JunkResourceFinderJunkResourceFinder  #    JunkResourceFinder.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:JunkResourceFinder       � Title$  CRM Junk File Explorer��$��644.737��$
��auto��.��� $
��auto��$��0,30,0,0q� � RowDefinitions �� $��274.316 ��� $��91.105 ��� $��* ��� $��10 �.+��� -    ��Name$ grpBoxCRMConnection��$��0��$��Connect to CRM��$
��Left=�$��	10,10,0,0q�$	��Top=�$	��262��$	��615��.���  � ColumnDefinitions �� $��100px ��� $��300px ��� $��* � �� $��* ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��10,10,10,10q�.��Server Url:�� -   $��txtBoxServerUri��$��1��$��2��$��10,10,10,10q��� $��10,10,10,10q�$��1��.��	Domain:�� -   $��txtBoxDomain��$��1��$��1��$��10,10,10,10q��� $��10,10,10,10q�$��2��.��	Username:�� -   $��txtBoxUserName��$��2��$��1��$��10,10,10,10q��� $��10,10,10,10q�$��3��.��	Password:G� -   $��txtBoxPassword��$��3��$��1��$��10,10,10,10q��� -   $��btnTestConnection��$��Connect��$��2��$��2��$
��30px��$��20,10,20,10q��� -   $ grpBoxLocalConnection��$��1��$!��Connect to Local Repository��$
��Left=�$��	10,10,0,0q�$	��Top=�$��73��$	��615��.���  �� $��* ��� $
��Auto � �� $��* �.+��� -   $��txtBoxBrowseFolder��$��3��$��0��$��10,10,20,10q��� -	   $��btnBrowseFolder��$��10,10,20,10q�$��1��$��160px��$
��30px��.��Browse Folder�� -
   $ grpBoxPublisherSettings��$y�	Collapsed=�$��2��$��	10,10,0,0q�$��	Publisher��$
��Left=�$	��Top=�$	��615��$��2��.����  �� Orientation$ 
Horizontal=�.+��� -    ��Name$ label��$��10,10,10,10q�$��Select a Publisher :���� -   $��cmmbBoxPublisherList�� �� DisplayMemberPath$
 _Key�� �� SelectedValuePath$ _Value��$��10,10,10,10q�$��200px���� -   $��
btnCompare��$��80,10,10,10q�$��Execute��
�������$��150px��� -   $��progressBar��$y�	Collapsed=�$��3��$��10��$
��auto�� � IsIndeterminate . 
����ح�