   M S B A M L   `   `   `  ���� C  ?UXModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:UXModule.ViewModelUXModule.ViewModel      UXModule.Views.HomePage   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     2local clr-namespace:UXModule.ViewModel       N� Title$  HomePage��.��� .+�� 
������� � ColumnDefinitions �� $
��2.4* ��� $��3* �.+��� $��0��$��Center=�$��Center=�.+��� -   $��Create Session��$��50��$��
20 0 15 15q��� -   $��Join Session��$��50��$��
20 15 15 0q��� $��1��$��Center=�$��Center=�.+���  �� Orientation$ 
Horizontal=�$��Center=�$��10q�.+�A� $	��100��$	��100������  �� ImageSource ��  �� ElementName$ UserProfilePicture�� �� Path$ Source��� -   $��UserProfilePicture��$��0��$��0��$y�	Collapsed=��� $ 
Horizontal=�$��10q�.+��� -   $��UserName��
 � Tag$ UserName��$	��200��$��Center=��� $ 
Horizontal=�$��10q�.+��� -   $��	UserEmail��$��	0 20 0 20q�$ 	UserEmail��$
��Auto��$��center=��� $ 
Horizontal=�$��10q�.+��� -   $��ServerIP��$ ServerIP��$	��200��$��Center=��� $ 
Horizontal=�$��10q�.+��� -   $��
ServerPort��$��	0 20 0 20q�$ 
ServerPort��$	��200��$��Center=�