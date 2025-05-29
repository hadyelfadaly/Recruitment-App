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

    public partial class Form7 : Form
    {

        int jobSeekerId { get; set; }

        public Form7(int jobSeekerID)
        {

            InitializeComponent();

            this.jobSeekerId = jobSeekerID;

        }

        private void Form7_Load(object sender, EventArgs e)
        {

            //hide skill controls
            label1.Visible = false;
            textBox1.Visible = false;
            button5.Visible = false;
            label2.Visible = false;
            textBox2.Visible = false;

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            //query to get jobseeker details
            string query = @"SELECT j.USERID AS ID, j.NAME, j.EMAIL, j.PASSWORD, j.PHONENUMBER, j.REGISTRATIONDATE, j.EXPRIENCE, COUNT(a.USERID) AS ApplicationCount
                            FROM JOBSEEKER j LEFT JOIN APPLICATION a ON j.USERID = a.USERID
                            WHERE j.USERID = '" + jobSeekerId +"' " +
                            "GROUP BY j.USERID, j.NAME, j.EMAIL, j.PASSWORD, j.PHONENUMBER,  j.REGISTRATIONDATE, j.EXPRIENCE";
 
            using(SqlCommand command = new SqlCommand(query, connection))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                //binding the datatable to the datagridview
                dataGridView1.DataSource = dt;

            }

            connection.Close();

        }

        //save changes button click event
        private void button1_Click(object sender, EventArgs e)
        {

            string newPassword = "", newName = "", newEmail = "", newPhoneNumber = "";

            //get the new values from the datagridview
            if(dataGridView1.CurrentRow != null)
            {

                if(dataGridView1.CurrentRow.Cells["NAME"].Value != null) newName = dataGridView1.CurrentRow.Cells["NAME"].Value.ToString();
                if(dataGridView1.CurrentRow.Cells["PHONENUMBER"].Value != null) newPhoneNumber = dataGridView1.CurrentRow.Cells["PHONENUMBER"].Value.ToString();
                if(dataGridView1.CurrentRow.Cells["PASSWORD"].Value != null) newPassword = dataGridView1.CurrentRow.Cells["PASSWORD"].Value.ToString();
                if(dataGridView1.CurrentRow.Cells["EMAIL"].Value != null) newEmail = dataGridView1.CurrentRow.Cells["EMAIL"].Value.ToString();
                
            }

            //updating user table
            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            string query = @"UPDATE [USER] SET Password = @Password, Name = @Name, PhoneNumber = @PhoneNumber, Email = @Email
                           WHERE UserID = @UserID";

            using(SqlCommand cmd = new SqlCommand(query, connection))
            {

                cmd.Parameters.AddWithValue("@Password", newPassword); //get newPassword from the datagrid
                cmd.Parameters.AddWithValue("@Name", newName);         //get newName from the datagrid\
                cmd.Parameters.AddWithValue("@PhoneNumber", newPhoneNumber); //get newPhoneNumber from the datagrid
                cmd.Parameters.AddWithValue("@Email", newEmail);       //get newEmail from the datagrid
                cmd.Parameters.AddWithValue("@UserID", jobSeekerId);   //use the correct user/jobseeker id

                cmd.ExecuteNonQuery();

            }

            //updating jobseeker table
            string query2 = @"UPDATE JOBSEEKER SET Password = @Password, Name = @Name, PhoneNumber = @PhoneNumber, Email = @Email
                           WHERE UserID = @UserID";

            using (SqlCommand cmd = new SqlCommand(query2, connection))
            {

                cmd.Parameters.AddWithValue("@Password", newPassword); //get newPassword from the datagrid
                cmd.Parameters.AddWithValue("@Name", newName);         //get newName from the datagrid\
                cmd.Parameters.AddWithValue("@PhoneNumber", newPhoneNumber); //get newPhoneNumber from the datagrid
                cmd.Parameters.AddWithValue("@Email", newEmail);       //get newEmail from the datagrid
                cmd.Parameters.AddWithValue("@UserID", jobSeekerId);   //use the correct user/jobseeker id

                cmd.ExecuteNonQuery();

            }

            MessageBox.Show("Profile updated successfully.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);

            connection.Close();

        }

        //logout button click event
        private void button2_Click(object sender, EventArgs e)
        {

            //open the login page
            Form1 login = new Form1();

            login.Show();
            this.Close();

        }

        //back button click event
        private void button3_Click(object sender, EventArgs e)
        {

            Form2 form2 = new Form2(jobSeekerId);

            form2.Show();
            this.Close();

        }

        //confirm button click event
        private void button5_Click(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            //query to get skill id
            string query = @"SELECT SKILLID FROM SKILL WHERE [NAME] = @name ";
            int skillId = 0;

            using (SqlCommand command = new SqlCommand(query, connection))
            {

                command.Parameters.AddWithValue("@name", textBox1.Text);

                //store skill id
                skillId = Convert.ToInt32(command.ExecuteScalar());

            }

            //query to insert skill id and jobseeker id into the userskill table
            string query2 = @"INSERT INTO USERSKILL VALUES (@UserID, @SkillID, @ExpYears)";

            using (SqlCommand command = new SqlCommand(query2, connection))
            {
                command.Parameters.AddWithValue("@UserID", jobSeekerId);
                command.Parameters.AddWithValue("@SkillID", skillId);
                command.Parameters.AddWithValue("@ExpYears", textBox2.Text);

                //execute the query
                command.ExecuteNonQuery();

                MessageBox.Show("Skill added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //clear the textboxes
                textBox1.Clear();
                textBox2.Clear();

            }

            connection.Close();

        }

        //add skills button click event
        private void button4_Click(object sender, EventArgs e)
        {

            //show skill controls
            label1.Visible = true;
            textBox1.Visible = true;
            button5.Visible = true;
            label2.Visible = true;
            textBox2.Visible = true; 

        }

    }

}
