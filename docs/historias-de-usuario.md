# Histórias de Usuário — Loja do Sr. Campos

## Épico 1 — Gestão de Clientes

### HU-01 · Cadastrar cliente
**Como** proprietário da loja,  
**Quero** cadastrar novos clientes informando nome e endereço,  
**Para** manter uma base de clientes vinculada às vendas realizadas.

**Critérios de aceite:**
- O sistema deve exigir nome e endereço para cadastro
- O nome deve ter no máximo 200 caracteres e o endereço no máximo 500
- A data de registro deve ser preenchida automaticamente pelo sistema
- A API deve retornar o objeto criado com status 201

---

### HU-02 · Consultar e editar clientes
**Como** proprietário da loja,  
**Quero** visualizar, editar e remover clientes,  
**Para** manter os dados cadastrais sempre atualizados.

**Critérios de aceite:**
- A listagem deve ser paginada (10 por página) e ordenada por nome
- A consulta por ID deve retornar 404 quando o cliente não existir
- A edição deve validar os mesmos campos do cadastro
- A remoção deve retornar status 204 em caso de sucesso

---

## Épico 2 — Gestão de Produtos

### HU-03 · Cadastrar produto com valor
**Como** proprietário da loja,  
**Quero** cadastrar produtos informando descrição e valor,  
**Para** que os produtos possam ser vinculados às vendas com o preço correto.

**Critérios de aceite:**
- O sistema deve exigir descrição (máx. 200 caracteres) e valor maior que zero
- O valor deve ser armazenado com precisão decimal (sem uso de float)
- A API deve retornar o produto criado com status 201

---

### HU-04 · Rastrear alterações de preço
**Como** proprietário da loja,  
**Quero** que o sistema registre automaticamente o histórico toda vez que o preço de um produto for alterado,  
**Para** ter rastreabilidade completa das mudanças de preço ao longo do tempo.

**Critérios de aceite:**
- Ao atualizar o valor de um produto, um registro de histórico deve ser criado automaticamente
- O histórico deve conter: valor anterior, valor novo e data da alteração
- Se o valor não mudar, nenhum registro de histórico deve ser criado
- Alterações de preço não devem impactar o valor registrado em vendas anteriores

---

## Épico 3 — Gestão de Vendas

### HU-05 · Registrar venda com múltiplos produtos
**Como** proprietário da loja,  
**Quero** registrar uma venda vinculada a um cliente com uma lista de produtos,  
**Para** ter controle detalhado de cada operação comercial.

**Critérios de aceite:**
- A venda deve estar obrigatoriamente vinculada a um cliente existente
- Deve ser possível incluir múltiplos produtos em uma única venda
- Cada item deve registrar: produto, quantidade, valor unitário (do momento da venda) e subtotal
- O valor total da venda deve ser calculado automaticamente (soma dos subtotais)
- O valor unitário do produto deve ser capturado no momento da venda e preservado no histórico
- A API deve retornar a venda criada com todos os itens e status 201

---

### HU-06 · Editar e excluir venda
**Como** proprietário da loja,  
**Quero** poder corrigir os itens de uma venda ou excluí-la,  
**Para** manter os registros comerciais precisos.

**Critérios de aceite:**
- A edição deve substituir a lista de itens e recalcular o total automaticamente
- Ao editar, os novos itens devem capturar o preço atual do produto
- A exclusão deve remover a venda e todos os seus itens
- Operações em vendas inexistentes devem retornar 404

---

### HU-07 · Consultar vendas por cliente
**Como** proprietário da loja,  
**Quero** consultar todas as vendas realizadas por um cliente específico,  
**Para** analisar o histórico de compras de cada cliente.

**Critérios de aceite:**
- A consulta deve receber o ID do cliente como parâmetro
- O retorno deve incluir todos os detalhes da venda (itens, valores, data)
- Se o cliente não tiver vendas, deve retornar uma lista vazia

---

### HU-08 · Visualizar ranking de maiores vendas
**Como** proprietário da loja,  
**Quero** consultar um ranking com as maiores vendas realizadas,  
**Para** identificar as operações de maior valor e os melhores clientes.

**Critérios de aceite:**
- O ranking deve ordenar as vendas por valor total de forma decrescente
- Cada item do ranking deve exibir o cliente, o valor total e a data da venda
- O ranking deve incluir todas as vendas registradas, sem paginação

---

## Épico 4 — Interface Web

### HU-09 · Gerenciar o negócio pelo navegador
**Como** proprietário da loja,  
**Quero** acessar uma interface web para gerenciar clientes, produtos e vendas,  
**Para** operar o negócio sem precisar de ferramentas técnicas como Postman.

**Critérios de aceite:**
- Deve haver telas de listagem, criação, edição e exclusão para clientes, produtos e vendas
- A tela de vendas deve permitir adicionar e remover itens dinamicamente
- A listagem de vendas deve exibir o ranking das maiores vendas na mesma tela
- O sistema deve exibir mensagens de erro amigáveis quando a API retornar falha
