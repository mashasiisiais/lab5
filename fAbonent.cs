using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using laba04;

namespace lab5._02
{
    public partial class fAbonent : Form
    {
        public Abonent TheAbonent;
        public fAbonent(Abonent t)
        {
            TheAbonent = t;
            InitializeComponent();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            TheAbonent.Name = tbName.Text.Trim();
            TheAbonent.Number = tbNumber.Text.Trim();
            TheAbonent.CostT = double.Parse(tbCostT.Text.Trim());
            TheAbonent.Extra = double.Parse(tbExtra.Text.Trim());
            TheAbonent.Service = double.Parse(tbService.Text.Trim());
            TheAbonent.Debt = chbDebt.Checked;
            TheAbonent.Contract = chbContract.Checked;
            DialogResult = DialogResult.OK;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private void fAbonent_Load(object sender, EventArgs e)
        {
            if (TheAbonent != null)
            {
                tbName.Text = TheAbonent.Name;
                tbNumber.Text = TheAbonent.Number;
                tbCostT.Text = TheAbonent.CostT.ToString("0.00");
                tbExtra.Text = TheAbonent.Extra.ToString("");

                tbService.Text = TheAbonent.Service.ToString("0.000");
                chbDebt.Checked = TheAbonent.Debt;
                chbContract.Checked = TheAbonent.Contract;
            }
        }
    }
}
