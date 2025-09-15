# User Registration

Esta é uma aplicação .NET e C#; também utilizei Entity Framework, SQL Server, AutoMapper e xUnit nos testes unitários.

#### Aqui está o [Front-end](https://github.com/filiperv7/userRegistration_front) desta aplicação.

## Nesta app você pode:
- visualizar lista de usuário cadastrados;
- criar contas (se for Admin);
- editar contas (se for Admin);
- excluir contas (se for Admin).

## Regras de negócio
1. Somente o Perfil "Admin" pode cadastrar um usuário.
2. O usuário de Perfil "Admin" pode editar as informaçãoes pessoais de qualquer usuário.
3. O usuário do perfil "User" só pode logar e ver lista de usuários

## Informação para primeiro login
Existe um usuário padrão que é criado ao atualizar o banco de dados usando o EF (passo 4).
CPF: 91649430035, Senha: $enHa32!

É importante que o projeto de inicialização seja o UserRegistration.API e que o appsettings.json dele seja alterado com uma nova Secret Key.

## Como rodar a aplicação (6 passos)
##### 1. Clone o projeto
```bash
git clone https://github.com/filiperv7/userRegistration_back
```

##### 2. Acesse a pasta do projeto
```bash
cd userRegistration_back
```

##### 3. Crie a primeira migração com o .NET
```bash
dotnet ef migrations add InitialMigration --project UserRegistration.Infrastructure --startup-project UserRegistration.API
```

##### 4. Crie as tabelas no banco com o .NET
```bash
dotnet ef database update --project UserRegistration.Infrastructure --startup-project UserRegistration.API
```

##### 5. Adicione um _Secret Key_ no arquivo appsettings.json (dentro do projeto UserRegistration.API)

##### 6. Agora é só rodar o projeto no Visual Studio ou como você preferir

##### Obs.: para uma experiência completa, não deixe de rodar também o [Front-end](https://github.com/filiperv7/userRegistration_front)
