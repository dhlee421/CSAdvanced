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

### WPF Global Exception Handling

- WPF How to catch global 

```csharp
public sealed partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // setting up the Dependency Injection container
        var resolver = ResolverFactory.Get();

        // getting the ILogger or ILog interface
        var logger = resolver.Resolve<ILogger>();
        RegisterGlobalExceptionHandling(logger);

        // Bootstrapping Dependency Injection 
        // injects ViewModel into MainWindow.xaml
        // remember to remove the StartupUri attribute in App.xaml
        var mainWindow = resolver.Resolve<Pages.MainWindow>();
        mainWindow.Show();
    }

    private void RegisterGlobalExceptionHandling(ILogger log)
    {
        // this is the line you really want 
        AppDomain.CurrentDomain.UnhandledException += 
            (sender, args) => CurrentDomainOnUnhandledException(args, log);

        // optional: hooking up some more handlers
        // remember that you need to hook up additional handlers when 
        // logging from other dispatchers, shedulers, or applications

        Application.Dispatcher.UnhandledException += 
            (sender, args) => DispatcherOnUnhandledException(args, log);

        Application.Current.DispatcherUnhandledException +=
            (sender, args) => CurrentOnDispatcherUnhandledException(args, log);

        TaskScheduler.UnobservedTaskException += 
            (sender, args) => TaskSchedulerOnUnobservedTaskException(args, log);
    }

    private static void TaskSchedulerOnUnobservedTaskException(UnobservedTaskExceptionEventArgs args, ILogger log)
    {
        log.Error(args.Exception, args.Exception.Message);
        args.SetObserved();
    }

    private static void CurrentOnDispatcherUnhandledException(DispatcherUnhandledExceptionEventArgs args, ILogger log)
    {
        log.Error(args.Exception, args.Exception.Message);
        // args.Handled = true;
    }

    private static void DispatcherOnUnhandledException(DispatcherUnhandledExceptionEventArgs args, ILogger log)
    {
        log.Error(args.Exception, args.Exception.Message);
        // args.Handled = true;
    }

    private static void CurrentDomainOnUnhandledException(UnhandledExceptionEventArgs args, ILogger log)
    {
        var exception = args.ExceptionObject as Exception;
        var terminatingMessage = args.IsTerminating ? " The application is terminating." : string.Empty;
        var exceptionMessage = exception?.Message ?? "An unmanaged exception occured.";
        var message = string.Concat(exceptionMessage, terminatingMessage);
        log.Error(exception, message);
    }
}

```

|Exception type	| Base type | Description |
| --- | --- | --- |
|Exception | Object | Base class for all `exceptions`. |
|SystemException | Exception | Base class for all runtime-generated errors. |
|IndexOutOfRangeException | SystemException | Thrown by the runtime only when an array is indexed improperly. |
|NullReferenceException	| SystemException | Thrown by the runtime only when a null object is referenced. |
|AccessViolationException | SystemException | Thrown by the runtime only when invalid memory is accessed. |
|InvalidOperationException | SystemException | Thrown by methods when in an invalid state. |
|ArgumentException | SystemException | Base class for all argument exceptions. |
|ArgumentNullException | ArgumentException | Thrown by methods that do not allow an argument to be null. |
|ArgumentOutOfRangeException | ArgumentException | Thrown by methods that verify that arguments are in a given range. |
|ExternalException | SystemException | Base class for exceptions that occur or are targeted at environments outside the runtime. |
|SEHException | ExternalException | Exception encapsulating Win32 structured exception handling information. |


```csharp
using System; 
using System.IO; 
using System.Windows;

namespace WPFExceptionHandling { 

   public partial class MainWindow : Window { 
	
      public MainWindow() { 
         InitializeComponent(); 
         ReadFile(0); 
      }
		
      void ReadFile(int index) { 
         string path = @"D:\Test.txt"; 
         StreamReader file = new StreamReader(path); 
         char[] buffer = new char[80]; 
			
         try { 
            file.ReadBlock(buffer, index, buffer.Length); 
            string str = new string(buffer); 
            str.Trim(); 
            textBox.Text = str; 
         }
         catch (Exception e) {
            MessageBox.Show("Error reading from "+ path + "\nMessage = "+ e.Message);
         } 
         finally { 
            if (file != null) { 
               file.Close(); 
            } 
         } 
      } 
   } 
}
```

```xaml
<Window x:Class = "WPFExceptionHandling.MainWindow" 
   xmlns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation"
   xmlns:x = "http://schemas.microsoft.com/winfx/2006/xaml"
   xmlns:d = "http://schemas.microsoft.com/expression/blend/2008"
   xmlns:mc = "http://schemas.openxmlformats.org/markup-compatibility/2006" 
   xmlns:local = "clr-namespace:WPFExceptionHandling"
   mc:Ignorable = "d" 
   Title = "MainWindow" Height = "350" Width = "604">
	
   <Grid> 
      <TextBox x:Name = "textBox" HorizontalAlignment = "Left"
         Height = "241" Margin = "70,39,0,0" TextWrapping = "Wrap" 
         VerticalAlignment = "Top" Width = "453"/> 
   </Grid> 
	
</Window>
```
