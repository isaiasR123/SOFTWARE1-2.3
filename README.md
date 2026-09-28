# SOFTWARE1-2.3
# Sistema de Logística

Proyecto realizado en C# con .NET 8, MySQL y Dapper.

## Requisitos

Para ejecutar el proyecto se necesita:

* .NET 8
* MySQL
* Dapper
* xUnit

## Estructura

```text
Logistica.slnx
src/
├── Aplicacion
├── Persistencia
└── Tests

scripts/
├── DDL.SQL
├── Usuarios.SQL
├── SP.SQL
└── Estadisticas.sql
```

## Como ejecutar el proyecto

Primero hay que crear la base de datos.

Desde la carpeta del proyecto:

```bash
mysql -u 5to_agbd -p < scripts/DDL.SQL
```

Después cargar los procedimientos:

```bash
mysql -u 5to_agbd -p logistica < scripts/SP.SQL
```

Y también:

```bash
mysql -u 5to_agbd -p logistica < scripts/Estadisticas.sql
```

La contraseña de MySQL se guarda en una variable de entorno:

```bash
export LOGISTICA_DB_PASSWORD='TU_CONTRASEÑA'
```

Después se puede compilar:

```bash
dotnet build Logistica.slnx
```

Para ejecutar los tests:

```bash
dotnet test Logistica.slnx
```

Para ejecutar la aplicación:

```bash
dotnet run --project src/Aplicacion/Aplicacion.csproj
```

## Sobre el proyecto

`Envio` es la clase base y de ella heredan:

* `EnvioEstandar`
* `EnvioExpress`
* `EnvioPrioritario`

Cada modalidad tiene su propio cálculo de costo y tiempo de entrega.

La aplicación usa Dapper para trabajar con MySQL y procedimientos almacenados para algunas operaciones.
