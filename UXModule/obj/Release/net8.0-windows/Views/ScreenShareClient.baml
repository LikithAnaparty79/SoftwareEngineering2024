   M S B A M L   `   `   `  ���� C  ?UXModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:UXModule.ViewsUXModule.Views  &     UXModule.Views.ScreenShareClient   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .localclr-namespace:UXModule.Views       N� Title$  ScreenShareClient��.��� 
���   � � RowDefinitions �� $	��10* ��� .+��� -   $��SharedScreen�� �� Orientation$ Vertical=�$��0��.+��� $��screen share started��
���\\��$��60(�$��Comic Sans MS)�$��WrapWithOverflow=�$��Center=� � 	Resources  �� 
TargetType �� Property �� Value �� Triggers �� Binding	 �� Mode
 �� Value��%�   '��       �� " ��.���  � y�$ Hidden=� ��  �� *SharingScreen+$	 OneWay=�
 True.a���  � y�$ Visible=��� -   $��NotSharedScreen��$ Vertical=�$��0��.+��� -   $��MainText��$%��You are not sharing your screen��
���\\��$��40(�$��Comic Sans MS)�$��WrapWithOverflow=�$��Center=�$��
0 150 0 0 q� ��%�   '��       �� " ��.���  � y�$ Visible=� ��  �� *SharingScreen+$	 OneWay=�
 True.a���  � y�$ Hidden=�]� $��1��$��Center=�$��Stretch=�.+��� -    ��Name$ StartScreenShare�� � ToolTip$ Start Screen Share��$��40��$��80��$��1��$��0q�$	��Top=� � Cursor$
 Hand��.���� $��Start��$��20(�$
��Bold"�
�������$��Center=�$��Center=� ��%�   '��       '�� �     �� " ��.���  � ��
 �"�"� ��  �� *SharingScreen+$	 OneWay=�
 True.a���  �@J�IsHitTestVisible$ False����  � ��
 ���w��� " ��.���  �@��CornerRadius$ 22���� $	��100��
������ �� -   $ StopScreenShare��$ Stop Screen Share��$��1��$��40��$��80��$	��Top=�$
 Hand��.���� $
��Stop��$��20(�$
��Bold"�
�������$��Center=�$��Center=� ��%�   '��       '�� 0     �� " ��.���  �@��CornerRadius$ 22���� " ��.���  � ��
 �  �� ��  �� *SharingScreen+$	 OneWay=�	
 False.a���  �@J�IsHitTestVisible$ False����  � ��
 ���w�