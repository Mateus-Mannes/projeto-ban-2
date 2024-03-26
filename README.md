# projeto-ban-2
Projeto banco de dados 2

Tema: Sistema para gerenciar uma loja de varejo.

Entidades:
- Produto
- Categoria
- Fornecedor
- Cliente
- Funcionário
- Venda
- ItemVenda
- Compra
- ItemCompra

Relacionamentos:
Produto está associado a Categoria e Fornecedor.

Um Produto pertence a uma Categoria.
Um Produto pode ser fornecido por um Fornecedor.
Venda envolve Cliente, Funcionário, e ItemVenda.

Uma Venda é feita para um Cliente por um Funcionário.
Uma Venda contém um ou mais ItemVenda, e cada ItemVenda está relacionado a um Produto.
Compra envolve Fornecedor, Funcionário, e ItemCompra.

Uma Compra é realizada de um Fornecedor por um Funcionário.
Uma Compra contém um ou mais ItemCompra, e cada ItemCompra está relacionado a um Produto.
