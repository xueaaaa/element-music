using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

namespace ElementMusic.Views.Controls
{
    public partial class MusicSlider : UserControl
    {
        public readonly static DependencyProperty SliderHeightProperty =
            DependencyProperty.Register("SliderHeight", typeof(double), typeof(MusicSlider));
        public readonly static DependencyProperty ValueProperty = 
            DependencyProperty.Register("Value", typeof(double), typeof(MusicSlider));

        public double SliderHeight
        {
            get => (double)GetValue(SliderHeightProperty);
            set => SetValue(SliderHeightProperty, value);
        }

        [Range(0, 1)]
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set
            {
                SetValue(ValueProperty, value);
                ConvertedValue = ValueToWidth();
            }
        }

        public double Minimum { get; private set; }
        public double Maximum { get; private set; }

        public double ConvertedValue { get; private set; }

        public MusicSlider()
        {
            InitializeComponent();

            SliderHeight = 4;
            Minimum = 0f;
            Maximum = 1f;
            ConvertedValue = 0f;
        }

        public double WidthToValue(float width) =>
            width / PART_SliderBackground.Width;

        public double ValueToWidth() =>
            Value * PART_SliderBackground.Width;
    }
}
