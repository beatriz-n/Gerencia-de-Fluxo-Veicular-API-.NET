# Gerencia-de-Fluxo-Veicular-API-.NET

API desenvolvida em .NET para gerenciar o fluxo de veículos. O projeto se integra a um sistema de visão computacional, que é responsável por identificar caracteres de placas de veículos e controlar o acesso a um local específico.

A API é o back-end que gerencia os dados de acesso e veículos. O sistema de visão computacional, por sua vez, interage com a API, utilizando requisições GET para consultar informações.

# Começando...

## Material de apoio

Acesse a playlist: https://www.youtube.com/playlist?list=PLY7efPd_oNTzXY3Wfg1hztHcDqlsu2XHY

## Instalação .NET

Instale o .Net seguindo este link https://dotnet.microsoft.com/pt-br/download

## Instação Visual Studio Insider

Instale o Visual Studio Insider neste link https://visualstudio.microsoft.com/insiders/?rwnlp=pt-br

- Instale o Community
- Next, next, next...
- Ao chegar na tela de Instalação de Componentes instale ASP.NET (assistir ai video com link anexado acima caso alguma duvida desta parte da instalção)
- Next, next, next...

## Abrindo Nossa Solução

Uma solução no Visual Studio é um arquivo .sln que organiza e agrupa um ou vários projetos.
Ela funciona como um “container” que mantém tudo estruturado, com configurações e dependências centralizadas.

Para abrir este projeto, primeiro garanta que você clonou o repositório no **Disco C:** da sua máquina.

Em seguida, abra o Visual Studio Insider, escolha “Abrir uma pasta ou solução”, selecione a pasta do repositório e então abra o arquivo .sln.
Isso fará com que todos os arquivos .csproj sejam carregados automaticamente, e a configuração de inicialização já estará pronta ao executar, o Visual Studio iniciará a API e a UI juntas.

## Configurando o banco de dados

Estamos utilizando o banco de dados PostgreSQL. Com a solução já aberta no Visual Studio Insider, clique no projeto ApiService e localize o arquivo appsettings.json.
Em seguida, configure o bloco "ConnectionStrings" de acordo com o seu banco.
O Host e o Database já estão corretos, você só precisa :

- Checar Porta do PostgreSQL

- Ajustar Username

- Ajustar Password

Depois disso, vá até a barra superior esquerda, clique em Exibir (se sua IDE estiver em português) e depois selecione Terminal.
Um terminal será aberto na parte inferior; navegue até a pasta FluxoVeicular.ApiService e execute o comando:
```bash
  dotnet ef database update
```

ficará algo assim:
```bash
 PS C:\TCC\Gerencia-de-Fluxo-Veicular-API-.NET\FluxoVeicular.ApiService> dotnet ef database update
```

É normal demorar um pouco mas ao final você irá ter sua tabela "migrada" para seu banco de dados.

Obs: provavelmente o terminal irá recomendar a restauração do dotnet ef migration então apenas digite o comando recomendado

## Instalação do HeidiSQL (Bônus)

A instalação do **HeidiSQL** é opcional, mas recomendada para facilitar a visualização dos dados do PostgreSQL.  
Embora seja possível consultar tudo pelo PostgreSQL por meio de comando SQL, o HeidiSQL oferece uma interface muito mais intuitiva, permitindo navegar pelas tabelas sem necessidade de comandos.

Acesse: https://www.heidisql.com/download.php e instale.

- Next, Next, Next…
- HeidiSQL instalado.

## Executando Nossa Aplicação
 Com tudo configurado, é so clicar no "Play" verde no menu superior, a solução importada já garante a configuração dos projetos de inicialização, caso dúvidas, assita ao video de link anexado acima.

### Conectando ao Banco de Dados

Para conectar ao PostgreSQL pelo HeidiSQL:

- Clique em **+Nova**.
- Uma nova sessão será criada.
- Em **Tipo de rede**, selecione **PostgreSQL (TCP/IP)**.
- Em **Biblioteca**, escolha **libpq-12.dll**.
- Em **Servidor/IP**, mantenha o valor padrão.
- Informe seu **usuário**.
- Informe sua **senha**.
- Verifique a **porta** configurada.
- No campo **Banco de dados**, clique na **seta azul** para listar as bases disponíveis.
- Se tudo estiver correto, selecione **fluxoveicular**.
- Clique em **OK**.

Pronto! Você poderá visualizar todas as tabelas do sistema através do HeidiSQL.


### Contato
Discord: showtriz
Email: beatriznespolid@gmail.com
Telefone: peça para o orientador



    
