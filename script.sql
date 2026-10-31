CREATE DATABASE IF NOT EXISTS TiendaDB;
USE TiendaDB;

CREATE TABLE Productos (
    Id          INT AUTO_INCREMENT PRIMARY KEY,
    Nombre      VARCHAR(100)  NOT NULL,
    Descripcion VARCHAR(250)  NULL,
    Precio      DECIMAL(10,2) NOT NULL,
    Stock       INT           NOT NULL DEFAULT 0,
    Categoria   VARCHAR(50)   NULL
);