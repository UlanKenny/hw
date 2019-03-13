using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace airastana11
{

    public partial class Form1 : Form
    {

        static string conString = "Server=localhost;Database=airastana;Uid=root;Pwd=;";
        MySqlConnection con = new MySqlConnection(conString);
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        DataTable dt = new DataTable();

        public Form1()
        {
            InitializeComponent();

            //DATAGRIDVIEW PROPERTIES
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "ID";
            dataGridView1.Columns[1].Name = "Name";
            dataGridView1.Columns[2].Name = "Birth";
            dataGridView1.Columns[3].Name = "Description";
            dataGridView1.Columns[3].Name = "Email";

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //SELECTION MODE
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

        }

        //INSERT INTO DB
        private void add(string name, string birth, string description, string email)
        {
            //SQL STMT
            string sql = "INSERT INTO contacts(Name,Birth,Description,Email) VALUES(@NAME,@BIRTH,@DESCRIPTION,@EMAIL)";
            cmd = new MySqlCommand(sql, con);

            //ADD PARAMETERS
            cmd.Parameters.AddWithValue("@NAME", name);
            cmd.Parameters.AddWithValue("@BIRTH", birth);
            cmd.Parameters.AddWithValue("@DESCRIPTION", description);
            cmd.Parameters.AddWithValue("@EMAIL", email);
            //OPEN CON AND EXEC insert
            try
            {
                con.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    clearTxts();
                    MessageBox.Show("Successfully Inserted");
                }

                con.Close();

                retrieve();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        //ADD TO DGVIEW
        private void populate(string id,string name, string birth, string description, string email)
        {
            dataGridView1.Rows.Add(id, name, birth, description,email);
        }

        //RETRIEVE FROM DB
        private void retrieve()
        {
            dataGridView1.Rows.Clear();

            //SQL STMT
            string sql = "SELECT * FROM contacts ";
            cmd = new MySqlCommand(sql, con);

            //OPEN CON,RETRIEVE,FILL DGVIEW
            try
            {
                con.Open();

                adapter = new MySqlDataAdapter(cmd);

                adapter.Fill(dt);

                //LOOP THRU DT
                foreach (DataRow row in dt.Rows)
                {
                    populate(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[0].ToString(),row[0].ToString());
                }

                con.Close();

                //CLEAR DT
                dt.Rows.Clear();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        //UPDATE DB
        private void update(Int32 id,string name, string birth, string description, string email)
        {
            //SQL STMT
            string sql = "UPDATE peopleTB SET Name='" + name + "',Birth='" + birth + "',Description='" + description + "' WHERE ID=" + id + "";
            cmd = new MySqlCommand(sql, con);

            //OPEN CON,UPDATE,RETRIEVE DGVIEW
            try
            {
                con.Open();
                adapter = new MySqlDataAdapter(cmd);

                adapter.UpdateCommand = con.CreateCommand();
                adapter.UpdateCommand.CommandText = sql;

                if (adapter.UpdateCommand.ExecuteNonQuery() > 0)
                {
                    clearTxts();
                    MessageBox.Show("Successfully Updated");
                }

                con.Close();

                retrieve();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }

        }

        //DELETE FROM DB
        private void delete(int id)
        {
            //SQLSTMT
            string sql = "DELETE FROM contacts WHERE ID=" + id + "";
            cmd = new MySqlCommand(sql, con);

            //'OPEN CON,EXECUTE DELETE,CLOSE CON
            try
            {
                con.Open();
                MessageBox.Show(con.State.ToString());
                adapter = new MySqlDataAdapter(cmd);

                adapter.DeleteCommand = con.CreateCommand();

                adapter.DeleteCommand.CommandText = sql;

                //PROMPT FOR CONFIRMATION
                if (MessageBox.Show("Sure ??", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        clearTxts();
                        MessageBox.Show("Successfully deleted");
                    }
                }

                con.Close();

                retrieve();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }

        }

        //clear txtx
        private void clearTxts()
        {
            nameTxt.Text = "";
            description.Text = "";
            email.Text = "";
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            nameTxt.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            birth.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            description.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

            email.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            add(nameTxt.Text, birth.CustomFormat="dd-mm-yyyy", description.Text,email.Text);
        }

        private void retrieveBtn_Click(object sender, EventArgs e)
        {
            retrieve();
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            String selected = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);

            update(id, nameTxt.Text, birth.CustomFormat="dd-mm-yyyy",description.Text, email.Text);
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            String selected = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);

            delete(id);
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
