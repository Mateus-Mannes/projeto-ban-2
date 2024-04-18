# projeto-ban-2
Projeto banco de dados 2

Tema: Sistema para gerenciar uma loja de varejo.

# Modelo conceitual:

![image](https://github.com/Mateus-Mannes/projeto-ban-2/assets/64140337/cfcf8dc5-c655-4315-bea6-b1d73cc4f6d1)


# Modelo lógico:

Endereco (#Id, Cidade, Bairro, Rua, Numero, Estado)

Fornecedor (#Id, [Cnpj], Email, Telefone, &EnderecoId)

Compra (#Id, Nfe, Data, &FornecedorId)

Produto (#Id, &CatalogoProdutoId, DataFabricacao, [DataValidade], DataEntrega, ValorUnitarioCompra, &CompraId, [&VendaId])

Venda (#Id, Nfe, Data, Valor, &ClienteId, &FuncionarioId)

Cliente (#Id, [Cpf], Nome, UltimoNome, Telefone, Email, &EnderecoId)

Funcionario (#Id, Cpf, Nome, UltimoNome, Salario, Email, &EnderecoId)

CatalogoProduto (#Id, Nome, Descricao, Preco, &CategoriaId)

Categoria (#Id, Nome)

## Tabelas e Descrições

### Tabela: Endereco

| Nome do Campo | Descrição                                      | Tipo de Dado | Tamanho | Restrições do Domínio            |
|---------------|------------------------------------------------|--------------|---------|----------------------------------|
| #Id           | Identificador único do endereço (Primary Key)  | Inteiro      |         | Chave Primária, Auto Incremento  |
| Cidade        | Nome da cidade                                 | Varchar      | 255     |                                  |
| Bairro        | Nome do bairro                                 | Varchar      | 255     |                                  |
| Rua           | Nome da rua                                    | Varchar      | 255     |                                  |
| Numero        | Número do endereço                             | Inteiro      | 10      |                                  |
| Estado        | Estado do endereço                             | Varchar      | 2       |                                  |

### Tabela: Fornecedor

| Nome do Campo | Descrição                                      | Tipo de Dado | Tamanho | Restrições do Domínio            |
|---------------|------------------------------------------------|--------------|---------|----------------------------------|
| #Id           | Identificador único do fornecedor (Primary Key)| Inteiro      |         | Chave Primária, Auto Incremento  |
| Cnpj          | Número de CNPJ do fornecedor (opcional)        | Varchar      | 18      | Único (se fornecido)             |
| Email         | Endereço de e-mail do fornecedor               | Varchar      | 255     |                                  |
| Telefone      | Número de telefone do fornecedor               | Varchar      | 20      |                                  |
| &EnderecoId   | Chave estrangeira referenciando o endereço     | Inteiro      |         | Chave Estrangeira para Endereco  |

### Tabela: Compra

| Nome do Campo | Descrição                                      | Tipo de Dado | Tamanho | Restrições do Domínio            |
|---------------|------------------------------------------------|--------------|---------|----------------------------------|
| #Id           | Identificador único da compra (Primary Key)    | Inteiro      |         | Chave Primária, Auto Incremento  |
| Nfe           | Número da Nota Fiscal Eletrônica da compra     | Varchar      | 50      |                                  |
| Data          | Data da compra                                 | Date         |         |                                  |
| &FornecedorId | Chave estrangeira referenciando o fornecedor   | Inteiro      |         | Chave Estrangeira para Fornecedor|

### Tabela: Produto

| Nome do Campo       | Descrição                                      | Tipo de Dado | Tamanho | Restrições do Domínio            |
|---------------------|------------------------------------------------|--------------|---------|----------------------------------|
| #Id                 | Identificador único do produto (Primary Key)    | Inteiro      |         | Chave Primária, Auto Incremento |
| &CatalogoProdutoId  | Chave estrangeira referenciando o catálogo      | Inteiro      |         | Chave Estrangeira para CatalogoProduto |
| DataFabricacao      | Data de fabricação do produto                   | Date         |         |                                  |
| [DataValidade]      | Data de validade do produto (opcional)          | Date         |         |                                  |
| DataEntrega         | Data de entrega do produto                      | Date         |         |                                  |
| ValorUnitarioCompra | Valor unitário de compra do produto             | Decimal      | (10,2)  |                                  |
| &CompraId           | Chave estrangeira referenciando a compra        | Inteiro      |         | Chave Estrangeira para Compra    |
| [&VendaId]          | Chave estrangeira opcional referenciando a venda| Inteiro      |         | Chave Estrangeira para Venda (opcional) |

### Tabela: Venda

| Nome do Campo   | Descrição                                      | Tipo de Dado | Tamanho | Restrições do Domínio            |
|-----------------|------------------------------------------------|--------------|---------|----------------------------------|
| #Id             | Identificador único da venda (Primary Key)     | Inteiro      |         | Chave Primária, Auto Incremento  |
| Nfe             | Número da Nota Fiscal Eletrônica da venda      | Varchar      | 50      |                                  |
| Data            | Data da venda                                  | Date         |         |                                  |
| Valor           | Valor da venda                                 | Decimal      | (10,2)  |                                  |
| &ClienteId      | Chave estrangeira referenciando o cliente      | Inteiro      |         | Chave Estrangeira para Cliente   |
| &FuncionarioId  | Chave estrangeira referenciando o funcionário  | Inteiro      |         | Chave Estrangeira para Funcionario|

### Tabela: Cliente

| Nome do Campo | Descrição                                      | Tipo de Dado | Tamanho | Restrições do Domínio            |
|---------------|------------------------------------------------|--------------|---------|----------------------------------|
| #Id           | Identificador único do cliente (Primary Key)   | Inteiro      |         | Chave Primária, Auto Incremento  |
| Cpf           | Número de CPF do cliente (opcional)            | Varchar      | 14      | Único (se fornecido)             |
| Nome          | Nome do cliente                                | Varchar      | 255     |                                  |
| UltimoNome    | Último nome do cliente                         | Varchar      | 255     |                                  |
| Telefone      | Número de telefone do cliente                  | Varchar      | 20      |                                  |
| Email         | Endereço de e-mail do cliente                  | Varchar      | 255     |                                  |
| &EnderecoId   | Chave estrangeira referenciando o endereço     | Inteiro      |         | Chave Estrangeira para Endereco  |

### Tabela: Funcionario

| Nome do Campo | Descrição                                      | Tipo de Dado | Tamanho | Restrições do Domínio            |
|---------------|------------------------------------------------|--------------|---------|----------------------------------|
| #Id           | Identificador único do funcionário (Primary Key)| Inteiro      |         | Chave Primária, Auto Incremento |
| Cpf           | Número de CPF do funcionário                    | Varchar      | 14      | Único                           |
| Nome          | Nome do funcionário                             | Varchar      | 255     |                                 |
| UltimoNome    | Último nome do funcionário                      | Varchar      | 255     |                                 |
| Salario       | Salário do funcionário                          | Decimal      |         |                                 |
| Email         | Endereço de e-mail do funcionário               | Varchar      | 255     |                                 |
| &EnderecoId   | Chave estrangeira referenciando o endereço      | Inteiro      |         | Chave Estrangeira para Endereco |

### Tabela: CatalogoProduto

| Nome do Campo   | Descrição                                      | Tipo de Dado | Tamanho | Restrições do Domínio            |
|-----------------|------------------------------------------------|--------------|---------|----------------------------------|
| #Id             | Identificador único do catálogo (Primary Key)  | Inteiro      |         | Chave Primária, Auto Incremento  |
| Nome            | Nome do produto                                | Varchar      | 255     |                                  |
| Descricao       | Descrição do produto                           | Varchar      | 255     |                                  |
| Preco           | Preço do produto                               | Decimal      | (10,2)  |                                  |
| &CategoriaId    | Chave estrangeira referenciando a categoria    | Inteiro      |         | Chave Estrangeira para Categoria |

### Tabela: Categoria

| Nome do Campo | Descrição                                      | Tipo de Dado | Tamanho | Restrições do Domínio            |
|---------------|------------------------------------------------|--------------|---------|----------------------------------|
| #Id           | Identificador único da categoria (Primary Key) | Inteiro      |         | Chave Primária, Auto Incremento  |
| Nome          | Nome da categoria                              | Varchar      | 255     |                                  |

## Acessar postgres:
```bash
psql -h db -U postgres -d postgres
```
