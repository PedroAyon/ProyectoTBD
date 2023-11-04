DROP SCHEMA IF EXISTS compras;
CREATE SCHEMA compras;
USE compras;

CREATE TABLE Proveedor
(
    id_proveedor INT PRIMARY KEY,
    nombre       VARCHAR(255),
    categoria    INT,
    ciudad       VARCHAR(255)
);

CREATE TABLE Articulo
(
    id_articulo INT PRIMARY KEY,
    descripcion VARCHAR(255),
    ciudad      VARCHAR(50),
    precio      DECIMAL(10, 2)
);

CREATE TABLE Cliente
(
    id_cliente INT PRIMARY KEY,
    nombre     VARCHAR(255),
    ciudad     VARCHAR(255)
);

CREATE TABLE Pedido
(
    id_pedido    INT PRIMARY KEY,
    id_proveedor INT,
    id_articulo  INT,
    id_cliente   INT,
    cantidad     INT,
    precio_total DECIMAL(10, 2),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedor (id_proveedor),
    FOREIGN KEY (id_articulo) REFERENCES Articulo (id_articulo),
    FOREIGN KEY (id_cliente) REFERENCES Cliente (id_cliente)
);

CREATE TABLE Provee
(
    id_proveedor INT,
    id_articulo  INT,
    FOREIGN KEY (id_proveedor) REFERENCES Proveedor (id_proveedor),
    FOREIGN KEY (id_articulo) REFERENCES Articulo (id_articulo),
    PRIMARY KEY (id_proveedor, id_articulo)
);

INSERT INTO Proveedor (id_proveedor, nombre, categoria, ciudad)
VALUES (1, 'Juan Pérez', 1, 'Tampico'),
       (2, 'María Gómez', 2, 'Madero'),
       (3, 'Pedro Rodríguez', 1, 'Altamira'),
       (4, 'Ana López', 3, 'Monterrey'),
       (5, 'Carlos Hernández', 2, 'CDMX'),
       (6, 'Laura Ramírez', 1, 'Tampico'),
       (7, 'José Morales', 4, 'Madero'),
       (8, 'Isabel García', 3, 'Altamira'),
       (9, 'Miguel Torres', 2, 'Monterrey'),
       (10, 'Alejandra Vargas', 1, 'CDMX');

-- Insertar registros en la tabla Articulo
INSERT INTO Articulo (id_articulo, descripcion, precio, ciudad)
VALUES (1, 'Auriculares Bluetooth', 79.99, 'Madero'),
       (2, 'Sudadera Deportiva', 29.99, 'Tampico'),
       (3, 'Cámara Digital', 299.99, 'Madero'),
       (4, 'Pasta de Dientes', 2.99, 'CDMX'),
       (5, 'Batidora de Mano', 39.99, 'Monterrey'),
       (6, 'Collar de Perlas', 89.99, 'CDMX'),
       (7, 'Televisor 50"', 699.99, 'Madero'),
       (8, 'Pantalones de Cuero', 129.99, 'CDMX'),
       (9, 'Set de Ollas', 59.99, 'Altamira'),
       (10, 'Galletas de Avena', 4.99, 'CDMX'),
       (11, 'Lavadora de Carga Frontal', 999.99, 'Altamira'),
       (12, 'Pulsera de Plata', 49.99, 'Madero'),
       (13, 'Altavoz Bluetooth', 49.99, 'CDMX'),
       (14, 'Blusa Estampada', 19.99, 'Tampico'),
       (15, 'Tabla de Cortar', 24.99, 'Monterrey');


-- Insertar registros en la tabla Cliente

INSERT INTO Cliente (id_cliente, nombre, ciudad)
VALUES (1, 'Juan Pérez', 'Tampico'),
       (2, 'María Gómez', 'Madero'),
       (3, 'Pedro Rodríguez', 'Altamira'),
       (4, 'Ana López', 'Monterrey'),
       (5, 'Carlos Hernández', 'CDMX'),
       (6, 'Laura Ramírez', 'Tampico'),
       (7, 'José Morales', 'Madero'),
       (8, 'Isabel García', 'Altamira'),
       (9, 'Miguel Torres', 'Monterrey'),
       (10, 'Alejandra Vargas', 'CDMX'),
       (11, 'Luisa Morales', 'Tampico'),
       (12, 'Javier González', 'Madero'),
       (13, 'María Fernández', 'Altamira'),
       (14, 'Ricardo Cruz', 'Monterrey'),
       (15, 'Sofía Martínez', 'CDMX');

-- Insertar registros en la tabla Provee
INSERT INTO Provee (id_proveedor, id_articulo)
VALUES (1, 1),
       (2, 2),
       (3, 3),
       (4, 4),
       (5, 5),
       (6, 6),
       (7, 7),
       (8, 8),
       (9, 9),
       (10, 10),
       (1, 11),
       (2, 12),
       (3, 13),
       (4, 14),
       (5, 15),
       (6, 1),
       (7, 2),
       (8, 3),
       (9, 4),
       (10, 5),
       (1, 6),
       (2, 7),
       (3, 8),
       (4, 9),
       (5, 10);

INSERT INTO Pedido (id_pedido, id_proveedor, id_articulo, id_cliente, cantidad, precio_total)
VALUES (1, 1, 1, 1, 2, 259.98),
       (2, 2, 2, 2, 1, 39.99),
       (3, 3, 3, 3, 3, 239.97),
       (4, 4, 4, 4, 5, 19.95),
       (5, 5, 5, 5, 1, 59.99),
       (6, 6, 6, 6, 1, 49.99),
       (7, 7, 7, 7, 2, 3999.98),
       (8, 8, 8, 8, 3, 209.97),
       (9, 9, 9, 9, 1, 49.99),
       (10, 10, 10, 10, 4, 23.96),
       (11, 1, 11, 11, 1, 899.99),
       (12, 2, 12, 12, 2, 1399.98),
       (13, 3, 13, 13, 1, 79.99),
       (14, 4, 14, 14, 2, 179.98),
       (15, 5, 15, 15, 1, 149.99),
       (16, 4, 10, 4, 5, 200.00);

-- 1. Hallar el código de los proveedores que proveen el artículo 3.
SELECT id_proveedor
FROM Provee
WHERE id_articulo = 3;

-- 2. Hallar los clientes que solicitan artículos provistos por el proveedor 10.
SELECT DISTINCT c.*
FROM Cliente c
         JOIN Pedido p ON c.id_cliente = p.id_cliente
         JOIN Provee pv ON p.id_articulo = pv.id_articulo
WHERE pv.id_proveedor = 10;

-- 3. Hallar los clientes que solicitan algún artículo provisto por proveedores cON categoría mayor que 2.
SELECT DISTINCT c.*
FROM Cliente c
         JOIN Pedido p ON c.id_cliente = p.id_cliente
         JOIN Proveedor pr ON p.id_proveedor = pr.id_proveedor
         JOIN Provee pv ON p.id_articulo = pv.id_articulo
WHERE pr.categoria > 2;

-- 4. Hallar los pedidos realizados por clientes de la ciudad de Tampico.
SELECT Pedido.*
FROM Pedido
         JOIN Cliente ON Pedido.id_cliente = Cliente.id_cliente
WHERE Cliente.ciudad = 'Tampico';

-- 5. Hallar los pedidos en los que un cliente de Tampico solicita artículos producidos en la CDMX.
SELECT Pedido.*
FROM Pedido
         JOIN Cliente ON Pedido.id_cliente = Cliente.id_cliente
         JOIN Articulo ON Pedido.id_articulo = Articulo.id_articulo
         JOIN Proveedor ON Pedido.id_proveedor = Proveedor.id_proveedor
WHERE Cliente.ciudad = 'Tampico'
  AND Articulo.ciudad = 'CDMX';

-- 6. Hallar los pedidos en los que el cliente 3 solicita artículos no pedidos por el cliente 4.
SELECT Pedido.*
FROM Pedido
         JOIN Cliente ON Pedido.id_cliente = Cliente.id_cliente
         JOIN Articulo ON Pedido.id_articulo = Articulo.id_articulo
WHERE Pedido.id_cliente = 3
  AND Pedido.id_articulo NOT IN (SELECT id_articulo
                                 FROM Pedido
                                 WHERE id_cliente = 4);

-- 7. Hallar los pares de ciudades (ciudad1, ciudad2), tales que un proveedor de la ciudad1 provee artículos pedidos por clientes de la ciudad2.
SELECT DISTINCT p.ciudad AS ciudad_proveedor, c.ciudad AS ciudad_cliente
FROM Proveedor p
         JOIN Pedido pd ON p.id_proveedor = pd.id_proveedor
         JOIN Cliente c ON pd.id_cliente = c.id_cliente
WHERE p.ciudad <> c.ciudad
GROUP BY p.ciudad, c.ciudad;

-- 8. Hallar el nombre de los proveedores cuya categoría sea mayor que la de todos los proveedores que proveen el artículo Pasta de Dientes.
SELECT p.nombre
FROM Proveedor p
JOIN Provee pr ON p.id_proveedor = pr.id_proveedor
JOIN Articulo a ON pr.id_articulo = a.id_articulo
WHERE p.categoria > (
    SELECT MAX(p2.categoria)
    FROM Proveedor p2
    JOIN Provee pr2 ON p2.id_proveedor = pr2.id_proveedor
    JOIN Articulo a2 ON pr2.id_articulo = a2.id_articulo
    WHERE a2.descripcion = 'Pasta de Dientes'
)
GROUP BY p.nombre;

-- 9. Hallar los proveedores que proveen el artículo más caro que haya sido comprado alguna vez por un cliente de la ciudad de CDMX.
SELECT p.id_proveedor, p.nombre, a.descripcion, a.precio
FROM Proveedor p
JOIN Provee pr ON p.id_proveedor = pr.id_proveedor
JOIN Articulo a ON pr.id_articulo = a.id_articulo
JOIN Pedido pd ON pr.id_articulo = pd.id_articulo
JOIN Cliente c ON pd.id_cliente = c.id_cliente
WHERE a.precio = (
    SELECT MAX(a2.precio)
    FROM Articulo a2
    JOIN Provee pr2 ON a2.id_articulo = pr2.id_articulo
    JOIN Pedido pd2 ON pr2.id_articulo = pd2.id_articulo
    JOIN Cliente c2 ON pd2.id_cliente = c2.id_cliente
    WHERE c2.ciudad = 'CDMX'
);

-- 10. Hallar los clientes que han pedido 2 o más artículos distintos.
SELECT c.id_cliente, c.nombre
FROM Cliente c
         JOIN Pedido p ON c.id_cliente = p.id_cliente
GROUP BY c.id_cliente, c.nombre
HAVING COUNT(DISTINCT p.id_articulo) >= 2;

-- 11. Hallar los proveedores que no tienen ningún pedido en los que el cliente es de la ciudad de Altamira y el artículo es producido en Monterrey.
SELECT *
FROM Proveedor
WHERE id_proveedor NOT IN (SELECT DISTINCT Pedido.id_proveedor
                           FROM Pedido
                                    JOIN Cliente ON Pedido.id_cliente = Cliente.id_cliente
                                    JOIN Articulo ON Pedido.id_articulo = Articulo.id_articulo
                           WHERE Cliente.ciudad = 'Altamira'
                             AND Articulo.ciudad = 'Monterrey');

-- 12. Hallar la cantidad de artículos diferentes que son provistos por cada uno de los proveedores de la base de datos.
SELECT p.id_proveedor, p.nombre, COUNT(DISTINCT pr.id_articulo) AS cantidad_articulos
FROM Proveedor p
         LEFT JOIN Provee pr ON p.id_proveedor = pr.id_proveedor
GROUP BY p.id_proveedor;


