   M S B A M L   `   `   `  ���� J  FPasswordManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:PasswordManagerPasswordManager  T PMahApps.Metro, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls     PasswordManager.NewWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:PasswordManager     PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     -     � Title$  Create and encrypt file��$��232.494��$��400.873��(  "MahApps.Metro.Controls.MetroWindow   ShowMinButton .     IsMinButtonEnabled .      IsMaxRestoreButtonEnabled .     ShowMaxRestoreButton .   � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterOwner=�
���   �   IsWindowDraggable .  .��� ���	#FFF3F3F3.+��� -    ��Name$ label��$B��<Enter your password, which will be used to encrypt database.��$
��Left=�$��	10,10,0,0q�$	��Top=��� -   $ 
label_Copy��$��(min 8 characters):��$
��Left=�$��	10,25,0,0q�$	��Top=�$��9.333(��� -   	 ��Name$	 
tbGenerate��$��Generate password��$
��Left=�$��
10,145,0,0q�$	��Top=�$	��138���� -   $	 btDone��$��OK��$
��Left=�$��218,145,0,0q�$	��Top=�$��75���� -   $	 btCancel��$��Cancel��$
��Left=�$��304,145,0,0q�$	��Top=�$��75��G� -   
 G�Name$
 
tbPassword��$
��Left=�$��
124,58,0,0q�$	��Top=�$	��255���� -   $ label1��$��Master password:��$
��Left=�$��	10,58,0,0q�$	��Top=�$
��Bold"��� -	   $ label2��$��Repeat password:��$
��Left=�$��	10,89,0,0q�$	��Top=�$
��Bold"�G� -
   $
 tbPasswordR��$
��Left=�$��
124,89,0,0q�$	��Top=�$	��255��