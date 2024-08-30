using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace appIfood
{
    public class Program
    {
        private static string connectionString = "Server=localhost;Database=db_Ifood;Uid=root;Pwd=1234567889;SslMode=none;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1 - Cadastrar Restaurante");
                Console.WriteLine("2 - Cadastrar Prato");
                Console.WriteLine("3 - Cadastrar Cliente");
                Console.WriteLine("4 - Realizar Pedido");
                Console.WriteLine("5 - Listar Pedidos");
                Console.WriteLine("6 - Sair");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                try
                {
                    switch (opcao)
                    {
                        case "1":
                            CadastrarRestaurante();
                            break;
                        case "2":
                            CadastrarPrato();
                            break;
                        case "3":
                            CadastrarCliente();
                            break;
                        case "4":
                            RealizarPedido();
                            break;
                        case "5":
                            ListarPedidos();
                            break;
                        case "6":
                            return;
                        default:
                            Console.WriteLine("Opção inválida");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                }
            }
        }

        static void CadastrarRestaurante()
        {
            Console.Write("Informe o Nome do Restaurante: ");
            string nome = Console.ReadLine();

            Console.Write("Informe o Endereço: ");
            string endereco = Console.ReadLine();

            Console.Write("Informe o Telefone: ");
            string telefone = Console.ReadLine();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO restaurantes (Nome, Endereco, Telefone) VALUES (@Nome, @Endereco, @Telefone)";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Nome", nome);
                        cmd.Parameters.AddWithValue("@Endereco", endereco);
                        cmd.Parameters.AddWithValue("@Telefone", telefone);
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Restaurante cadastrado com sucesso");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar restaurante: {ex.Message}");
            }
        }

        static void CadastrarPrato()
        {
            Console.Write("Informe o Id do Restaurante: ");
            int restauranteId;
            while (!int.TryParse(Console.ReadLine(), out restauranteId))
            {
                Console.Write("ID inválido. Informe o Id do Restaurante: ");
            }

            Console.Write("Informe o Nome do Prato: ");
            string nome = Console.ReadLine();

            Console.Write("Informe a Descrição do Prato: ");
            string descricao = Console.ReadLine();

            Console.Write("Informe o Preço do Prato: ");
            decimal preco;
            while (!decimal.TryParse(Console.ReadLine(), out preco) || preco <= 0)
            {
                Console.Write("Preço inválido. Informe o Preço do Prato: ");
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    var checkQuery = "SELECT COUNT(*) FROM pratos WHERE Nome = @Nome AND RestauranteId = @RestauranteId";
                    using (var checkCmd = new MySqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@Nome", nome);
                        checkCmd.Parameters.AddWithValue("@RestauranteId", restauranteId);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            Console.WriteLine("Prato já existe no restaurante.");
                            return;
                        }
                    }

                    var query = "INSERT INTO pratos (RestauranteId, Nome, Descricao, Preco) VALUES (@RestauranteId, @Nome, @Descricao, @Preco)";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@RestauranteId", restauranteId);
                        cmd.Parameters.AddWithValue("@Nome", nome);
                        cmd.Parameters.AddWithValue("@Descricao", descricao);
                        cmd.Parameters.AddWithValue("@Preco", preco);
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Prato cadastrado com sucesso");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar prato: {ex.Message}");
            }
        }

        static void CadastrarCliente()
        {
            Console.Write("Informe o Nome do Cliente: ");
            string nome = Console.ReadLine();

            Console.Write("Informe o Endereço: ");
            string endereco = Console.ReadLine();

            Console.Write("Informe o Telefone: ");
            string telefone = Console.ReadLine();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO clientes (Nome, Endereco, Telefone) VALUES (@Nome, @Endereco, @Telefone)";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Nome", nome);
                        cmd.Parameters.AddWithValue("@Endereco", endereco);
                        cmd.Parameters.AddWithValue("@Telefone", telefone);
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Cliente cadastrado com sucesso");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar cliente: {ex.Message}");
            }
        }

        static void RealizarPedido()
        {
            Console.Write("Informe o Id do Cliente: ");
            int clienteId;
            while (!int.TryParse(Console.ReadLine(), out clienteId))
            {
                Console.Write("ID inválido. Informe o Id do Cliente: ");
            }

            Console.Write("Informe o Id do Restaurante: ");
            int restauranteId;
            while (!int.TryParse(Console.ReadLine(), out restauranteId))
            {
                Console.Write("ID inválido. Informe o Id do Restaurante: ");
            }

            Console.Write("Informe o Status do Pedido: ");
            string statusPedido = Console.ReadLine();

            decimal total = 0;
            var itensPedido = new List<(int pratoId, int quantidade, decimal preco)>();

            while (true)
            {
                Console.Write("Informe o Id do Prato: ");
                int pratoId;
                while (!int.TryParse(Console.ReadLine(), out pratoId))
                {
                    Console.Write("ID inválido. Informe o Id do Prato: ");
                }

                Console.Write("Informe a Quantidade: ");
                int quantidade;
                while (!int.TryParse(Console.ReadLine(), out quantidade) || quantidade <= 0)
                {
                    Console.Write("Quantidade inválida. Informe a Quantidade: ");
                }

                try
                {
                    using (var connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        var queryPreco = "SELECT Preco FROM pratos WHERE Id = @PratoId";
                        using (var cmdPreco = new MySqlCommand(queryPreco, connection))
                        {
                            cmdPreco.Parameters.AddWithValue("@PratoId", pratoId);
                            var precoObj = cmdPreco.ExecuteScalar();
                            if (precoObj == null)
                            {
                                Console.WriteLine($"Prato com ID {pratoId} não encontrado.");
                                return;
                            }

                            decimal preco = Convert.ToDecimal(precoObj);
                            itensPedido.Add((pratoId, quantidade, preco));
                            total += preco * quantidade;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao consultar o preço do prato: {ex.Message}");
                    return;
                }

                Console.Write("Deseja adicionar mais itens? (s/n): ");
                if (Console.ReadLine().ToLower() != "s")
                    break;
            }

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var queryPedido = "INSERT INTO pedidos (ClienteId, RestauranteId, StatusPedido, Total, DataPedido) VALUES (@ClienteId, @RestauranteId, @StatusPedido, @Total, @DataPedido)";
                            int pedidoId;
                            using (var cmdPedido = new MySqlCommand(queryPedido, connection, transaction))
                            {
                                cmdPedido.Parameters.AddWithValue("@ClienteId", clienteId);
                                cmdPedido.Parameters.AddWithValue("@RestauranteId", restauranteId);
                                cmdPedido.Parameters.AddWithValue("@StatusPedido", statusPedido);
                                cmdPedido.Parameters.AddWithValue("@Total", total);
                                cmdPedido.Parameters.AddWithValue("@DataPedido", DateTime.Now); // Adicionando DataPedido
                                cmdPedido.ExecuteNonQuery();
                                pedidoId = Convert.ToInt32(cmdPedido.LastInsertedId);
                            }

                            var queryItem = "INSERT INTO itens_pedido (PedidoId, PratoId, Quantidade, Preco) VALUES (@PedidoId, @PratoId, @Quantidade, @Preco)";
                            foreach (var item in itensPedido)
                            {
                                using (var cmdItem = new MySqlCommand(queryItem, connection, transaction))
                                {
                                    cmdItem.Parameters.AddWithValue("@PedidoId", pedidoId);
                                    cmdItem.Parameters.AddWithValue("@PratoId", item.pratoId);
                                    cmdItem.Parameters.AddWithValue("@Quantidade", item.quantidade);
                                    cmdItem.Parameters.AddWithValue("@Preco", item.preco);
                                    cmdItem.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"Erro ao realizar o pedido: {ex.Message}");
                        }
                    }
                }
                Console.WriteLine("Pedido realizado com sucesso");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao abrir a conexão ou iniciar a transação: {ex.Message}");
            }
        }

        static void ListarPedidos()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT p.IdPedido, c.Nome AS Cliente, r.Nome AS Restaurante, p.DataPedido, " +
                    "p.StatusPedido, p.Total " + "FROM pedidos p " + "JOIN clientes c ON p.ClienteId = c.IdCliente "
                    + "JOIN restaurantes r ON p.RestauranteId = r.IdRestaurante";
                var command = new MySqlCommand(query, connection);
                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                              Console.WriteLine($"ID: {reader["IdPedido"]}");
                              Console.WriteLine($"Cliente: {reader["Cliente"]}");
                              Console.WriteLine($"Restaurante: {reader["Restaurante"]}");
                              Console.WriteLine($"Data do Pedido: {reader["DataPedido"]}");
                              Console.WriteLine($"Status do Pedido: {reader["StatusPedido"]}");
                              Console.WriteLine($"Total: {reader["Total"]}");
                              Console.WriteLine(new string('-', 40));
        }
      }
    } catch (Exception ex) {
      Console.WriteLine("Erro ao listar pedidos: " + ex.Message);
    }
  }
}
}
}
            
            