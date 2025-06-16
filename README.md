# 📋 API de Vendas - Sistema de Vendas com Desconto Automático

## 🎯 Visão Geral

Esta documentação explica o funcionamento da API de Vendas, com foco especial na **criação de vendas** e no **sistema de desconto automático** aplicado durante o fechamento das vendas.

## 📖 Índice

- [🚀 Como Executar a API](#-como-executar-a-api)
- [⚠️ Pré-requisitos para Vendas](#️-pré-requisitos-importantes)
- [🗂️ Cadastros Básicos](#️-cadastros-básicos-necessários)
- [🛒 Criação de Vendas](#-1-criação-de-nova-venda)
- [📦 Adição de Itens](#-2-adição-de-itens-à-venda)
- [🔒 Sistema de Desconto Automático](#-3-fechamento-da-venda-sistema-de-desconto-automático)
- [🎯 Exemplo Completo](#-fluxo-completo---exemplo-prático)
- [📋 Checklist](#-checklist-para-criar-sua-primeira-venda)

---

## 🚀 Como Executar a API

### 📋 Pré-requisitos
- **Docker** e **Docker Compose** instalados
- **Git** (para clonar o repositório)

### ⚡ Execução Rápida

1. **Clone o repositório:**
```bash
git clone <url-do-repositorio>
cd backend
```

2. **Execute com Docker Compose:**
```bash
docker-compose up -d
```

3. **Acesse a API:**
- **Swagger UI**: http://localhost:8080/swagger
- **API Base URL**: http://localhost:8080/api

### 🔧 O que acontece ao executar

O Docker Compose irá subir automaticamente:
- **🐘 PostgreSQL** (Banco de dados principal) - porta 5432
- **🍃 MongoDB** (Banco NoSQL) - porta 27017  
- **🔴 Redis** (Cache) - porta 6379
- **🌐 API .NET** (Aplicação principal) - porta 8080

### 📊 Migrations Automáticas

As **migrations do banco de dados são executadas automaticamente** quando a API inicia. Não é necessário rodar comandos manuais.

### 🔐 Credenciais dos Bancos

**PostgreSQL:**
- Host: localhost:5432
- Database: developer_evaluation
- Username: developer
- Password: ev@luAt10n

**MongoDB:**
- Host: localhost:27017
- Username: developer
- Password: ev@luAt10n

**Redis:**
- Host: localhost:6379
- Password: ev@luAt10n

### 🛠️ Comandos Úteis

```bash
# Parar todos os serviços
docker-compose down

# Ver logs da API
docker-compose logs -f ambev.developerevaluation.webapi

# Reconstruir e executar
docker-compose up --build

# Executar apenas o banco de dados
docker-compose up -d ambev.developerevaluation.database
```

### 🏥 Health Check

A API possui health checks disponíveis:
- **Health Check**: http://localhost:8080/health

### 🎉 Primeiros Passos após Executar

1. **Acesse o Swagger**: http://localhost:8080/swagger
2. **Crie os cadastros básicos** (veja seção de pré-requisitos abaixo)
3. **Teste o fluxo de vendas** (veja seção de exemplos práticos)

### 🛠️ Tecnologias Utilizadas

- **🔷 .NET 8** - Framework principal
- **🐘 PostgreSQL** - Banco de dados relacional
- **🍃 MongoDB** - Banco NoSQL
- **🔴 Redis** - Cache
- **📊 Entity Framework Core** - ORM
- **🏗️ MediatR** - Padrão CQRS
- **✅ FluentValidation** - Validações
- **🗺️ AutoMapper** - Mapeamentos
- **📝 Serilog** - Logging
- **🧪 xUnit** - Testes unitários
- **🐳 Docker** - Containerização

---

## ⚠️ PRÉ-REQUISITOS IMPORTANTES

**🚨 ANTES DE CRIAR VENDAS, você precisa ter os seguintes cadastros básicos no sistema:**

### 1. 👥 Clientes (Customers)
### 2. 🏢 Filiais (Branches)  
### 3. 📦 Produtos (Products)

**Sem esses cadastros, não será possível criar vendas!**

---

## 🗂️ Cadastros Básicos Necessários

### 👥 1. Cadastro de Clientes

#### Criar Cliente
```http
POST /api/customers
```

**Request Body:**
```json
{
  "name": "João Silva",
  "email": "joao.silva@email.com",
  "phone": "+55 11 99999-9999"
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "message": "Customer created successfully",
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "name": "João Silva",
    "email": "joao.silva@email.com",
    "phone": "+55 11 99999-9999"
  }
}
```

#### Listar Clientes
```http
GET /api/customers?page=1&size=10&name=João&email=joao
```

---

### 🏢 2. Cadastro de Filiais

#### Criar Filial
```http
POST /api/branches
```

**Request Body:**
```json
{
  "name": "Filial Centro - São Paulo"
}
```

**Response (201 Created):**
```json
{
  "id": "456e7890-e89b-12d3-a456-426614174001",
  "name": "Filial Centro - São Paulo"
}
```

#### Listar Filiais
```http
GET /api/branches?page=1&size=10&name=Centro
```

---

### 📦 3. Cadastro de Produtos

#### Criar Produto
```http
POST /api/products
```

**Request Body:**
```json
{
  "name": "Notebook Dell Inspiron 15",
  "price": 2500.00
}
```

**Response (201 Created):**
```json
{
  "id": "987e6543-e89b-12d3-a456-426614174003",
  "name": "Notebook Dell Inspiron 15",
  "price": 2500.00,
  "isActive": true
}
```

#### Listar Produtos
```http
GET /api/products?page=1&size=10&name=Notebook
```

---

## ✅ Verificação dos Pré-requisitos

Antes de criar uma venda, certifique-se de que você tem:

1. **✅ Pelo menos 1 cliente cadastrado** - Use `GET /api/customers` para verificar
2. **✅ Pelo menos 1 filial cadastrada** - Use `GET /api/branches` para verificar  
3. **✅ Pelo menos 1 produto cadastrado** - Use `GET /api/products` para verificar

### 🔍 Exemplo de Verificação Rápida

```bash
# Verificar se há clientes
curl GET /api/customers?page=1&size=1

# Verificar se há filiais  
curl GET /api/branches?page=1&size=1

# Verificar se há produtos
curl GET /api/products?page=1&size=1
```

Se qualquer uma dessas consultas retornar uma lista vazia, você precisa criar os respectivos cadastros antes de prosseguir com as vendas.

---

## 🔄 Fluxo Principal de Vendas

### 1️⃣ Criação de uma Nova Venda
### 2️⃣ Adição de Itens à Venda  
### 3️⃣ Fechamento da Venda (com Cálculo Automático de Desconto)

---

## 📊 Status das Vendas

A API trabalha com os seguintes status de venda:

| Status | Valor | Descrição | Permite Modificações |
|--------|-------|-----------|---------------------|
| `Pending` | 0 | Venda em aberto, permite adicionar/remover itens | ✅ Sim |
| `Closed` | 1 | Venda fechada, descontos automáticos aplicados | ❌ Não |
| `Paid` | 2 | Venda paga | ❌ Não |
| `Cancelled` | 3 | Venda cancelada | ❌ Não |

---

## 🛒 1. Criação de Nova Venda

### Endpoint
```http
POST /api/sales
```

### Request Body
```json
{
  "saleNumber": "VENDA-2024-001",
  "saleDate": "2024-01-15T10:30:00Z",
  "customerId": "123e4567-e89b-12d3-a456-426614174000",
  "branchId": "456e7890-e89b-12d3-a456-426614174001",
  "items": []
}
```

### Campos Obrigatórios
- **saleNumber**: Número único da venda
- **saleDate**: Data e hora da venda
- **customerId**: ID do cliente (deve existir no sistema)
- **branchId**: ID da filial (deve existir no sistema)
- **items**: Lista de itens (pode ser vazia inicialmente)

### Response (201 Created)
```json
{
  "id": "789e0123-e89b-12d3-a456-426614174002",
  "saleNumber": "VENDA-2024-001",
  "saleDate": "2024-01-15T10:30:00Z",
  "customerId": "123e4567-e89b-12d3-a456-426614174000",
  "branchId": "456e7890-e89b-12d3-a456-426614174001",
  "totalAmount": 0.00,
  "status": "Pending",
  "createdAt": "2024-01-15T10:30:00Z"
}
```

### ⚠️ Regras de Negócio
- O número da venda deve ser único
- Cliente e filial devem existir no sistema
- A venda é criada com status `Pending` por padrão
- Valor total inicial é 0,00

---

## 📦 2. Adição de Itens à Venda

### Endpoint
```http
POST /api/sales/{saleId}/items
```

### Request Body
```json
{
  "productId": "987e6543-e89b-12d3-a456-426614174003",
  "quantity": 5
}
```

### Campos Obrigatórios
- **productId**: ID do produto (deve existir e estar ativo)
- **quantity**: Quantidade do produto (deve ser > 0)

### Response (201 Created)
```json
{
  "id": "abc1234d-e89b-12d3-a456-426614174004",
  "saleId": "789e0123-e89b-12d3-a456-426614174002",
  "productId": "987e6543-e89b-12d3-a456-426614174003",
  "quantity": 5,
  "unitPrice": 100.00,
  "discount": 0.00,
  "totalAmount": 500.00
}
```

### ⚠️ Regras de Negócio
- Só é possível adicionar itens em vendas com status `Pending`
- O produto deve existir e estar ativo
- O preço unitário é obtido automaticamente do cadastro do produto
- Desconto inicial é sempre 0% (descontos são aplicados no fechamento)
- Máximo de 20 itens idênticos por venda

---

## 🔒 3. Fechamento da Venda (Sistema de Desconto Automático)

### Endpoint
```http
PATCH /api/sales/{saleId}/status
```

### Request Body
```json
{
  "status": "Closed"
}
```

### ⭐ **SISTEMA DE DESCONTO AUTOMÁTICO**

Quando uma venda é fechada (status alterado para `Closed`), o sistema aplica **automaticamente** descontos baseados na quantidade de itens idênticos:

#### 📊 Tabela de Descontos Automáticos

| Quantidade de Itens Idênticos | Desconto Aplicado | Observação |
|-------------------------------|-------------------|------------|
| 1 - 3 itens | 0% | Sem desconto |
| 4 - 9 itens | 10% | Desconto automático de 10% |
| 10 - 20 itens | 20% | Desconto automático de 20% |
| Mais de 20 itens | ❌ ERRO | Venda rejeitada |

#### 🔍 Como o Sistema Calcula

1. **Agrupamento**: Os itens são agrupados por `productId`
2. **Contagem**: Para cada produto, conta-se a quantidade total
3. **Validação**: Se algum produto tiver mais de 20 itens, a operação falha
4. **Aplicação**: Aplica-se o desconto correspondente a todos os itens do produto
5. **Recálculo**: O valor total da venda é recalculado

#### 💡 Exemplo Prático

**Cenário**: Venda com os seguintes itens:
- Produto A: 6 unidades × R$ 100,00 = R$ 600,00
- Produto B: 15 unidades × R$ 50,00 = R$ 750,00
- Produto C: 2 unidades × R$ 200,00 = R$ 400,00

**Ao fechar a venda:**
- Produto A (6 itens): Desconto de 10% → R$ 540,00
- Produto B (15 itens): Desconto de 20% → R$ 600,00  
- Produto C (2 itens): Sem desconto → R$ 400,00
- **Total da Venda**: R$ 1.540,00

### Response (204 No Content)
A operação retorna status 204 quando bem-sucedida.

### ⚠️ Regras de Negócio do Fechamento

1. **Venda deve ter itens**: Não é possível fechar uma venda vazia
2. **Status deve ser Pending**: Só vendas pendentes podem ser fechadas
3. **Limite de itens**: Máximo 20 itens idênticos por produto
4. **Cálculo automático**: Descontos são aplicados automaticamente
5. **Irreversível**: Uma vez fechada, a venda não pode ser modificada

### 🚫 Erros Possíveis

#### Erros de Pré-requisitos
```json
// Erro: Cliente não encontrado
{
  "error": "Customer with ID {customerId} not found",
  "statusCode": 404
}

// Erro: Filial não encontrada
{
  "error": "Branch with ID {branchId} not found", 
  "statusCode": 404
}

// Erro: Produto não encontrado
{
  "error": "Product with ID {productId} not found",
  "statusCode": 404
}

// Erro: Produto inativo
{
  "error": "Product with ID {productId} is not active",
  "statusCode": 400
}
```

#### Erros de Desconto
```json
// Erro: Mais de 20 itens idênticos
{
  "error": "Cannot sell more than 20 identical items. Product {productId} has {quantity} items.",
  "statusCode": 400
}

// Erro: Venda sem itens
{
  "error": "Sale with ID {saleId} has no items",
  "statusCode": 400
}

// Erro: Status inválido
{
  "error": "Sale with ID {saleId} has not pending status.",
  "statusCode": 400
}
```

---

## 🔍 4. Consulta de Vendas

### Buscar Venda por ID
```http
GET /api/sales/{saleId}
```

### Listar Vendas (com Paginação)
```http
GET /api/sales?page=1&size=10&search=VENDA-2024
```

### Buscar Itens de uma Venda
```http
GET /api/sales/{saleId}/items?page=1&size=10
```

---

## 🎯 Fluxo Completo - Exemplo Prático

### 0. Preparar Cadastros Básicos (PRÉ-REQUISITO)

```bash
# 1. Criar um cliente
curl -X POST /api/customers \
  -H "Content-Type: application/json" \
  -d '{
    "name": "João Silva",
    "email": "joao@email.com",
    "phone": "+55 11 99999-9999"
  }'
# Resposta: {"data": {"id": "123e4567-e89b-12d3-a456-426614174000", ...}}

# 2. Criar uma filial
curl -X POST /api/branches \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Filial Centro - São Paulo"
  }'
# Resposta: {"id": "456e7890-e89b-12d3-a456-426614174001", ...}

# 3. Criar produtos
curl -X POST /api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Notebook Dell",
    "price": 2500.00
  }'
# Resposta: {"id": "prod-a-id", ...}

curl -X POST /api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Mouse Wireless",
    "price": 75.00
  }'
# Resposta: {"id": "prod-b-id", ...}
```

### 1. Criar uma Nova Venda
```bash
curl -X POST /api/sales \
  -H "Content-Type: application/json" \
  -d '{
    "saleNumber": "VENDA-2024-001",
    "saleDate": "2024-01-15T10:30:00Z",
    "customerId": "123e4567-e89b-12d3-a456-426614174000",
    "branchId": "456e7890-e89b-12d3-a456-426614174001"
  }'
```

### 2. Adicionar Itens
```bash
# Adicionar 8 unidades do Produto A
curl -X POST /api/sales/{saleId}/items \
  -H "Content-Type: application/json" \
  -d '{
    "productId": "prod-a-id",
    "quantity": 8
  }'

# Adicionar 12 unidades do Produto B
curl -X POST /api/sales/{saleId}/items \
  -H "Content-Type: application/json" \
  -d '{
    "productId": "prod-b-id", 
    "quantity": 12
  }'
```

### 3. Fechar a Venda (Aplicar Descontos)
```bash
curl -X PATCH /api/sales/{saleId}/status \
  -H "Content-Type: application/json" \
  -d '{
    "status": "Closed"
  }'
```

**Resultado:**
- Produto A (8 itens): Desconto de 10%
- Produto B (12 itens): Desconto de 20%
- Status alterado para `Closed`
- Valor total recalculado com descontos

---

## 📝 Outros Status Disponíveis

### Cancelar Venda
```json
{
  "status": "Cancelled"
}
```
- Cancela a venda e todos os seus itens
- Não aplica descontos
- Operação irreversível

### Marcar como Paga
```json
{
  "status": "Paid"
}
```
- Marca a venda como paga
- Só funciona em vendas fechadas
- Não altera valores ou descontos

### Reativar Venda Cancelada
```json
{
  "status": "Pending"
}
```
- Só funciona em vendas canceladas
- Reativa a venda para edição
- Reativa também todos os itens cancelados

---

## ⚡ Dicas de Performance

1. **Paginação**: Sempre use paginação ao listar vendas
2. **Filtros**: Use o parâmetro `search` para filtrar resultados
3. **Cache**: As informações de produtos são cacheadas
4. **Batch**: Prefira adicionar itens individualmente para melhor controle

---

## 🛡️ Validações e Segurança

### Validações de Entrada
- Todos os GUIDs são validados
- Quantidades devem ser positivas
- Datas são validadas
- Campos obrigatórios são verificados

### Regras de Negócio
- Produtos inativos não podem ser adicionados
- Vendas fechadas não podem ser modificadas
- Limite de 20 itens idênticos é aplicado
- Clientes e filiais devem existir

### Tratamento de Erros
- Códigos HTTP apropriados (400, 404, 500)
- Mensagens de erro descritivas
- Validação tanto no front-end quanto no back-end

---

## 📈 Monitoramento e Logs

O sistema registra automaticamente:
- Criação de vendas
- Adição/remoção de itens  
- Alterações de status
- Aplicação de descontos
- Erros de validação

Exemplo de log:
```
UpdateSaleStatusHandler: Sale closed successfully with ID 'abc123' and total amount 'R$ 1.540,00'
CreateSaleItemHandler: Sale item created successfully with ID 'def456' and total amount 'R$ 500,00'
```

---

## 📋 Checklist para Criar sua Primeira Venda

### ✅ Pré-requisitos
- [ ] **Cliente cadastrado** → `POST /api/customers`
- [ ] **Filial cadastrada** → `POST /api/branches`  
- [ ] **Produto(s) cadastrado(s)** → `POST /api/products`
- [ ] **Verificar se os produtos estão ativos** → `GET /api/products/{id}`

### ✅ Processo de Venda
- [ ] **Criar venda** → `POST /api/sales`
- [ ] **Adicionar itens** → `POST /api/sales/{id}/items`
- [ ] **Verificar quantidades** (máximo 20 itens idênticos)
- [ ] **Fechar venda** → `PATCH /api/sales/{id}/status` (status: "Closed")
- [ ] **Verificar desconto aplicado** → `GET /api/sales/{id}`

### ⚠️ Pontos de Atenção
- Produtos devem estar **ativos** para serem vendidos
- Máximo **20 itens idênticos** por produto
- Descontos são aplicados **automaticamente** no fechamento
- Uma vez fechada, a venda **não pode ser modificada**

---

## 🎉 Conclusão

O sistema de vendas oferece um fluxo completo e automatizado, com destaque para:

- ✅ **Cadastros básicos obrigatórios** (Clientes, Filiais, Produtos)
- ✅ **Criação simplificada** de vendas
- ✅ **Adição flexível** de itens
- ✅ **Desconto automático** inteligente baseado em quantidade
- ✅ **Validações robustas** de regras de negócio
- ✅ **Controle de status** completo
- ✅ **API RESTful** bem estruturada

**🔑 Lembre-se**: Sempre verifique se os cadastros básicos (clientes, filiais e produtos) estão criados antes de iniciar o processo de vendas. O **sistema de desconto automático** é o diferencial, aplicando descontos de forma transparente e eficiente durante o fechamento das vendas, garantindo que as regras de negócio sejam sempre respeitadas. 