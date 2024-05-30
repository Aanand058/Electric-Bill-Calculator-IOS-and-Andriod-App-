using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UtilityBills_Aanand
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();


        }

        private void CalculateBtnClicked(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrEmpty(ProvincePicker.SelectedItem?.ToString()) ||
                string.IsNullOrEmpty(DaytimeUsageEntry.Text) ||
                string.IsNullOrEmpty(EveningUsageEntry.Text))
            {
                ErrorLabel.IsVisible = true;
                return;
            }

            if (!double.TryParse(DaytimeUsageEntry.Text, out double daytimeUsage) ||
                !double.TryParse(EveningUsageEntry.Text, out double eveningUsage) ||
                daytimeUsage < 0 || eveningUsage < 0)
            {
                ErrorLabel.IsVisible = true;
                return;
            }

            //Calculation 
            const double daytimeRate = 0.314;
            const double eveningRate = 0.186;

            double daytimeUsageCharge = daytimeUsage * daytimeRate;
            double eveningUsageCharge = eveningUsage * eveningRate;
            double totalUsageCharge = daytimeUsageCharge + eveningUsageCharge;

            string province = ProvincePicker.SelectedItem.ToString();
            double salesTaxRate = 0.0;

            if (province.Equals("AB"))
            {
                salesTaxRate = 0.5;
            }
            else if (province.Equals("ON"))
            {
                salesTaxRate = 0.13;
            }
            else if (province.Equals("BC"))
            {
                salesTaxRate = 0.12;  // Note: BC has 5% GST + 7% PST for non-residential; Residential is exempt from PST
            }
            else if (province.Equals("NL"))
            {
                salesTaxRate = 0.15;
            }
            else if (province.Equals("NB"))
            {
                salesTaxRate = 0.15;
            }
            else if (province.Equals("NS"))
            {
                salesTaxRate = 0.15;
            }
            else if (province.Equals("PE"))
            {
                salesTaxRate = 0.15;
            }
            else if (province.Equals("QC"))
            {
                salesTaxRate = 0.14975;  // 5% GST + 9.975% QST
            }
            else if (province.Equals("MB"))
            {
                salesTaxRate = 0.12;  // Note: MB has 5% GST + 7% RST for non-residential; Residential is exempt from RST
            }
            else if (province.Equals("SK"))
            {
                salesTaxRate = 0.11;  // Note: SK has 5% GST + 6% PST for non-residential; Residential is exempt from PST
            }
            else if (province.Equals("NT"))
            {
                salesTaxRate = 0.05;
            }
            else if (province.Equals("NU"))
            {
                salesTaxRate = 0.05;
            }
            else if (province.Equals("YT"))
            {
                salesTaxRate = 0.05;
            }
            else
            {
                salesTaxRate = 0.0;  // Default case if the province is not matched
            }


            double salesTaxAmt = totalUsageCharge * salesTaxRate;
            bool renewableSwitch = RenewableSwitch.IsToggled || province == "BC";
            double envRebate = renewableSwitch ? totalUsageCharge * 0.095 : 0;
            double totalBill = totalUsageCharge + salesTaxAmt - envRebate;


            //F2 formats two decimal places 
            DayTimeUsageLabel.Text = $"Daytime Usage Charge: ${daytimeUsageCharge:F2}";
            EveningUsageLabel.Text = $"Evening Usage Charge: ${eveningUsageCharge:F2}";
            TotalUsageLabel.Text = $"Total Usage Charge: ${totalUsageCharge:F2}";
            SalesTaxLabel.Text = $"Sales Tax ({salesTaxRate *100})%: ${salesTaxAmt:F2}";
            EnvRebateLabel.Text = $"Environment Rebate: -${envRebate:F2}";
            PayLabel.Text = $"You Must Pay: ${totalBill:F2}";

            //Make Output Visible
            OutputStack.IsVisible = true;
            PayLabel.IsVisible = true;

        }

        private void ResetBtnClicked(object sender, EventArgs e)
        {
            DaytimeUsageEntry.Text = "";
            EveningUsageEntry.Text = "";
            ProvincePicker.SelectedIndex = -1;
            RenewableSwitch.IsToggled = false;
            RenewableSwitch.IsEnabled = true;
            ErrorLabel.IsVisible = false;
            OutputStack.IsVisible = false;
        }

        private void ProvincePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // BC province switched must be on all time. 
            if (ProvincePicker.SelectedItem != null && ProvincePicker.SelectedItem.ToString().Equals("BC"))
            {
                RenewableSwitch.IsToggled = true;
                RenewableSwitch.IsEnabled = false;

            }
            else
            {
                // For Other Provinces. 
                RenewableSwitch.IsToggled = false;
                RenewableSwitch.IsEnabled = true;
            }
        }
    }
}
