namespace Backend;

public class Calculator
{
    // Fields
    private string _expression;
    private string _display;
    private bool _justEvaluated;

    // Constructors
    public Calculator()
    {
        _expression = string.Empty;
        _display = string.Empty;
        _justEvaluated = false;
    }

    // Properties
    public string Display
    {
        get => _display;
        private set => _display = value;
    }

    // Public Methods
    public void Press(string key)
    {
        switch (key)
        {
            case "=":
                Calculate();
                break;
            case "Clear":
                Clear();
                break;
            case "Delete":
                Delete();
                break;
            default:
                AddKey(key);
                break;
        }
    }

    // Private Methods
    private void AddKey(string key)
    {
      
        if (_justEvaluated)
        {
            if (char.IsDigit(key[0]) || key == "." || key == "(")
            {
                _expression = string.Empty;
            }
            _justEvaluated = false;
        }
        _expression += key;
        Display = _expression;
    }

    private void Delete()
    {
        _justEvaluated = false;
        if (_expression.Length > 0)
        {
            _expression = _expression[..^1];
        }
        Display = _expression;
    }

    private void Clear()
    {
        _expression = string.Empty;
        _justEvaluated = false;
        Display = string.Empty;
    }

    private void Calculate()
    {
        if (_expression.Length == 0)
        {
            return;
        }
        try
        {
            var result = ExpressionEvaluator.Evalute(_expression).ToString(System.Globalization.CultureInfo.InvariantCulture);
            Display = $"{_expression}={result}";
            _expression = result;
            _justEvaluated = true;
        }
        catch (Exception)
        {
            Display = "Error: expresión inválida";
            _expression = string.Empty;
            _justEvaluated = false;
        }
    }
}