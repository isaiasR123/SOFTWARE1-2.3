USE logistica;

-- =========================================
-- PROCEDURE 1: REGISTRAR ENVÍO COMPLETO
-- =========================================

DELIMITER //

CREATE PROCEDURE RegistrarEnvioCompleto(
    IN p_IdCliente INT,
    IN p_IdPaquete INT,
    IN p_IdModalidad INT,
    IN p_IdDireccionOrigen INT,
    IN p_IdDireccionDestino INT,
    IN p_Distancia DECIMAL(10,2),
    IN p_Costo DECIMAL(10,2),
    IN p_TiempoEstimado INT
)
BEGIN

    DECLARE v_IdEstado INT;
    DECLARE v_IdEnvio INT;

    START TRANSACTION;

    -- Buscar el estado inicial "Pendiente"
    SELECT IdEstado
    INTO v_IdEstado
    FROM Estado
    WHERE Nombre = 'Pendiente';

    -- Validar que exista el estado inicial
    IF v_IdEstado IS NULL THEN
        ROLLBACK;
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'No existe el estado Pendiente';
    END IF;

    -- Registrar el envío
    INSERT INTO Envio (
        IdCliente,
        IdPaquete,
        IdModalidad,
        IdEstado,
        IdDireccionOrigen,
        IdDireccionDestino,
        Distancia,
        Costo,
        TiempoEstimado
    )
    VALUES (
        p_IdCliente,
        p_IdPaquete,
        p_IdModalidad,
        v_IdEstado,
        p_IdDireccionOrigen,
        p_IdDireccionDestino,
        p_Distancia,
        p_Costo,
        p_TiempoEstimado
    );

    -- Obtener el ID del envío creado
    SET v_IdEnvio = LAST_INSERT_ID();

    -- Registrar el estado inicial en el historial
    INSERT INTO HistorialEstado (
        IdEnvio,
        IdEstado
    )
    VALUES (
        v_IdEnvio,
        v_IdEstado
    );

    COMMIT;

END //

DELIMITER ;


-- =========================================
-- PROCEDURE 2: ACTUALIZAR ESTADO
-- =========================================

DELIMITER //

CREATE PROCEDURE ActualizarEstadoEnvio(
    IN p_IdEnvio INT,
    IN p_IdEstadoNuevo INT
)
BEGIN

    DECLARE v_IdEstadoActual INT;

    START TRANSACTION;

    -- Obtener el estado actual
    SELECT IdEstado
    INTO v_IdEstadoActual
    FROM Envio
    WHERE IdEnvio = p_IdEnvio;

    -- Validar que exista el envío
    IF v_IdEstadoActual IS NULL THEN
        ROLLBACK;
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El envío no existe';
    END IF;

    -- Validar que exista el nuevo estado
    IF NOT EXISTS (
        SELECT 1
        FROM Estado
        WHERE IdEstado = p_IdEstadoNuevo
    ) THEN
        ROLLBACK;
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El estado indicado no existe';
    END IF;

    -- Evitar actualizar al mismo estado
    IF v_IdEstadoActual = p_IdEstadoNuevo THEN
        ROLLBACK;
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El envío ya posee ese estado';
    END IF;

    -- Actualizar estado
    UPDATE Envio
    SET IdEstado = p_IdEstadoNuevo
    WHERE IdEnvio = p_IdEnvio;

    -- Registrar el cambio en el historial
    INSERT INTO HistorialEstado (
        IdEnvio,
        IdEstado
    )
    VALUES (
        p_IdEnvio,
        p_IdEstadoNuevo
    );

    COMMIT;

END //

DELIMITER ;


-- =========================================
-- PROCEDURE 3: CANCELAR ENVÍO
-- =========================================

DELIMITER //

CREATE PROCEDURE CancelarEnvio(
    IN p_IdEnvio INT
)
BEGIN

    DECLARE v_IdEstadoActual INT;
    DECLARE v_IdEstadoCancelado INT;

    START TRANSACTION;

    -- Obtener estado actual
    SELECT IdEstado
    INTO v_IdEstadoActual
    FROM Envio
    WHERE IdEnvio = p_IdEnvio;

    -- Validar que exista el envío
    IF v_IdEstadoActual IS NULL THEN
        ROLLBACK;
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El envío no existe';
    END IF;

    -- Buscar estado Cancelado
    SELECT IdEstado
    INTO v_IdEstadoCancelado
    FROM Estado
    WHERE Nombre = 'Cancelado';

    -- Validar que exista
    IF v_IdEstadoCancelado IS NULL THEN
        ROLLBACK;
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'No existe el estado Cancelado';
    END IF;

    -- No permitir cancelar un envío ya entregado
    IF EXISTS (
        SELECT 1
        FROM Estado
        WHERE IdEstado = v_IdEstadoActual
        AND Nombre = 'Entregado'
    ) THEN
        ROLLBACK;
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'No se puede cancelar un envío entregado';
    END IF;

    -- No permitir cancelar uno ya cancelado
    IF v_IdEstadoActual = v_IdEstadoCancelado THEN
        ROLLBACK;
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El envío ya está cancelado';
    END IF;

    -- Actualizar estado
    UPDATE Envio
    SET IdEstado = v_IdEstadoCancelado
    WHERE IdEnvio = p_IdEnvio;

    -- Registrar en historial
    INSERT INTO HistorialEstado (
        IdEnvio,
        IdEstado
    )
    VALUES (
        p_IdEnvio,
        v_IdEstadoCancelado
    );

    COMMIT;

END //

DELIMITER ;


-- =========================================
-- PROCEDURE 4: RESUMEN POR PERÍODO
-- =========================================

DELIMITER //

CREATE PROCEDURE ResumenEnviosPorPeriodo(
    IN p_FechaInicio DATETIME,
    IN p_FechaFin DATETIME
)
BEGIN

    SELECT
        m.Nombre AS Modalidad,
        e.Nombre AS Estado,
        COUNT(*) AS CantidadEnvios,
        SUM(en.Costo) AS CostoTotal,
        AVG(en.Costo) AS CostoPromedio
    FROM Envio en
    INNER JOIN Modalidad m
        ON en.IdModalidad = m.IdModalidad
    INNER JOIN Estado e
        ON en.IdEstado = e.IdEstado
    WHERE en.FechaEnvio BETWEEN p_FechaInicio AND p_FechaFin
    GROUP BY
        m.Nombre,
        e.Nombre
    ORDER BY
        m.Nombre,
        e.Nombre;

END //

DELIMITER ;