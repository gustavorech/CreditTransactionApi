# CreditTransactionApi

Desenvolvido como parte de avaliação. 'Aplicação' para autorizar e, acredito, efetivar uma operação de crédito.

Está desenvolvido utilizando .Net Core no formato minimal api com banco de dados Postgres, mas está conteinerizado para facilitar a execução.

## Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

**OU**

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

**E** se quiser executar localmente, mas não é necessário:
- [SDK do .NET Core](https://dotnet.microsoft.com/download)

## Primeiros Passos

### Clonar o Repositório

```sh
git clone git@github.com:gustavorech/CreditTransactionApi.git
cd CreditTransactionApi
```

### Verificar portas da aplicação

As portas `5201` e `5432` estão sendo utilizadas e mapeadas para fora dos containers, se isso for conflitante com o ambiente de testes, altere-as em `./Infrastructure/.env`:

```
WEB_PORT: 5201
POSTGRES_PORT: 5432
```

### Construir e Executar com Docker Compose

Há um arquivo Makefile como facilitador para a execução `./Makefile`, se for possível utilize-o. Segue o conteúdo do mesmo:

```
up:
	docker compose -f Infrastructure/compose.yaml up --build

down:
	docker compose -f Infrastructure/compose.yaml down

unit:
	docker compose -f Infrastructure/compose.UnitTests.yaml up --build --abort-on-container-exit

integration:
	docker compose -f Infrastructure/compose.IntegrationTests.yaml up --build --abort-on-container-exit
```

Então para executar a aplicação, no diretório raiz do projeto, execute `make up` **OU** o conteúdo do comando dentro do arquivo `docker compose -f Infrastructure/compose.yaml up --build`.

Após a execução a API estará servida em `http://localhost:5201` ou em outra porta definida.

### Endpoints da API

Para o único endpoint previsto foi utilizado o payload de exemplo do arquivo. Para utilização, é necessário criar uma conta no endpoint listado abaixo.

- `POST /transaction` - Executa a transação (o que foi proposto na avaliação)
```
POST http://localhost:5201/transaction
{
    "account": "123",
    "totalAmount": 100.00,
    "mcc": "5811",
    "merchant": "PADARIA DO ZE               SAO PAULO BR"
}
```

Também alguns endpoints adicionais para averiguação:

- `POST /out-of-scope/generate-account` - Cria uma estrutura base e uma conta com as informações informadas
```
POST http://localhost:5201/out-of-scope/generate-account
{
    "accountId": "123",
    "foodPartitionInitialAmount": 650.00,
    "mealPartitionInitialAmount": 50.00,
    "cashPartitionInitialAmount": 1500.00
}
```
- `GET /out-of-scope/account/{accountId}/balance` - Retorna o balanço atual da conta
- `GET /out-of-scope/account/{accountId}/requests` - Retorna todas as requisições de transações junto ao resultado

Adicionalmente você pode importar o arquivo `./CreditTransactionApi.postman_collection.json` contendo a coleção dos endpoints listados acima.

### Execução dos Testes

Verifique o tópico acima listando o arquivo Makefile e execute `make unit` e `make integration` **OU** os comandos descritos para os mesmos.

A organização, nome e quantidade de testes estão definidos, mas sem a implementação. Para isso precisaria de mais 1 dia e ficaria feliz em implementá-los se assim acharem útil. Mas, em resumo:
- Nos testes unitários eu usária fakes/stubs para todas as comunicações externas ao método testado; faria apenas as pré configurações necessárias até ao ponto no método que o teste averigua (pensando na cyclomatic complexity aqui); checaria que comunicações externas foram ou não chamadas conforme o esperado; e verificaria apenas os dados úteis para o teste em específico;
- Já nos testes de integração, eu pré configuraria dados no banco; chamaria o endpoint específico; verificaria direto no banco se o estado esperado pode ser observado;

Sei que o tempo de vocês é limitado, mas utilizem os endpoints acima para uma checagem básica. Os comerciantes e MCCs cadastrados podem ser vistos no arquivo: `./Services/OutOfScopeHelperService.cs` no método `GenerateAdditionalDataIfNecessary`.

## Detalhes da Implementação

Algumas pastas importantes para verificar detalhes da implementação:
- `./Web/Endpoints`: Onde o endpoint está definido;
- `./Services`: A implementação da lógica de negócio;
- `./Data`: Configuração do banco em geral, migrations, conexão, classes representando as tabelas;
- `./Data/Models`: Classes de modelo;
- `./UnitTests`: Onde estão definidos os testes unitários;
- `./IntegrationTests`: Onde estão definidos os testes de integração;

Alguns outros detalhes:
- Sem validações desnecessárias de presença de dados que agregam complexidade. Se algo anormal acontecer é esperado que uma exceção seja lançada;
- Testes unitários, ao meu ver, devem ser na mesma proporção da `cyclomatic complexity`, o que corrobora com o primeiro ponto ao reduzir a quantidade de testes desnecessários;
- Acredito que o codigo e os testes precisam contar uma história e não devemos poupar palavras ao conta-la, por isso, podem verificar que tudo está `verboso` -- O mínimo possível de palavras que descrevam corretamente o que será feito e na dimensão macro/micro correspondente.

## Questão Aberta (L4)

Sobre a questão em si, qual seria o correto, aprovar a primeria e negar a segunda? Aprovar ambas mas só executar uma vez a transação? (estranho mas não tenho conhecimentos profundos sobre essas regras).

Está descrito que todas as transações são síncronas, acredito que isso se refira a resposta, mas que tenha um não sincronismo pensando na escalabilidade da aplicação, certamente na parte dos serviços, e possivelmente na base de dados (mas acredito que não).

O ChatGPT me informou que uma boa solução seria implementar um lock por ID no Redis (instância única), com o TTL da chave igual a 100ms, como garantia caso der algum problema na aquisição / processamento da transação. Essa solução abriria margens para um monte de problemas acredito eu.

Na solução que implementei eu salvo todas as requisições de transação no banco assim que chegam. Seria possível checar a presença (verificando a data) de uma transação idêntica, mas isso só seria possível se fosse garantida que uma transação para uma mesma pessoa não sejá executada em paralelo. Filas são interessantes para manter ordenação, mas aí abrimos um monte de questões arquiteturais a serem decididas.

Podemos optar por uma fila única de alta vazão e baixa latência (mas pode não ser o melhor pois acredito que a escala seja gigante) e utilizar mecanísmos de deduplicação de chaves, também não sei se existe filas de alta vazão, baixa latência e que ainda propicie deduplicação de chaves.

Mas... acredito que mais camadas sejam necessárias, mesmo agregando complexidade, primeiro uma fila única (distribuída ou não) com alta vazão e baixa latência que funcione como um router para filas secundárias, decidido sobre um hash do número da conta ou outra estratégia que garanta boa distribuição de carga e que duas mensagens para a mesma conta irão para a mesma fila. Bom, isso pra parte do producer.

Agora que garantimos a ordenação, o consumidor pode implementar a estratégia para autorizar a transação, de preferência a mais simmples possível, a própria base de dados da aplicação se possível.

Esse é um problema interessante e nunca trabalhei em um software com tal requisito. Acredito que já existam boas práticas e até regras de execução dada a importância do problema.