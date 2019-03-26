using System.Windows;

namespace spring
{
        public enum Dx
        {
            x, v, a, coord, F
        }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModels.MainWindowModel viewModel;

        public MainWindow()
        {
            viewModel = new ViewModels.MainWindowModel();
            DataContext = viewModel;
            InitializeComponent();
        }

        private float[] getT(float dt, float tmax)
        {
            int maxI = (int)(tmax / dt);
            float[] tCounts = new float[maxI];
            for (int i = 1; i < maxI; i++)
            {
                tCounts[i] = tCounts[i - 1] + dt;
            }
            return tCounts;
        }


        private void GetF_Click(object sender, RoutedEventArgs e)
        {
            float tmax = 0.0009f;
            float dt = 0.00005f;
            float[] tCounts = getT(dt, tmax);
            Rope_t[] ropeOverT = new Rope_t[tCounts.Length];
            for (int i = 0; i < ropeOverT.Length; i++)
            {
                ropeOverT[i].GetState();
            }
        }
    }
}