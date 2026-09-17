USE logistica;

-- =====================================================
-- 1. RESUMEN DE ENVÍOS POR PERÍODO, MODALIDAD Y ESTADO
-- =====================================================

DELIMITER //


DELIMITER ;


-- =====================================================
-- 2. COSTOS ACUMULADOS Y PROMEDIO POR MODALIDAD
-- =====================================================

DELIMITER //

CREATE PROCEDURE CostosPorModalidad(
    IN p_FechaInicio DATETIME,
    IN p_FechaFin DATETIME
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    SELECT
        m.Nombre AS Modalidad,
        SUM(en.Costo) AS CostoAcumulado,
        AVG(en.Costo) AS CostoPromedio
    FROM Envio en
    INNER JOIN Modalidad m
        ON en.IdModalidad = m.IdModalidad
    WHERE en.FechaEnvio BETWEEN p_FechaInicio AND p_FechaFin
    GROUP BY
        m.Nombre
    ORDER BY
        m.Nombre;

    COMMIT;
END //

DELIMITER ;


-- =====================================================
-- 3. COMPARATIVA DE ENTREGADOS, CANCELADOS Y PENDIENTES
-- =====================================================

DELIMITER //

CREATE PROCEDURE ComparativaEstadosPorPeriodo(
    IN p_FechaInicio DATETIME,
    IN p_FechaFin DATETIME
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    SELECT
        e.Nombre AS Estado,
        COUNT(en.IdEnvio) AS CantidadEnvios
    FROM Envio en
    INNER JOIN Estado e
        ON en.IdEstado = e.IdEstado
    WHERE en.FechaEnvio BETWEEN p_FechaInicio AND p_FechaFin
      AND e.Nombre IN ('Entregado', 'Cancelado', 'Pendiente')
    GROUP BY
        e.Nombre
    ORDER BY
        e.Nombre;

    COMMIT;
END //

DELIMITER ;


-- =====================================================
-- 4. TIEMPO PROMEDIO DE ENTREGA POR MODALIDAD
-- =====================================================

DELIMITER //

CREATE PROCEDURE TiempoPromedioEntrega(
    IN p_FechaInicio DATETIME,
    IN p_FechaFin DATETIME
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    SELECT
        m.Nombre AS Modalidad,
        AVG(en.TiempoEstimado) AS TiempoPromedioEntrega
    FROM Envio en
    INNER JOIN Modalidad m
        ON en.IdModalidad = m.IdModalidad
    INNER JOIN Estado e
        ON en.IdEstado = e.IdEstado
    WHERE en.FechaEnvio BETWEEN p_FechaInicio AND p_FechaFin
      AND e.Nombre = 'Entregado'
    GROUP BY
        m.Nombre
    ORDER BY
        m.Nombre;

    COMMIT;
END //

DELIMITER ;


-- =====================================================
-- 5. FACTURACIÓN TOTAL POR MODALIDAD Y PERÍODO
-- =====================================================

DELIMITER //

CREATE PROCEDURE FacturacionPorModalidad(
    IN p_FechaInicio DATETIME,
    IN p_FechaFin DATETIME
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    SELECT
        m.Nombre AS Modalidad,
        SUM(en.Costo) AS FacturacionTotal
    FROM Envio en
    INNER JOIN Modalidad m
        ON en.IdModalidad = m.IdModalidad
    WHERE en.FechaEnvio BETWEEN p_FechaInicio AND p_FechaFin
    GROUP BY
        m.Nombre
    ORDER BY
        m.Nombre;

    COMMIT;
END //

DELIMITER ;