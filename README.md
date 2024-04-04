# projeto-ban-2
Projeto banco de dados 2

Tema: Sistema para gerenciar uma loja de varejo.

Modelo conceitual:


![Captura de tela 2024-04-01 204301](https://github.com/Mateus-Mannes/projeto-ban-2/assets/64140337/1766145d-0531-4372-9fb5-f62023174cec)

Endereco (#Id, Cidade, Bairro, Rua, Numero, Estado)

Fornecedor (#Id, [Cnpj], Email, Telefone, &EnderecoId)

Compra (Id, Nfe, Data, ValorTotal, &FornecedorId)

Produto (#Id, &CatalogoProdutoId, DataFabricacao, [DataValidade], DataEntrega, ValorUnitarioCompra, &CompraId, [&VendaId])

Venda (#Id, Nfe, Data, ValorTotal, &ClienteId, &FuncionarioId)

Cliente (#Id, [Cpf], Nome, UltimoNome, Telefone, Email, &EnderecoId)

Funcionario (#Id, Cpf, Nome, UltimoNome, Salario, Email, &EnderecoId)

CatalogoProduto (#Id, Nome, Descricao, Preco, &CategoriaId)

Categoria (#Id, Nome)

