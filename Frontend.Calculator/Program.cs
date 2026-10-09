using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;

namespace Frontend.Calculator;
internal static class Program
{
    // Public Methods
    [STAThread]
    public static void Main(string[] args) =>
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}

// window calculator
public class App : Application
{
    // Public Methods
    public override void Initialize()
    {
        Styles.Add(new FluentTheme());
        RequestedThemeVariant = ThemeVariant.Light;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
        base.OnFrameworkInitializationCompleted();
    }
}

public class MainWindow : Window
{
    // Fields
    private readonly Backend.Calculator _calculator;
    private readonly TextBlock _display;
    private readonly Border _displayBorder;
    private readonly Grid _keypad;

    // Constructors
    public MainWindow()
    {
        _calculator = new Backend.Calculator();
        _display = new TextBlock();
        _displayBorder = new Border();
        _keypad = new Grid();

        Title = "Calculadoreichon";
        Width = 400;
        Height = 400;
        CanResize = true;
        MinWidth = 320;
        MinHeight = 340;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Background = Brushes.Black;

        BuildDisplay();
        BuildKeypad();

        // Pricipal layout
        var root = new Grid
        {
            Margin = new Thickness(4),
            RowDefinitions = RowDefinitions.Parse("Auto,*"),
        };
        root.Children.Add(_displayBorder);
        Grid.SetRow(_keypad, 1);
        root.Children.Add(_keypad);
        Content = root;
    }

    // Private Methods
    private void BuildDisplay()
    {
        _display.FontSize = 20;
        _display.Foreground = Brushes.White;
        _display.MinHeight = 36;
        _display.VerticalAlignment = VerticalAlignment.Center;
        // If the text is very long, move to the next line.
        _display.TextWrapping = TextWrapping.Wrap; 

        // green calculator window
        _displayBorder.Background = Brushes.Green;
        _displayBorder.Padding = new Thickness(8);
        _displayBorder.Margin = new Thickness(3, 3, 3, 6);
        _displayBorder.Child = _display;
    }

    private void BuildKeypad()
    {
        _keypad.ColumnDefinitions = ColumnDefinitions.Parse("*,*,*,*,*,2*");
        _keypad.RowDefinitions = RowDefinitions.Parse("*,*,*,*");

        // Row 1
        AddButton("7", 0, 0, 1, true);
        AddButton("8", 0, 1, 1, true);
        AddButton("9", 0, 2, 1, true);
        AddButton("(", 0, 3, 1, false);
        AddButton(")", 0, 4, 1, false);
        AddButton("Delete", 0, 5, 1, false);
        // Row 2
        AddButton("4", 1, 0, 1, true);
        AddButton("5", 1, 1, 1, true);
        AddButton("6", 1, 2, 1, true);
        AddButton("*", 1, 3, 1, false);
        AddButton("/", 1, 4, 1, false);
        AddButton("Clear", 1, 5, 1, false);
        // Row 3
        AddButton("1", 2, 0, 1, true);
        AddButton("2", 2, 1, 1, true);
        AddButton("3", 2, 2, 1, true);
        AddButton("+", 2, 3, 1, false);
        AddButton("-", 2, 4, 1, false);
        AddButton("^", 2, 5, 1, false);
        // Row 4
        AddButton("0", 3, 0, 2, true);
        AddButton(".", 3, 2, 1, true);
        AddButton("=", 3, 3, 3, false);
    }

    private void AddButton(string text, int row, int column, int columnSpan, bool isNumber)
    {
        IBrush background = isNumber ? Brushes.White : new SolidColorBrush(Color.Parse("#ff8000"));

        var button = new Button
        {
            Content = text,
            FontSize = 20,
            Margin = new Thickness(3),
            Foreground = Brushes.Black,
            Background = background,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
        };

        // Change colors when hovering or pressing the button
        var hoverBackground = isNumber ? new SolidColorBrush(Color.Parse("#E0E0E0")) : background;
        var hoverForeground = isNumber ? Brushes.Black : Brushes.White;
        button.Resources["ButtonBackgroundPointerOver"] = hoverBackground;
        button.Resources["ButtonBackgroundPressed"] = hoverBackground;
        button.Resources["ButtonForegroundPointerOver"] = hoverForeground;
        button.Resources["ButtonForegroundPressed"] = hoverForeground;
        button.Resources["ButtonBorderBrushPointerOver"] = Brushes.Transparent;
        button.Resources["ButtonBorderBrushPressed"] = Brushes.Transparent;

        button.Click += (_, _) =>
        {
            _calculator.Press(text);
            _display.Text = _calculator.Display;
        };

        Grid.SetRow(button, row);
        Grid.SetColumn(button, column);
        Grid.SetColumnSpan(button, columnSpan);
        _keypad.Children.Add(button);
    }
}