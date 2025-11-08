using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Calculator
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private double operand1;
        private double operand2;
        private double result;

        public double Operand1
        {
            get => operand1;
            set { operand1 = value; OnPropertyChanged(); }
        }

        public double Operand2
        {
            get => operand2;
            set { operand2 = value; OnPropertyChanged(); }
        }

        public double Result
        {
            get => result;
            set { result = value; OnPropertyChanged(); }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        void Oper(object sender, RoutedEventArgs e)
        {
            var name = (sender as FrameworkElement)?.Name;
            Result = name switch
            {
                "add" => Operand1 + Operand2,
                "sub" => Operand1 - Operand2,
                "mul" => Operand1 * Operand2,
                "div" => Operand2 != 0 ? Operand1 / Operand2 : double.NaN,
                _ => Result
            };
        }

        void CopyResult(object sender, RoutedEventArgs e) => Operand1 = Result;
        void Clear(object sender, RoutedEventArgs e) => Operand1 = Operand2 = Result = 0;

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
