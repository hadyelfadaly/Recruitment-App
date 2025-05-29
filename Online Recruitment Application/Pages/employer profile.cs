using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecruitmentApp
{

    public partial class Form9 : Form
    {

        int employerId { get; set; }

        public Form9(int employerId)
        {

            InitializeComponent();

            this.employerId = employerId;

        }

        private void Form9_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'recruitmentDataSet3.EMPLOYER' table. You can move, or remove it, as needed.  
            this.eMPLOYERTableAdapter.FillBy(this.recruitmentDataSet3.EMPLOYER, this.employerId);

        }

        //save changes button click event
        private void button1_Click(object sender, EventArgs e)
        {

            //end edit to push changes from controls to the DataTable
            this.Validate();
            this.eMPLOYERBindingSource.EndEdit();

            //update the database with changes
            this.eMPLOYERTableAdapter.Update(this.recruitmentDataSet3.EMPLOYER);

            string newPassword = "", newName = "", newEmail = "", newPhoneNumber = "";

            //get the new values from the datagridview
            if(dataGridView1.CurrentRow != null)
            {

                if (dataGridView1.CurrentRow.Cells["Name"].Value != null) newName = dataGridView1.CurrentRow.Cells["Name"].Value.ToString();
                if (dataGridView1.CurrentRow.Cells["PhoneNumber"].Value != null) newPhoneNumber = dataGridView1.CurrentRow.Cells["PhoneNumber"].Value.ToString();
                if (dataGridView1.CurrentRow.Cells["Password"].Value != null) newPassword = dataGridView1.CurrentRow.Cells["Password"].Value.ToString();
                if (dataGridView1.CurrentRow.Cells["Email"].Value != null) newEmail = dataGridView1.CurrentRow.Cells["Email"].Value.ToString();

            }

            //updating user table too
            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            string query = @"UPDATE [USER] SET Password = @Password, Name = @Name, PhoneNumber = @PhoneNumber, Email = @Email
                           WHERE UserID = @UserID";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {

                cmd.Parameters.AddWithValue("@Password", newPassword); //get newPassword from the datagrid
                cmd.Parameters.AddWithValue("@Name", newName);         //get newName from the datagrid
                cmd.Parameters.AddWithValue("@PhoneNumber", newPhoneNumber); //get newPhoneNumber from the datagrid
                cmd.Parameters.AddWithValue("@Email", newEmail);       //get newEmail from the datagrid
                cmd.Parameters.AddWithValue("@UserID", employerId);   //use the correct user/jobseeker id

                cmd.ExecuteNonQuery();

            }

            MessageBox.Show("Profile updated successfully.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);

            connection.Close();

        }

        //logout button click event
        private void button2_Click_1(object sender, EventArgs e)
        {

            //open the login page
            Form1 login = new Form1();

            login.Show();
            this.Close();

        }

        //back button click event
        private void button3_Click_1(object sender, EventArgs e)
        {

            Form3 form3 = new Form3(employerId);

            form3.Show();
            this.Close();

        }

    }

}
