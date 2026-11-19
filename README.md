# SOFTWARE1-2.3
# Sistema de Logística

Proyecto realizado en C# con .NET 10, MySQL y Dapper. Gestiona envíos con distintas modalidades de entrega.

---

## Requisitos

* .NET 10
* MySQL
* Dapper
* xUnit (para pruebas)

---

## Estructura del proyecto

```text
Logistica.slnx
src/
├── Aplicacion/          # Lógica de negocio, servicios y modalidades
├── Persistencia/        # Acceso a datos, conexión y repositorios
└── Tests/               # Pruebas unitarias
sql/
├── 01_estructura.sql    # Tabla Envio
├── 02_usuarios.sql      # Usuarios de base: administrador e invitado
└── 03_datos_muestra.sql # Ejemplos de envíos
backup/
└── envios.sql           # Respaldo completo de la base de datos
