-- ============================================================
--  Auto Parts Store -- Complete Database Setup Script
--  Engine : SQL Server 2016+ (compatible with 2019 / 2022)
--
--  HOW TO USE:
--    Fresh install  -> run entire file on an empty database
--    Existing DB    -> run SECTION 5 (Migration Patches) only
-- ============================================================

USE master;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'AutoPartsStoreDB')
    CREATE DATABASE AutoPartsStoreDB COLLATE Arabic_CI_AS;
GO

USE AutoPartsStoreDB;
GO

-- ============================================================
-- SECTION 1 : TABLES
-- ============================================================

IF OBJECT_ID('person','U') IS NULL
CREATE TABLE person (
    ID         INT           IDENTITY(1,1) PRIMARY KEY,
    PersonName NVARCHAR(150) NOT NULL,
    Phone      NVARCHAR(20)  NULL,
    address    NVARCHAR(255) NULL,
    isDeleted  BIT           NOT NULL DEFAULT 0
);
GO

IF OBJECT_ID('partscategories','U') IS NULL
CREATE TABLE partscategories (
    categoryID   INT           IDENTITY(1,1) PRIMARY KEY,
    categoryName NVARCHAR(100) NOT NULL,
    IsDeleted    BIT           NOT NULL DEFAULT 0
);
GO

IF OBJECT_ID('Parts','U') IS NULL
CREATE TABLE Parts (
    PartID        INT            IDENTITY(1,1) PRIMARY KEY,
    PartName      NVARCHAR(200)  NOT NULL,
    PartNumber    NVARCHAR(50)   NOT NULL,
    PurchasePrice DECIMAL(18,2)  NOT NULL DEFAULT 0 CONSTRAINT chk_Parts_PurchasePrice CHECK (PurchasePrice >= 0),
    SellingPrice  DECIMAL(18,2)  NOT NULL DEFAULT 0 CONSTRAINT chk_Parts_SellingPrice  CHECK (SellingPrice  >= 0),
    Quantity      INT            NOT NULL DEFAULT 0 CONSTRAINT chk_Parts_Quantity      CHECK (Quantity      >= 0),
    MinimumStock  INT            NOT NULL DEFAULT 0 CONSTRAINT chk_Parts_MinimumStock  CHECK (MinimumStock  >= 0),
    CategoryID    INT            NOT NULL CONSTRAINT fk_Parts_Category REFERENCES partscategories(categoryID),
    Notes         NVARCHAR(500)  NULL,
    IsDeleted     BIT            NOT NULL DEFAULT 0
);
GO

IF OBJECT_ID('customers','U') IS NULL
CREATE TABLE customers (
    ID      INT           PRIMARY KEY CONSTRAINT fk_customers_person REFERENCES person(ID),
    Balance DECIMAL(18,2) NOT NULL DEFAULT 0
);
GO

-- Table name kept as 'supplieres' (original spelling) to avoid breaking existing queries
IF OBJECT_ID('supplieres','U') IS NULL
CREATE TABLE supplieres (
    ID      INT           PRIMARY KEY CONSTRAINT fk_supplieres_person REFERENCES person(ID),
    Balance DECIMAL(18,2) NOT NULL DEFAULT 0
);
GO

IF OBJECT_ID('SystemRoles','U') IS NULL
CREATE TABLE SystemRoles (
    RoleID      INT           IDENTITY(1,1) PRIMARY KEY,
    RoleName    NVARCHAR(50)  NOT NULL UNIQUE,
    Description NVARCHAR(255) NULL,
    IsBuiltIn   BIT           NOT NULL DEFAULT 0
);
GO

IF OBJECT_ID('Users','U') IS NULL
CREATE TABLE Users (
    ID       INT           PRIMARY KEY CONSTRAINT fk_Users_person REFERENCES person(ID),
    UserName NVARCHAR(50)  NOT NULL UNIQUE,
    password NVARCHAR(256) NOT NULL,      -- PBKDF2-SHA256 Base64 hash
    Role     NVARCHAR(50)  NOT NULL DEFAULT 'Cashier'
);
GO

IF OBJECT_ID('Invoices','U') IS NULL
CREATE TABLE Invoices (
    ID            INT            IDENTITY(1,1) PRIMARY KEY,
    Time          DATETIME       NOT NULL DEFAULT GETDATE(),
    PaymentMethod NVARCHAR(50)   NOT NULL,
    TotalAmount   DECIMAL(18,2)  NOT NULL CONSTRAINT chk_Invoices_Total CHECK (TotalAmount >= 0),
    paidamount    DECIMAL(18,2)  NOT NULL DEFAULT 0 CONSTRAINT chk_Invoices_Paid CHECK (paidamount >= 0),
    invoiceType   NVARCHAR(20)   NOT NULL
        CONSTRAINT chk_Invoices_Type CHECK (invoiceType IN (N'مبيعات', N'توريد')),
    UserID        INT            NOT NULL CONSTRAINT fk_Invoices_User     REFERENCES Users(ID),
    customerID    INT            NULL     CONSTRAINT fk_Invoices_Customer REFERENCES customers(ID),
    supplierID    INT            NULL     CONSTRAINT fk_Invoices_Supplier REFERENCES supplieres(ID),
    -- Exactly one party per invoice
    CONSTRAINT chk_Invoices_Party CHECK (
        (customerID IS NOT NULL AND supplierID IS NULL) OR
        (customerID IS NULL     AND supplierID IS NOT NULL)
    )
);
GO

IF OBJECT_ID('InvoiceDetails','U') IS NULL
CREATE TABLE InvoiceDetails (
    ID        INT           IDENTITY(1,1) PRIMARY KEY,
    InvoiceID INT           NOT NULL CONSTRAINT fk_InvDet_Invoice REFERENCES Invoices(ID),
    PartID    INT           NOT NULL CONSTRAINT fk_InvDet_Part    REFERENCES Parts(PartID),
    Quantity  INT           NOT NULL CONSTRAINT chk_InvDet_Qty    CHECK (Quantity  > 0),
    PartPrice DECIMAL(18,2) NOT NULL CONSTRAINT chk_InvDet_Price  CHECK (PartPrice >= 0),
    Total     DECIMAL(18,2) NOT NULL CONSTRAINT chk_InvDet_Total  CHECK (Total     >= 0)
);
GO

IF OBJECT_ID('inventoryTransactions','U') IS NULL
CREATE TABLE inventoryTransactions (
    ID               INT           IDENTITY(1,1) PRIMARY KEY,
    TransactionsType NVARCHAR(50)  NOT NULL,
    Quantity         INT           NOT NULL,
    Date             DATETIME      NOT NULL DEFAULT GETDATE(),
    notes            NVARCHAR(500) NULL,
    PartId           INT           NOT NULL CONSTRAINT fk_InvTrans_Part REFERENCES Parts(PartID),
    userId           INT           NULL     CONSTRAINT fk_InvTrans_User REFERENCES Users(ID)
);
GO

IF OBJECT_ID('Returns','U') IS NULL
CREATE TABLE Returns (
    ReturnID            INT            IDENTITY(1,1) PRIMARY KEY,
    InvoiceID           INT            NOT NULL CONSTRAINT fk_Returns_Invoice REFERENCES Invoices(ID),
    ReturnDate          DATETIME       NOT NULL DEFAULT GETDATE(),
    PaymentMethod       NVARCHAR(50)   NOT NULL,
    TotalRefundedAmount DECIMAL(18,2)  NOT NULL CONSTRAINT chk_Returns_Total CHECK (TotalRefundedAmount >= 0),
    CashReturned        DECIMAL(18,2)  NOT NULL DEFAULT 0 CONSTRAINT chk_Returns_Cash CHECK (CashReturned >= 0),
    UserID              INT            NOT NULL CONSTRAINT fk_Returns_User REFERENCES Users(ID),
    Notes               NVARCHAR(500)  NULL
);
GO

IF OBJECT_ID('ReturnDetails','U') IS NULL
CREATE TABLE ReturnDetails (
    ID           INT           IDENTITY(1,1) PRIMARY KEY,
    ReturnID     INT           NOT NULL CONSTRAINT fk_RetDet_Return REFERENCES Returns(ReturnID),
    PartID       INT           NOT NULL CONSTRAINT fk_RetDet_Part   REFERENCES Parts(PartID),
    Quantity     INT           NOT NULL CONSTRAINT chk_RetDet_Qty   CHECK (Quantity > 0),
    RefundAmount DECIMAL(18,2) NOT NULL CONSTRAINT chk_RetDet_Amt   CHECK (RefundAmount >= 0)
);
GO

IF OBJECT_ID('SafeTransactions','U') IS NULL
CREATE TABLE SafeTransactions (
    ID              INT            IDENTITY(1,1) PRIMARY KEY,
    Amount          DECIMAL(18,2)  NOT NULL CONSTRAINT chk_Safe_Amount CHECK (Amount > 0),
    TransactionType NVARCHAR(20)   NOT NULL
        CONSTRAINT chk_Safe_Type CHECK (TransactionType IN (N'إيداع', N'سحب')),
    Description     NVARCHAR(500)  NULL,
    TransactionDate DATETIME       NOT NULL DEFAULT GETDATE(),
    UserID          INT            NOT NULL CONSTRAINT fk_Safe_User    REFERENCES Users(ID),
    InvoiceID       INT            NULL     CONSTRAINT fk_Safe_Invoice REFERENCES Invoices(ID),
    PersonID        INT            NULL     CONSTRAINT fk_Safe_Person  REFERENCES person(ID)
);
GO

IF OBJECT_ID('SystemPermissions','U') IS NULL
CREATE TABLE SystemPermissions (
    PermissionID  INT           IDENTITY(1,1) PRIMARY KEY,
    PermissionKey NVARCHAR(100) NOT NULL UNIQUE,
    DisplayName   NVARCHAR(150) NOT NULL,
    Module        NVARCHAR(50)  NOT NULL
);
GO

IF OBJECT_ID('RolePermissions','U') IS NULL
CREATE TABLE RolePermissions (
    RoleID       INT NOT NULL CONSTRAINT fk_RP_Role REFERENCES SystemRoles(RoleID),
    PermissionID INT NOT NULL CONSTRAINT fk_RP_Perm REFERENCES SystemPermissions(PermissionID),
    IsGranted    BIT NOT NULL DEFAULT 1,
    CONSTRAINT pk_RolePermissions PRIMARY KEY (RoleID, PermissionID)
);
GO

IF OBJECT_ID('StoreSettings','U') IS NULL
CREATE TABLE StoreSettings (
    ID           INT            IDENTITY(1,1) PRIMARY KEY,
    SettingKey   NVARCHAR(100)  NOT NULL UNIQUE,
    SettingValue NVARCHAR(2000) NULL,
    SettingGroup NVARCHAR(50)   NOT NULL DEFAULT N'General'
);
GO

-- ============================================================
-- SECTION 2 : INDEXES
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_Parts_PartNumber' AND object_id=OBJECT_ID('Parts'))
    CREATE INDEX ix_Parts_PartNumber ON Parts(PartNumber) WHERE IsDeleted = 0;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_Parts_PartName' AND object_id=OBJECT_ID('Parts'))
    CREATE INDEX ix_Parts_PartName ON Parts(PartName) WHERE IsDeleted = 0;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_Parts_CategoryID' AND object_id=OBJECT_ID('Parts'))
    CREATE INDEX ix_Parts_CategoryID ON Parts(CategoryID) WHERE IsDeleted = 0;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_Parts_LowStock' AND object_id=OBJECT_ID('Parts'))
    CREATE INDEX ix_Parts_LowStock ON Parts(Quantity, MinimumStock) WHERE IsDeleted = 0;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_person_PersonName' AND object_id=OBJECT_ID('person'))
    CREATE INDEX ix_person_PersonName ON person(PersonName) WHERE isDeleted = 0;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_Invoices_Time' AND object_id=OBJECT_ID('Invoices'))
    CREATE INDEX ix_Invoices_Time ON Invoices(Time);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_Invoices_CustomerID' AND object_id=OBJECT_ID('Invoices'))
    CREATE INDEX ix_Invoices_CustomerID ON Invoices(customerID) WHERE customerID IS NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_Invoices_SupplierID' AND object_id=OBJECT_ID('Invoices'))
    CREATE INDEX ix_Invoices_SupplierID ON Invoices(supplierID) WHERE supplierID IS NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_Invoices_Type_Time' AND object_id=OBJECT_ID('Invoices'))
    CREATE INDEX ix_Invoices_Type_Time ON Invoices(invoiceType, Time);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_InvDet_InvoiceID' AND object_id=OBJECT_ID('InvoiceDetails'))
    CREATE INDEX ix_InvDet_InvoiceID ON InvoiceDetails(InvoiceID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_InvDet_PartID' AND object_id=OBJECT_ID('InvoiceDetails'))
    CREATE INDEX ix_InvDet_PartID ON InvoiceDetails(PartID);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_InvTrans_PartId' AND object_id=OBJECT_ID('inventoryTransactions'))
    CREATE INDEX ix_InvTrans_PartId ON inventoryTransactions(PartId, Date);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_InvTrans_Date' AND object_id=OBJECT_ID('inventoryTransactions'))
    CREATE INDEX ix_InvTrans_Date ON inventoryTransactions(Date);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_Returns_InvoiceID' AND object_id=OBJECT_ID('Returns'))
    CREATE INDEX ix_Returns_InvoiceID ON Returns(InvoiceID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_Returns_ReturnDate' AND object_id=OBJECT_ID('Returns'))
    CREATE INDEX ix_Returns_ReturnDate ON Returns(ReturnDate);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_RetDet_ReturnID' AND object_id=OBJECT_ID('ReturnDetails'))
    CREATE INDEX ix_RetDet_ReturnID ON ReturnDetails(ReturnID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_RetDet_PartID' AND object_id=OBJECT_ID('ReturnDetails'))
    CREATE INDEX ix_RetDet_PartID ON ReturnDetails(PartID);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_Safe_Date' AND object_id=OBJECT_ID('SafeTransactions'))
    CREATE INDEX ix_Safe_Date ON SafeTransactions(TransactionDate);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='ix_Safe_PersonID' AND object_id=OBJECT_ID('SafeTransactions'))
    CREATE INDEX ix_Safe_PersonID ON SafeTransactions(PersonID) WHERE PersonID IS NOT NULL;
GO

-- ============================================================
-- SECTION 3 : SEED DATA
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM SystemRoles WHERE RoleName=N'Admin')
    INSERT INTO SystemRoles (RoleName, Description, IsBuiltIn) VALUES (N'Admin',       N'مدير النظام',  1);
IF NOT EXISTS (SELECT 1 FROM SystemRoles WHERE RoleName=N'Cashier')
    INSERT INTO SystemRoles (RoleName, Description, IsBuiltIn) VALUES (N'Cashier',     N'كاشير',        1);
IF NOT EXISTS (SELECT 1 FROM SystemRoles WHERE RoleName=N'StoreKeeper')
    INSERT INTO SystemRoles (RoleName, Description, IsBuiltIn) VALUES (N'StoreKeeper', N'أمين مخزن',    1);
GO

MERGE SystemPermissions AS t
USING (VALUES
    (N'Inventory.View',   N'عرض المخزون',           N'Inventory'),
    (N'Inventory.Edit',   N'تعديل المخزون',          N'Inventory'),
    (N'Sales.View',       N'عرض المبيعات',           N'Sales'),
    (N'Sales.Create',     N'إنشاء فاتورة مبيعات',    N'Sales'),
    (N'Purchases.View',   N'عرض المشتريات',          N'Purchases'),
    (N'Purchases.Create', N'إنشاء فاتورة مشتريات',  N'Purchases'),
    (N'Returns.View',     N'عرض المرتجعات',          N'Returns'),
    (N'Returns.Create',   N'إنشاء مرتجع',            N'Returns'),
    (N'People.View',      N'عرض العملاء والموردين',  N'People'),
    (N'People.Edit',      N'تعديل العملاء والموردين',N'People'),
    (N'Safe.View',        N'عرض الخزينة',            N'Safe'),
    (N'Safe.Transact',    N'إجراء حركات خزينة',      N'Safe'),
    (N'Reports.View',     N'عرض التقارير',           N'Reports'),
    (N'Settings.View',    N'عرض الإعدادات',          N'Settings'),
    (N'Settings.Edit',    N'تعديل الإعدادات',        N'Settings'),
    (N'Users.View',       N'عرض المستخدمين',         N'Users'),
    (N'Users.Edit',       N'تعديل المستخدمين',       N'Users')
) AS s(PermissionKey, DisplayName, Module)
ON t.PermissionKey = s.PermissionKey
WHEN NOT MATCHED THEN
    INSERT (PermissionKey, DisplayName, Module)
    VALUES (s.PermissionKey, s.DisplayName, s.Module);
GO

MERGE StoreSettings AS t
USING (VALUES
    (N'Store.Name',       N'متجر قطع الغيار', N'Store'),
    (N'Store.Phone',      N'',                 N'Store'),
    (N'Store.Address',    N'',                 N'Store'),
    (N'Store.TaxNumber',  N'',                 N'Store'),
    (N'UI.Theme',         N'Light',            N'UI'),
    (N'UI.PrintFormat',   N'A4',               N'UI'),
    (N'Security.AdminPin',N'CHANGE_ME_ON_FIRST_LOGIN', N'Security')
) AS s(SettingKey, SettingValue, SettingGroup)
ON t.SettingKey = s.SettingKey
WHEN NOT MATCHED THEN
    INSERT (SettingKey, SettingValue, SettingGroup)
    VALUES (s.SettingKey, s.SettingValue, s.SettingGroup);
GO

-- ============================================================
-- SECTION 4 : REPORTING VIEWS
-- ============================================================

CREATE OR ALTER VIEW vw_StockAudit AS
SELECT p.PartID, p.PartName, p.PartNumber,
    c.categoryName AS Category,
    p.Quantity, p.PurchasePrice, p.SellingPrice,
    CAST(p.Quantity * p.PurchasePrice AS DECIMAL(18,2)) AS TotalCostValue,
    CAST(p.Quantity * p.SellingPrice  AS DECIMAL(18,2)) AS TotalSellingValue,
    CAST(p.Quantity * (p.SellingPrice - p.PurchasePrice) AS DECIMAL(18,2)) AS PotentialProfit,
    p.MinimumStock,
    CASE WHEN p.Quantity <= 0              THEN N'نفد المخزون'
         WHEN p.Quantity <= p.MinimumStock THEN N'تحت الحد الأدنى'
         ELSE                                   N'طبيعي' END AS StockStatus
FROM Parts p WITH (NOLOCK)
JOIN partscategories c WITH (NOLOCK) ON p.CategoryID = c.categoryID
WHERE p.IsDeleted = 0;
GO

CREATE OR ALTER VIEW vw_StagnantItems AS
SELECT p.PartID, p.PartName, p.PartNumber, c.categoryName AS Category,
    p.Quantity, p.PurchasePrice,
    CAST(p.Quantity * p.PurchasePrice AS DECIMAL(18,2)) AS TotalValue,
    COALESCE(DATEDIFF(DAY, MAX(i.Time), GETDATE()), 9999) AS DaysSinceLastSale
FROM Parts p WITH (NOLOCK)
JOIN partscategories c WITH (NOLOCK) ON p.CategoryID = c.categoryID
LEFT JOIN InvoiceDetails id WITH (NOLOCK) ON id.PartID = p.PartID
LEFT JOIN Invoices       i  WITH (NOLOCK) ON id.InvoiceID = i.ID AND i.invoiceType = N'مبيعات'
WHERE p.IsDeleted = 0 AND p.Quantity > 0
GROUP BY p.PartID, p.PartName, p.PartNumber, c.categoryName, p.Quantity, p.PurchasePrice
HAVING COALESCE(DATEDIFF(DAY, MAX(i.Time), GETDATE()), 9999) >= 90;
GO

CREATE OR ALTER VIEW vw_LowStockAlert AS
SELECT p.PartID, p.PartName, p.PartNumber, c.categoryName AS Category,
    p.Quantity AS CurrentStock, p.MinimumStock,
    (p.MinimumStock - p.Quantity) AS Shortage,
    p.PurchasePrice,
    CAST((p.MinimumStock - p.Quantity) * p.PurchasePrice AS DECIMAL(18,2)) AS EstimatedReplenishmentCost
FROM Parts p WITH (NOLOCK)
JOIN partscategories c WITH (NOLOCK) ON p.CategoryID = c.categoryID
WHERE p.IsDeleted = 0 AND p.Quantity <= p.MinimumStock;
GO

CREATE OR ALTER VIEW vw_ProfitReport AS
SELECT i.ID AS InvoiceID, CAST(i.Time AS DATE) AS SaleDate,
    p.PartID, p.PartName, c.categoryName AS Category,
    CAST(d.Quantity AS DECIMAL(18,2)) AS Quantity,
    d.PartPrice AS SellingPrice, p.PurchasePrice,
    CAST(d.Total AS DECIMAL(18,2)) AS Revenue,
    CAST(d.Total - (d.Quantity * p.PurchasePrice) AS DECIMAL(18,2)) AS GrossProfit,
    CASE WHEN d.Total > 0
         THEN CAST(((d.Total - d.Quantity*p.PurchasePrice)/d.Total)*100 AS DECIMAL(18,2))
         ELSE 0 END AS ProfitMarginPct
FROM Invoices i WITH (NOLOCK)
JOIN InvoiceDetails  d WITH (NOLOCK) ON d.InvoiceID  = i.ID
JOIN Parts           p WITH (NOLOCK) ON d.PartID     = p.PartID
JOIN partscategories c WITH (NOLOCK) ON p.CategoryID = c.categoryID
WHERE i.invoiceType = N'مبيعات';
GO

CREATE OR ALTER VIEW vw_TopSellingItems AS
SELECT p.PartID, p.PartName, p.PartNumber, c.categoryName AS Category,
    SUM(d.Quantity) AS TotalSold,
    CAST(SUM(d.Total) AS DECIMAL(18,2)) AS TotalRevenue,
    CAST(SUM(d.Total - d.Quantity * p.PurchasePrice) AS DECIMAL(18,2)) AS TotalProfit,
    CASE WHEN SUM(d.Total) > 0
         THEN CAST(SUM(d.Total - d.Quantity*p.PurchasePrice)/SUM(d.Total)*100 AS DECIMAL(18,2))
         ELSE 0 END AS AvgMarginPct
FROM InvoiceDetails  d WITH (NOLOCK)
JOIN Invoices        i WITH (NOLOCK) ON d.InvoiceID  = i.ID
JOIN Parts           p WITH (NOLOCK) ON d.PartID     = p.PartID
JOIN partscategories c WITH (NOLOCK) ON p.CategoryID = c.categoryID
WHERE i.invoiceType = N'مبيعات'
GROUP BY p.PartID, p.PartName, p.PartNumber, c.categoryName;
GO

CREATE OR ALTER VIEW vw_SafeLedger AS
SELECT s.ID, s.TransactionDate, s.TransactionType, s.Amount, s.Description,
    ISNULL(u.UserName, N'غير معروف') AS UserName,
    ISNULL(i.invoiceType, N'—')       AS InvoiceType,
    s.InvoiceID,
    SUM(CASE WHEN s.TransactionType = N'إيداع' THEN  s.Amount
             WHEN s.TransactionType = N'سحب'   THEN -s.Amount
             ELSE 0 END)
        OVER (ORDER BY s.ID ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS RunningBalance
FROM SafeTransactions s
LEFT JOIN Users    u ON s.UserID    = u.ID
LEFT JOIN Invoices i ON s.InvoiceID = i.ID;
GO

CREATE OR ALTER VIEW vw_AccountsReceivable AS
SELECT p.ID AS PersonID, p.PersonName, p.Phone, p.address AS Address,
    c.Balance AS OutstandingBalance, COUNT(DISTINCT i.ID) AS InvoiceCount
FROM customers c
JOIN person    p ON c.ID           = p.ID
LEFT JOIN Invoices i ON i.customerID = c.ID
WHERE p.isDeleted = 0 AND c.Balance > 0
GROUP BY p.ID, p.PersonName, p.Phone, p.address, c.Balance;
GO

CREATE OR ALTER VIEW vw_AccountsPayable AS
SELECT p.ID AS PersonID, p.PersonName, p.Phone, p.address AS Address,
    s.Balance AS OutstandingBalance, COUNT(DISTINCT i.ID) AS InvoiceCount
FROM supplieres s
JOIN person     p ON s.ID           = p.ID
LEFT JOIN Invoices i ON i.supplierID = s.ID
WHERE p.isDeleted = 0 AND s.Balance > 0
GROUP BY p.ID, p.PersonName, p.Phone, p.address, s.Balance;
GO

-- ============================================================
-- SECTION 5 : MIGRATION PATCHES  (safe to run on existing DB)
-- ============================================================

-- Patch 1: Add PersonID to SafeTransactions
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('SafeTransactions') AND name='PersonID')
BEGIN
    ALTER TABLE SafeTransactions ADD PersonID INT NULL CONSTRAINT fk_Safe_Person REFERENCES person(ID);
    CREATE INDEX ix_Safe_PersonID ON SafeTransactions(PersonID) WHERE PersonID IS NOT NULL;
    PRINT 'Patch 1: SafeTransactions.PersonID added.';
END
GO

-- Patch 2: Non-negative stock
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id=OBJECT_ID('Parts') AND name='chk_Parts_Quantity')
BEGIN
    UPDATE Parts SET Quantity = 0 WHERE Quantity < 0;
    ALTER TABLE Parts ADD CONSTRAINT chk_Parts_Quantity CHECK (Quantity >= 0);
    PRINT 'Patch 2: Parts.Quantity >= 0 constraint added.';
END
GO

-- Patch 3: Invoice party exclusivity
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id=OBJECT_ID('Invoices') AND name='chk_Invoices_Party')
BEGIN
    UPDATE Invoices SET customerID = (SELECT TOP 1 ID FROM customers ORDER BY ID)
    WHERE customerID IS NULL AND supplierID IS NULL;
    ALTER TABLE Invoices ADD CONSTRAINT chk_Invoices_Party CHECK (
        (customerID IS NOT NULL AND supplierID IS NULL) OR
        (customerID IS NULL     AND supplierID IS NOT NULL));
    PRINT 'Patch 3: Invoices party exclusivity constraint added.';
END
GO

-- Patch 4: Widen password column and flag plain-text passwords for reset
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('Users') AND name='password' AND max_length < 512)
BEGIN
    ALTER TABLE Users ALTER COLUMN password NVARCHAR(256) NOT NULL;
    UPDATE Users SET password = N'RESET_REQUIRED' WHERE LEN(password) < 60;
    PRINT 'Patch 4: Password column widened; plain-text accounts marked for reset.';
END
GO

-- Patch 5: Security.AdminPin setting
IF NOT EXISTS (SELECT 1 FROM StoreSettings WHERE SettingKey = N'Security.AdminPin')
BEGIN
    INSERT INTO StoreSettings (SettingKey, SettingValue, SettingGroup)
    VALUES (N'Security.AdminPin', N'CHANGE_ME_ON_FIRST_LOGIN', N'Security');
    PRINT 'Patch 5: Security.AdminPin row added.';
END
GO

PRINT '--- AutoPartsStoreDB schema completed ---';
GO