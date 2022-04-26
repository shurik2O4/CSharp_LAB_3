using WinUIEx;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project ArrayLengthTextlates, see: http://aka.ms/winui-project-info.

namespace LAB_3 {
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #pragma warning disable CS0628 // New protected member declared in sealed type
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #pragma warning restore CS0628

        private bool _CalcButtonEnabled = false;
        public bool CalcButtonEnabled
        {
            get { return _CalcButtonEnabled; }
            set { if (value != _CalcButtonEnabled) {_CalcButtonEnabled = value; OnPropertyChanged(nameof(CalcButtonEnabled)); } }
        }

        private string _ArrayLengthText = "";
        public string ArrayLengthText
        {
            get { return _ArrayLengthText; }
            set { if (value != _ArrayLengthText) { _ArrayLengthText = value; OnPropertyChanged(nameof(ArrayLengthText)); } }
        }

        private string _CalcResultText = "";
        public string CalcResultText
        {
            get { return _CalcResultText; }
            set { if (value != _CalcResultText) { _CalcResultText = value; OnPropertyChanged(nameof(CalcResultText)); } }
        }

        //private int _DeletedElements = 0;
        //public int DeletedElements {
        //    get { return _DeletedElements; }
        //    set { if (value != _DeletedElements) { _DeletedElements = value; OnPropertyChanged(nameof(DeletedElements)); } }
        //}

        private string _ErrorText = "None";
        public string ErrorText
        {
            get { return _ErrorText; }
            set { if (value != _ErrorText) { _ErrorText = value; OnPropertyChanged(nameof(ErrorText)); } }
        }

        public ObservableCollection<int> InputArray = new();
        public ObservableCollection<int> OutputArray = new();

        public MainWindow()
        {

            InitializeComponent();

            ArrayLengthTextBox.DataContext = this;
            CalcResultTextBox.DataContext = this;
            CalcButton.DataContext = this;
            InputArrayListBox.ItemsSource = InputArray;
            OutputArrayListBox.ItemsSource = OutputArray;
            ErrorTip.DataContext = this;
            //DeletedElementsTextBox.DataContext = this;
        }

        private void PopulateArray(int NewArraySize) {
            Random R = new();
            for (int i = 0; i < NewArraySize; i++) {
                InputArray.Add(R.Next(-NewArraySize, NewArraySize + 1));
            };
        }

        private void PopulateButton_Click(object sender, RoutedEventArgs e) {
            CalcResultText = "";
            InputArray.Clear();
            OutputArray.Clear();

            int NewArraySize;
            if (!int.TryParse(ArrayLengthText, out NewArraySize) || NewArraySize <= 0) {

                ErrorText = "Invalid array size provided";
                ErrorTip.IsOpen = true;

                return;
            };

            // Generate array
            PopulateArray(NewArraySize);

            CalcButtonEnabled = true;
        }

        private void CalcButton_Click(object sender, RoutedEventArgs e) {

            int result = CRPO();
            CRPT();

            CalcButtonEnabled = false;
            if (result != 0)
                CalcResultText = result.ToString();
            else {
                ErrorText = "No value in the input array found, that satisfies given condition.";
                ErrorTip.IsOpen = true;
            }
        }

        // Calculate result (but with a terrible name)
        private int CRPO() {
            int result = 0;

            foreach (int v in InputArray) {
                // Check if negative and even
                if (v < 0 && v % 2 == 0) {
                    // If result is not initialized, do it
                    if (result == 0) { result = v; }
                    // otherwise multiply.
                    else { result *= v; }
                }
            }

            return result;
        }

        // Calculate result (but with a terrible name) 2
        private void CRPT() {
            OutputArray.Clear();

            bool delete = true;
            try {
                for (int i = 0; i < InputArray.Count; i++) {
                    // Copy to output
                    OutputArray.Add(InputArray[i]);
                    // Check if negative and even
                    if (InputArray[i] < 0 && InputArray[i] % 2 == 0 && delete) {
                        i += new Random().Next(1, 10);
                        delete = false;
                    }
                }
            } catch (IndexOutOfRangeException) { }
        }

        private void ArrayLengthTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            CalcResultText = "";
            if (InputArray.Count != 0) InputArray.Clear();
            if (OutputArray.Count != 0) OutputArray.Clear();
        }
    }
}