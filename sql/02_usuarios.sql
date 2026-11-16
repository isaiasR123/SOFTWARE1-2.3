-- IMPORTANTE: Reemplazá 'TU_CONTRASENA_ADMIN' y 'TU_CONTRASENA_INVITADO' en tu copia local.
-- No subas contraseñas reales a GitHub.

-- Crear usuario Administrador con acceso completo
CREATE USER IF NOT EXISTS 'admin_envios'@'localhost' IDENTIFIED BY 'TU_CONTRASENA_ADMIN';
GRANT ALL PRIVILEGES ON logistica_db.* TO 'admin_envios'@'localhost';

-- Crear usuario Invitado con solo permisos de lectura (SELECT)
CREATE USER IF NOT EXISTS 'invitado_envios'@'localhost' IDENTIFIED BY 'TU_CONTRASENA_INVITADO';
GRANT SELECT ON logistica_db.* TO 'invitado_envios'@'localhost';

FLUSH PRIVILEGES;