
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
```

---

## Base de Datos

### 1. Crear la estructura
```bash
mysql -u tu_usuario -p < sql/01_estructura.sql
```

### 2. Configurar usuarios de MySQL
El archivo contiene marcadores de posición. Creá una copia local con tus contraseñas reales antes de ejecutar:
```bash
mysql -u root -p < sql/02_usuarios.sql
```
-  **Administrador:** acceso completo a la base
-  **Invitado:** solo puede leer (SELECT), sin permisos de escritura

### 3. Cargar datos de ejemplo
```bash
mysql -u tu_usuario -p logistica_db < sql/03_datos_muestra.sql
```

---

## Respaldo y Restauración

**Generar respaldo:**
```bash
mysqldump -u tu_usuario -p logistica_db > backup/envios.sql
```

**Restaurar respaldo:**
```bash
mysql -u tu_usuario -p logistica_db < backup/envios.sql
```
>  El respaldo NO incluye usuarios ni permisos. Después de restaurar, volvé a ejecutar `02_usuarios.sql` si es necesario.

---

## Ejecutar las pruebas

El proyecto no incluye una API ejecutable. La lógica se verifica mediante pruebas automatizadas:

```bash
dotnet test Logistica.slnx
```

### ¿Qué prueban los tests?
-  Cálculo correcto de costo y días para envío estándar
-  Cálculo correcto de costo y días para envío express
-  Validaciones: peso y distancia deben ser mayores que cero

---

## Lógica de negocio

### Modalidades de envío

| Modalidad | Fórmula de costo | Plazo |
|---|---|---|
| Estándar | 1000 + 200 × kg + 10 × km | 5 días |
| Express | 2000 + 350 × kg + 20 × km | 2 días |

### Cambio de estados
Los envíos pasan por estados en orden estricto:

**PENDIENTE → DESPACHADO → ENTREGADO**

- Todo envío nuevo comienza como **PENDIENTE**
- No se puede saltear estados ni volver atrás
- Cualquier cambio inválido es rechazado por el sistema

### Conexiones a la base de datos
- **Usuario invitado** → solo para consultas de lectura
- **Usuario administrador** → para crear, modificar y eliminar registros
- Las credenciales reales se configuran de forma local y NO se suben a GitHub

---

## Sobre el diseño

El proyecto usa el **patrón Estrategia**: cada modalidad calcula su propio costo implementando la interfaz `IEnvioModalidad`. El servicio de envíos delega el cálculo sin necesidad de conocer los detalles de cada fórmula. Esto permite agregar nuevas modalidades sin modificar el código del servicio.
```

---
