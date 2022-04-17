### Main difference between __dependency properties__
- CLR Properties는 클래스의 프라이빗멤버를 getter setter 를 이용하여 읽고 쓰고 할 수 있다. 
- 대조적으로 Dependency Properties는 로컬객체로 저장되지 않는다.

```csharp
      public static readonly DependencyProperty SetTextProperty = 
         DependencyProperty.Register("SetText", typeof(string), typeof(UserControl1), new 
            PropertyMetadata("", new PropertyChangedCallback(OnSetTextChanged))); 
				
      public string SetText { 
         get { return (string)GetValue(SetTextProperty); } 
         set { SetValue(SetTextProperty, value); } 
      } 
		
      private static void OnSetTextChanged(DependencyObject d,
         DependencyPropertyChangedEventArgs e) { 
         UserControl1 UserControl1Control = d as UserControl1; 
         UserControl1Control.OnSetTextChanged(e); 
      } 
		
      private void OnSetTextChanged(DependencyPropertyChangedEventArgs e) { 
         tbTest.Text = e.NewValue.ToString(); 
      }  
```

```csharp
<Window x:Class = "WpfApplication3.MainWindow" 
   	xmlns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation" 
	xmlns:x = "http://schemas.microsoft.com/winfx/2006/xaml" 
   	xmlns:views = "clr-namespace:WpfApplication3"
   	Title = "MainWindow" Height = "350" Width = "604"> 
   <Grid> 
      <views:UserControl1 SetText = "Hellow World"/> 
   </Grid> 
</Window> 
```
