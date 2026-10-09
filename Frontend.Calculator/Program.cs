using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using Backend;

namespace Frontend.Calculator;

// ---------- Punto de entrada ----------
internal static class Program
{
    [STAThread]
    public static void Main(string[] args) =>
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}

// ---------- Aplicación (tema y ventana principal) ----------
public class App : Application
{
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

// ---------- Ventana de la calculadora ----------
// Interfaz de la calculadora, construida 100% en C# (sin XAML).
// Solo dibuja y delega la lógica en Backend.Calculator.
public class MainWindow : Window
{
    private readonly TextBlock display = new();
    private readonly Border displayBorder = new();
    private readonly Grid keypad = new();
    private readonly Backend.Calculator calculator = new();

    public MainWindow()
    {
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

        // Contenedor principal: pantalla arriba, botones abajo.
        var root = new Grid
        {
            Margin = new Thickness(4),
            RowDefinitions = RowDefinitions.Parse("Auto,*"),
        };
        root.Children.Add(displayBorder);
        Grid.SetRow(keypad, 1);
        root.Children.Add(keypad);
        Content = root;
    }

    private void BuildDisplay()
    {
        display.FontSize = 20;
        display.Foreground = Brushes.White;
        display.MinHeight = 36;
        display.VerticalAlignment = VerticalAlignment.Center;
        display.TextWrapping = TextWrapping.Wrap; // el texto largo baja a otra línea, no se recorta

        // Borde verde que contiene el texto de la pantalla.
        displayBorder.Background = Brushes.Green;
        displayBorder.Padding = new Thickness(8);
        displayBorder.Margin = new Thickness(3, 3, 3, 6);
        displayBorder.Child = display;
    }

    private void BuildKeypad()
    {
        keypad.ColumnDefinitions = ColumnDefinitions.Parse("*,*,*,*,*,2*");
        keypad.RowDefinitions = RowDefinitions.Parse("*,*,*,*");

        // Fila 1
        AddButton("7", 0, 0, 1, true);
        AddButton("8", 0, 1, 1, true);
        AddButton("9", 0, 2, 1, true);
        AddButton("(", 0, 3, 1, false);
        AddButton(")", 0, 4, 1, false);
        AddButton("Delete", 0, 5, 1, false);
        // Fila 2
        AddButton("4", 1, 0, 1, true);
        AddButton("5", 1, 1, 1, true);
        AddButton("6", 1, 2, 1, true);
        AddButton("*", 1, 3, 1, false);
        AddButton("/", 1, 4, 1, false);
        AddButton("Clear", 1, 5, 1, false);
        // Fila 3
        AddButton("1", 2, 0, 1, true);
        AddButton("2", 2, 1, 1, true);
        AddButton("3", 2, 2, 1, true);
        AddButton("+", 2, 3, 1, false);
        AddButton("-", 2, 4, 1, false);
        AddButton("^", 2, 5, 1, false);
        // Fila 4
        AddButton("0", 3, 0, 2, true);
        AddButton(".", 3, 2, 1, true);
        AddButton("=", 3, 3, 3, false);
    }

    private void AddButton(string text, int row, int column, int columnSpan, bool isNumber)
    {
        var button = new Button
        {
            Content = text,
            FontSize = 20,
            Margin = new Thickness(3),
            Foreground = Brushes.Black,
            Background = isNumber ? Brushes.White : new SolidColorBrush(Color.Parse("#FF8000")),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
        };
        // Colores al pasar el cursor o presionar: el botón conserva su color
        // y en los naranjas el texto se pone blanco.
        IBrush background = isNumber ? Brushes.White : new SolidColorBrush(Color.Parse("#FF8000"));
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
            calculator.Press(text);
            display.Text = calculator.Display;
        };

        Grid.SetRow(button, row);
        Grid.SetColumn(button, column);
        Grid.SetColumnSpan(button, columnSpan);
        keypad.Children.Add(button);
    }
}