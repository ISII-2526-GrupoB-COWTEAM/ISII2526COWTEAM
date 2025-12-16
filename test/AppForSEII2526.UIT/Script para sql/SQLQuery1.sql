-- =========================
-- USUARIOS DE PRUEBA (Identity)
-- =========================

DECLARE @UserId1 NVARCHAR(450) = NEWID();
DECLARE @UserId2 NVARCHAR(450) = NEWID();

INSERT INTO AspNetUsers
(
    Id,
    Name,
    Surname,
    Country,
    UserName,
    NormalizedUserName,
    Email,
    NormalizedEmail,
    EmailConfirmed,
    PhoneNumberConfirmed,
    TwoFactorEnabled,
    LockoutEnabled,
    AccessFailedCount
)
VALUES
(
    @UserId1,
    'Carlos',
    'García',
    'Spain',
    'carlos@test.com',
    'CARLOS@TEST.COM',
    'carlos@test.com',
    'CARLOS@TEST.COM',
    1, 0, 0, 1, 0
),
(
    @UserId2,
    'Laura',
    'Martínez',
    'Spain',
    'laura@test.com',
    'LAURA@TEST.COM',
    'laura@test.com',
    'LAURA@TEST.COM',
    1, 0, 0, 1, 0
);

-- =========================
-- DATOS BÁSICOS
-- =========================

INSERT INTO Scale (Name) VALUES
('Low'), ('Medium'), ('High');

INSERT INTO Model (NameModel) VALUES
('iPhone 14'),
('Samsung Galaxy S23'),
('MacBook Pro'),
('Dell XPS 15');

INSERT INTO Device
(Brand, Color, Description, Name, PriceForPurchase, PriceForRent, Quality,
 QuantityForPurchase, QuantityForRent, Year, ModelId)
VALUES
('Apple', 'Black', 'High-end smartphone', 'iPhone 14', 999, 25, 'New', 10, 5, 2023, 1),
('Samsung', 'White', 'Android flagship phone', 'Galaxy S23', 899, 22, 'New', 8, 6, 2023, 2),
('Apple', 'Gray', 'Professional laptop', 'MacBook Pro', 2200, 50, 'New', 4, 2, 2022, 3),
('Dell', 'Silver', 'Powerful Windows laptop', 'XPS 15', 1800, 45, 'New', 6, 3, 2022, 4);

INSERT INTO Repair (Cost, Description, Name, ScaleId)
VALUES
(50, 'Screen replacement', 'Screen repair', 2),
(80, 'Battery replacement', 'Battery repair', 1),
(150, 'Motherboard fix', 'Hardware repair', 3);

-- =========================
-- PURCHASE
-- =========================
INSERT INTO Purchase
(TotalPrice, TotalQuantity, PurchaseDate, DeliveryAddress, ApplicationUserId, PaymentMethod)
VALUES
(1898, 2, GETDATE(), 'Calle Mayor 1', @UserId1, 1);

INSERT INTO PurchaseItem
(DeviceId, PurchaseId, Description, Price, Quantity)
VALUES
(1, 1, 'iPhone purchase', 999, 1),
(2, 1, 'Samsung purchase', 899, 1);

-- =========================
-- RENTAL
-- =========================
INSERT INTO Rental
(TotalPrice, RentalDate, RentalDateFrom, RentalDateTo, DeliveryAddress, PaymentMethod, ApplicationUserId)
VALUES
(95, GETDATE(), GETDATE(), DATEADD(day, 7, GETDATE()), 'Calle Sol 23', 2, @UserId2);

INSERT INTO RentDevice
(DeviceId, RentalId, Quantity, Price)
VALUES
(3, 1, 1, 50),
(4, 1, 1, 45);

-- =========================
-- RECEIPT
-- =========================
INSERT INTO Receipt
(ApplicationUserId, PaymentMethod, DeliveryAddress, ReceiptDate, TotalPrice)
VALUES
(@UserId1, 1, 'Calle Luna 7', GETDATE(), 130);

INSERT INTO ReceiptItem
(ReceiptID, RepairID, Model)
VALUES
(1, 1, 'iPhone 14'),
(1, 2, 'Samsung Galaxy S23');

-- =========================
-- REVIEW
-- =========================
INSERT INTO Review
(ApplicationUserId, DateOfReview, OverallRating, ReviewTitle)
VALUES
(@UserId2, GETDATE(), 5, 'Excellent service');

INSERT INTO ReviewItem
(DeviceId, ReviewId, Comments, Rating)
VALUES
(1, 1, 'Amazing phone', 5),
(2, 1, 'Very good quality', 4);
