﻿using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using zapatosUPIICSA.Properties;

namespace zapatosUPIICSA
{
    public partial class usersForm : Form
    {
        public usersForm()
        {
            InitializeComponent();
            drawTable();    
        }

        UsersCRUD usersCRUD = new UsersCRUD();
        Connect newConnection = new Connect();



        public void clearInputs()
        {
            txtId.Text = "";
            txtUser.Text = "";
            txtPassword.Text =  "";
            txtEmail.Text = "";
            txtPhone.Text = "";
        }

        public void drawTable()
        {
            SqlConnection connection = newConnection.getConnection();
            SqlCommand statement;
            SqlDataReader results;
            dataGridView1.Rows.Clear();

            try
            {
                connection.Open();
                statement = new SqlCommand("SELECT * FROM users", connection);
                results = statement.ExecuteReader();
                if (results == null)
                {
                    MessageBox.Show("¡La consulta es nula!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                try
                {
                    while (results.Read())
                    {
                        dataGridView1.Rows.Add(results.GetValue(0), results.GetValue(1), results.GetValue(2), results.GetValue(3), results.GetValue(4));
                    }
                }
                catch (SystemException e)
                {
                    MessageBox.Show("¡Error al llenar la tabla!" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
            catch (SystemException e)
            {
                MessageBox.Show("¡Algo ha fallado!" + e.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            clearInputs();
        }


        



        #region botones de control
        private void button1_MouseHover(object sender, EventArgs e)
        {
            btnClose.BackgroundImage = Resources.Close_2;
            btnClose.ForeColor = Color.White;
            btnClose.BackColor = Color.FromArgb(126, 1, 63);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackgroundImage = Resources.Close;
            btnClose.ForeColor = Color.FromArgb(126, 1, 63);
            btnClose.BackColor = Color.White;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region Botones del crud.
        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtUser.Text;
            string password = txtPassword.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            
            if ((!"".Equals(name) && !"".Equals(password)) || (!"".Equals(email) && !"".Equals(password)) || (!"".Equals(phone) && !"".Equals(password)))
            {
                usersCRUD.saveUser(name, password, email, phone);
                clearInputs();
                drawTable();}
            else
            {
                MessageBox.Show("¡Tiene que poner al menos un campo y una contraseña!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = txtId.Text;
            if ("".Equals(id))
            {
                MessageBox.Show("¡Especifíque un ID en el campo de filtro por favor!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                usersCRUD.deleteUserById(id);
                clearInputs();
                drawTable();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string id = txtId.Text;
            if ("".Equals(id))
            {
                MessageBox.Show("No podemos actualizar un elemento que no está seleccionado!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string name = txtUser.Text;
                string password = txtPassword.Text;
                string email = txtEmail.Text;
                string phone = txtPhone.Text;
                if ((!"".Equals(name) && !"".Equals(password)) || (!"".Equals(email) && !"".Equals(password)) || (!"".Equals(phone) && !"".Equals(password)))
                { 
                    usersCRUD.updateUser(id, name, password, email, phone);
                    clearInputs();
                }
                else
                {
                    MessageBox.Show("¡Tiene que poner al menos un campo y una contraseña!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            drawTable();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {

            string id = txtId.Text;
            if ("".Equals(id))
            {
                drawTable();
            }
            else
            {
                SqlConnection connection = newConnection.getConnection();
                SqlCommand statement;
                SqlDataReader results;
                dataGridView1.Rows.Clear();

                connection.Open();
                statement = new SqlCommand("SELECT * FROM users WHERE id_user = @id", connection);
                statement.Parameters.AddWithValue("@id", id);
                results = statement.ExecuteReader();

                if (results == null)
                {
                    MessageBox.Show("¡La consulta es nula!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                while (results.Read())
                {
                    dataGridView1.Rows.Add(results.GetValue(0), results.GetValue(1), results.GetValue(2), results.GetValue(3), results.GetValue(4));
                }
                connection.Close();
            }
            clearInputs();
        }
        #endregion

        public void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!(e.RowIndex > -1))
            {
                return;
            }

            DataGridViewRow row = dataGridView1.Rows.SharedRow(e.RowIndex);
            if (row.Cells[0].Value != null)
            {
                txtId.Text = row.Cells[0].Value.ToString();
                txtUser.Text = row.Cells[1].Value.ToString();
                txtPassword.Text = row.Cells[2].Value.ToString();
                txtEmail.Text = row.Cells[3].Value.ToString();
                txtPhone.Text = row.Cells[4].Value.ToString();
            }
            
        }

    }
}
