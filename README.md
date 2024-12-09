# Conversor de logs MINHA CDN x Agora

Esta API Rest tem por objetivo converter os logs do formato MNHA CDN para o formato Agora.

## Instru��es

1. Abra o projeto no Visual Studio 2019 e certifique-se que voc� tenha o ambiente configurado corretamente para rodar aplica��es .Net Core 2.1.
2. Antes de executar o projeto, configure os arquivos `appsettings.json` e `appsettings.Development.json` com as connection strings do banco de dados corretas.
3. No console do Gerenciador de Pacotes, rode o comando para criar as tabelas no banco de dados:
    ```sh
    Update-Database -v
    ```
4. Em seguida, rode a aplica��o.

## Endpoints

### Salva um log no banco
- **POST** `/api/source`
- **Body:**
    ```json
    {
        "url": "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt"
    }
    ```

### Lista os logs salvos
- **GET** `/api/source`

### Busca um log pelo ID
- **GET** `/api/source/{ID}`

### Converte o log
- **POST** `/api/destination/convert`
- **Par�metros:**
    - `id`: se preenchido com ID de log salvo no banco, faz a convers�o com o mesmo.
    - `response`: se for igual a `response`, ent�o somente grava no banco e retorna o log convertido. Caso seja `file`, grava no banco e gera um arquivo de log para download.
- **Body:**
    ```json
    {
        "url": "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt"
    }
    ```
    Se no body a URL estiver preenchida, ele desconsidera o ID e faz a convers�o com o conte�do do log.

### Lista todos os logs convertidos
- **GET** `/api/destination`

### Busca um log convertido pelo ID
- **GET** `/api/destination/{ID}`

### Swagger
- **GET** `/swagger`

## Testes

Foram gerados testes unit�rios para reposit�rios, servi�os e controladores.

## Tecnologias usadas
- .Net Core 2.1
- SQL Server 2022
- Visual Studio 2019
- xUnit
- Swagger
