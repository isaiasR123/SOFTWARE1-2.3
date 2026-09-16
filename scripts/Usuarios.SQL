-- =========================================
-- ACTIVIDAD 4 - USUARIOS Y PERMISOS
-- =========================================

-- =========================================
-- USUARIO ADMINISTRADOR
-- =========================================

CREATE USER IF NOT EXISTS 'administrador'@'localhost'
IDENTIFIED BY 'Admin1234';

GRANT ALL PRIVILEGES
ON *.*
TO 'administrador'@'localhost'
WITH GRANT OPTION;


-- =========================================
-- USUARIO DESARROLLO
-- =========================================

CREATE USER IF NOT EXISTS 'desarrollo'@'localhost'
IDENTIFIED BY 'Desarrollo1234';

GRANT ALL PRIVILEGES
ON logistica.*
TO 'desarrollo'@'localhost';


-- =========================================
-- ACTUALIZAR PRIVILEGIOS
-- =========================================

FLUSH PRIVILEGES;