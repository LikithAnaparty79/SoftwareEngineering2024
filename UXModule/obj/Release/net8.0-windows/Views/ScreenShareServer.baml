   M S B A M L   `   `   `  ���� C  ?UXModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:UXModule.ViewsUXModule.Views  &     UXModule.Views.ScreenShareServer   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .localclr-namespace:UXModule.Views       N� Title$  ScreenShareServer�� � 	Resources    Button.MouseOver.Background �� Color  Button.MouseOver.Border  Button.Pressed.Background  Button.Pressed.Border  Button.Disabled.Background  Button.Disabled.Border  Button.Disabled.Foreground  RoundedButton �� 
TargetType ��Name J� SnapsToDevicePixels ��Name �� Triggers P� Property	 P� Value
 �� 
TargetName �� Value �� Property��%  &	        &	      &	 (     &	 =     &	 R     &	 g     &	 |     &	 �     7   7  7  7  7  7  7  �� $ #55595c���� $ 	#FF3C7FB1���� $ 	#FFC4E5F6���� $ 	#FF2C628B���� $ 	#FFF4F4F4���� $ 	#FFADB2B5���� $ 	#FF838383���� " ��.R��� $ border��#��z��#��z��#��z��$
 true��._��� $ contentPresenter��$|�False��#��z��#��z��$
��True��# z #��z�� P�  �@��IsDefaulted$
	 true��. ��� $
 border��# � �� � ��P�  �@J�IsMouseOver$
	 true��. ��� $
 border��8    � ���� $
 border��8   � ��P�  � ��$
	 true��. ��� $
 border��8   � ���� $
 border��8   � ��P�  � {�$	 false��. ��� $
 border��8   � ���� $
 border��8   � ���� $
 contentPresenter��8   � ��.���� -   $��0��$
��Auto=�$��Disabled=�.��� ���#202124 � RowDefinitions �� $	��16* ��� .+�]� $��1��$��Center=�$��Stretch=�.+��� -   #���   � ToolTip$ Previous Page��$��40��$��40��$��0q� � Cursor$
 Hand�����#3C4043  �� 
TargetType �� Triggers �� Binding �� Mode �� Value��%�   '��       �� " ��.���  �@��CornerRadius$ 50�� ��  �� *CurrentPage+$ OneWay=� 1.a���  � y�$ 	Collapsed=�.���� $.��(ScreenShareAssets/PreviousPageButton.png��$��30���� $��Center=�$��Center=�
������ 
�������$��	20,0,20,0q�$��DemiBold"�$��14(��� ��� *CurrentPage+$ OneWay=� �� $
� of �� �� ��� *
TotalPages+$ OneWay=� ��%�   '��       �� " ��.���  � y�$ Visible=� ��  �� *
TotalPages+$ OneWay=� 0.a���  � y�$ 	Collapsed=��� -   #���  $ 	Next Page��$��40��$��40��$��0q�$
 Hand�����#3C4043 ��%�   '��       �� " ��.���  �@��CornerRadius$ 50�� ��  �� *
IsLastPage+$ OneWay=� True.a���  � y�$ 	Collapsed=�.���� $*��$ScreenShareAssets/NextPageButton.png��$��30���� -   $��Popupone��$��Right=�$��Bottom=�.+��  � AllowsTransparency$
 True��$
��Fade=� � VerticalOffset$	 -46�� � 	StaysOpen$ False������ *IsPopupOpen+$ TwoWay=� � PlacementTarget ��  �� ElementName$ Popupone��.���� ���#212024 �� TextAlignment$ Center=�$��20,10,20,10q�$	��2.3q�
�������$��Verdana)�$��15(� J� Opacity$	 0.7Q����� *	PopupText+$ OneWay=�.��.�� ��%A   '��       �� " ��.���  �@��CornerRadius$ 	0,30,0,30��.����  �� Orientation$ Vertical=�$��0��.+��� $��No One's Here��
���\\��$��60(�$��Comic Sans MS)�$��WrapWithOverflow=�$��Center=� ��%�   '��       �� " ��.���  � y�$ Hidden=� ��  �� *CurrentWindowClients.Count+$ OneWay=� 0.a���  � y�$ Visible=��� $ Vertical=�$��0��$��4,5,4,5q�.+��� -    ��Name$ Items������ *CurrentWindowClients+$ OneWay=����� .R�A�  A� Rows �� *CurrentPageRows+$ OneWay=� A� Columns �� *CurrentPageColumns+$ OneWay=����� .R��� $ Vertical=�.+�� $��Center=�$��Center=�$��5,3,5,3q����� *	TileWidth+$ OneWay=����� *
TileHeight+$ OneWay=�.+��� $��2q�  �� CornerRadius$  5�����#3C4043._��� $��Uniform=����� *CurrentImage+$ OneWay=��� -   #���  $��50��$��50��$ Pin User��$��0q�$
 Hand��$	 0.7Q����� *Id+$ OneWay=�.���� $$��ScreenshareIcons/PinButton.png��$��23��$��23��$��0,3,0,0q� ! ^� 
Conditions" �� Binding# �� RelativeSource$ �� Mode% �� AncestorType& �� Path' �� Value��%,  '��       '�� �     �� " ��.���  � y�$ Hidden=���  � �� �#3C4043 ^� ! �� " �� # �� $$ FindAncestor=�"% �$& IsMouseOver�' True�� " �� *Pinned+$ OneWay=�	' False.0���  � y�$ Visible=��� " ��.���  �@��CornerRadius$ 50���� -   #���  $��50��$��50��$	 0.7Q�$��Center=�$
 Hand��$ 
Unpin User��$��0q����� *Id+$ OneWay=�.���� $&�� ScreenshareIcons/UnpinButton.png��$��25��$��25�� ��%+  '��       '�� �     �� " ��.���  � y�$ Hidden=���  � �� �#3C4043 ^� ! �� " �� # �� $$ FindAncestor=�"% �$& IsMouseOver�' True�� " �� *Pinned+$ OneWay=�' True.0���  � y�$ Visible=��� " ��.���  �@��CornerRadius$ 50���� 
�������$��SemiBold"�
������ $��Bottom=�$��15,0,0,7q�$��12(����� *Name+$ OneWay=�