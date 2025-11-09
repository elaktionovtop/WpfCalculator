using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Calculator
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        enum Op { Add, Sub, Mul, Div }

        private string operand1;
        private string operand2;
        private double result;

        public string Operand1
        {
            get => operand1;
            set { operand1 = value; OnPropertyChanged(); NotifyCommands(); }
        }

        public string Operand2
        {
            get => operand2;
            set { operand2 = value; OnPropertyChanged(); NotifyCommands(); }
        }

        public double Result
        {
            get => result;
            set { result = value; OnPropertyChanged(); }
        }

        // Команды
        public RelayCommand AddCommand { get; }
        public RelayCommand SubCommand { get; }
        public RelayCommand MulCommand { get; }
        public RelayCommand DivCommand { get; }
        public RelayCommand NextCommand { get; }
        public RelayCommand ClearCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            AddCommand = new RelayCommand(_ => ExecuteOper(Op.Add), _ => CanExecuteBinary());
            SubCommand = new RelayCommand(_ => ExecuteOper(Op.Sub), _ => CanExecuteBinary());
            MulCommand = new RelayCommand(_ => ExecuteOper(Op.Mul), _ => CanExecuteBinary());
            DivCommand = new RelayCommand(_ => ExecuteOper(Op.Div), _ => CanExecuteBinary(divCheck: true));
            NextCommand = new RelayCommand(_ => CopyResult(), _ => true);
            ClearCommand = new RelayCommand(_ => Clear(), _ => true);
        }

        void ExecuteOper(Op op)
        {
            if(!TryGetOperands(out var a, out var b, out var err))
            {
                MessageBox.Show(err, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double res = op switch
            {
                Op.Add => a + b,
                Op.Sub => a - b,
                Op.Mul => a * b,
                Op.Div => b != 0 ? a / b : double.NaN,
                _ => Result
            };

            Result = Math.Round(res, 2);
        }

        // Централизованная проверка и выполнение
        bool TryGetOperands(out double a, out double b, out string error)
        {
            a = b = 0;
            error = string.Empty;
            if(!double.TryParse(Operand1, out a))
            {
                error = "Ошибка ввода: первое число";
                return false;
            }
            if(!double.TryParse(Operand2, out b))
            {
                error = "Ошибка ввода: второе число";
                return false;
            }
            return true;
        }

        bool CanExecuteBinary(bool divCheck = false)
        {
            if(!double.TryParse(Operand1, out var a)) return false;
            if(!double.TryParse(Operand2, out var b)) return false;
            if(divCheck && b == 0) return false;
            return true;
        }

        void CopyResult() => Operand1 = Result.ToString();
        void Clear()
        {
            Operand1 = Operand2 = string.Empty;
            Result = double.NaN;
        }

        // Уведомляем команды, что вход изменился
        void NotifyCommands()
        {
            AddCommand.RaiseCanExecuteChanged();
            SubCommand.RaiseCanExecuteChanged();
            MulCommand.RaiseCanExecuteChanged();
            DivCommand.RaiseCanExecuteChanged();
            NextCommand.RaiseCanExecuteChanged();
            ClearCommand.RaiseCanExecuteChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
