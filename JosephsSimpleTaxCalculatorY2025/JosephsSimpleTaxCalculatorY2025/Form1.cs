using System.Configuration;
using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace JosephsSimpleTaxCalculatorY2025
{
    public partial class Form1 : Form
    {
        string state;//holds the state name
        int stateId;//needed for the SQL query
        decimal income;//holds the income value
        decimal stateTax;//holds state taxable income. In hind sight, I would have named it stateTaxable.
        decimal federalTax;//ditto but for federal.
        decimal fica;//holds the value for how much FICA is owed
        bool oldOrBlind;//can get additional deductions if true.
        string filingStatus;//holds filing status
        int filingId;//needed for the SQL query
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void incomeNumericUpDown_ValueChanged(object sender, EventArgs e)//sets income value
        {
            income = incomeNumericUpDown.Value;
        }

        private void statesComboBox_SelectedIndexChanged(object sender, EventArgs e)//uses a combo box to select for state
        {
            state = statesComboBox.SelectedItem.ToString();
        }

        private void filingStatusComboBox_SelectedIndexChanged(object sender, EventArgs e)//sets filing status
        {
            filingStatus = filingStatusComboBox.SelectedItem.ToString();
        }

        private void OldOrBlindcheckBox_CheckedChanged(object sender, EventArgs e)//check box for if old or blind
        {
            oldOrBlind = OldOrBlindcheckBox.Checked;
        }

        private void CalculateButton_Click(object sender, EventArgs e)//this is it. The button that will calculate your taxes. Approximately.
        {
            fica = 0;//resets fica
            
            //this will turn state and filing status into an ID number that can be queried.
            if (state == "Alabama") { stateId = 1; }
            else if (state == "Alaska") { stateId = 2; }
            else if (state == "Arizona") { stateId = 3; }
            else if (state == "Arkansas") { stateId = 4; }
            else if (state == "California") { stateId = 5; }
            else if (state == "Colorado") { stateId = 6; }
            else if (state == "Connecticut") { stateId = 7; }
            else if (state == "Delaware") { stateId = 8; }
            else if (state == "Florida") { stateId = 9; }
            else if (state == "Georgia") { stateId = 10; }
            else if (state == "Hawaii") { stateId = 11; }
            else if (state == "Idaho") { stateId = 12; }
            else if (state == "Illinois") { stateId = 13; }
            else if (state == "Indiana") { stateId = 14; }
            else if (state == "Iowa") { stateId = 15; }
            else if (state == "Kansas") { stateId = 16; }
            else if (state == "Kentucky") { stateId = 17; }
            else if (state == "Louisiana") { stateId = 18; }
            else if (state == "Maine") { stateId = 19; }
            else if (state == "Maryland") { stateId = 20; }
            else if (state == "Massachusetts") { stateId = 21; }
            else if (state == "Michigan") { stateId = 22; }
            else if (state == "Minnesota") { stateId = 23; }
            else if (state == "Mississippi") { stateId = 24; }
            else if (state == "Missouri") { stateId = 25; }
            else if (state == "Montana") { stateId = 26; }
            else if (state == "Nebraska") { stateId = 27; }
            else if (state == "Nevada") { stateId = 28; }
            else if (state == "New Hampshire") { stateId = 29; }
            else if (state == "New Jersey") { stateId = 30; }
            else if (state == "New Mexico") { stateId = 31; }
            else if (state == "New York") { stateId = 32; }
            else if (state == "North Carolina") { stateId = 33; }
            else if (state == "North Dakota") { stateId = 34; }
            else if (state == "Ohio") { stateId = 35; }
            else if (state == "Oklahoma") { stateId = 36; }
            else if (state == "Oregon") { stateId = 37; }
            else if (state == "Pennsylvania") { stateId = 38; }
            else if (state == "Rhode Island") { stateId = 39; }
            else if (state == "South Carolina") { stateId = 40; }
            else if (state == "South Dakota") { stateId = 41; }
            else if (state == "Tennessee") { stateId = 42; }
            else if (state == "Texas") { stateId = 43; }
            else if (state == "Utah") { stateId = 44; }
            else if (state == "Vermont") { stateId = 45; }
            else if (state == "Virginia") { stateId = 46; }
            else if (state == "Washington") { stateId = 47; }
            else if (state == "West Virginia") { stateId = 48; }
            else if (state == "Wisconsin") { stateId = 49; }
            else if (state == "Wyoming") { stateId = 50; }

            if (filingStatus == "Single") { filingId = 1; }
            else if (filingStatus == "Married Filing Jointly") { filingId = 2; }
            else if (filingStatus == "Head of Household") { filingId = 3; }
            else if (filingStatus == "Married Filing Separately") { filingId = 4; }
            else if (filingStatus == "Widowed") { filingId = 5; }

            //MessageBox.Show($"State: {state}\nIncome: {income:C}\nFiling Status: {filingStatus}\nOld/Blind: {oldOrBlind}" +
            //$"\nState Id: {stateId}\nFiling Status Id: {filingId}");//This line is to help me test. This won't be in the final product.


            if (income == 0)//error message if income is set at zero
            {
                MessageBox.Show("Income is 0. Please enter your income. If you don't have income, I don't know why you're using this.");
                return;
            }
            if (string.IsNullOrWhiteSpace(state) || string.IsNullOrWhiteSpace(filingStatus))//error message is state or filing status haven't been selected yet.
            {
                MessageBox.Show("Make sure that State and Filing Status have both been selected.");
                return;
            }

            stateTax = income;//this will be changed by a lot of math before we're done. It will only represent the actual tax owed once we're done.
            federalTax = income;//ditto

            FederalDeductions();//federal deductions will be calculated
            StateDeductions();//state deductions will be calculated

            if (stateTax < 0) { stateTax = 0; }//stop deductions from taking this into the negatives
            if (federalTax < 0) { federalTax = 0; }

            TaxBrackets taxBrackets = new TaxBrackets();//for more info, look at the class.

            List<FederalTaxBracket> fedBrackets = taxBrackets.GetFederalTaxBrackets(filingId);
            List<StateTaxBracket> stateBrackets = taxBrackets.GetStateTaxBrackets(stateId, filingId);

            decimal finalStateTax = 0;//These will hold the true final tax owed after all the math is done.
            decimal finalFederalTax = 0;

            foreach (var bracket in fedBrackets)//finally we calculate for marginal brackets
            {
                //MessageBox.Show($"Fed brackets\nBracket start: {bracket.MinValue}\nBracket end: {bracket.MaxValue}\nTax rate: {bracket.TaxRate}");//for testing

                decimal lower = bracket.MinValue;
                decimal upper = bracket.MaxValue ?? federalTax;//if MaxValue is null, use taxable income instead.
                decimal rate = bracket.TaxRate;

                if (!bracket.MaxValue.HasValue)//handle for if in top bracket or flat rate
                {
                    finalFederalTax += ((federalTax - lower) * rate);
                    break;
                }
                if (federalTax < bracket.MaxValue)//will break the loop if we're in our income bracket
                {
                    finalFederalTax += ((federalTax - lower) * rate);
                    break;
                }
                else
                {
                    finalFederalTax += ((upper - lower) * rate);
                }
            }

            foreach (var bracket in stateBrackets)
            {
                //MessageBox.Show($"State brackets\nState ID: {bracket.StateId}Bracket start: {bracket.MinValue}\nBracket end: {bracket.MaxValue}\nTax rate: {bracket.TaxRate}");//for testing

                decimal lower = bracket.MinValue;
                decimal upper = bracket.MaxValue ?? stateTax;//if MaxValue is null, use taxable income instead.
                decimal rate = bracket.TaxRate;

                if (!bracket.MaxValue.HasValue)//handle for if in top bracket or flat rate
                {
                    finalStateTax += ((stateTax - lower) * rate);
                    break;
                }
                if (stateTax < bracket.MaxValue)//will break the loop if we're in our income bracket
                {
                    finalStateTax += ((stateTax - lower) * rate);
                    break;
                }
                else
                {
                    finalStateTax += ((upper - lower) * rate);
                }
            }

            //FICA stuff here.
            fica += (income*0.0145m);//medicare

            if (filingStatus == "Married Filing Jointly" && income > 250000)//additional medicare tax
            { fica += ((income - 250000) * 0.009m); }
            else if (income > 200000) { fica += ((income - 200000) * 0.009m); }

            if (income >= 168600) { fica += (168600 * 0.062m); }//wage cap for social security
            else { fica += (income * 0.062m); }//social security


            MessageBox.Show($"Calculations done. Making {income:C} a year in {state}, you will pay roughly..." +
                $"\nFederal taxes: {finalFederalTax:C}\nState taxes: {finalStateTax:C}\nFICA: {fica:C}");

        }

        private void FederalDeductions()//calculates federal deductions
        {
            if (filingStatus == "Single")
            {
                federalTax -= 15750;
                if (oldOrBlind)
                {
                    federalTax -= 2000;
                }
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                federalTax -= 31500;
                if (oldOrBlind)
                {
                    federalTax -= 1600;
                }
            }
            else if (filingStatus == "Head of Household")
            {
                federalTax -= 23625;
                if (oldOrBlind)
                {
                    federalTax -= 1950;
                }
            }
            else if (filingStatus == "Married Filing Separately")
            {
                federalTax -= 15750;
                if (oldOrBlind)
                {
                    federalTax -= 1600;
                }
            }
            else if (filingStatus == "Widowed")
            {
                federalTax -= 31500;
                if (oldOrBlind)
                {
                    federalTax -= 1600;
                }
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show("Something went wrong with Joseph's code related to calculating federal decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }

        //state deductions start here
        private void AlabamaDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 3000;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 8500;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 5200;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 4250;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 8500;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void ArizonaDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 15750;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 31500;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 23625;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 15750;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 31500;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void ArkansasDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 2410;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 4820;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 2410;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 2410;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 2410;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void CaliforniaDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 5706;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 11412;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 11412;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 5706;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 11412;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }

        private void ColoradoDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 15750;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 31500;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 23625;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 15750;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 31500;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void DelawareDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 3250;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 6500;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 3250;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 3250;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 6500;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void GeorgiaDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 7100;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 24000;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 12000;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 7100;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 24000;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void HawaiiDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 4400;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 8800;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 6424;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 4400;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 8800;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void IdahoDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 15000;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 30000;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 22500;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 15000;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 30000;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void KansasDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 3605;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 8240;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 6180;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 3605;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 8240;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void KentuckyDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 3270;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 6540;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 3270;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 3270;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 6540;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void LouisianaDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 4500;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 9000;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 4500;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 4500;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 9000;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void MaineDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 15000;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 30000;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 22500;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 15000;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 30000;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void MarylandDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 3350;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 6700;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 6700;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 3350;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 6700;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void MinnesotaDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 14950;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 29900;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 22500;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 14950;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 29900;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void MississippiDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 2300;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 4600;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 2300;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 2300;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 4600;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void MissouriDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 15750;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 31500;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 23625;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 15750;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 31500;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void NebraskaDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 8600;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 17200;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 12600;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 8600;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 17200;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void NewMexicoDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 14600;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 29200;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 21900;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 14600;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 29200;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void NewYorkDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 8000;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 16050;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 11200;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 8000;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 16050;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void NorthCarolinaDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 12750;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 25500;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 19125;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 12750;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 25500;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void NorthDakotaDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 15750;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 31500;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 23625;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 15750;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 31500;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void OklahomaDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 6350;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 12700;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 9350;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 6350;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 12700;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void OregonDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 2835;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 5670;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 4560;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 2835;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 5670;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void RhodeIslandDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 10900;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 21800;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 16350;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 10900;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 21800;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void SouthCarolinaDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 14600;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 29200;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 21900;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 14600;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 29200;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void VermontDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 6500;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 13050;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 9800;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 6500;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 13050;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void VirginiaDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 8750;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 17500;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 8750;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 8750;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 17500;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        private void WisconsinDeductions()
        {
            if (filingStatus == "Single")
            {
                stateTax -= 12760;
            }
            else if (filingStatus == "Married Filing Jointly")
            {
                stateTax -= 23620;
            }
            else if (filingStatus == "Head of Household")
            {
                stateTax -= 16840;
            }
            else if (filingStatus == "Married Filing Separately")
            {
                stateTax -= 12760;
            }
            else if (filingStatus == "Widowed")
            {
                stateTax -= 23620;
            }
            else//if the code is all written right, this should never execute regardless of user input. If this ever fires, it means I need to fix the code.
            {
                MessageBox.Show($"Something went wrong with Joseph's code related to calculating {state} decutions based off of filing status. " +
                    "Please contact him to inform him of his failure.");
            }
        }
        //End of state deductions

        private void StateDeductions()//in order to make CalculateButton_Click() cleaner, this exists as a box to put the code for determining which state deductions to use.
        {
            if (state == "Alabama")
            {
                AlabamaDeductions();
            }
            else if (state == "Arizona")
            {
                ArizonaDeductions();
            }
            else if (state == "Arkansas")
            {
                ArkansasDeductions();
            }
            else if (state == "California")
            {
                CaliforniaDeductions();
            }
            else if (state == "Colorado")
            {
                ColoradoDeductions();
            }
            else if (state == "Delaware")
            {
                DelawareDeductions();
            }
            else if (state == "Georgia")
            {
                GeorgiaDeductions();
            }
            else if (state == "Hawaii")
            {
                HawaiiDeductions();
            }
            else if (state == "Idaho")
            {
                IdahoDeductions();
            }
            else if (state == "Kansas")
            {
                KansasDeductions();
            }
            else if (state == "Kentucky")
            {
                KentuckyDeductions();
            }
            else if (state == "Louisiana")
            {
                LouisianaDeductions();
            }
            else if (state == "Maine")
            {
                MaineDeductions();
            }
            else if (state == "Maryland")
            {
                MarylandDeductions();
            }
            else if (state == "Minnesota")
            {
                MinnesotaDeductions();
            }
            else if (state == "Mississippi")
            {
                MississippiDeductions();
            }
            else if (state == "Missouri")
            {
                MissouriDeductions();
            }
            else if (state == "Nebraska")
            {
                NebraskaDeductions();
            }
            else if (state == "New Mexico")
            {
                NewMexicoDeductions();
            }
            else if (state == "New York")
            {
                NewYorkDeductions();
            }
            else if (state == "North Carolina")
            {
                NorthCarolinaDeductions();
            }
            else if (state == "North Dakota")
            {
                NorthDakotaDeductions();
            }
            else if (state == "Oklahoma")
            {
                OklahomaDeductions();
            }
            else if (state == "Oregon")
            {
                OregonDeductions();
            }
            else if (state == "Rhode Island")
            {
                RhodeIslandDeductions();
            }
            else if (state == "South Carolina")
            {
                SouthCarolinaDeductions();
            }
            else if (state == "Vermont")
            {
                VermontDeductions();
            }
            else if (state == "Virginia")
            {
                VirginiaDeductions();
            }
            else if (state == "Wisconsin")
            {
                WisconsinDeductions();
            }
            //Not all state have deductions. If a state wasn't mentioned, it doesn't have standard deductions.
        }

    }
}
//Alabama
//Alaska
//Arizona
//Arkansas
//California
//Colorado
//Connecticut
//Delaware
//Florida
//Georgia
//Hawaii
//Idaho
//Illinois
//Indiana
//Iowa
//Kansas
//Kentucky
//Louisiana
//Maine
//Maryland
//Massachusetts
//Michigan
//Minnesota
//Mississippi
//Missouri
//Montana
//Nebraska
//Nevada
//New Hampshire
//New Jersey
//New Mexico
//New York
//North Carolina
//North Dakota
//Ohio
//Oklahoma
//Oregon
//Pennsylvania
//Rhode Island
//South Carolina
//South Dakota
//Tennessee
//Texas
//Utah
//Vermont
//Virginia
//Washington
//West Virginia
//Wisconsin
//Wyoming
