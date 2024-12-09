# Conversor de logs MINHA CDN x Agora

Esta API Rest tem por objetivo converter os logs do formato MNHA CDN para o formato Agora.

## Instruções

1. Abra o projeto no Visual Studio 2019 e certifique-se que você tenha o ambiente configurado corretamente para rodar aplicações .Net Core 2.1.
2. Antes de executar o projeto, configure os arquivos `appsettings.json` e `appsettings.Development.json` com as connection strings do banco de dados corretas.
3. No console do Gerenciador de Pacotes, rode o comando para criar as tabelas no banco de dados:
    ```sh
    Update-Database -v
    ```
4. Em seguida, rode a aplicação.

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
- **Parâmetros:**
    - `id`: se preenchido com ID de log salvo no banco, faz a conversão com o mesmo.
    - `response`: se for igual a `response`, então somente grava no banco e retorna o log convertido. Caso seja `file`, grava no banco e gera um arquivo de log para download.
- **Body:**
    ```json
    {
        "url": "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt"
    }
    ```
    Se no body a URL estiver preenchida, ele desconsidera o ID e faz a conversão com o conteúdo do log.

### Lista todos os logs convertidos
- **GET** `/api/destination`

### Busca um log convertido pelo ID
- **GET** `/api/destination/{ID}`

### Swagger
- **GET** `/swagger`

## Testes

Foram gerados testes unitários para repositórios, serviços e controladores.

## Tecnologias usadas
- .Net Core 2.1
- SQL Server 2022
- Visual Studio 2019
- xUnit
- Swagger
