using System;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void Oper(object sender, RoutedEventArgs e)
        {
            if(!double.TryParse(op1.Text, out double a)) return;
            if(!double.TryParse(op2.Text, out double b)) return;

            string op = (sender as Button)?.Name;
            double result = op switch
            {
                "add" => a + b,
                "sub" => a - b,
                "mul" => a * b,
                "div" => b != 0 ? a / b : double.NaN,
                _ => 0
            };
            res.Text = result.ToString();
        }

        void Clear(object sender, RoutedEventArgs e)
        {
            op1.Text = op2.Text = res.Text = "0";
        }

        void CopyResult(object sender, RoutedEventArgs e)
        {
            
            op1.Text = res.Text;
            op2.Text = "0";
            res.Text = "0";
        }
    }
}
