USE [AutoPartsStoreDB]
GO
/****** Object:  Table [dbo].[customers]    Script Date: 4/13/2026 8:57:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[customers](
	[ID] [int] NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK__customer__3214EC27F432F876] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[inventoryTransactions]    Script Date: 4/13/2026 8:57:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[inventoryTransactions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TransactionsType] [nvarchar](50) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Date] [datetime2](7) NULL,
	[notes] [nvarchar](50) NULL,
	[PartId] [int] NOT NULL,
	[userId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceDetails]    Script Date: 4/13/2026 8:57:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Quantity] [int] NOT NULL,
	[PartPrice] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[PartID] [int] NOT NULL,
	[InvoiceID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoices]    Script Date: 4/13/2026 8:57:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoices](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Time] [datetime2](7) NOT NULL,
	[PaymentMethod] [nvarchar](20) NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[UserID] [int] NOT NULL,
	[supplierID] [int] NULL,
	[customerID] [int] NULL,
	[invoiceType] [nvarchar](50) NOT NULL,
	[paidamount] [nchar](10) NOT NULL,
 CONSTRAINT [PK__Invoices__3214EC27DAB333BE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parts]    Script Date: 4/13/2026 8:57:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parts](
	[PartID] [int] IDENTITY(1,1) NOT NULL,
	[PartName] [nvarchar](50) NOT NULL,
	[PartNumber] [nvarchar](50) NOT NULL,
	[PurchasePrice] [decimal](12, 2) NOT NULL,
	[SellingPrice] [decimal](12, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[MinimumStock] [int] NULL,
	[CategoryID] [int] NULL,
	[Notes] [nvarchar](max) NULL,
	[isDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Parts__7C3F0D305AF73EC6] PRIMARY KEY CLUSTERED 
(
	[PartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[partscategories]    Script Date: 4/13/2026 8:57:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[partscategories](
	[categoryID] [int] IDENTITY(1,1) NOT NULL,
	[categoryName] [nvarchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__partscat__23CAF1F86D683528] PRIMARY KEY CLUSTERED 
(
	[categoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[person]    Script Date: 4/13/2026 8:57:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[person](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PersonName] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](11) NULL,
	[address] [nvarchar](50) NULL,
	[isdeleted] [bit] NOT NULL,
 CONSTRAINT [PK__person__3214EC2711D5BFC9] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SafeTransactions]    Script Date: 4/13/2026 8:57:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SafeTransactions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[TransactionType] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[TransactionDate] [datetime] NULL,
	[UserID] [int] NULL,
	[InvoiceID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[supplieres]    Script Date: 4/13/2026 8:57:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[supplieres](
	[ID] [int] NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK__supplier__3214EC2790003C8E] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/13/2026 8:57:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserName] [nvarchar](50) NULL,
	[password] [nvarchar](50) NULL,
	[Role] [nvarchar](20) NOT NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Parts] ADD  CONSTRAINT [DF_Parts_isDeleted]  DEFAULT ((0)) FOR [isDeleted]
GO
ALTER TABLE [dbo].[partscategories] ADD  CONSTRAINT [DF_partscategories_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[person] ADD  CONSTRAINT [DF_person_isdeleted]  DEFAULT ((0)) FOR [isdeleted]
GO
ALTER TABLE [dbo].[SafeTransactions] ADD  DEFAULT (getdate()) FOR [TransactionDate]
GO
ALTER TABLE [dbo].[customers]  WITH CHECK ADD  CONSTRAINT [FK_customers_person] FOREIGN KEY([ID])
REFERENCES [dbo].[person] ([ID])
GO
ALTER TABLE [dbo].[customers] CHECK CONSTRAINT [FK_customers_person]
GO
ALTER TABLE [dbo].[inventoryTransactions]  WITH CHECK ADD  CONSTRAINT [FK_inventoryTransactions_Parts] FOREIGN KEY([PartId])
REFERENCES [dbo].[Parts] ([PartID])
GO
ALTER TABLE [dbo].[inventoryTransactions] CHECK CONSTRAINT [FK_inventoryTransactions_Parts]
GO
ALTER TABLE [dbo].[inventoryTransactions]  WITH CHECK ADD  CONSTRAINT [FK_inventoryTransactions_Users] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[inventoryTransactions] CHECK CONSTRAINT [FK_inventoryTransactions_Users]
GO
ALTER TABLE [dbo].[InvoiceDetails]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceDetails_Invoices] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoices] ([ID])
GO
ALTER TABLE [dbo].[InvoiceDetails] CHECK CONSTRAINT [FK_InvoiceDetails_Invoices]
GO
ALTER TABLE [dbo].[InvoiceDetails]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceDetails_Parts] FOREIGN KEY([PartID])
REFERENCES [dbo].[Parts] ([PartID])
GO
ALTER TABLE [dbo].[InvoiceDetails] CHECK CONSTRAINT [FK_InvoiceDetails_Parts]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoices_customers1] FOREIGN KEY([customerID])
REFERENCES [dbo].[customers] ([ID])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoices_customers1]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoices_supplieres1] FOREIGN KEY([supplierID])
REFERENCES [dbo].[supplieres] ([ID])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoices_supplieres1]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoices_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoices_Users]
GO
ALTER TABLE [dbo].[Parts]  WITH CHECK ADD  CONSTRAINT [FK_Parts_partscategories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[partscategories] ([categoryID])
GO
ALTER TABLE [dbo].[Parts] CHECK CONSTRAINT [FK_Parts_partscategories]
GO
ALTER TABLE [dbo].[SafeTransactions]  WITH CHECK ADD  CONSTRAINT [FK_Safe_Invoices] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoices] ([ID])
GO
ALTER TABLE [dbo].[SafeTransactions] CHECK CONSTRAINT [FK_Safe_Invoices]
GO
ALTER TABLE [dbo].[SafeTransactions]  WITH CHECK ADD  CONSTRAINT [FK_Safe_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SafeTransactions] CHECK CONSTRAINT [FK_Safe_Users]
GO
ALTER TABLE [dbo].[supplieres]  WITH CHECK ADD  CONSTRAINT [FK_supplieres_person] FOREIGN KEY([ID])
REFERENCES [dbo].[person] ([ID])
GO
ALTER TABLE [dbo].[supplieres] CHECK CONSTRAINT [FK_supplieres_person]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_person] FOREIGN KEY([ID])
REFERENCES [dbo].[person] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_person]
GO
