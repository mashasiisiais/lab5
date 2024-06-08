using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using laba04;

namespace lab5._02
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }
        private void fMain_Resize(object sender, EventArgs e)
        {
            int buttonsSize = 9 * btnAdd.Width + 3 * toolStripSeparator1.Width;
            btnExit.Margin = new Padding(Width - buttonsSize, 0, 0, 0);
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            gvAbonents.AutoGenerateColumns = false;
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Name";
            column.Name = "ФІО";
            gvAbonents.Columns.Add(column);
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Number";
            column.Name = "Номер";
            gvAbonents.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Service";
            column.Name = "Кількість активацій";
            gvAbonents.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "CostT";
            column.Name = "Тариф";
            gvAbonents.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Extra";
            column.Name = "Додаткові послуги";
            column.Width = 80;
            gvAbonents.Columns.Add(column);

            column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = "Debt";
            column.Name = "Борг";
            column.Width = 80;
            gvAbonents.Columns.Add(column);

            column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = "Contract";
            column.Name = "Контракт";
            column.Width = 60;
            gvAbonents.Columns.Add(column);

            bindSrcAbonents.Add(new Abonent("Сирченко Олег Вікторович", "+6788875", 78, 8, 10, false, true));
            EventArgs args = new EventArgs();
            OnResize(args);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Abonent abonent = new Abonent();
            fAbonent ft = new fAbonent(abonent);
            if (ft.ShowDialog() == DialogResult.OK)
            {
                bindSrcAbonents.Add(abonent);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Abonent abonent = (Abonent)bindSrcAbonents.List[bindSrcAbonents.Position];
            fAbonent ft = new fAbonent(abonent);
            if (ft.ShowDialog() == DialogResult.OK)
            {
                bindSrcAbonents.List[bindSrcAbonents.Position] = abonent;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Видалити поточний запис?", "Видалення запису", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                bindSrcAbonents.RemoveCurrent();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Очистити таблицю?\n\nВсі дані будуть втрачені", "Очищення даних", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                bindSrcAbonents.Clear();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Закрити застосунок?", "Вихід з програми", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Application.Exit();
            }
        }



        private void btnSaveAsText_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Текстові файли (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.Title = "Зберегти дані у текстовому форматі";
            saveFileDialog1.InitialDirectory = Application.StartupPath;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = null;
                try
                {
                    sw = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                    foreach (Abonent abonent in bindSrcAbonents.List)
                    {
                        sw.WriteLine($"{abonent.Name}\t{abonent.Service}\t{abonent.Extra}\t{abonent.CostT}\t{abonent.Number}\t{abonent.Debt}\t{abonent.Contract}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Сталась помилка: \n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    sw?.Close();
                }
            }
        
    }
        private void btnOpenFromText_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Текстові файли (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Прочитати дані у текстовому форматі";
            openFileDialog.InitialDirectory = Application.StartupPath;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bindSrcAbonents.Clear();

                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog.FileName, Encoding.UTF8))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] split = s.Split('\t');
                            Abonent abonent = new Abonent
                            {
                                Name = split[0],
                                Service = double.Parse(split[1]),
                                Extra = double.Parse(split[2]),
                                CostT = double.Parse(split[3]),
                                Number = split[4],
                                Debt = bool.Parse(split[5]),
                                Contract = bool.Parse(split[6])
                            };
                            bindSrcAbonents.Add(abonent);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Сталась помилка: \n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
    

        private void btnSaveAsBinary_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Файли даних (*.towns)|*.towns|All files (*.*)|*.*";
            saveFileDialog1.Title = "Зберегти дані у бінарному форматі";
            saveFileDialog1.InitialDirectory = Application.StartupPath;
            BinaryWriter bw;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bw = new BinaryWriter(saveFileDialog1.OpenFile());
                try
                {
                    foreach (Abonent abonent in bindSrcAbonents.List)
                    {
                        bw.Write(abonent.Name);
                        bw.Write(abonent.Number);
                        bw.Write(abonent.Contract);
                        bw.Write(abonent.CostT);
                        bw.Write(abonent.Extra);
                        bw.Write(abonent.Debt);
                        bw.Write(abonent.Service);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    bw.Close();
                }
            }
        }


        private void btnOpenFromBinary_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файли даних (*.towns)|*.towns|All files(*.*) | *.* ";
            openFileDialog.Title = "Прочитати дані у бінарному форматі";
            openFileDialog.InitialDirectory = Application.StartupPath;
            BinaryReader br;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bindSrcAbonents.Clear();
                br = new BinaryReader(openFileDialog.OpenFile());
                try
                {
                    Abonent abonent;
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {

                        abonent = new Abonent();
                        for (int i = 1; i <= 7; i++)
                        {
                            switch (i)
                            {
                                case 1:
                                    abonent.Name = br.ReadString();
                                    break;
                                case 2:
                                    abonent.Number = br.ReadString();
                                    break;
                                case 3:
                                    abonent.Debt = br.ReadBoolean();
                                    break;
                                case 4:
                                    abonent.Contract = br.ReadBoolean();
                                    break;
                                case 5:
                                    abonent.Service = br.ReadDouble();
                                    break;
                                case 6:
                                    abonent.Extra = br.ReadDouble();
                                    break;
                                case 7:
                                    abonent.CostT = br.ReadDouble();
                                    break;
                            }
                        }
                        bindSrcAbonents.Add(abonent);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    br.Close();
                }
            }
        }




    }
}

    



   

