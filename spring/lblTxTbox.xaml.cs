using System.Windows;
using System.Windows.Controls;

namespace spring
{
    /// <summary>
    /// Interaction logic for lblTxTbox.xaml
    /// </summary>
    public partial class lblTxTbox : UserControl
    {
        public lblTxTbox()
        {
            InitializeComponent();
        }

        public string LabelTxt
        {
            get => (string)GetValue(LabelTxtProperty); set => SetValue(LabelTxtProperty, value);
        }

        // Using a DependencyProperty as the backing store for LabelTxt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelTxtProperty =
            DependencyProperty.Register("LabelTxt", typeof(string), typeof(lblTxTbox), new PropertyMetadata(""));

        //private float numVal;
        //public float NumVal { get => numVal; set => numVal = value; }
        //// Using a DependencyProperty as the backing store for BoxTxt.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty NumValProperty =
        //    DependencyProperty.Register("NumVal", typeof(float), typeof(lblTxTbox), new PropertyMetadata(0f));

        public string BoxTxt
        {
            get => (string)GetValue(BoxTxtProperty); set => SetValue(BoxTxtProperty, value);
        }

        // Using a DependencyProperty as the backing store for BoxTxt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoxTxtProperty =
            DependencyProperty.Register("BoxTxt", typeof(string), typeof(lblTxTbox), new PropertyMetadata(""));
    }
}