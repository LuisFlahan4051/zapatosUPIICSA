﻿using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using zapatosUPIICSA.Properties;

namespace zapatosUPIICSA
{
    public partial class clientsForm : Form
    {
        public clientsForm()
        {
            InitializeComponent();
            drawTable();
        }

        ClientsCRUD clientsCRUD = new ClientsCRUD();
        Connect newConnection = new Connect();



        public void clearInputs()
        {
            txtId.Text = "";
            txtClient.Text = "";
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
                statement = new SqlCommand("SELECT * FROM clients", connection);
                results = statement.ExecuteReader();
                if (results == null)
                {
                    MessageBox.Show("¡La consulta es nula!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                try
                {
                    while (results.Read())
                    {
                        dataGridView1.Rows.Add(results.GetValue(0), results.GetValue(1), results.GetValue(2), results.GetValue(3));
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
            string name = txtClient.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            if ((!"".Equals(name) && !"".Equals(email)) || (!"".Equals(name) && !"".Equals(email)))
            {
                clientsCRUD.saveClient(name, email, phone);
                clearInputs();
            }
            else
            {
                MessageBox.Show("¡Tiene que poner al menos el nombre y una forma de contacto!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
            drawTable();
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
                clientsCRUD.deleteClientById(id);
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
                string name = txtClient.Text;
                string email = txtEmail.Text;
                string phone = txtPhone.Text;
                if ((!"".Equals(name) && !"".Equals(email)) || (!"".Equals(name) && !"".Equals(email)))
                {
                    clientsCRUD.updateClient(id, name, email, phone);
                    clearInputs();
                }
                else
                {
                    MessageBox.Show("¡Tiene que poner al menos el nombre y una forma de contacto!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                statement = new SqlCommand("SELECT * FROM clients WHERE id_client = @id", connection);
                statement.Parameters.AddWithValue("@id", id);
                results = statement.ExecuteReader();

                if (results == null)
                {
                    MessageBox.Show("¡La consulta es nula!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                while (results.Read())
                {
                    dataGridView1.Rows.Add(results.GetValue(0), results.GetValue(1), results.GetValue(2), results.GetValue(3));
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
                txtClient.Text = row.Cells[1].Value.ToString();
                txtEmail.Text = row.Cells[2].Value.ToString();
                txtPhone.Text = row.Cells[3].Value.ToString();
            }

        }

    }
}
