   M S B A M L   `   `   `  ����C  ?UXModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:UXModule.ViewsUXModule.Views  5      D @ViewModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null`>clr-namespace:ViewModel.FileClonerViewModel;assembly=ViewModelViewModel.FileClonerViewModel 5      #    UXModule.Views.FileClonerPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:UXModule.Views     5      T	viewModel>clr-namespace:ViewModel.FileClonerViewModel;assembly=ViewModel     5        N� Title$  FileClonerPage��5       � 	Resources 5   
      ButtonBackground �� Color  ButtonHoverBackground  StartButtonColor  StopButtonColor �� 
TargetType  DefaultButtonStyle �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius   ShadowDepth   	Direction	   Color
 �� 
TargetType �� CornerRadius �� Triggers P� Property P� Value��%P  &	        &	 !     &	 B     &	 a     &	 �     7   7   7  �� 5      $ #c23b22��67   �� 5      $ #db5856��6<   �� 5      $ Green��67   �� 5      $ #c23b22��66   �� 5      " ��6   .�5      ��  � ��$ Segoe UI)�6/   �� 5       � ��$ 12(�6-   �� 5       � ��$ SemiBold"�6/   �� 5       � ��
 �����6/   �� 5       � ��8   6/   �� 5       � ��8  60   �� 5       � ��$ 1q�64   �� 5       � ��$
 10,5q�6,   �� 5       � ��$
 10,5q�6+   �� 5       � ��$ 35��6+   �� 5        �@�Cursor$
 Hand��6+   �� 5!       �@J�Effect 5"        5#      $ 8Q�6+   $ 2Q�6:   $	 270Q�6J   $	 #AAAAAA��6Z   5%      �� 5&       � �� 5'      �� 5(      "
 ��6*   .R�5)      �� #��z��6%   #��z��5*      #��z��5+      $ 10��5,      ._�5-   "   �� $��Center=�63   $��Center=�6P   #��z��6k   5.       5/      P� 50   "    �@J�IsMouseOver$
 True��6A   . �51   &   �� 8   � ��6C   52   #   54      56      57      58      .��59      �� $
��Auto=�6   $
��Auto=�68   .��5;   
   ]� .+�5>      � $	��Top=�6   $
��auto��6(    � ColumnDefinitions 5?      �� 5@      $��* �6'   �� 5A      $
��auto �6'   .+�5C      �� -   5        $��DirectoryPathTextBox��5C       �� 
IsReadOnly$
 True��66   ��6H   �� *RootDirectoryPath+ �� Mode$ OneWay=��� 5D      $��1��6   $	��...��6)   #��[ 6`   ��67   �� *BrowseFoldersCommand+6`   5E      � 5H      .+�5I      �  K� ZIndex$ 1��6    5J      �� 5K      $	��265��6+   $��3* �6:   �� 5L      $��5 �6+   �� 5M      $	��190��6+   $��2* �6:   .+�5P      � $��0��6    � RowDefinitions 5Q      �� 5R      $��* �6,   �� 5S      $
��Auto �6,   .+�5V      �� $��0��6'   $
��Auto=�64   $
��Auto=�6Y   
�������.��5W      R� -   5        $��0��5W   w   
���������6'   �� *Tree+��5X   "   �� 5Y   &    �� DataType(  "ViewModel.FileClonerViewModel.Node"  6?    �� ItemsSource 6b   �� *
Children+.R�5Z   *   �� 
������� �� Orientation$ 
Horizontal=�6N   $��Stretch=�6g   .+�5[   .   ��  d� 	IsChecked 67   �� *	IsChecked+��6W   �� *CheckBoxCommand+�� 5\   .   $��18��6P   ��64   �� *
IconPath+6P   �� 5]   .   $
��Bold"�6N   ��64   �� *Name+6N   5^   +   5_   '   5a      5b      �� 5d      
�������$��0��6<   $��1��6L   $��5q�6Y   .+�5e      �� $��File Metadata��6(   $
��Bold"�6=   $��0,0,0,5q�6O   � 5f      $ 1��6#    5g   "   �� 5h   &   $
��Auto �67   �� 5i   &   $��* �67    5l   "   �� 5m   &   $
��Auto �64   �� 5n   &   $
��Auto �64   �� 5o   &   $
��Auto �64   �� 5p   &   $
��Auto �64   �� 5q   &   $
��Auto �64   .+�5t   "   �� $��0��6,   $��0��69   $��Name:��6I   $��SemiBold"�6V   $��0,0,5,0q�6l   �� 5u   "   $��0��6,   $��1��69   ��6I   �� *SelectedNode.Name+�� 5w   "   $��1��6,   $��0��69   $��Size (bytes):��6I   $��SemiBold"�6^   $��0,0,5,0q�6t   �� 5x   "   $��1��6,   $��1��69   ��6I   �� *SelectedNode.Size+�� 5z   "   $��2��6,   $��0��69   $��
File Path:��6I   $��SemiBold"�6[   $��0,0,5,0q�6q   �� 5{   "   $��2��6,   $��1��69   ��6I   �� *SelectedNode.FullFilePath+�� 5}   "   $��3��6,   $��0��69   $��IP Address:��6I   $��SemiBold"�6\   $��0,0,5,0q�6r   �� 5~   "   $��3��6,   $��1��69   ��6I   �� *SelectedNode.IpAddress+�� 5�   "   $��4��6,   $��0��69   $��Last Modified:��6I   $��SemiBold"�6_   $��0,0,5,0q�6u   �� 5�   "   $��4��6,   $��1��69   ��6I   �� *SelectedNode.LastModified+5�      5�      5�      �� 5�      $��1��6#   $��5��63   $��Stretch=�6=   �� 5�      $��2��6!   $ Vertical=�61   .+�5�      �� $ 
Horizontal=�6%   .+�5�      �� $��Folders:��6$   $��SemiBold"�67   �� 5�      ��6$   �� *FolderCount+5�      �� 5�      $ 
Horizontal=�6%   .+�5�      �� $��Files:��6$   $��SemiBold"�65   �� 5�      ��6$   �� *	FileCount+5�      �� 5�      $ 
Horizontal=�6%   .+�5�      �� $��Selected Folders:��6$   $��SemiBold"�6@   �� 5�      ��6$   �� *SelectedFoldersCount+5�      �� 5�      $ 
Horizontal=�6%   .+�5�      �� $��Selected Files:��6$   $��SemiBold"�6>   �� 5�      ��6$   �� *SelectedFilesCount+5�      �� 5�      $ 
Horizontal=�6%   .+�5�      �� $*��$Total Size of Selected Files(bytes):��6$   $��SemiBold"�6S   �� 5�      ��6$   �� *SumofSelectedFilesSizeInBytes+5�      � 5�       5�      �� 5�   "   $��* �63   �� 5�   "   $��* �63    5�      �� 5�   "   $
��Auto �60   �� 5�   "   $
��Auto �60   .+�5�      �� $��0��6%   $��0��62   $��Send Request��6B   #��[ 6�   #��[ 6�   ��6Y   �� *SendRequestCommand+{�6�   �� *IsSendRequestEnabled+�� 5�      $��0��6%   $��1��62   $��	Summarise��6B   #��[ 6{   #��[ 6�   ��6V   �� *SummarizeCommand+{�6�   �� *IsSummarizeEnabled+�� 5�      $��1��6%   $��0��62   $��Start Cloning��6B   $	��200��6�   $��30��6�   $��5q�6�   #��[ 6�   #��[ 6�   ��6Z   �� *StartCloningCommand+{�6   �� *IsStartCloningEnabled+�� 5�      $��1��6%   $��1��62   $��Stop Session��6B   $	��200��6�   $��30��6�   $��5q�6�   #��[ 6�   #��[ 6�   ��6Y   �� *StopSessionCommand+{�6�   �� *IsStopCloningEnabled+5�      �� 5�      $	��Log��6    $
��Bold"�6.   �� 5�      $	��450��6F   $	��400��6R   $��5q�6_   
���������6"   �� *LogMessages+6j   5�      5�      5�      5�      5�      5�      