   M S B A M L   `   `   `  ����M  IJunkResourceFinder, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:JunkResourceFinderJunkResourceFinder  5   	   &     JunkResourceFinder.ExecuteWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:JunkResourceFinder     5   	     � Title$  ExecuteWindow��5   	   $
��auto��6   $
��auto��6,    � 
ResizeMode$ 	CanResize=�6:   .��5	      � $��Stretch=�6    � RowDefinitions 5
   
   �� 5      $
��auto �6   6,   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� -   5        $��
grpBoxInfo��5   (   $��10,10,10,10q�6   $y�	Collapsed=�6:   .��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� .��6   Total Resource Found :60   �� 5      -   5        $��lblTotalResource��5      62   �� 5      $��90,0,0,0q�6   .��6*   Total Matches :6;   �� 5      -   5        $��lblTotalMatches��5      61   �� 5      $��90,0,0,0q�6   .��6*   Total Junks :69   �� 5      -   5        $��lblTotalJunks��5      6/   �� 5      -   5        $��btnExportToExcel��5   +   $��90,0,0,0q�6   .��6b   Export to Excel6s   5      5      �� 5    
   -   5        $��	loadPanel��5    "   $��0��6   $ Vertical=�63   $��10,10,10,10q�6J   $y�Visible=�6_   .+�5!      �� .��6   Please wait....6%   � 5"      
������� � IsIndeterminate . $
��Left=�6D   $��5��6_   $	��Top=�6j   $
��1000��6�    J� RenderTransformOrigin$ 0.5,0.5&�6�   z�5#      V� 5$      .~�5%      ��  �� ScaleY$ -1Q�6)   �� 5&      �� 5'      T� 5(      5)      5+      5,      &   System.Windows.Controls.DataGrid  5-   
   -   5          Name$ dataGrid��5-   a   $��1��6   	   
IsReadOnly	 . 
   AutoGenerateColumns
 . $��1��6M   $��Center=�6s   $��	10,10,0,0q�6�   $	��Top=�6�   $
��auto��6�   $
��auto��6�    � 	Resources 5.       �� 
TargetType��%E   '��       �� 5/      " ��6   .�50      -   5        51      52      53      55      56      