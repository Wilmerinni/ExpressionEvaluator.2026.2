using Backend;

namespace Frontend.Windows
{
    public partial class Form1 : Form
    {
        // Fields
        private readonly Calculator _calculator;
        private readonly Label _lblDisplay;

        // Constructors
        public Form1()
        {
            InitializeComponent();
            _calculator = new Calculator();
            _lblDisplay = new Label();
            BuildCalculator();
        }

        // Private Methods
        
        private void BuildCalculator()
        {
            Text = "Functions Evaluator";
            BackColor = Color.Black;
            ClientSize = new Size(380, 340);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            _lblDisplay.Dock = DockStyle.Top;
            _lblDisplay.Height = 56;
            _lblDisplay.BackColor = Color.Green;
            _lblDisplay.ForeColor = Color.White;
            _lblDisplay.Font = new Font("Segoe UI", 14F);
            _lblDisplay.TextAlign = ContentAlignment.MiddleLeft;
            _lblDisplay.AutoEllipsis = true;
            _lblDisplay.Margin = new Padding(0);

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 4,
                BackColor = Color.Black,
                Padding = new Padding(4),
            };
            for (var c = 0; c < 6; c++)
            {
                // Delete wider
                grid.ColumnStyles.Add(c < 5 ? new ColumnStyle(SizeType.Percent, 14F) : new ColumnStyle(SizeType.Percent, 30F));
            }
            for (var r = 0; r < 4; r++)
            {
                grid.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            }

            // Row 1
            AddButton(grid, "7", 0, 0, 1, true);
            AddButton(grid, "8", 1, 0, 1, true);
            AddButton(grid, "9", 2, 0, 1, true);
            AddButton(grid, "(", 3, 0, 1, false);
            AddButton(grid, ")", 4, 0, 1, false);
            AddButton(grid, "Delete", 5, 0, 1, false);
            // Row 2
            AddButton(grid, "4", 0, 1, 1, true);
            AddButton(grid, "5", 1, 1, 1, true);
            AddButton(grid, "6", 2, 1, 1, true);
            AddButton(grid, "*", 3, 1, 1, false);
            AddButton(grid, "/", 4, 1, 1, false);
            AddButton(grid, "Clear", 5, 1, 1, false);
            // Row 3
            AddButton(grid, "1", 0, 2, 1, true);
            AddButton(grid, "2", 1, 2, 1, true);
            AddButton(grid, "3", 2, 2, 1, true);
            AddButton(grid, "+", 3, 2, 1, false);
            AddButton(grid, "-", 4, 2, 1, false);
            AddButton(grid, "^", 5, 2, 1, false);
            // Row 4
            AddButton(grid, "0", 0, 3, 2, true);
            AddButton(grid, ".", 2, 3, 1, true);
            AddButton(grid, "=", 3, 3, 3, false);

            Controls.Add(grid);
            Controls.Add(_lblDisplay);
        }

        private void AddButton(TableLayoutPanel grid, string text, int col, int row, int colSpan, bool isNumber)
        {
            var button = new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                Margin = new Padding(3),
                Font = new Font("Segoe UI", 14F),
                FlatStyle = FlatStyle.Flat,
                BackColor = isNumber ? Color.White : Color.FromArgb(255, 128, 0),
                ForeColor = Color.Black,
                TabStop = false,
            };
            button.FlatAppearance.BorderColor = Color.FromArgb(200, 220, 255);
            button.Click += Button_Click;
            grid.Controls.Add(button, col, row);
            if (colSpan > 1)
            {
                grid.SetColumnSpan(button, colSpan);
            }
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            _calculator.Press(((Button)sender!).Text);
            _lblDisplay.Text = _calculator.Display;
        }
    }
}