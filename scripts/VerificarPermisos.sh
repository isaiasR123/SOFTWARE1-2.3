#!/usr/bin/env bash
set -euo pipefail

database="${LOGISTICA_DB_NAME:-logistica}"
guest_login_path="${LOGISTICA_DB_GUEST_LOGIN_PATH:-logistica_invitado}"

mysql --login-path="$guest_login_path" --database="$database" \
  --execute="SELECT Id, Modalidad, Estado FROM EnvioApi LIMIT 1;" >/dev/null
printf 'OK: el invitado puede consultar EnvioApi.\n'

verificar_denegado() {
  local operacion="$1"
  local sql="$2"

  if mysql --login-path="$guest_login_path" --database="$database" \
    --execute="$sql" >/dev/null 2>&1; then
    printf 'ERROR: el invitado pudo ejecutar %s.\n' "$operacion" >&2
    return 1
  fi

  printf 'OK: el invitado no puede %s.\n' "$operacion"
}

verificar_denegado insertar \
  "START TRANSACTION; INSERT INTO EnvioApi (Modalidad, PesoKg, DistanciaKm, Destino) VALUES ('estandar', 1, 1, 'Prueba'); ROLLBACK"
verificar_denegado actualizar \
  "UPDATE EnvioApi SET Destino = 'Prueba' WHERE Id = 0"
verificar_denegado eliminar \
  "DELETE FROM EnvioApi WHERE Id = 0"