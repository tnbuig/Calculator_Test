using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Calculator_MVVM_OranG.Commands;
using Calculator_MVVM_OranG.Models;

namespace Calculator_MVVM_OranG.ViewModels
{
    //For More then one View i would have inherent from ViewModelBase that would have implement INotifyPropertyChanged
    //in case of more time i would have add better exception handling and maybe some logs(but only for more complex application)
    public class CalculatorViewModel : INotifyPropertyChanged
    {

        private State _appState;
        private bool isOld;
        private double _result;
        private StringBuilder _history;
        private readonly Calculator _calculator;
        private string _screen;
        private CancellationTokenSource _cancelSource;

        #region Properties

        private bool _isButtonsVisible;
        public string Operation;
        public StringBuilder _input;
        public event PropertyChangedEventHandler PropertyChanged;

        public CalculatorViewModel()
        {
            _input = new StringBuilder();
            _history = new StringBuilder();
            History = "place for previous operations e.g: 3+4-5...";
            _calculator = new Calculator();
            _appState = State.New;
            isOld = false;
            Result = 0;
            IsButtonsVisible = true;
        }

        public bool IsButtonsVisible
        {
            get { return _isButtonsVisible; }
            set
            {
                _isButtonsVisible = value;
                OnPropertyChanged("IsButtonsVisible");
            }
        }

        public String Input
        {
            get { return _input.ToString(); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _input.Clear();
                    Screen = "0";
                }
                else
                {
                    _input.Append(value);
                    Screen = _input.ToString();
                }
            }
        }

        //as you can see in windows calculator, once starting a calculation one can see previous operation
        //my plan was to do here the same - not too difficult just not enough time
        public String History
        {
            get
            {
                return _history.ToString();
            }
            set
            {
                _history.Append(value);
                OnPropertyChanged("History");
            }
        }


        public double Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
                Screen = _result.ToString();
            }
        }

        public string Screen
        {
            get { return _screen; }
            set
            {
                _screen = value;
                OnPropertyChanged("Screen");
            }
        }

        public ICommand CalculateCommand
        {
            get { return new DelegateCommand(CalculateAction); }
        }

        public ICommand OprationKeyCommand
        {
            get { return new DelegateCommand(OprationAction); }
        }

        public ICommand DigitKeyCommand
        {
            get { return new DelegateCommand(DigitAction); }
        }

        public ICommand ClearKeyCommand
        {
            get { return new DelegateCommand(ClearAction); }
        }

        public ICommand ClearAllKeyCommand
        {
            get { return new DelegateCommand(ClearAllAction); }
        }

        public ICommand OffKeyCommand
        {
            get { return new DelegateCommand(OffAction); }
        }

        public ICommand SignKeyCommand
        {
            get { return new DelegateCommand(SignAction); }
        }

        public ICommand FibonacciKeyCommand
        {
            get { return new DelegateCommand(FibonacciAction); }
        }
        #endregion

        #region Action Method

        private void CalculateAction(string obj)
        {
            if (_appState == State.Operation)
            {
                Result = StringToInteger(Input);
                Result = _calculator.Calc(Result, StringToInteger(Input), Operation);
            }
            else
            {
                Result = string.IsNullOrEmpty(Operation) ? StringToInteger(Input) : _calculator.Calc(Result, StringToInteger(Input), Operation);
            }
            _appState = State.New;
        }

        private void OprationAction(string operation)
        {
            if (_appState == State.Digit)
            {
                if (!string.IsNullOrEmpty(Operation))
                {
                    Result = _calculator.Calc(Result, StringToInteger(Input), Operation);
                    isOld = true;
                }
            }
            if (_appState == State.New)
            {
                Input = string.Empty;
                Input = Result.ToString();
            }
            _appState = State.Operation;
            Operation = operation;

        }

        private void ClearAllAction(string obj)
        {
            if (!IsButtonsVisible)
            {
                _cancelSource.Cancel();
                IsButtonsVisible = true;
            }
            Operation = string.Empty;
            Input = string.Empty;
            Result = 0;
            _appState = State.New;
            isOld = false;
        }

        private void ClearAction(string clearType)
        {
            Input = string.Empty;
        }
        private void OffAction(string obj)
        {
            Application.Current.Shutdown();
        }

        private void SignAction(string obj)
        {
            int num = StringToInteger(Input);
            Input = (num * -1).ToString();
        }

        //This is not the best practice for canceling token in MVVM - i was not sure wetter to 
        //add this functionality or not. i currently do not remember the best practice and don't have enough time to find it
        private async void FibonacciAction(string obj)
        {
            _cancelSource = new CancellationTokenSource();

            var slowTask = Task<string>.Factory.StartNew(() => SlowFibonacci(_cancelSource.Token), _cancelSource.Token);
            if (Screen.Length != 0)
            {
                int num = StringToInteger(Screen);
                if (num > 50)
                {
                    MessageBox.Show("please do not perform Fibonacci calculation on values greater then 50");
                }
                else
                {
                    Screen = "Please Wait. for Cancel press \"C\"";
                    IsButtonsVisible = false;
                    try
                    {
                        await slowTask;
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    IsButtonsVisible = true;
                    Input = string.Empty;
                    Input = slowTask.Result;
                }
            }
        }


        private void DigitAction(string newDigit)
        {
            if (_appState == State.New)
            {
                isOld = false;
                Operation = string.Empty;
                Input = string.Empty;
            }
            else
            {
                if (_appState == State.Operation)
                {
                    if (!isOld)
                    {
                        Result = StringToInteger(Input);
                    }
                    Input = string.Empty;
                }
            }
            _appState = State.Digit;
            Input = newDigit;
        }

        #endregion
        private enum State
        {
            New, // starting point after pressing "C" or "=" or starting the application.
            Operation, //"After pressing an operation button.
            Digit, //after inserting a digit.
        }

        #region Private Helper Methods

        private string SlowFibonacci(CancellationToken token)
        {
            
            int num;
            int.TryParse((Input), out num);
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(9);
                if(token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();
            }
            string result =_calculator.Fib(num).ToString();
            return result;

        }


        private int StringToInteger(string str)
        {
            int num;
            if (str.Contains(".")) return 0;//currently do not handle non integer input.
            if (int.TryParse(str, out num))
            {
                return num;
            }
            throw new Exception(
                "error while attempt to convert the String to integer the string Builder content is:" + Input);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
