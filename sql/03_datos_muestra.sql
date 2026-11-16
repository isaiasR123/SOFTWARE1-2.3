USE logistica_db;

INSERT INTO Envio (Modalidad, PesoKg, DistanciaKm, Destino, Costo, DiasEstimados, Estado) VALUES
('estandar', 5.0, 10.0, 'Av. Corrientes 1234, CABA', 2100.00, 5, 'PENDIENTE'),
('express', 3.5, 20.0, 'Mitre 500, Avellaneda', 3625.00, 2, 'PENDIENTE');