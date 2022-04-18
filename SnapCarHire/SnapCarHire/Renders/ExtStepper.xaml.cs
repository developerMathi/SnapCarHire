using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnapCarHire.Renders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtStepper : ContentView
    {
        public int MinimumValue { get; set; } = 0;
        public int MaximumValue { get; set; } = 100;

        public int Value { get; set; }
        public static readonly BindableProperty ValueProperty = BindableProperty.Create<ExtStepper, int>(p => p.Value, 0, BindingMode.TwoWay, null, ValueChanged);
        private static void ValueChanged(BindableObject obj, int oldValue, int newValue)
        {
            var customStepper = obj as ExtStepper;
            if (customStepper != null)
            {
                customStepper.Value = newValue;
            }
        }

        public double sHeightRequest { get; set; } = 30;
        public double sWidthRequest { get; set; } = 120;
        public float CornerRadius { get; set; } = 5;
        public Color StepperColor { get; set; } = Color.Transparent;

        public event EventHandler AddClicked;
        public event EventHandler SubClicked;

        public ExtStepper()
        {
            InitializeComponent();

            // Color
            FrmStepper.BackgroundColor = StepperColor;
            bx1.BackgroundColor = StepperColor;
            bx2.BackgroundColor = StepperColor;
            lblCounter.BackgroundColor = Color.Transparent;
            lblCounter.TextColor = Color.Black;

            // Radius
            FrmStepper.CornerRadius = CornerRadius;
            FrmSubStepper.CornerRadius = CornerRadius;

            // Height and Width
            StkStepper.HeightRequest = sHeightRequest;
            StkStepper.WidthRequest = sWidthRequest;
            VisibilityControl();
        }

        public void VisibilityControl()
        {
            if (Value > MinimumValue && Value < MaximumValue)
            {
                btnDecrease.IsEnabled = true;
                btnincrease.IsEnabled = true;
            }
            else if (Value > MinimumValue)
            {
                if (Value == MaximumValue)
                {
                    btnincrease.IsEnabled = false;
                }

                btnDecrease.IsEnabled = true;
            }
            else if (Value < MaximumValue)
            {
                if (Value == MinimumValue)
                {
                    btnDecrease.IsEnabled = false;
                }

                btnincrease.IsEnabled = true;
            }
            else
            {
                btnincrease.IsEnabled = false;
                btnDecrease.IsEnabled = false;
            }
        }

        private void BtnDecrease_Clicked(object sender, EventArgs e)
        {

            if (Value == MinimumValue)
            {
                btnDecrease.IsEnabled = false;
            }
            else
            {
                Value--;
                lblCounter.Text = Value.ToString();

                VisibilityControl();
            }
            if (SubClicked != null)
            {
                SubClicked(this, EventArgs.Empty);
            }
        }

        private void Btnincrease_Clicked(object sender, EventArgs e)
        {
            if (Value == MaximumValue)
            {
                btnincrease.IsEnabled = false;
            }
            else
            {
                Value++;
                lblCounter.Text = Value.ToString();

                VisibilityControl();
            }

            if (AddClicked != null)
            {
                AddClicked(this, EventArgs.Empty);
            }
        }

    }
}