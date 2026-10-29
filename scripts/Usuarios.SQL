CREATE USER IF NOT EXISTS 'logistica_admin'@'localhost'
IDENTIFIED BY 'REEMPLAZAR_ADMIN_PASSWORD';
ALTER USER 'logistica_admin'@'localhost'
IDENTIFIED BY 'REEMPLAZAR_ADMIN_PASSWORD';
GRANT ALL PRIVILEGES ON logistica.* TO 'logistica_admin'@'localhost';

CREATE USER IF NOT EXISTS 'logistica_invitado'@'localhost'
IDENTIFIED BY 'REEMPLAZAR_INVITADO_PASSWORD';
ALTER USER 'logistica_invitado'@'localhost'
IDENTIFIED BY 'REEMPLAZAR_INVITADO_PASSWORD';
REVOKE ALL PRIVILEGES, GRANT OPTION FROM 'logistica_invitado'@'localhost';
GRANT SELECT ON logistica.* TO 'logistica_invitado'@'localhost';
GRANT EXECUTE ON PROCEDURE logistica.ResumenEnviosPorPeriodo TO 'logistica_invitado'@'localhost';
GRANT EXECUTE ON PROCEDURE logistica.CostosPorModalidad TO 'logistica_invitado'@'localhost';
GRANT EXECUTE ON PROCEDURE logistica.ComparativaEstadosPorPeriodo TO 'logistica_invitado'@'localhost';
GRANT EXECUTE ON PROCEDURE logistica.TiempoPromedioEntrega TO 'logistica_invitado'@'localhost';
GRANT EXECUTE ON PROCEDURE logistica.FacturacionPorModalidad TO 'logistica_invitado'@'localhost';