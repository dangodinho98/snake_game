   M S B A M L   `   `   `  ����	  Snake    Snake.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�6.    � WindowStyle$
 None=�6D    � Title$ SnakeWPF - Score: 0��5       � SizeToContent$ WidthAndHeight=�66   .��5	      ]� 
���   �.+�5
   
   � -   5        $��pnlTitleBar��5
   $   $	��Top=�6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   �� 5      $
��Auto �6#    � 	Resources 5       �� 
TargetType �� Property	 �� Value��%�   '��       �� 5      " ��6   .�5      ��  � ��$	 Consolas)�63   �� 5       � ��
	 �����63   �� 5       � ��$	 24(�61   �� 5       � ��$
	 Bold"�63   5      5      .+�5      � $��10,0,0,0q�6   .+�5      �� Score:6$   �� 5      -   5        $��tbStatusScore��5      064   5      � 5      $��1��6   .+�5      �� Speed:6$   �� 5       -   5        $��tbStatusSpeed��5       064   5!      �� 5"      -   5        $��btnClose��5"   �   $��2��6   $��Right=�6%   
������ 
�������$
��Bold"�6h   $��20(�6z   $��0q�6�   $
��10,0q�6�   .��6�   X6�   5#      �� 5$   
   
���   �$��5q�6%   ._�5%      �� -   5        $��GameArea��5%      $
}�True��6%   $	��400��69   $	��400��6E   5&      5'      5(      