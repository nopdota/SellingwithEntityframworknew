# SellingwithEntityframwork
แก้ไข การเชื่อมต่อ Model ให้เป็นของตัวเอง
สร้าง Table ตามนี้

CREATE TABLE [dbo].[PD_ORDER](
	[OD_ID] [nvarchar](10) NOT NULL,
	[PD_ID] [nvarchar](10) NOT NULL,
	[OD_DATE] [datetime] NULL,
	[OD_QUANTITY] [decimal](18, 2) NULL,
	[PRICE] [decimal](18, 2) NULL,
	[OD_STATUS] [nvarchar](50) NULL,
 CONSTRAINT [PK_PD_ORDER] PRIMARY KEY CLUSTERED 
(
	[OD_ID] ASC,
	[PD_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[PD_PRODUCT](
	[PD_ID] [nvarchar](10) NOT NULL,
	[PD_NAME] [nvarchar](255) NULL,
	[PD_VALUE] [decimal](18, 2) NULL,
	[PD_PRICE] [decimal](18, 2) NULL,
	[TP_ID] [nvarchar](10) NULL,
	[PD_UNIT] [nvarchar](50) NULL,
 CONSTRAINT [PK_PD_PRODUCT] PRIMARY KEY CLUSTERED 
(
	[PD_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[PD_TYPE](
	[TP_ID] [nvarchar](10) NOT NULL,
	[TP_NAME] [nvarchar](255) NULL,
 CONSTRAINT [PK_PD_TYPE] PRIMARY KEY CLUSTERED 
(
	[TP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[PD_USER](
	[USERNAME] [nvarchar](50) NOT NULL,
	[PASSWORD] [nvarchar](50) NULL,
 CONSTRAINT [PK_PD_USER] PRIMARY KEY CLUSTERED 
(
	[USERNAME] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
