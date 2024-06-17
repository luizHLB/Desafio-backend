##Desafio backend
Estrtura de projeto base para avaliação.

#Restições

1 - O projeto está preparado para subir no docker, existe já um docker-compose em "src/docker-compose.yaml"
Toda a criação da estrutura do projeto (database, filas no RabbitMQ e folder no Azurite) vai ocorrer no startup da API.

2 - A API tem um endpoint para autenticação, "/api/v1/Authentication" somente este endpoint está aberto para receber requisições sem um token.
Esta API não tem endpoints para a criação de um usuário novo, porém existe um usuário pré cadastrado para utilizar o sistema.
o usuário e senha estão criptografados e para realizar o login deve ser utilizado a seguinte:
ywUB54Vih5gwAfVhHbEwVt73ZSjVnDvLbxo2EGaehjQv/n3R/TZOTVHhK8468Z8dnl3Tmb3I0uiT+ibj/RphIg==

O Token na resposta deve ser colocado no Authorize do swagger no formato "Bearer {seu_token}"
O mesmo é valido por 24 horas.

#Observação

1 - No endpoint /api/v1/Driver o campo CNHCategory possui os seguintes valores:
 A = 1,
 B = 2,
 C = 4,
 D = 8,
 E = 16

2 - Os parâmetros no arquivo de configuração estão adequados para a aplicação rodar no docker, se houver a necessidade de rodar por dentro de alguma interface de desenvolvimento, existem configurações comentadas no arquivo, elas devem ser alterarnadas coma s configurações do docker vigentes.
