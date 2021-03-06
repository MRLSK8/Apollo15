﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace PLPMonitoria
{
	public partial class newOrder : Form
	{
        OleDbConnection con;
		public newOrder()
		{
            connection c = new connection();
            con = c.con;

			InitializeComponent();
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}

		private void dataOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void label1_Click(object sender, EventArgs e)
		{
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{

			// Checando se o numero de produto é maior que zero
			if (numFood.Value != 0)
			{
				// Checando se foi selecionada alguma comida
				if (cmbFood.SelectedIndex != -1)
				{
					if (txtTable.Text != "")
					{
						
						if (txtName.Text != "")
						{
							// Conectando com o banco de dados
							try
							{	
								// Abrindo o banco de dados
								con.Open();

								string cmd1 = @"SELECT price FROM Food WHERE nome = '"+ cmbFood.GetItemText(cmbFood.SelectedItem)+"'";
								OleDbCommand comand = new OleDbCommand(cmd1, con);
								OleDbDataReader read_price_food = comand.ExecuteReader();
								read_price_food.Read();

								// Total gasto no pedido da comida
								Double totalfood = Convert.ToDouble(read_price_food["price"]) * (double)numFood.Value;
								
								// Inserindo o pedido (comida) no banco de dados
								string orderfood = "Insert Into FoodOrders(client, board_number, food, quant_food, total) Values";
								orderfood += "('" + txtName.Text + "','" + txtTable.Text + "', '" + cmbFood.GetItemText(cmbFood.SelectedItem) + "','" + numFood.Value.ToString() + "','" + String.Format("{0:0.00}", totalfood) + "')";
								OleDbCommand comando = new OleDbCommand(orderfood, con);
								comando.ExecuteNonQuery();

								// Alterando o label Detalhes do pedido (adicionando o nome do cliente)
								lblDetails.Text = "Detalhes do pedido de " + txtName.Text.ToString();
								// Adicionando o pedido ao DataGridView
								dataOrder.Rows.Add("Comida",cmbFood.GetItemText(cmbFood.SelectedItem), numFood.Value.ToString(), "R$ " + String.Format("{0:0.00}", read_price_food["price"]), txtTable.Text);

								// Fechando o banco de dados
								con.Close();
							}
							catch 
							{
								MessageBox.Show("Erro ao fazer conexão com o banco de dado!");
							}
						}
						else
						{
							MessageBox.Show("Campo \"Nome\" obrigatório! ");
							txtTable.Text = "";
						}
					}
					else
					{
						MessageBox.Show("Campo \"Mesa\" obrigatório! ");
					}
				}
				else
				{
					MessageBox.Show("Campo \"Pratos\" obrigatório! ");

				}
			}
			// Checando se o numero de produto é maior que zero
			if (numDrink.Value != 0)
			{
				// Checando se foi selecionada alguma comida
				if (cmbDrink.SelectedIndex != -1)
				{
					if (txtTable.Text != "")
					{
						if (txtName.Text != "")
						{
							// Conectando com o banco de dados
							try
							{
								// Abrindo o banco de dados
								con.Open();

								// Lendo o preço da comida
								string cmd1 = @"SELECT price fROM Drink WHERE nome = '" + cmbDrink.GetItemText(cmbDrink.SelectedItem) + "'";
								OleDbCommand comand= new OleDbCommand(cmd1, con);
								OleDbDataReader read_price_drink = comand.ExecuteReader();
								read_price_drink.Read();

								// Total gasto no pedido da comida
								double totaldrink = Convert.ToDouble(read_price_drink["price"]) * (double)numDrink.Value;

								// Inserindo o pedido (comida) no banco de dados
								string orderfood = "Insert Into DrinkOrders(client, board_number, drink, quant_drink, total) Values";
								orderfood += "('" + txtName.Text + "','" + txtTable.Text + "', '" + cmbDrink.GetItemText(cmbDrink.SelectedItem) + "','" + numDrink.Value.ToString() + "','" + String.Format("{0:0.00}", totaldrink) + "')";
								OleDbCommand comando = new OleDbCommand(orderfood, con);
								comando.ExecuteNonQuery();

								// Alterando o label Detalhes do pedido (adicionando o nome do cliente)
								lblDetails.Text = "Detalhes do pedido de " + txtName.Text.ToString();
								// Adicionando o pedido ao DataGridView
								dataOrder.Rows.Add("Bebida", cmbDrink.GetItemText(cmbDrink.SelectedItem), numDrink.Value.ToString(), "R$ " + String.Format("{0:0.00}", read_price_drink["price"]), txtTable.Text); 								
								// Fechando o banco de dados
								con.Close();
							}
							catch (Exception erro)
							{
								MessageBox.Show(erro.Message);
							}
						}
						else
						{
							MessageBox.Show("Campo \"Nome\" obrigatório! ");
							txtTable.Text = "";
						}
					}
					else
					{
						MessageBox.Show("Campo \"Mesa\" obrigatório! ");
					}
				}
				else
				{
					MessageBox.Show("Campo \"Bebidas\" obrigatório! ");

				}
			}
			
			try
			{
				// Gasto total do cliente
				double spent = 0;
				// Abrindo o banco de dados
				con.Open();
				
				// Selecionando quanto foi gasto em comida com pedidos anteriores
				string sqlconnection1 = "SELECT * FROM FoodOrders WHERE client =  '" + txtName.Text + "'";
				OleDbCommand cmd1 = new OleDbCommand(sqlconnection1, con);
				OleDbDataReader prices = cmd1.ExecuteReader();
				
				// Somando gastos com pedidos anteriores
				while (prices.Read())
				{
					spent += Convert.ToDouble(prices["total"]);
				}

				// Selecionando quanto foi gasto em bebida com pedidos anteriores
				string sqlconnection2 = "SELECT * FROM DrinkOrders WHERE client = '" + txtName.Text + "'";
				OleDbCommand cmd2 = new OleDbCommand(sqlconnection2, con);
				OleDbDataReader prices2 = cmd2.ExecuteReader();

				while (prices2.Read())
				{
					spent += Convert.ToDouble(prices2["total"]);
				}

				// Mudando o total de todos o pedidos (tela de pedidos)
				lblTotal.Text = "Total: R$ " + String.Format("{0:f2}", spent);

				// Verifica se é uma mesa que já está no banco de dados
				string sqlJaExiste = "SELECT * FROM ClientOrders";
				OleDbCommand cmd3 = new OleDbCommand(sqlJaExiste, con);
				OleDbDataReader verificamesa = cmd3.ExecuteReader();

				bool jatem = false;
				while (verificamesa.Read())
				{
					if(verificamesa["board_number"].ToString() == txtTable.Text)
					{
						string atualiza_spent = @"UPDATE ClientOrders SET spent =  '" + String.Format("{0:0.00}", spent) + "' WHERE client =  '" + txtName.Text  + "'";
						OleDbCommand comand = new OleDbCommand(atualiza_spent, con);
						comand.ExecuteNonQuery();
						jatem = true;
						break;
					}
				}

				if (!jatem)
				{
					// Adicionando pedido do client no banco de dados
					string SQL = "Insert Into ClientOrders(status, client, board_number, spent) Values";
					SQL += "('Em andamento','" + txtName.Text + "','" + txtTable.Text + "','" + String.Format("{0:0.00}", spent) + "')";
					OleDbCommand comando = new OleDbCommand(SQL, con);
					comando.ExecuteNonQuery();
				}
				// Fechando o banco de dados
				con.Close();
			}
			catch
			{
				MessageBox.Show("Erro ao fazer conexão com o banco de dado!");
			}

			// Limpando dados
			cmbFood.SelectedIndex = -1;
			cmbDrink.SelectedIndex = -1;
			numFood.Value = 0;
			numDrink.Value = 0;
		}
		private ComboBox GetCmbFood()
		{
			return cmbFood;
		}
		private void btnFinalizar_Click(object sender, EventArgs e)
		{
			// Checando se foi adicionado algum pedido
			if (dataOrder.RowCount != 0)
			{
				MessageBox.Show("\n Pedido realizado!");
			}
			mainScrenn go = new mainScrenn();
			go.Show();
			this.Hide();
		}
		private void txtTable_TextChanged(object sender, KeyPressEventArgs e)
		{
			if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
			{
				e.Handled = true;
			}
			if (e.Handled)
			{
				MessageBox.Show(" Só é permitido número!!");
			}
		}
	
		private void newOrder_Load(object sender, EventArgs e)
		{
			// Preechendo o Combobox com dados do banco de dados 
			try
			{
				con.Open();

				cmbFood.Items.Clear();
				string cmd1 = "SELECT * FROM Food ORDER BY nome ASC";
				OleDbCommand comand1 = new OleDbCommand(cmd1, con);
				OleDbDataAdapter adapter1 = new OleDbDataAdapter(comand1);
				DataTable table1 = new DataTable();

				adapter1.Fill(table1);
				cmbFood.DataSource = table1;
				cmbFood.ValueMember = "id";
				cmbFood.DisplayMember = "nome";

				// -------------------------------------------------------------------
				cmbDrink.Items.Clear();
				string cmd2 = "SELECT * FROM Drink ORDER BY nome ASC";
				OleDbCommand comand2 = new OleDbCommand(cmd2, con);
				OleDbDataAdapter adapter2 = new OleDbDataAdapter(comand2);
				DataTable table2 = new DataTable();
            

				adapter2.Fill(table2);
				cmbDrink.DataSource = table2;
				cmbDrink.ValueMember = "id";
				cmbDrink.DisplayMember = "nome";

				cmbFood.SelectedIndex = -1;
				cmbDrink.SelectedIndex = -1;
				// Fechando o banco de dados
				con.Close();
			}
			catch
			{
				MessageBox.Show("Erro ao fazer conexão com o banco de dado (newOrder)");
			}
		}
		private void cmbDrink_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

        private void button3_Click(object sender, EventArgs e)
        {
            mainScrenn ns = new mainScrenn();
            ns.Show();
            this.Hide();
        }

		private void btnDelete_Click(object sender, EventArgs e)
		{
			
			try
			{
				con.Open();

				// Nome do produto selecionado 
				string Produto_selecionado = dataOrder.Rows[dataOrder.CurrentRow.Index].Cells[1].Value.ToString();
				// Numero da mesa do produto selecionado
				string Mesa = dataOrder.Rows[dataOrder.CurrentRow.Index].Cells[4].Value.ToString();
				
				double spent = 0;
				
				if (dataOrder.Rows[dataOrder.CurrentRow.Index].Cells[0].Value.ToString() == "Bebida")
				{
					// Seleciona o total gasto(spent)
					string totalgasto = @"SELECT spent FROM ClientOrders WHERE board_number = '" + Mesa + "'";
					OleDbCommand comando = new OleDbCommand(totalgasto, con);
					OleDbDataReader lerspent = comando.ExecuteReader();
					lerspent.Read();
					// Adiciona o total gasto à variavel "spent"
					spent = Convert.ToDouble(lerspent["spent"]);

					// Ler quanto foi gasto no pedido q vai ser deletado
					string totalgastodeletado = @"SELECT total FROM DrinkOrders WHERE drink = '" + Produto_selecionado + "' and board_number = '" + Mesa + "'";
					OleDbCommand cmd = new OleDbCommand(totalgastodeletado, con);
					OleDbDataReader lertotaldeletado = cmd.ExecuteReader();
					lertotaldeletado.Read();

					// Subtrai o total do podruto deletado do total gasto
					spent -= Convert.ToDouble(lertotaldeletado["total"]);

					// Exclui o pedido
					string ExluiPedido = @"DELETE FROM DrinkOrders WHERE drink = '" + Produto_selecionado + "' and board_number = '" + Mesa +"'";
					OleDbCommand comand = new OleDbCommand(ExluiPedido, con);
					comand.ExecuteNonQuery();

					// Atualiza o total gasto do client
					string atualiza_spent = @"UPDATE ClientOrders SET spent =  '" + String.Format("{0:0.00}", spent) + "' WHERE board_number = '" + Mesa + "'";
					OleDbCommand comando3 = new OleDbCommand(atualiza_spent, con);
					comando3.ExecuteNonQuery();

					// Mudando o total de todos o pedidos (tela de pedidos)
					lblTotal.Text = "Total: R$ " + String.Format("{0:f2}", spent);

				}
				// Se for comida entra aqui
				else
				{
					// Seleciona o total gasto(spent)
					string totalgasto = @"SELECT spent FROM ClientOrders WHERE board_number = '" + Mesa + "'";
					OleDbCommand comando = new OleDbCommand(totalgasto, con);
					OleDbDataReader lerspent = comando.ExecuteReader();
					lerspent.Read();

					// Adiciona o total gasto à variavel "spent"
					spent = Convert.ToDouble(lerspent["spent"]);

					// Ler quanto foi gasto no pedido q vai ser deletado
					string totalgastodeletado = @"SELECT total FROM FoodOrders WHERE food = '" + Produto_selecionado + "' and board_number = '" + Mesa + "'";
					OleDbCommand cmd = new OleDbCommand(totalgastodeletado, con);
					OleDbDataReader lertotaldeletado = cmd.ExecuteReader();
					lertotaldeletado.Read();

					// Subtrai o total do podruto deletado do total gasto
					spent -= Convert.ToDouble(lertotaldeletado["total"]);


					string ExluiPedido2 = @"DELETE FROM FoodOrders WHERE food = '" + Produto_selecionado + "' and board_number = '" + Mesa + "'";
					OleDbCommand comando4 = new OleDbCommand(ExluiPedido2, con);
					comando4.ExecuteNonQuery();

					// Atualiza o total gasto do client
					string atualiza_spent = @"UPDATE ClientOrders SET spent =  '" + String.Format("{0:0.00}", spent) + "' WHERE board_number = '" + Mesa + "'";
					OleDbCommand comando3 = new OleDbCommand(atualiza_spent, con);
					comando3.ExecuteNonQuery();

					// Mudando o total de todos o pedidos (tela de pedidos)
					lblTotal.Text = "Total: R$ " + String.Format("{0:f2}", spent);

				}

				// Remove a linha selecionada no datagridview
				dataOrder.Rows.Remove(dataOrder.CurrentRow);

				// Fechando o banco de dados
				con.Close();
			}
			catch

			{
				MessageBox.Show("Erro ao fazer conexão com o banco de dados (Delete)");
			}
			
		}
		private void btnEdit_Click(object sender, EventArgs e)
		{
			try
			{
				con.Open();

				// Nome do produto selecionado 
				string Produto_selecionado = dataOrder.Rows[dataOrder.CurrentRow.Index].Cells[1].Value.ToString();
				// Quantidade do produto selecionado
				int quantidade_do_produto = Convert.ToInt32(dataOrder.Rows[dataOrder.CurrentRow.Index].Cells[2].Value);
				// Numero da mesa do produto selecionado
				string Mesa = dataOrder.Rows[dataOrder.CurrentRow.Index].Cells[4].Value.ToString();

				double spent = 0;

				if (dataOrder.Rows[dataOrder.CurrentRow.Index].Cells[0].Value.ToString() == "Bebida")
				{
					// Coloca no combobox o produto selecionado e a quantidade
					cmbDrink.Text = Produto_selecionado;
					numDrink.Value = quantidade_do_produto;

					// Seleciona o total gasto(spent)
					string totalgasto = @"SELECT spent FROM ClientOrders WHERE board_number = '" + Mesa + "'";
					OleDbCommand comando = new OleDbCommand(totalgasto, con);
					OleDbDataReader lerspent = comando.ExecuteReader();
					lerspent.Read();

					// Adiciona o total gasto à variavel "spent"
					spent = Convert.ToDouble(lerspent["spent"]);

					// Ler quanto foi gasto no pedido q vai ser deletado
					string totalgastodeletado = @"SELECT total FROM DrinkOrders WHERE drink = '" + Produto_selecionado + "' and board_number = '" + Mesa + "'";
					OleDbCommand cmd = new OleDbCommand(totalgastodeletado, con);
					OleDbDataReader lertotaldeletado = cmd.ExecuteReader();
					lertotaldeletado.Read();

					// Subtrai o total do podruto deletado do total gasto
					spent -= Convert.ToDouble(lertotaldeletado["total"]);

					// Atualiza o total gasto do client
					string atualiza_spent = @"UPDATE ClientOrders SET spent =  '" + String.Format("{0:0.00}", spent) + "' WHERE board_number = '" + Mesa + "'";
					OleDbCommand comando3 = new OleDbCommand(atualiza_spent, con);
					comando3.ExecuteNonQuery();

					// Mudando o total de todos o pedidos (tela de pedidos)
					lblTotal.Text = "Total: R$ " + String.Format("{0:f2}", spent);

					// Deleta do banco de dados este pedido
					string ExluiPedido = @"DELETE FROM DrinkOrders WHERE drink = '" + Produto_selecionado + "'";
					OleDbCommand comand = new OleDbCommand(ExluiPedido, con);
					comand.ExecuteNonQuery();
				}
				// Caso o pedido selecionado for comida
				else
				{
					// Coloca no combobox o produto selecionado e a quantidade
					cmbFood.Text = Produto_selecionado;
					numFood.Value = quantidade_do_produto;

					// Seleciona o total gasto(spent)
					string totalgasto = @"SELECT spent FROM ClientOrders WHERE board_number = '" + Mesa + "'";
					OleDbCommand comando = new OleDbCommand(totalgasto, con);
					OleDbDataReader lerspent = comando.ExecuteReader();
					lerspent.Read();

					// Adiciona o total gasto à variavel "spent"
					spent = Convert.ToDouble(lerspent["spent"]);

					// Ler quanto foi gasto no pedido q vai ser deletado
					string totalgastodeletado = @"SELECT total FROM FoodOrders WHERE food= '" + Produto_selecionado + "' and board_number = '" + Mesa + "'";
					OleDbCommand cmd = new OleDbCommand(totalgastodeletado, con);
					OleDbDataReader lertotaldeletado = cmd.ExecuteReader();
					lertotaldeletado.Read();

					// Subtrai o total do podruto deletado do total gasto
					spent -= Convert.ToDouble(lertotaldeletado["total"]);

					// Atualiza o total gasto do client
					string atualiza_spent = @"UPDATE ClientOrders SET spent =  '" + String.Format("{0:0.00}", spent) + "' WHERE board_number = '" + Mesa + "'";
					OleDbCommand comando3 = new OleDbCommand(atualiza_spent, con);
					comando3.ExecuteNonQuery();

					// Mudando o total de todos o pedidos (tela de pedidos)
					lblTotal.Text = "Total: R$ " + String.Format("{0:f2}", spent);

					string ExluiPedido2 = @"DELETE FROM FoodOrders WHERE food = '" + Produto_selecionado + "'";
					OleDbCommand comand = new OleDbCommand(ExluiPedido2, con);
					comand.ExecuteNonQuery();
				}

				// Remove a linha selecionada no datagridview
				dataOrder.Rows.Remove(dataOrder.CurrentRow);

				// Fechando o banco de dados
				con.Close();
			}
			catch 
			{
				MessageBox.Show("Erro ao fazer conexão com o banco de dados ");
			}
		}

		private void txtTable_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
