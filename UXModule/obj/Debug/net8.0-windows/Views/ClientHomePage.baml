   M S B A M L   `   `   `  ����D  @ViewModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null^=clr-namespace:ViewModel.DashboardViewModel;assembly=ViewModelViewModel.DashboardViewModel  5      U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5       UXModule#   UXModule.Views.ClientHomePage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Olocal=clr-namespace:ViewModel.DashboardViewModel;assembly=ViewModel     5      Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5        N� Title$  ClientHomePage��5
      .��5      � 
������� � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��121* �6   �� 5      $
��436* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   ���#F5F5F5 �� Padding$ 15,10q�63   $��	0,0,0,121q�6C   $��2��6V   ._�5      �  � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $
��Auto �6'   .+�5      �� $��1��6    �� Orientation$ 
Horizontal=�6-   $��Right=�6F   .+�5      A� $��32��6   $��32��6)   $��0,0,10,0q�65   ��5      �� 5       �� ImageSource 6)   �� *ProfilePicURL+5!      �� 5"      $��0,0,0,0q�5#      $��Center=�5$      $
��Bold"�5%      ��5"       �� *
UserName+5%      5&      5'      5(      � 5+   
   $��1��6   $��15,15,15,15q�6   $��2��61    5,      �� 5-      $	��300 �6#   $	��250��6/   �� 5.      $��* �6#   $	��400��6-   .+�52      � $��0��6   $��0,0,15,0q�6#    53      �� 54      $
��Auto �6$   �� 55      $��* �6$   .+�59      �� $��0��6   $��Participants��6)   $��18(�5:      $��SemiBold"�6'   $��0,0,0,10q�5;      �� 5>      $��1��6   $��1q�6&   ���#E0E0E0 �� CornerRadius$ 4��5?      
�������._�5@      �� $
��Auto=�6#   $
��Auto=�5A       $	��300��5B       .��5D      �� $��5q�5E   $   ��5D   '   �� *UserDetailsList+��5F      �� 5G   "   .R�5H   &   �� $ 8q�6-   $	��0,2q�69   ���#F8F9FA$ 4��5I   A   ._�5J   *   �  5K   .   �� 5L   2   $
��Auto �6C   �� 5M   2   $��* �6C   .+�5P   .   A� $��32��66   $��32��6A   $��0,0,10,0q�6M   ��5Q   2   �� 5R   6    6A   �� *ProfilePictureUrl+5T   /   �� 5V   .   $��1��68   $��Center=�5X   5   $��CharacterEllipsis=�5Y   5   ��5W   5   �� *
userName+5Y   5   5Z   +   5[   '   5\   #   5^      5_      5`      5b      � 5e      $��1��6    5f      �� 5g      $��* �6$   �� 5h      $��20 �6$   �� 5j      $��* �6$   .+�5n      �� $��0��6   $��1q�6&   ���#E0E0E0$ 4��5o      
�������._�5p      �  5q      �� 5r      $
��Auto �6,   �� 5s      $��* �6,   .+�5v      �� $��Users Over Time��6$   $��16(�5w   !   $��SemiBold"�6/   $��	15,10,0,0q�5x   !   #  LiveCharts.Wpf.CartesianChart  5y      $��1��5z      &   LiveCharts.Wpf.Charts.Base.Chart   DisableAnimations$
 True��5|         	Hoverable$
 True��5}      $��15q�5      	   Series	 5{      �� *SeriesCollection+
   DataTooltip
 5~      �� *DataTooltip+   AxisX 5�   "     LiveCharts.Wpf.Axis     Title$
 Time��6+      FontSize$ 12Q�68      	Separator 5�   &     LiveCharts.Wpf.Separator  5�   *      Step$ 1Q�68   5�   #      AxisY 5�   "     $ Number of Users��6+   $ 12Q�6C      MinValue$ 0Q�6Q    5�   &     5�   *   $ 1Q�68   5�   #   5�      5�      5�      �� 5�      $��2��6   $��1q�6&   ���#E0E0E0$ 4��5�      
�������._�5�      �  5�      �� 5�      $
��Auto �6,   �� 5�      $��* �6,   .+�5�      �� $��Additional Chart��6$   $��16(�5�   !   $��SemiBold"�6/   $��	15,10,0,0q�5�   !   5�      5�      5�      5�      �� 5�   
   $��3��6   ���#F5F5F5$ 15q�63   ._�5�      �� -   5        $��Leave Session��5�      $��Center=�5�      $��20,10q�5�      $��0q�5�       � 	Resources 5�       �� 
TargetType �� Property �� Value��%^   '��       �� 5�      " ��6   .�5�      ��  �@��CornerRadius$ 4��69   5�      5�      5�      5�      5�      5�      