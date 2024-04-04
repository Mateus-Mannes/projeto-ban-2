# projeto-ban-2
Projeto banco de dados 2

Tema: Sistema para gerenciar uma loja de varejo.

# Modelo conceitual:

![image](https://github.com/Mateus-Mannes/projeto-ban-2/assets/64140337/a65497fa-2fc8-4bfe-90b6-4b8f72051e9f)

# Modelo lógico:

Endereco (#Id, Cidade, Bairro, Rua, Numero, Estado)

Fornecedor (#Id, [Cnpj], Email, Telefone, &EnderecoId)

Compra (Id, Nfe, Data, ValorTotal, &FornecedorId)

Produto (#Id, &CatalogoProdutoId, DataFabricacao, [DataValidade], DataEntrega, ValorUnitarioCompra, &CompraId, [&VendaId])

Venda (#Id, Nfe, Data, ValorTotal, &ClienteId, &FuncionarioId)

Cliente (#Id, [Cpf], Nome, UltimoNome, Telefone, Email, &EnderecoId)

Funcionario (#Id, Cpf, Nome, UltimoNome, Salario, Email, &EnderecoId)

CatalogoProduto (#Id, Nome, Descricao, Preco, &CategoriaId)

Categoria (#Id, Nome)

