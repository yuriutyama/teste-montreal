# Aplicar Migrations

Para aplicar as migrations ao banco de dados SQL Server, execute o seguinte comando:

```bash
dotnet ef database update
```

Ou, se preferir aplicar manualmente, você pode usar o SQL Server Management Studio ou Azure Data Studio para executar o script SQL gerado pela migration.

## Configuração da Connection String

A connection string está configurada nos arquivos:
- `appsettings.json` (Produção)
- `appsettings.Development.json` (Desenvolvimento)

Por padrão, está configurada para usar LocalDB:
```
Server=(localdb)\mssqllocaldb;Database=TesteMonterealDB;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true
```

Para usar um SQL Server diferente, altere a connection string no arquivo `appsettings.json` ou `appsettings.Development.json`.

