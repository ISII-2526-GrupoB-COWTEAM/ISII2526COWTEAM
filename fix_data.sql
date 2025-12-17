-- ============================================================
-- SCRIPT DE CORRECCIÓN DE DATOS (FIX DATA) v2
-- Ejecuta esto en tu base de datos para corregir nombres, asociaciones y APELLIDOS
-- ============================================================

-- 1. Corregir nombres de Modelos mal formateados
UPDATE Model SET NameModel = 'iPhone 11' WHERE NameModel = 'Iphone11';
UPDATE Model SET NameModel = 'Samsung Galaxy A11' WHERE NameModel = 'SamsungA11';
UPDATE Model SET NameModel = 'Huawei P30' WHERE NameModel = 'HuaweiP30';
UPDATE Model SET NameModel = 'iPhone 17 Pro' WHERE NameModel = 'Iphone17Pro';

-- 2. Asegurar que existen los Modelos correctos para nuestros tests (iPhone 14, Galaxy S23)
IF NOT EXISTS (SELECT 1 FROM Model WHERE NameModel = 'iPhone 14')
BEGIN
    INSERT INTO Model (NameModel) VALUES ('iPhone 14');
END

IF NOT EXISTS (SELECT 1 FROM Model WHERE NameModel = 'Samsung Galaxy S23')
BEGIN
    INSERT INTO Model (NameModel) VALUES ('Samsung Galaxy S23');
END

-- 3. Corregir la asociación de Dispositivos a Modelos
DECLARE @ModelId_iPhone14 INT = (SELECT Id FROM Model WHERE NameModel = 'iPhone 14');
DECLARE @ModelId_S23 INT = (SELECT Id FROM Model WHERE NameModel = 'Samsung Galaxy S23');

UPDATE Device 
SET ModelId = @ModelId_iPhone14 
WHERE Name LIKE '%iPhone 14%';

UPDATE Device 
SET ModelId = @ModelId_S23 
WHERE Name LIKE '%Galaxy S23%';

-- 4. ACTUALIZAR APELLIDOS (> 10 caracteres)
UPDATE AspNetUsers 
SET Surname = 'García-Fernández' 
WHERE Name = 'Carlos';

UPDATE AspNetUsers 
SET Surname = 'Martínez-Rodríguez' 
WHERE Name = 'Laura';

-- 5. Verificación
SELECT Name, Surname FROM AspNetUsers;
