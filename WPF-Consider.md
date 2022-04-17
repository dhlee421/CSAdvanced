### Main difference between __dependency properties__ and other CLR properties
- CLR Properties는 클래스의 프라이빗멤버를 getter setter 를 이용하여 읽고 쓰고 할 수 있다. 대조적으로 Dependency Properties는 로컬객체로 저장되지 않는다.

- Dependency properties는 DependencyObject class로 부터 상속받은 Dictionary에 key/value 쌍으로 저장된다.

- Windows Presentation Foundation (WPF) provides a set of services that can be used to extend the functionality of a type's property. Collectively, these services are referred to as the __WPF property system__. A property that's __backed__ by the __WPF property system__ is known as a __dependency property__. This overview describes the WPF property system and the capabilities of a dependency property, including how to use existing dependency properties in XAML and in code. This overview also introduces specialized aspects of dependency properties, such as dependency property metadata, and how to create your own dependency property in a custom class.



```csharp
    public static readonly DependencyProperty SetTextProperty = 
         DependencyProperty.Register("SetText", typeof(string), typeof(UserControl1), new 
            PropertyMetadata("", new PropertyChangedCallback(OnSetTextChanged))); 
				
    public string SetText { 
        get { return (string)GetValue(SetTextProperty); } 
        set { SetValue(SetTextProperty, value); } 
    } 
		
    private static void OnSetTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
    { 
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
