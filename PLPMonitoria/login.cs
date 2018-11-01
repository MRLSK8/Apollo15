using System;
using System.Windows.Forms;
using System.Data.OleDb;

namespace PLPMonitoria
{
    public partial class login : Form
    {
        OleDbConnection con;

        public login()
        {
            connection c = new connection();
            this.con = c.con;


            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
			// Checkbox no banco de dados
			try
			{
				con.Open();

				if (checkBox1.Checked == true)
				{
					string sqlcheckbox2 = @"UPDATE login SET checked =  '" + "true" + "'";
					OleDbCommand cmd2 = new OleDbCommand(sqlcheckbox2, con);
					cmd2.ExecuteNonQuery();
				}
				else
				{
					string sqlcheckbox2 = @"UPDATE login SET checked =  '" + "false" + "'";
					OleDbCommand cmd2 = new OleDbCommand(sqlcheckbox2, con);
					cmd2.ExecuteNonQuery();
				}

				// Pegando o login e a senha pra checar se estão corretas!
				string sqlverifica = "SELECT * FROM login";
				OleDbCommand cmd = new OleDbCommand(sqlverifica, con);
				OleDbDataReader read = cmd.ExecuteReader();
				read.Read();

				if (txtLogin.Text.Equals(read["loginName"].ToString()) && txtPassword.Text.Equals(read["senha"].ToString()))
				{
					mainScrenn ms = new mainScrenn();
					ms.login = txtLogin.Text;
					ms.Show();
					this.Hide();
				}
				else
				{
					MessageBox.Show("Usuário ou senha inválidos. Tente novamente !");
				}

				// Fechando o banco de dados
				con.Close();
			}
			catch
			{
				MessageBox.Show("Erro ao fazer conexão com o banco de dado (login/update)");
			}

        }

		private void login_Load(object sender, EventArgs e)
		{
			try
			{
				// Abrindo o banco de dados
				con.Open();

				string sqllogin = "SELECT * FROM login";
				OleDbCommand cmd = new OleDbCommand(sqllogin, con);
				OleDbDataReader read = cmd.ExecuteReader();
				read.Read();

				if (read["checked"].ToString() == "true")
				{
					checkBox1.Checked = true;
					txtLogin.Text = Convert.ToString(read["loginName"].ToString());
				}
				else
				{
					txtLogin.Text = "";
				}
				// Fechando o banco de dados
				con.Close();
			}
			catch
			{
				MessageBox.Show("Erro ao fazer conexão com o banco de dado (login/save)");
			}
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			
		}
	}
}
