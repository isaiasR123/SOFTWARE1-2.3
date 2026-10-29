# SOFTWARE1-2.3
# Sistema de Logística

Proyecto realizado en C# con .NET 10, MySQL y Dapper.

## Requisitos

Para ejecutar el proyecto se necesita:

* .NET 10
* MySQL
* Dapper
* xUnit

## Estructura

```text
Logistica.slnx
src/
├── API
├── Aplicacion
├── Persistencia
└── Tests

scripts/
├── DDL.SQL
├── Usuarios.SQL
├── SP.SQL
└── Estadisticas.sql
```

## Ejecutar la API

La API guarda los envíos en MySQL. Prepará el esquema y los procedimientos legacy como administrador de MySQL:

```bash
mysql -u root -p < scripts/DDL.SQL
mysql -u root -p logistica < scripts/SP.SQL
mysql -u root -p logistica < scripts/Estadisticas.sql
mysql -u root -p < sql/04_estructura_api.sql
```

El script versionado `scripts/Usuarios.SQL` contiene placeholders, no contraseñas reales. Creá una copia local ignorada por Git y reemplazá ambos valores (`REEMPLAZAR_ADMIN_PASSWORD` y `REEMPLAZAR_INVITADO_PASSWORD`) por contraseñas distintas:

```bash
cp scripts/Usuarios.SQL scripts/Usuarios.local.sql
# Editar scripts/Usuarios.local.sql localmente y reemplazar los dos placeholders.
mysql -u root -p < scripts/Usuarios.local.sql
```

El administrador recibe todos los privilegios de `logistica`; el invitado recibe `SELECT` sobre la base y `EXECUTE` únicamente sobre los cinco procedimientos legacy de estadísticas de solo lectura. No recibe permisos de escritura ni ejecución de procedimientos de mutación. No subas la copia local ni sus contraseñas.

En la misma terminal desde la que ejecutes la API, configurá las credenciales por variables de entorno. `read -s` las solicita sin mostrarlas:

```bash
export LOGISTICA_DB_HOST=localhost
export LOGISTICA_DB_NAME=logistica
export LOGISTICA_DB_ADMIN_USER=logistica_admin
export LOGISTICA_DB_GUEST_USER=logistica_invitado
read -rsp 'Contraseña administrador: ' LOGISTICA_DB_ADMIN_PASSWORD; export LOGISTICA_DB_ADMIN_PASSWORD; printf '\n'
read -rsp 'Contraseña invitado: ' LOGISTICA_DB_GUEST_PASSWORD; export LOGISTICA_DB_GUEST_PASSWORD; printf '\n'
```

La API y la aplicación de consola usan invitado para consultas y administrador para crear, modificar y eliminar. `LOGISTICA_DB_PORT` es opcional; por defecto usa `3306`.

```bash
dotnet run --project src/API/API.csproj
```

Swagger UI queda disponible en la URL que indique la consola, bajo `/swagger`. Incluye ejemplos válidos e inválidos y los códigos de respuesta de las siete operaciones. El estado se avanza con `PATCH /api/envios/{id}/estado`, únicamente de `PENDIENTE` a `DESPACHADO` y de `DESPACHADO` a `ENTREGADO`.

Para verificar que invitado puede consultar y no puede escribir, configurá un login path local (solicita la contraseña sin incluirla en el comando) y ejecutá el script:

```bash
mysql_config_editor set --login-path=logistica_invitado --host=localhost --user=logistica_invitado --password
bash scripts/VerificarPermisos.sh
```

El script prueba un `SELECT` y espera que `INSERT`, `UPDATE` y `DELETE` sean rechazados.

## Verificar

Para compilar y ejecutar las pruebas:

```bash
dotnet test Logistica.slnx
```

## Sobre el proyecto

`Envio` es la clase base y de ella heredan:

* `EnvioEstandar`
* `EnvioExpress`
* `EnvioPrioritario`

Cada modalidad tiene su propio cálculo de costo y tiempo de entrega.

La aplicación usa Dapper para trabajar con MySQL y procedimientos almacenados para algunas operaciones.
