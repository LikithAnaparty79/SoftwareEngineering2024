   M S B A M L   `   `   `  ����C  ?UXModule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:UXModule.ViewsUXModule.Views  5      B >Content, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullN4clr-namespace:Content.ChatViewModel;assembly=ContentContent.ChatViewModel 5      N4clr-namespace:Content.ChatViewModel;assembly=ContentContent.ChatViewModel 5          UXModule.Views.ChatPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:UXModule.Views     5      Pchatapplication4clr-namespace:Content.ChatViewModel;assembly=Content     5      J	Converter4clr-namespace:Content.ChatViewModel;assembly=Content     5        N� Title$  ChatPage��5       � 	Resources 5      <  6Content.ChatViewModel.EmptyStringToVisibilityConverter $   EmptyStringToVisibilityConverter5  /Content.ChatViewModel.NullToVisibilityConverter  NullToVisibilityConverter0  *Content.ChatViewModel.BoolToBrushConverter  BoolToBrushConverter4  .Content.ChatViewModel.BoolToAlignmentConverter  BoolToAlignmentConverter5  /Content.ChatViewModel.BoolToVisibilityConverter  BoolToVisibilityConverter  CornerRadiusStyle �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius ��Name  ChatSendButton �� Triggers	 P� Property
 P� Value3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   Color   Opacity   
BlurRadius�� 5   
   %  &	        &	      &	 &     &	 9     &	 L     &	 _     &	 �      5      69     5      62     5      6-     5      61     5      62   �� 5      " ��6.   .�5      ��  � �� 5      �� 5      " ��6*   .R�5      �� $ 10��6%   
����������#A6A6A6$��1q�6`   ._�5   "   �� $ PART_ContentHost��6/   5      5      5      �� 5        � ��$ 5q�6,   �� 5!       � ��
 �   �6/   5"      �� 5%      " ��6+   .�5&      ��  � �� �#854F6C6/   �� 5'       � ��
 �����6/   �� 5(       � ��$ 11(�6-   �� 5)       � ��$ 2q�6+   �� 5*       � �� 5+      �� 5,      " ��6*   .R�5-      �� #��z��6%   $ 5��6O   ._�5.   "   �� $��Center=�63   $��Center=�6P   5/      50      52       53      P� 54      	 �@J�IsMouseOver$

 True��65   . �55      ��  �@J�Effect 56        57   "   $ 0Q�63   $ White��6C   $ 1Q�6Q   $ 20Q�6]   59      5:      5<      5=      .��5@      � ��5C   
   �� 5D       �� ImageSource$ Resources/bg2.jpeg��6    j� Stretch$
 Fill=�6:    �� Opacity$
 0.89Q�6I   .+�5H   
   ]� -   5        $��TopDockPanel��5H      $��50��6(   ���#190019$	��Top=�6I   $y�Visible=�6a   .+�5I      �� $��In-Call Messages��6   $��18(�60   $
��Bold"�6>   
�������$
��Auto��6c   $��Center=�5J      $��15q�6+   �� 5K       �� Orientation$ 
Horizontal=�6   $��Right=�62   $��Center=�6N   $��	10,0,10,5q�6i   .+�5N      �� -   5        $��SearchButton��5N      $
��🔍��6-   $��39��6:   $��39��6E   $��18(�6Q   ���#854F6C$��0q�5O   &   
���������5P      �� 5Q      " ��6    .�5R      ��  � �� 5S   "   �� 5T   &   " ��66   .R�5U   *   � .+�5V   .   A� #��z��66   �� 5W   .   $��Center=�6?   $��Center=�6\   5X   +   5Y   '   5[       5\      P� 5]   "   	 �@J�IsMouseOver$

 True��6A   . �5^   &   ��  �@J�Effect 5_   *     5`   .   $ 0Q�6?   $ White��6O   $ 1Q�6]   $ 20Q�6i   5b   '   5c   #   5e      5g      5h      5i      �� 5k   
   $ Vertical=�6   $��10q�6,   $	��Top=�68   .+�5l      �� -   5        $��SearchPanel��5l      $ 
Horizontal=�6,   $y�	Collapsed=�6E    J� Opacity$ 0Q�6\   $��Center=�6h   $��0,5,0,20q�6�   .+�5m      �� -   5        $��BackFromSearchButton��5m      $	��◄��65   $��30��6A   $��28��6L   $��12(�6X   ���#C0A392
�������6�   �� 5n      -   5        $��SearchTextBox��5n      $	��200��6/   $��5,5,5,5q�6;   $��12(�5o      ���#F0F0F0$�� ��5p      6"   �� 5q      -   5        $��ClearSearchButton��5q      $	��✖��62   $��30��6>   $��28��6I   $��18(�6U   
������ $��0q�6|   
�������6�   5r      �� 5u      -   5         ��Name$ MessagesListView��5u      $��	0,50,0,50q�5w      
������ $��Stretch=�5y      $
��Auto=�5z      $
��True��5{      $	��650��5|      ��5v      �� *
Messages+��5}      �� 5~      .R�5      �� $ 
Horizontal=�6%   $��5q�6>   ��6J   �� *IsSentByUser+ �� 	Converter# [ .+�5�      A� $��40��6&   $��40��61   y�6=   �� *IsSentByUser+# [  �� ConverterParameter$ False����5�   "   �� 5�   &    61   �� *UserProfileUrl+5�      �� 5�      $��UserProfilePicture��6$   $��0��6>   $��0��6H   �� 5�      $ 10��5�       �� Padding$ 10q�5�      $��5q�6"   $	��300��6-   $
��Auto��6<   ��5�      �� *IsSentByUser+# [ ._�5�   "   �� $ 
Horizontal=�6-   .+�5�   &   �� $ Vertical=�61   .+�5�   *   �  � ColumnDefinitions 5�   .   �� 5�   2   $��* �6C   �� 5�   2   $
��Auto �6C   .+�5�   .   �� $��11(�5�   ,   $
��Bold"�5�   ,   ���#2B124C$��Center=�5�   ,   ��5�   8   �� *User+5�   ,   �� 5�   .   -   5        $��1��5�   5   $	��⋮��6G   $��20��5�   )   $��20��5�   )   $��12(�5�   )   $��Center=�5�   )   $��Right=�5�   )   
������ 
�������
������ $��0q�5�   )    � DataContext 5�   )   �� y�5�   6   �� *IsSentByUser+# [ $
 True�� � ContextMenu 5�   2   �� 5�   6   .@�5�   :   m� -	   5        $��Delete For Me��5�   C   5�   +   5�   7   5�   /   5�   +   �� 5�   *   $��MessageTextBlock��5�   (   $
��Auto��5�   4   $��12(�5�   (   $
��Wrap=�5�   (   $	��300��5�   (   
�������
�5�   .   �� 5�   2   �66   �� *	Content+ �� 5�   2   ���#190019$
��Bold"�6l   �66   �� *HighlightedText+6l    �� 5�   2   �66   �� *HighlightedAfterText+5�   +   �� 5�   *   $��8(�6J   $��Right=�6W   
���OO/���64   �� *Time+6s   5�   '   5�   #   5�      5�      5�      5�      � 5�      -
   5        $��NotFoundPopup��5�       � 	StaysOpen$ False��6)   $��Center=�6b   ��6;   �� *IsNotFoundPopupOpen+.��5�      �� 
�������$ 5��60   $ 10q�6A   ._�5�      �� $��Message Not Found��6    
���  ��$��14(�6N   5�      5�      5�      ]� 5�   
   $��Bottom=�6   $��50��6/   
������ $��7,0,7,2q�6T   .+�5�      � .+�5�      �� -   5         ��Name$ ClientDropdown��5�      $��62��64   $��1,5,0,5q�6a   $��36��6r   
����� ���6?   �� *
Clientte+ �� SelectedItem 6�   �� *
Recipientt+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5�      $��Members��6   
�������$��Center=�6=   $��5,0,0,0q�6X     J� IsHitTestVisible$  False��6i   $
��10.4(�6�   y�5�      �� *SelectedItem+! �� ElementName$! ClientDropdown��# [ 5�      � 5�       5�      �� 5�      $��* �6'   �� 5�      $
��Auto �6'   �� 5�      $
��Auto �6'   .+�5�      �� -   5        $��MessageTextBox��5�   *   $��0��6   #��[ 5�      $��	7,10,5,10q�5�      ���#190019$��12(�67   $��segoe ui)�5�      $	��182��6$   $	��210��63   ���#F0F0F0��5�      �� *MessageTextBox_Text+$ TwoWay=�$ PropertyChanged=�.��5�   1   " J� InputBindings" 5�      �� 5�      
# �� Key$# Enter��6%   $ �� Command$ 61   �� *EnterKeyCommand+.��5�   -   5�      �� 5�      -   5        $��1��5�      $	��☻��6)   $��32��65   $��32��6@   $��22(�6L   
�������$��5,5,5,5q�5�   $   ���#854F6C��5�      �� 5�      " ��6    .�5�      ��  � �� 5�   "   �� 5�   &   " ��66   .R�5�   *   � .+�5�   .   A� #��z��66   �� 5�   .   $��Center=�6?   $��Center=�6\   5�   +   5�   '   5�       5�      P� 5�   "   	 �@J�IsMouseOver$

 True��6A   . �5�   &   ��  �@J�Effect 5�   *     5�   .   $ 0Q�6?   $ White��6O   $ 1Q�6]   $ 20Q�6i   5�   '   5�   #   5�      5      � 5     -   5        $��
EmojiPopup��5     $ False��6*   $	��Top=�6<   % � AllowsTransparency$
% True��6L   .��5     �� 
�������$ 5��60   $ 10q�6A   
�������$��1q�6a   ._�5     A� & A� Rows$& 4��6&   ' A� Columns$' 7��6/   .+�5     �� -   5        $
��😀��5  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��😂��5  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��😍��5  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��😎��5  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��😢��5  %   $��30��62   $��30��6=   6I   �� 5	     -   5        $
��😡��5	  %   $��30��62   $��30��6=   6I   �� 5
     -   5        $
��😇��5
  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��😆��5  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��😅��5  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��😂��5  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��😊��5  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��🙂��5  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��🙃��5  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��😉��5  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��😌��5  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��😍��5  %   $��30��62   $��30��6=   6I   �� 5     -   5        $
��🥰��5  %   $��30��62   $��30��6=   6I   �� 5     -    5        $
��😘��5  %   $��30��62   $��30��6=   6I   �� 5     -!   5        $
��😗��5  %   $��30��62   $��30��6=   6I   �� 5     -"   5        $
��😚��5  %   $��30��62   $��30��6=   6I   �� 5     -#   5        $
��😋��5  %   $��30��62   $��30��6=   6I   �� 5     -$   5        $
��😛��5  %   $��30��62   $��30��6=   6I   �� 5     -%   5        $
��😝��5  %   $��30��62   $��30��6=   6I   �� 5     -&   5        $
��🤪��5  %   $��30��62   $��30��6=   6I   �� 5     -'   5        $��🤲🏻��5  %   $��30��64   $��30��6?   6K   �� 5     -(   5        $
��🧠��5  %   $��30��62   $��30��6=   6I   �� 5     -)   5        $��👂🏻��5  %   $��30��64   $��30��6?   6K   �� 5     -*   5        $
��🐽��5  %   $��30��62   $��30��6=   6I   �� 5      -+   5        $
��😂��5   %   $��30��62   $��30��6=   6I   �� 5!     -,   5        $
��😍��5!  %   $��30��62   $��30��6=   6I   �� 5"     --   5        $
��😎��5"  %   $��30��62   $��30��6=   6I   �� 5#     -.   5        $
��😢��5#  %   $��30��62   $��30��6=   6I   �� 5$     -/   5        $
��😡��5$  %   $��30��62   $��30��6=   6I   �� 5%     -0   5        $
��😇��5%  %   $��30��62   $��30��6=   6I   �� 5&     -1   5        $
��😆��5&  %   $��30��62   $��30��6=   6I   �� 5'     -2   5        $
��😅��5'  %   $��30��62   $��30��6=   6I   �� 5(     -3   5        $
��😂��5(  %   $��30��62   $��30��6=   6I   �� 5)     -4   5        $
��😊��5)  %   $��30��62   $��30��6=   6I   �� 5*     -5   5        $
��🙂��5*  %   $��30��62   $��30��6=   6I   �� 5+     -6   5        $
��🙃��5+  %   $��30��62   $��30��6=   6I   �� 5,     -7   5        $
��😉��5,  %   $��30��62   $��30��6=   6I   �� 5-     -8   5        $
��😌��5-  %   $��30��62   $��30��6=   6I   �� 5.     -9   5        $
��😍��5.  %   $��30��62   $��30��6=   6I   �� 5/     -:   5        $
��🥰��5/  %   $��30��62   $��30��6=   6I   �� 50     -;   5        $
��😘��50  %   $��30��62   $��30��6=   6I   �� 51     -<   5        $
��😗��51  %   $��30��62   $��30��6=   6I   �� 52     -=   5        $
��😚��52  %   $��30��62   $��30��6=   6I   �� 53     ->   5        $
��😋��53  %   $��30��62   $��30��6=   6I   �� 54     -?   5        $
��😛��54  %   $��30��62   $��30��6=   6I   �� 55     -@   5        $
��😝��55  %   $��30��62   $��30��6=   6I   �� 56     -A   5        $
��🤪��56  %   $��30��62   $��30��6=   6I   �� 57     -B   5        $��🤲🏻��57  %   $��30��64   $��30��6?   6K   �� 58     -C   5        $
��🧠��58  %   $��30��62   $��30��6=   6I   �� 59     -D   5        $��👂🏻��59  %   $��30��64   $��30��6?   6K   �� 5:     -E   5        $
��🐽��5:  %   $��30��62   $��30��6=   6I   5<     5=     5>     �� 5@     $��2��6   #��[ 6)   $��30��5A     $��30��6   $��	5,10,5,10q�6(   $��Right=�6;   ��5B     �� *SendMessageCommand+.��5C     �� $��Resources/send-mail.png��6   $��25��6=   $��20��6H   5D     5E     5F     5G     5H     