using System;
using System.Data.SQLite;

namespace Employee_Management
{
    public partial class Form1 : Form
    {
        private DataGridView dgv;
        private string filename;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            menuStrip1.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:";
            openFileDialog.Filter = "Database files (*.db)|*.db|All files(*.*)|*.*";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog.FileName;
                button1.Hide();
                button2.Hide();
                panel1.Hide();
                openDB();
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "c:";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.DefaultExt = "db";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                button1.Hide();
                button2.Hide();
                panel1.Hide();
                SQLiteConnection.CreateFile(saveFileDialog.FileName);
                using (var connection = new SQLiteConnection($"Data Source={saveFileDialog.FileName};Version=3;"))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();
                        command.CommandText = @"CREATE TABLE pracownicy(
                                              id INT PRIMARY KEY NOT NULL, 
                                              name TEXT NOT NULL,
                                              surname TEXT NOT NULL,
                                              age INT NOT NULL,
                                              stance TEXT NOT NULL);";
                        command.ExecuteReader();
                        filename = saveFileDialog.FileName;
                        openDB();
                    }
                }
            }
        }

        private void openDB()
        {
            menuStrip1.Show();

            dgv = new DataGridView();

            dgv.ColumnCount = 5;
            dgv.Columns[0].Name = "ID";
            dgv.Columns[1].Name = "Name";
            dgv.Columns[2].Name = "Surname";
            dgv.Columns[3].Name = "Age";
            dgv.Columns[4].Name = "Stance";

            dgv.Size = new Size(543, 440);
            dgv.Location = new Point(0, 23);

            string[] row = { "ID", "Name", "Surname", "Age", "Stance" };

            this.Controls.Add(dgv);

            using (var connection = new SQLiteConnection($"Data Source={filename};Version=3;"))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "SELECT * FROM pracownicy;";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgv.Rows.Add(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4));
                        }
                    }
                }
            }
        }
        private void deleteEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 newForm = new Form3();
            newForm.ShowDialog();

            using (var connection = new SQLiteConnection($"Data Source={filename};Version=3;"))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM pracownicy WHERE id={newForm.id};";
                    command.ExecuteReader();
                }
            }
        }

        private void addEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            newForm.ShowDialog();
            

            using (var connection = new SQLiteConnection($"Data Source={filename};Version=3;"))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO pracownicy VALUES({newForm.id}, '{newForm.name}', '{newForm.surname}', {newForm.age}, '{newForm.stance}');";
                    try
                    {
                        command.ExecuteReader();
                    }
                    catch(Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
            }
        }

        private void modifyEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 newForm = new Form4();
            newForm.ShowDialog();

            using (var connection = new SQLiteConnection($"Data Source={filename};Version=3;"))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"UPDATE pracownicy SET name='{newForm.name}', surname='{newForm.surname}', age={newForm.age}, stance='{newForm.stance}' WHERE id={newForm.id};";
                    command.ExecuteReader();
                }
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgv.Dispose();
            openDB();
        }
    }
}