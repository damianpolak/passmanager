   M S B A M L   `   `   `  ���� J  FPasswordManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:PasswordManagerPasswordManager  T PMahApps.Metro, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls     PasswordManager.Generator   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:PasswordManager     PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro       � Title$  Password Generator��$	��392��$��409.916��
���   � � 
ResizeMode$ NoResize=�(  "MahApps.Metro.Controls.MetroWindow    IsMaxRestoreButtonEnabled .     ShowMaxRestoreButton .     IsMinButtonEnabled .     ShowMinButton .   � WindowStartupLocation$ CenterOwner=�.���  � ColumnDefinitions �� $	��45* ��� $	��56* �.+��� -    ��Name$ cbAlpha��$��ABCDE...XYZ��$
��Left=�$��	10,10,0,0q�$	��Top=�$��96��	 d� 	IsChecked$
	 TrueY��� -   $ 	cbNumbers��$��
1234567890��$
��Left=�$��	10,33,0,0q�$	��Top=�$��96��$
	 TrueY��� -   $ 
cbSpecChar��$��!@#$%^&*(_+)":><?��$
��Left=�$��	10,56,0,0q�$	��Top=�$	��135���� -   $ cbMd5��$	��md5��$
��Left=�$��	10,79,0,0q�$	��Top=�$	��135���� -   $ cbUpperCase��$��Upper-case (ABCD)��$
��Left=�$��
175,10,0,0q�$	��Top=�$	��135��$��2���� -   $ cbLowerCase��$��Lower-case (abcd)��$
��Left=�$��
175,33,0,0q�$	��Top=�$	��135��$��2���� -   
 ��Name$
 label��$#��Length of generated password:��$
��Left=�$��
175,51,0,0q�$	��Top=�$��2���� -    ��Name$ tbLength��$
��Left=�$��23��$��173.682,51,0,0q� �� TextWrapping$
 Wrap=�$��8��$	��Top=�$��36��$��1���� -	   $ 	tbPreview��$
��Left=�$	��163��$��
10,114,0,0q�$
 Wrap=�$	��Top=�$	��380�� �� AcceptsReturn . " �� VerticalScrollBarVisibility$ Visible=�$��2���� -
    ��Name$ 
btGenerate��$��Generate��$
��Left=�$��160,302,0,0q�$	��Top=�$��78��$��2��