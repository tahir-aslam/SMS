   M S B A M L   `   `   `  ����H  DWebcamControl, Version=3.0.0.0, Culture=neutral, PublicKeyToken=nullD2clr-namespace:WebcamControl;assembly=WebcamControlWebcamControl  5       
WPF Webcam   WPF_Webcam.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Bcam2clr-namespace:WebcamControl;assembly=WebcamControl     5      -   5          � Title$  
WPF Webcam��5      $	��495��6   $	��353��6%    � WindowStartupLocation$ CenterScreen=�61    � 	Resources 5         DevicesListTemplate��%I   &	        �� 5   
   .R�5      �� ��6   �� *Name+5	      5
      .��5      � .+�5   
      WebcamControl.Webcam  -   5        $��
WebcamCtrl��5      $��0,10,0,0q�6'   $��Center=�69   $	��Top=�6V   $	��240��5      $	��326��6"   �� 5   
   $��	0,0,0,140q�6    �� Orientation$ Vertical=�6(   $��Center=�6?   $��Bottom=�6\   .+�5      �� $ 
Horizontal=�6   $��Center=�62   $��22��6O   .+�5      �� $��22��6   $��78��6(   $
��Left=�63   $	��Top=�6N   $��Video Device��5       �� TextAlignment$
 Left=�60   $��LeftToRight=�6E   �� 5      -   5         ��Name$ VideoDevicesComboBox��5      $��22��5      $	��210��6'   $
��Left=�63   $	��Top=�6N   #��[  5   9   5      �� 5      $ 
Horizontal=�6   $��Center=�62   $��23��6O   $��0,10,0,0q�6[   .+�5      �� $��22��6   $��78��6(   $
��Left=�63   $	��Top=�6N   $��Audio Device��5      $
 Left=�60   $��LeftToRight=�6E   �� 5      -   5        $ AudioDevicesComboBox��5      $
��Left=�5      $��23��66   $	��210��6B   $��Bottom=�6N   #��[  5   9   5      5      � 5    
   $��0,0,0,21q�6   $��Center=�6!   $��Bottom=�6>   .+�5!      �� -   5         ��Name$ StartRecordingButton��5"   -   $��Start Recording��5!      $��24��6/   $	��115��6;   $��0,40,0,0q�6G   $
��Left=�6Y   $	��Top=�5"      6K   �� 5#      -   5        $ StopRecordingButton��5$   -   $��Stop Recording��5#      $��24��6.   $	��112��6:   $��0,40,0,0q�6F   $��Right=�6X   $	��Top=�5$      6J   �� 5%      -   5        $ StopCaptureButton��5&   -   $��Stop Capture��5%      $��24��6,   $	��112��68   $��Right=�6D   $	��Top=�5&      6H   �� 5'      -   5        $ StartCaptureButton��5(   -   $��Start Capture��5'      $��24��6-   $	��115��69   $
��Left=�6E   $	��Top=�5(      6I   �� 5)      -	   5        $ TakeSnapshotButton��5*   -   $��Take Snapshot��5)      $��24��6-   $	��120��69   $��
60,78,60,0q�6E   $��Center=�6Y   $	��Top=�5*      6I   5+      5,      5-      