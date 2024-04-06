-- Endereco
CREATE TABLE Endereco (
    Id SERIAL PRIMARY KEY,
    Cidade VARCHAR(255),
    Bairro VARCHAR(255),
    Rua VARCHAR(255),
    Numero VARCHAR(50),
    Estado VARCHAR(255)
);

-- Fornecedor
CREATE TABLE Fornecedor (
    Id SERIAL PRIMARY KEY,
    Cnpj VARCHAR(14) NULL,
    Email VARCHAR(255) NOT NULL,
    Telefone VARCHAR(20) NOT NULL,
    EnderecoId INTEGER REFERENCES Endereco(Id)
);

-- Compra
CREATE TABLE Compra (
    Id SERIAL PRIMARY KEY,
    Nfe VARCHAR(255) NOT NULL,
    Data DATE NOT NULL,
    ValorTotal NUMERIC(10, 2) NOT NULL,
    FornecedorId INTEGER REFERENCES Fornecedor(Id)
);

-- Produto
CREATE TABLE Produto (
    Id SERIAL PRIMARY KEY,
    CatalogoProdutoId INTEGER REFERENCES CatalogoProduto(Id),
    DataFabricacao DATE NOT NULL,
    DataValidade DATE NULL,
    DataEntrega DATE NULL,
    ValorUnitarioCompra NUMERIC(10, 2) NOT NULL,
    CompraId INTEGER REFERENCES Compra(Id),
    VendaId INTEGER REFERENCES Venda(Id) NULL
);

-- Venda
CREATE TABLE Venda (
    Id SERIAL PRIMARY KEY,
    Nfe VARCHAR(255) NOT NULL,
    Data DATE NOT NULL,
    ValorTotal NUMERIC(10, 2) NOT NULL,
    ClienteId INTEGER REFERENCES Cliente(Id),
    FuncionarioId INTEGER REFERENCES Funcionario(Id)
);

-- Cliente
CREATE TABLE Cliente (
    Id SERIAL PRIMARY KEY,
    Cpf VARCHAR(11) NULL,
    Nome VARCHAR(255) NOT NULL,
    UltimoNome VARCHAR(255) NOT NULL,
    Telefone VARCHAR(20) NULL,
    Email VARCHAR(255) NULL,
    EnderecoId INTEGER REFERENCES Endereco(Id)
);

-- Funcionario
CREATE TABLE Funcionario (
    Id SERIAL PRIMARY KEY,
    Cpf VARCHAR(11) NOT NULL,
    Nome VARCHAR(255) NOT NULL,
    UltimoNome VARCHAR(255) NOT NULL,
    Salario NUMERIC(10, 2) NOT NULL,
    Email VARCHAR(255) NULL,
    EnderecoId INTEGER REFERENCES Endereco(Id)
);

-- CatalogoProduto
CREATE TABLE CatalogoProduto (
    Id SERIAL PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Descricao TEXT NULL,
    Preco NUMERIC(10, 2) NOT NULL,
    CategoriaId INTEGER REFERENCES Categoria(Id)
);

-- Categoria
CREATE TABLE Categoria (
    Id SERIAL PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL
);