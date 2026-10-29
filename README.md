# SistemaSoporte (referencia Semana 8)

API de soporte con 4 capas (.NET 10): `Abstracciones`, `DA`, `Flujo`, `API`, más `Pruebas`.

## Correr

```bash
# 1. Cadena de conexión (LocalDB ya viene en appsettings.json; para Azure use User Secrets)
dotnet user-secrets set "ConnectionStrings:SistemaSoporte" "<cadena>" --project API

# 2. Migraciones (la primera vez, o cada vez que cambie el modelo)
dotnet ef migrations add Inicial --project DA --startup-project API --output-dir Migrations
dotnet ef database update --project DA --startup-project API

# 3. Datos de demostración (muestra el servidor destino y pide confirmación)
dotnet run --project API -- --seed

# 4. Levantar el API
dotnet run --project API       # http://localhost:5190/scalar/v1  y  /tickets.html

# 5. Pruebas
dotnet test
```

## Modo Práctica 1 (tickets en memoria)

En `API/appsettings.json` cambie `"Almacenamiento": { "Tickets": "Memoria" }`.
`TicketDAMemoria` reemplaza a `TicketDA` (Entity Framework) sin tocar Flujo ni API.
Los clientes siguen viniendo de la base de datos (corra la semilla para tener los clientes 1 a 8).
Etiquetas y comentarios requieren `"Sql"`.

## Regla de dependencias

```
API  ->  Flujo  ->  Abstracciones  <-  DA
 \________________________________/
   (API referencia DA solo en Program.cs)
```

Documento completo: `Arquitectura-4-capas-SistemaSoporte.md`.
