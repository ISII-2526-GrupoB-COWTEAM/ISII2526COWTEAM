-- =========================================================================================
-- SCRIPT DE DATOS DE PRUEBA (SEED DATA) v2
-- Optimizado para pasar los tests UIT que usan diferentes variaciones de nombres de usuario.
-- =========================================================================================

BEGIN TRANSACTION;

-- =========================
-- 1. USUARIOS (AspNetUsers)
-- =========================

-- 1.1 Usuario Original (Base para otros tests)
-- Carlos García Fernández (carlos@test.com)
DECLARE @CarlosOriginalId NVARCHAR(450);
SELECT @CarlosOriginalId = Id FROM AspNetUsers WHERE Email = 'carlos@test.com';
IF @CarlosOriginalId IS NULL
BEGIN
    SET @CarlosOriginalId = NEWID();
    INSERT INTO AspNetUsers (Id, Name, Surname, Country, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
    VALUES (@CarlosOriginalId, 'Carlos', 'García Fernández', 'Spain', 'carlos@test.com', 'CARLOS@TEST.COM', 'carlos@test.com', 'CARLOS@TEST.COM', 1, 
    'AQAAAAIAAYagAAAAEI+d3+j...', NEWID(), NEWID(), 0, 0, 1, 0);
END

-- 1.2 Usuario para Test de COMPRA (CP_01_02)
-- El test envía "Carlos@test.com" como Nombre y "García Fernández" como Apellido.
-- Creamos un usuario que coincida exactamente con estos criterios de búsqueda.
IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Name = 'Carlos@test.com' AND Surname = 'García Fernández')
BEGIN
    INSERT INTO AspNetUsers (Id, Name, Surname, Country, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
    VALUES (NEWID(), 'Carlos@test.com', 'García Fernández', 'Spain', 'carlos_compra@test.com', 'CARLOS_COMPRA@TEST.COM', 'carlos_compra@test.com', 'CARLOS_COMPRA@TEST.COM', 1, 
    'AQAAAAIAAYagAAAAEI+d3+j...', NEWID(), NEWID(), 0, 0, 1, 0);
END

-- 1.3 Usuario para Test de REPARACIÓN (UC1_1)
-- El test envía "Carlos" como Nombre y "García" como Apellido (sin Fernández).
IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Name = 'Carlos' AND Surname = 'García')
BEGIN
    INSERT INTO AspNetUsers (Id, Name, Surname, Country, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
    VALUES (NEWID(), 'Carlos', 'García', 'Spain', 'carlos_reparacion@test.com', 'CARLOS_REPARACION@TEST.COM', 'carlos_reparacion@test.com', 'CARLOS_REPARACION@TEST.COM', 1, 
    'AQAAAAIAAYagAAAAEI+d3+j...', NEWID(), NEWID(), 0, 0, 1, 0);
END

-- 1.4 Usuario Laura (Original)
-- Laura Martínez (laura@test.com)
DECLARE @LauraId NVARCHAR(450);
SELECT @LauraId = Id FROM AspNetUsers WHERE Email = 'laura@test.com';
IF @LauraId IS NULL
BEGIN
    SET @LauraId = NEWID();
    INSERT INTO AspNetUsers (Id, Name, Surname, Country, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
    VALUES (@LauraId, 'Laura', 'Martínez', 'Spain', 'laura@test.com', 'LAURA@TEST.COM', 'laura@test.com', 'LAURA@TEST.COM', 1, 
    'AQAAAAIAAYagAAAAEI+d3+j...', NEWID(), NEWID(), 0, 0, 1, 0);
END


-- =========================
-- 2. SCALES
-- =========================
SET IDENTITY_INSERT Scale ON;
IF NOT EXISTS (SELECT 1 FROM Scale WHERE Id = 1) INSERT INTO Scale (Id, Name) VALUES (1, 'Low');
IF NOT EXISTS (SELECT 1 FROM Scale WHERE Id = 2) INSERT INTO Scale (Id, Name) VALUES (2, 'Medium');
IF NOT EXISTS (SELECT 1 FROM Scale WHERE Id = 3) INSERT INTO Scale (Id, Name) VALUES (3, 'High');
SET IDENTITY_INSERT Scale OFF;

-- =========================
-- 3. MODELS
-- =========================
SET IDENTITY_INSERT Model ON;
IF NOT EXISTS (SELECT 1 FROM Model WHERE Id = 1) INSERT INTO Model (Id, NameModel) VALUES (1, 'iPhone 14');
IF NOT EXISTS (SELECT 1 FROM Model WHERE Id = 2) INSERT INTO Model (Id, NameModel) VALUES (2, 'Samsung Galaxy S23');
IF NOT EXISTS (SELECT 1 FROM Model WHERE Id = 3) INSERT INTO Model (Id, NameModel) VALUES (3, 'MacBook Pro');
IF NOT EXISTS (SELECT 1 FROM Model WHERE Id = 4) INSERT INTO Model (Id, NameModel) VALUES (4, 'Dell XPS 15');
SET IDENTITY_INSERT Model OFF;

-- =========================
-- 4. DEVICES
-- =========================
SET IDENTITY_INSERT Device ON;

-- iPhone 14 (Usado en Compra UIT)
IF NOT EXISTS (SELECT 1 FROM Device WHERE Id = 1)
    INSERT INTO Device (Id, Brand, Color, Description, Name, PriceForPurchase, PriceForRent, Quality, QuantityForPurchase, QuantityForRent, Year, ModelId)
    VALUES (1, 'Apple', 'Black', 'High-end smartphone', 'iPhone 14', 999, 25, 'New', 10, 5, 2023, 1);

-- Galaxy S23 (Usado en Compra UIT y Repair UIT)
IF NOT EXISTS (SELECT 1 FROM Device WHERE Id = 2)
    INSERT INTO Device (Id, Brand, Color, Description, Name, PriceForPurchase, PriceForRent, Quality, QuantityForPurchase, QuantityForRent, Year, ModelId)
    VALUES (2, 'Samsung', 'White', 'Android flagship phone', 'Galaxy S23', 899, 22, 'New', 8, 6, 2023, 2);

-- MacBook Pro
IF NOT EXISTS (SELECT 1 FROM Device WHERE Id = 3)
    INSERT INTO Device (Id, Brand, Color, Description, Name, PriceForPurchase, PriceForRent, Quality, QuantityForPurchase, QuantityForRent, Year, ModelId)
    VALUES (3, 'Apple', 'Gray', 'Professional laptop', 'MacBook Pro', 2200, 50, 'New', 4, 2, 2022, 3);

-- Dell XPS 15
IF NOT EXISTS (SELECT 1 FROM Device WHERE Id = 4)
    INSERT INTO Device (Id, Brand, Color, Description, Name, PriceForPurchase, PriceForRent, Quality, QuantityForPurchase, QuantityForRent, Year, ModelId)
    VALUES (4, 'Dell', 'Silver', 'Powerful Windows laptop', 'XPS 15', 1800, 45, 'New', 6, 3, 2022, 4);

SET IDENTITY_INSERT Device OFF;

-- =========================
-- 5. REPAIRS
-- =========================
-- Usados en Repair UIT ("Screen repair", "Battery repair", "Hardware repair")
SET IDENTITY_INSERT Repair ON;
IF NOT EXISTS (SELECT 1 FROM Repair WHERE Id = 1) INSERT INTO Repair (Id, Cost, Description, Name, ScaleId) VALUES (1, 50, 'Screen replacement', 'Screen repair', 2);
IF NOT EXISTS (SELECT 1 FROM Repair WHERE Id = 2) INSERT INTO Repair (Id, Cost, Description, Name, ScaleId) VALUES (2, 80, 'Battery replacement', 'Battery repair', 1);
IF NOT EXISTS (SELECT 1 FROM Repair WHERE Id = 3) INSERT INTO Repair (Id, Cost, Description, Name, ScaleId) VALUES (3, 150, 'Motherboard fix', 'Hardware repair', 3);
SET IDENTITY_INSERT Repair OFF;

-- ===============================================
-- DATOS TRANSACCIONALES (Purchases, Rentals, etc)
-- ===============================================
-- Se añaden algunos datos de ejemplo si no existen, para pruebas manuales o tests de lectura.

-- Purchase (Carlos Original)
IF NOT EXISTS (SELECT 1 FROM Purchase WHERE ApplicationUserId = @CarlosOriginalId AND TotalPrice = 1898)
BEGIN
    INSERT INTO Purchase (TotalPrice, TotalQuantity, PurchaseDate, DeliveryAddress, ApplicationUserId, PaymentMethod)
    VALUES (1898, 2, GETDATE(), 'Calle Mayor 1', @CarlosOriginalId, 1);
    
    DECLARE @NewPurchaseId INT = SCOPE_IDENTITY();
    
    INSERT INTO PurchaseItem (PurchaseId, DeviceId, Description, Price, Quantity) VALUES (@NewPurchaseId, 1, 'iPhone 14', 999, 1);
    INSERT INTO PurchaseItem (PurchaseId, DeviceId, Description, Price, Quantity) VALUES (@NewPurchaseId, 2, 'Galaxy S23', 899, 1);
END

-- Rental (Laura)
IF NOT EXISTS (SELECT 1 FROM Rental WHERE ApplicationUserId = @LauraId AND TotalPrice = 95)
BEGIN
    INSERT INTO Rental (TotalPrice, RentalDate, RentalDateFrom, RentalDateTo, DeliveryAddress, PaymentMethod, ApplicationUserId)
    VALUES (95, GETDATE(), GETDATE(), DATEADD(day, 7, GETDATE()), 'Calle Sol 23', 2, @LauraId);
    
    DECLARE @NewRentalId INT = SCOPE_IDENTITY();
    
    INSERT INTO RentDevice (RentalId, DeviceId, Quantity, Price) VALUES (@NewRentalId, 3, 1, 50);
    INSERT INTO RentDevice (RentalId, DeviceId, Quantity, Price) VALUES (@NewRentalId, 4, 1, 45);
END

COMMIT TRANSACTION;
PRINT 'Datos de prueba insertados/verificados correctamente (incluyendo usuarios para tests UIT).';
