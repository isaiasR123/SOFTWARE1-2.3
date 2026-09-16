CREATE DATABASE IF NOT EXISTS logistica;

USE logistica;

-- =========================================
-- TABLA DIRECCION
-- =========================================

CREATE TABLE Direccion (
    IdDireccion INT AUTO_INCREMENT PRIMARY KEY,
    Calle VARCHAR(100) NOT NULL,
    Numero INT NOT NULL,
    Ciudad VARCHAR(100) NOT NULL,

    CONSTRAINT CK_Direccion_Numero
        CHECK (Numero > 0)
);


-- =========================================
-- TABLA CLIENTE
-- =========================================

CREATE TABLE Cliente (
    IdCliente INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    IdDireccion INT NOT NULL,

    CONSTRAINT FK_Cliente_Direccion
        FOREIGN KEY (IdDireccion)
        REFERENCES Direccion(IdDireccion)
);


-- =========================================
-- TABLA PAQUETE
-- =========================================

CREATE TABLE Paquete (
    IdPaquete INT AUTO_INCREMENT PRIMARY KEY,
    Peso DECIMAL(10,2) NOT NULL,
    Largo DECIMAL(10,2) NOT NULL,
    Ancho DECIMAL(10,2) NOT NULL,
    Alto DECIMAL(10,2) NOT NULL,
    Descripcion VARCHAR(255) NOT NULL,

    CONSTRAINT CK_Paquete_Peso
        CHECK (Peso > 0),

    CONSTRAINT CK_Paquete_Largo
        CHECK (Largo > 0),

    CONSTRAINT CK_Paquete_Ancho
        CHECK (Ancho > 0),

    CONSTRAINT CK_Paquete_Alto
        CHECK (Alto > 0)
);


-- =========================================
-- TABLA MODALIDAD
-- =========================================

CREATE TABLE Modalidad (
    IdModalidad INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL UNIQUE
);


-- =========================================
-- TABLA ESTADO
-- =========================================

CREATE TABLE Estado (
    IdEstado INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL UNIQUE
);


-- =========================================
-- TABLA ENVIO
-- =========================================

CREATE TABLE Envio (
    IdEnvio INT AUTO_INCREMENT PRIMARY KEY,

    IdCliente INT NOT NULL,
    IdPaquete INT NOT NULL,
    IdModalidad INT NOT NULL,
    IdEstado INT NOT NULL,

    IdDireccionOrigen INT NOT NULL,
    IdDireccionDestino INT NOT NULL,

    Distancia DECIMAL(10,2) NOT NULL,
    Costo DECIMAL(10,2) NOT NULL,
    TiempoEstimado INT NOT NULL,

    FechaEnvio DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_Envio_Cliente
        FOREIGN KEY (IdCliente)
        REFERENCES Cliente(IdCliente),

    CONSTRAINT FK_Envio_Paquete
        FOREIGN KEY (IdPaquete)
        REFERENCES Paquete(IdPaquete),

    CONSTRAINT FK_Envio_Modalidad
        FOREIGN KEY (IdModalidad)
        REFERENCES Modalidad(IdModalidad),

    CONSTRAINT FK_Envio_Estado
        FOREIGN KEY (IdEstado)
        REFERENCES Estado(IdEstado),

    CONSTRAINT FK_Envio_DireccionOrigen
        FOREIGN KEY (IdDireccionOrigen)
        REFERENCES Direccion(IdDireccion),

    CONSTRAINT FK_Envio_DireccionDestino
        FOREIGN KEY (IdDireccionDestino)
        REFERENCES Direccion(IdDireccion),

    CONSTRAINT CK_Envio_Distancia
        CHECK (Distancia > 0),

    CONSTRAINT CK_Envio_Costo
        CHECK (Costo >= 0),

    CONSTRAINT CK_Envio_TiempoEstimado
        CHECK (TiempoEstimado > 0)
);


-- =========================================
-- TABLA HISTORIAL DE ESTADOS
-- =========================================

CREATE TABLE HistorialEstado (
    IdHistorial INT AUTO_INCREMENT PRIMARY KEY,

    IdEnvio INT NOT NULL,
    IdEstado INT NOT NULL,

    FechaCambio DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_Historial_Envio
        FOREIGN KEY (IdEnvio)
        REFERENCES Envio(IdEnvio),

    CONSTRAINT FK_Historial_Estado
        FOREIGN KEY (IdEstado)
        REFERENCES Estado(IdEstado)
);

-- =========================================
-- MODALIDADES INICIALES
-- =========================================

INSERT INTO Modalidad (Nombre)
VALUES
    ('Estandar'),
    ('Express'),
    ('Prioritario');


-- =========================================
-- ESTADOS INICIALES
-- =========================================

INSERT INTO Estado (Nombre)
VALUES
    ('Pendiente'),
    ('En camino'),
    ('Entregado'),
    ('Cancelado');