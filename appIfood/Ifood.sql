


CREATE TABLE restaurantes (
    IdRestaurante INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Endereco VARCHAR(255),
    Telefone VARCHAR(20)
);


CREATE TABLE IF NOT EXISTS pratos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    RestauranteId INT,
    Nome VARCHAR(100) NOT NULL,
    Descricao TEXT,
    Preco DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (RestauranteId) REFERENCES restaurantes(IdRestaurante) ON DELETE CASCADE
);


CREATE TABLE  clientes (
    IdCliente INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Endereco VARCHAR(255),
    Telefone VARCHAR(20)
);


CREATE TABLE  pedidos (
    IdPedido INT AUTO_INCREMENT PRIMARY KEY,
    ClienteId INT,
    RestauranteId INT,
    DataPedido DATE NOT NULL,
    StatusPedido VARCHAR(50) NOT NULL,
    Total DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (ClienteId) REFERENCES clientes(IdCliente) ON DELETE CASCADE,
    FOREIGN KEY (RestauranteId) REFERENCES restaurantes(IdRestaurante) ON DELETE CASCADE
);


CREATE TABLE  itens_pedido (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PedidoId INT,
    PratoId INT,
    Quantidade INT NOT NULL,
    Preco DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (PedidoId) REFERENCES pedidos(IdPedido) ON DELETE CASCADE,
    FOREIGN KEY (PratoId) REFERENCES pratos(Id) ON DELETE CASCADE
);
