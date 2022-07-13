using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employee_Management
{
    public partial class Form2 : Form
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public int age { get; set; }
        public string stance { get; set; }
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                id = Convert.ToInt32(textBox1.Text);
                name = Convert.ToString(textBox2.Text);
                surname = Convert.ToString(textBox3.Text);
                age = Convert.ToInt32(textBox4.Text);
                stance = Convert.ToString(textBox5.Text);
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }
            this.Hide();
        }
    }
}
