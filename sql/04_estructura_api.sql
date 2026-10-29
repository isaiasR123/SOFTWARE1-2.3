CREATE DATABASE IF NOT EXISTS logistica;
USE logistica;

CREATE TABLE IF NOT EXISTS EnvioApi (
    Id INT NOT NULL AUTO_INCREMENT,
    Modalidad VARCHAR(16) NOT NULL,
    PesoKg DECIMAL(10,2) NOT NULL,
    DistanciaKm DECIMAL(10,2) NOT NULL,
    Destino VARCHAR(255) NOT NULL,
    Estado ENUM('PENDIENTE', 'DESPACHADO', 'ENTREGADO') NOT NULL DEFAULT 'PENDIENTE',
    PRIMARY KEY (Id),
    CONSTRAINT CK_EnvioApi_Modalidad CHECK (Modalidad IN ('estandar', 'express')),
    CONSTRAINT CK_EnvioApi_Peso CHECK (PesoKg > 0),
    CONSTRAINT CK_EnvioApi_Distancia CHECK (DistanciaKm > 0),
    CONSTRAINT CK_EnvioApi_Destino CHECK (CHAR_LENGTH(TRIM(Destino)) > 0)
);