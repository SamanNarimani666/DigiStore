/*
   Auther:SamanNarimani
   Develope:SamanNarimani
   Object:DigiStore
   Date:2021-10-28
   Email: Samannarimani666@gmail.com
*/
CREATE DATABASE DigiStore_DB
GO
USE DigiStore_DB
GO


/*#########################################################################  SCHEMA Users  ##########################################################################################*/
CREATE SCHEMA Users
GO
CREATE TABLE Users.[User]
(
 UserID	     int			 NOT NULL identity(1,1)   CONSTRAINT PK_Users_User_UserID       PRIMARY KEY
,UserName	 nvarchar(200)	 NOT NULL				  CONSTRAINT UQ_Users_User_UserName     UNIQUE
,Email		 nvarchar(200)	 NOT NULL				  CONSTRAINT UQ_Users_User_Email        UNIQUE
,[PassWord]  nvarchar(200)	 NOT NULL				  CONSTRAINT CK_Users_User_PassWord     CHECK(LEN([PassWord])>=8)
,FirstName   nvarchar(200)       NULL
,LastName	 nvarchar(200)		 NULL
,FullName    AS(FirstName+' '+LastName)				  PERSISTED
,ActiveCode  nvarchar(50)        NULL
,UserAvatar  nvarchar(200)	     NULL				 
,Mobile      nvarchar(50)        NULL                 CONSTRAINT UQ_Users_User_Mobile        CHECK(LEN(Mobile)=11) UNIQUE
,IsActive    bit			 NOT NULL				  CONSTRAINT CK_Users_User_IsActive	     DEFAULT(0)
,IsBlock     bit             NOT NULL                 CONSTRAINT DF_Users_User_IsBlock       DEFAULT(0)
,IsDelete    bit			 NOT NULL				  CONSTRAINT DF_Users_User_IsDelete	     DEFAULT(0)
,CreateDate   Datetime       NOT NULL				  CONSTRAINT DF_Users_User_CreateDate	 DEFAULT(GETDATE())
,ModifiedDate Datetime       NOT NULL				  CONSTRAINT DF_Users_User_ModifiedDate	 DEFAULT(GETDATE())
,rowguid	uniqueidentifier NOT NULL				  CONSTRAINT DF_Users_User_rowguid	     DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE INDEX I_NC_Users_User_UserName ON Users.[User](UserName)
GO
CREATE INDEX I_NC_Users_User_Email    ON Users.[User](Email)
GO
CREATE INDEX I_NC_Users_User_Mobile    ON Users.[User](Mobile)
GO
CREATE INDEX I_NC_Users_User_PassWord    ON Users.[User]([PassWord])
GO
/*_______________________________________________________________________________________________________________________*/
CREATE TABLE Users.Roles
(
 RoleID		int				 NOT NULL identity(1,1)  CONSTRAINT PK_Users_Roles_RoleID  PRIMARY KEY
,RoleTitle  nvarchar(200)	 NOT NULL
,IsDelete    bit			 NOT NULL				 CONSTRAINT DF_Users_Roles_IsDelete	DEFAULT(0)
,ModifiedDate Datetime       NOT NULL				 CONSTRAINT DF_Users_Roles_ModifiedDate	 DEFAULT(GETDATE())
,rowguid	uniqueidentifier NOT NULL				 CONSTRAINT DF_Users_Roles_rowguid	     DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE INDEX I_NC_Users_Roles_RoleTitle    ON Users.Roles(RoleTitle)
GO
CREATE INDEX I_NC_Users_Roles_IsDelete    ON Users.Roles(IsDelete)
GO
/*_______________________________________________________________________________________________________________________*/
CREATE TABLE Users.UserRoles
(
 UserRoleID   int			    NOT NULL identity(1,1)  CONSTRAINT PK_Users_UserRoles_UserRoleID  PRIMARY KEY
,UserID	      int				NOT NULL                CONSTRAINT FK_Users_UserRoles_UserID      FOREIGN KEY REFERENCES Users.[User](UserID) 
,RoleID	      int			    NOT NULL				CONSTRAINT FK_Users_UserRoles_RoleID      FOREIGN KEY REFERENCES Users.Roles(RoleID)
,IsDelete     bit		    	NOT NULL                
,ModifiedDate Datetime          NOT NULL				CONSTRAINT DF_Users_UserRoles_ModifiedDate	 DEFAULT(GETDATE())
,rowguid	uniqueidentifier    NOT NULL				CONSTRAINT DF_Users_UserRoles_rowguid	     DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE INDEX I_NC_Users_UserRoles_UserID ON Users.UserRoles(UserID)
GO
CREATE INDEX I_NC_Users_UserRoles_RoleID ON Users.UserRoles(RoleID)
GO

CREATE TABLE Users.Permission
(
 PermissionID    int		  NOT NULL  identity(1,1)   CONSTRAINT PK_Users_Permission_PermissionID PRIMARY KEY
,PermissionTitle nvarchar(50) NOT NULL					
,ParentID        int			  NULL					CONSTRAINT FK_Users_Permission_ParentID FOREIGN KEY REFERENCES Users.Permission(PermissionID)
)
GO	
CREATE INDEX I_NC_Users_Permission_PermissionTitle  ON Users.Permission(PermissionTitle)
GO

CREATE TABLE Users.RolePermission
(
 RolePermissionID   int  NOT NULL identity(1,1)   CONSTRAINT PK_Users_RolePermission_RolePermissionID  PRIMARY KEY
,PermissionID       int  NOT NULL                 CONSTRAINT FK_Users_RolePermission_PermissionID      FOREIGN KEY REFERENCES Users.Permission(PermissionID)
,RoleID				int  NOT NULL                 CONSTRAINT FK_Users_RolePermission_RoleID            FOREIGN KEY REFERENCES Users.Roles(RoleID)
)
GO

CREATE SCHEMA Addresses
GO
CREATE TABLE Addresses.[State]
(
 StateID   int					 NOT NULL  identity(1,1)  CONSTRAINT PK_Addresses_State_StateID			PRIMARY KEY
,StateName nvarchar(300)		 NOT NULL
,ModifiedDate Datetime           NOT NULL				  CONSTRAINT DF_Addresses_State_ModifiedDate	 DEFAULT(GETDATE())
,rowguid	uniqueidentifier     NOT NULL				  CONSTRAINT DF_Addresses_State_rowguid			 DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE TABLE Addresses.City
(
 CityID     int					 NOT NULL  identity(1,1)  CONSTRAINT  PK_Addresses_City_CityID		     PRIMARY KEY 
,StateID    int					 NOT NULL				  CONSTRAINT  FK_Addresses_City_StateID          FOREIGN KEY REFERENCES Addresses.[State](StateID)	
,CityName   nvarchar(300)		 NOT NULL   
,ModifiedDate Datetime           NOT NULL				  CONSTRAINT DF_Addresses_City_ModifiedDate	 DEFAULT(GETDATE())
,rowguid	uniqueidentifier     NOT NULL				  CONSTRAINT DF_Addresses_City_rowguid	     DEFAULT(NEWSEQUENTIALID())
)
GO

CREATE TABLE Users.[Address]
(
 AddressID     int				     NOT NULL  identity(1,1)    CONSTRAINT  PK_Users_Address_AddressID    PRIMARY KEY 
,UserID        int				     NOT NULL                   CONSTRAINT  FK_Users_Address_UserID       FOREIGN KEY REFERENCES Users.[User](UserID) ON DELETE NO ACTION  
,StateID       int					 NOT NULL				    CONSTRAINT  FK_Users_Address_StateID      FOREIGN KEY REFERENCES Addresses.[State](StateID)	
,CityID        int					 NOT NULL				    CONSTRAINT  FK_Users_Address_CityID       FOREIGN KEY REFERENCES Addresses.City(CityID)	
,Zipcode     char(5)                 NOT NULL              
,PostalCode  char(10)                NOT NULL                CONSTRAINT CK_Users_Address_PostalCode     CHECK(LEN(PostalCode)=10)  
,Unit        char(5)                     NULL  
,[Address]    nvarchar(800)		     NOT NULL
,IsDelete     bit                    NOT NULL                CONSTRAINT DE_Users_Address_IsDelete         DEFAULT(0)        
,rowguid   uniqueidentifier          NOT NULL                                                             DEFAULT(NEWSEQUENTIALID())
,ModifiedDate datetime               NOT NULL                                                             DEFAULT(GETDATE())    
)
GO
CREATE INDEX I_NC_Users_Address_Zipcode ON Users.[Address](Zipcode)
GO
CREATE INDEX I_NC_Users_Address_postalcard ON Users.[Address](PostalCode)
GO
CREATE INDEX I_NC_Users_Address_IsDelete ON Users.[Address](IsDelete)
GO
/*#########################################################################  Site  ##########################################################################################*/
CREATE SCHEMA Site 
GO
CREATE TABLE Site.ContactUs
(
 ContactUsid int				 NOT NULL identity(1,1)   CONSTRAINT PK_Site_ContactUs_ContactUs   PRIMARY KEY
,UserIp      nvarchar(50)        NOT NULL
,Email	     nvarchar(200)	     NOT NULL
,FullName    nvarchar(200)       NOT NULL
,[Subject]   nvarchar(200)	     NOT NULL
,[Text]      nvarchar(MAX)       NOT NULL
,ModifiedDate Datetime           NOT NULL				  CONSTRAINT DF_Site_ContactUs_ModifiedDate	 DEFAULT(GETDATE())
,rowguid	uniqueidentifier     NOT NULL				  CONSTRAINT DF_Site_ContactUs_rowguid	     DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE INDEX I_NC_Stie_ContactUs_Email ON  Site.ContactUs(Email)
GO
CREATE INDEX I_NC_Stie_ContactUs_FullName ON  Site.ContactUs(FullName)
GO
CREATE TABLE Site.SiteSetting 
(
 SiteSettingID  int				 NOT NULL identity(1,1)  CONSTRAINT PK_Site_SiteSetting_SiteSettingID PRIMARY KEY
,Phone          nvarchar(200)        NULL
,Email			nvarchar(200)	     NULL
,FooterText     nvarchar(MAX)        NULL
,CopyRight		nvarchar(MAX)        NULL
,[Address]      nvarchar(MAX)        NULL
,IsDefault      bit                  NULL
,AboutUS		nvarchar(MAX)        NULL
,ModifiedDate Datetime           NOT NULL				  CONSTRAINT DF_Site_SiteSetting_ModifiedDate	 DEFAULT(GETDATE())
,rowguid	uniqueidentifier     NOT NULL				  CONSTRAINT DF_Site_SiteSetting_rowguid	     DEFAULT(NEWSEQUENTIALID())
)
GO

CREATE TABLE Site.Slider
(
 SliderID  int					NOT NULL identity(1,1)    CONSTRAINT PK_Site_Slider_SliderID  PRIMARY KEY
,ImageName nvarchar(200)		NOT NULL 
,Link      nvarchar(200)		NOT NULL
,DisplayPrority    tinyint		    NULL
,IsActive  bit					NOT NULL                  CONSTRAINT DF_Site_Slider_IsActive DEFAULT(0)
,IsDelete  bit					NOT NULL				  CONSTRAINT DF_Site_Slider_IsDelete DEFAULT(0)
,CreatedDate Datetime		    NOT NULL				  CONSTRAINT DF_Site_Slider_ModifiedDate DEFAULT(GETDATE())
,rowguid	uniqueidentifier    NOT NULL				  CONSTRAINT DF_Site_Slider_rowguid	     DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE INDEX I_NC_Site_Slider_ImageName ON Site.Slider(ImageName)
GO
CREATE INDEX I_NC_Site_Slider_Link ON Site.Slider(Link)
GO

/*#########################################################################  Ticket  ##########################################################################################*/
CREATE SCHEMA Ticket
GO
CREATE TABLE  Ticket.Ticket  
(
 TicketID	    int			      NOT NULL  identity(1,1)  CONSTRAINT PK_Ticket_Ticket_TicketID		     PRIMARY KEY
,UserID         int			          NULL                 CONSTRAINT FK_Ticket_Ticket_UserID		     FOREIGN KEY REFERENCES Users.[User](UserID)
,Title 	        nvarchar(200)	      NULL
,TicketState      tinyint		      NULL                
,TicketSection    tinyint		      NULL				  
,TicketPriority   tinyint             NULL				   
,IsReadByOwner  bit                   NULL
,IsReadByAdmin  bit                   NULL
,CreatedDate Datetime             NOT NULL				   CONSTRAINT DF_Ticket_Ticket_ModifiedDate DEFAULT(GETDATE())
,rowguid	uniqueidentifier      NOT NULL				   CONSTRAINT DF_Ticket_Ticket_rowguid	     DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE TABLE Ticket.TicketMessage 
(
 TicketMessageID int				NOT NULL identity(1,1)  CONSTRAINT PK_Ticket_TicketMessage_TicketMessageID PRIMARY KEY
,TicketID	     int					NULL                CONSTRAINT FK_Ticket_TicketMessage_TicketID  FOREIGN KEY REFERENCES Ticket.Ticket (TicketID)
,UserID          int					NULL                CONSTRAINT FK_Ticket_TicketMessage_UserID    FOREIGN KEY REFERENCES Users.[User](UserID)
,[Text]          nvarchar(MAX)		NOT NULL
,CreatedDate Datetime               NOT NULL				CONSTRAINT DF_Ticket_TicketMessage_CreatedDate	 DEFAULT(GETDATE())
,rowguid	uniqueidentifier        NOT NULL				CONSTRAINT DF_Ticket_TicketMessage_rowguid	     DEFAULT(NEWSEQUENTIALID())
)
GO
/*#########################################################################  Sotre  ##########################################################################################*/
CREATE SCHEMA Store 
GO
CREATE TABLE Store.Seller
(
SellerId			 int				NOT NULL  identity(1,1)   CONSTRAINT PK_Store_Seller_SellerId		   PRIMARY KEY
,UserID				 int				    NULL                  CONSTRAINT FK_Store_Seller_UserID            FOREIGN KEY REFERENCES Users.[User](UserID)   
,StoreName			 nvarchar(200)      NOT NULL
,Phone				 nvarchar(50)	    NOT NULL
,Email				 nvarchar(200)	    NOT NULL				      
,Mobile				 nvarchar(50)			NULL                       CHECK(LEN(Mobile)=11) 
,[Address]			 nvarchar(700)		NOT NULL
,Descriptions		 nvarchar(700)		NOT NULL
,AdminDescription    nvarchar(700)			NULL
,StoreAceptanceStateDescription   nvarchar(500)			NULL
,IsDelete			 bit			    NOT NULL				  CONSTRAINT DF_Store_Seller_IsDelete	     DEFAULT(0)
,Logo				 nvarchar(200)	        NULL	
,StoreDoucument      nvarchar(200)	        NULL
,StoreaceptanceState tinyint			NOT NULL
,CreatedDate Datetime                   NOT NULL				CONSTRAINT DF_Store_Selle_CreatedDate	 DEFAULT(GETDATE())
,rowguid	uniqueidentifier            NOT NULL				CONSTRAINT DF_Store_Selle_rowguid	     DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE INDEX I_NC_Store_Seller_StoreName ON Store.Seller(StoreName)
GO
CREATE INDEX I_NC_Store_Seller_IsDelete ON Store.Seller(IsDelete)
GO
CREATE INDEX I_NC_Store_Seller_Logo ON Store.Seller(Logo)
GO
CREATE INDEX I_NC_Store_Seller_StoreaceptanceState ON Store.Seller(StoreaceptanceState)
GO
CREATE INDEX I_NC_Store_Seller_Phone ON Store.Seller(Phone)
GO
CREATE TABLE Store.SellerWallet
(
 SellerWalletId  int			  NOT NULL identity(1,1)   CONSTRAINT PK_Store_SellerWallet_SellerWalletId    PRIMARY KEY
,SellerId		 int			  NOT NULL				   CONSTRAINT FK_Store_SellerWallet_SellerId		  FOREIGN KEY REFERENCES Store.Seller(SellerId)
,Price           int				  NULL           
,Descriptions	nvarchar(500)         NULL
,TransactionType tinyint		      NULL
,CreateDate   Datetime			  NOT NULL					CONSTRAINT DF_Store_SellerWallet_CreateDate			DEFAULT(GETDATE())
,ModifiedDate   Datetime		  NOT NULL				    CONSTRAINT DF_Store_SellerWallet_ModifiedDate		DEFAULT(GETDATE())
,rowguid	    uniqueidentifier  NOT NULL					CONSTRAINT DF_Store_SellerWallet_rowguid	        DEFAULT(NEWSEQUENTIALID())
)
GO
/*#########################################################################  Brand  ##########################################################################################*/
CREATE SCHEMA Brands
GO
CREATE TABLE Brands.Brand
(
 BrandId         int				NOT NULL identity(1,1)  CONSTRAINT PK_Brands_Brand_BrandId   PRIMARY KEY
,BrandName		 nvarchar(200)      NOT NULL
,Logo			 nvarchar(200)          NULL
,IsDelete		 bit				NOT NULL			    CONSTRAINT DF_Brands_Brand_IsDelete		 DEFAULT(0)   
,ModifiedDate    Datetime           NOT NULL				CONSTRAINT DF_Brands_Brand_ModifiedDate	 DEFAULT(GETDATE())
,rowguid	     uniqueidentifier   NOT NULL				CONSTRAINT DF_Brands_Brand_rowguid	     DEFAULT(NEWSEQUENTIALID())
)
GO
/*#########################################################################  Product  ##########################################################################################*/
CREATE SCHEMA Production
GO
CREATE TABLE Production.Product
(
 ProductId						   int				NOT NULL  identity(1,1)  CONSTRAINT PK_Production_Product_ProductId      PRIMARY KEY 
,SellerId						   int				NOT NULL                 CONSTRAINT FK_Production_Product_SellerId       FOREIGN KEY REFERENCES Store.Seller(SellerId)
,BrandId                           int                  NULL                 CONSTRAINT FK_Production_Product_BrandId		 FOREIGN KEY REFERENCES Brands.Brand(BrandId)
,[Name]							   nvarchar(300)	NOT NULL
,Price							   int              NOT NULL                 CONSTRAINT DF_Production_Product_Price		     DEFAULT(0) 
,ShortDescription				   nvarchar(500)	NOT NULL
,[Description]					   nvarchar(MAX)        NULL
,ProductAcceptOrRejectDescription  nvarchar(MAX)        NULL
,IsDelete                          bit              NOT NULL			    CONSTRAINT DF_Production_Product_IsDelete		 DEFAULT(0)     
,IsActive                          bit              NOT NULL			    CONSTRAINT DF_Production_Product_IsActive		 DEFAULT(0)     
,ProductAcceptanceState			   tinyint              NULL
,ImageName						   nvarchar(300)    NOT NULL
,SiteProfile					   int				    NULL				CONSTRAINT DF_Production_Product_SiteProfile	 DEFAULT(0)
,CreatedDate Datetime							    NOT NULL				CONSTRAINT DF_Production_Product_CreatedDate	 DEFAULT(GETDATE())
,ModifiedDate Datetime                              NOT NULL				CONSTRAINT DF_Production_Product_ModifiedDate	 DEFAULT(GETDATE())
,rowguid	uniqueidentifier					    NOT NULL				CONSTRAINT DF_Production_Product_rowguid	     DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE INDEX I_NC_Production_Product_Name ON Production.Product(Name)
GO
CREATE INDEX I_NC_Production_Product_Price ON Production.Product(Price)
GO
CREATE INDEX I_NC_Production_Product_IsDelete ON Production.Product(IsDelete)
GO
CREATE INDEX I_NC_Production_Product_IsActive ON Production.Product(IsActive)
GO
CREATE TABLE Production.ProductCategory
(
 ProductCategoryId   int			NOT NULL  identity(1,1) CONSTRAINT PK_Production_ProductCategory_ProductCategoryId PRIMARY KEY
,ParentId            int                NULL                CONSTRAINT FK_Production_ProductCategory_ParentId          FOREIGN KEY  REFERENCES Production.ProductCategory(ProductCategoryId)
,Title               nvarchar(200)  NOT NULL   
,UrlName             nvarchar(200)  NOT NULL   
,IconName            nvarchar(200)      NULL
,IsDelete            bit            NOT NULL			    CONSTRAINT DF_Production_ProductCategory_IsDelete		 DEFAULT(0)     
,IsActive            bit            NOT NULL			    CONSTRAINT DF_Production_ProductCategory_IsActive		 DEFAULT(0)    
,ModifiedDate Datetime              NOT NULL				CONSTRAINT DF_Production_ProductCategory_ModifiedDate	 DEFAULT(GETDATE())
,rowguid	uniqueidentifier	    NOT NULL				CONSTRAINT DF_Production_ProductCategory_rowguid	     DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE INDEX I_NC_Production_ProductCategory_Title ON Production.ProductCategory(Title)
GO
CREATE INDEX I_NC_Production_ProductCategory_IsDelete ON Production.ProductCategory(IsDelete)
GO
CREATE INDEX I_NC_Production_ProductCategory_IsActive ON Production.ProductCategory(IsActive)
GO
CREATE TABLE Production.ProductSelectedCategory 
(
ProductSelectedCategoryId int              NOT NULL identity(1,1) CONSTRAINT PK_Production_ProductSelectedCategory_ProductCategoryId	 PRIMARY KEY
,ProductId			int				         NULL			      CONSTRAINT FK_Production_ProductSelectedCategory_ProductId			 FOREIGN KEY REFERENCES Production.Product(ProductId)
,ProductCategoryId  int					     NULL			      CONSTRAINT FK_Production_ProductSelectedCategory_ProductCategoryId     FOREIGN KEY REFERENCES Production.ProductCategory(ProductCategoryId) 
,ModifiedDate       Datetime			 NOT NULL				  CONSTRAINT DF_Production_ProductSelectedCategory_ModifiedDate	         DEFAULT(GETDATE())
,rowguid	        uniqueidentifier	 NOT NULL				  CONSTRAINT DF_Production_ProductSelectedCategory_rowguid	             DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE TABLE Production.Color
(
 ColorId		int					 NOT NULL  identity(1,1)  CONSTRAINT PK_Production_Color_ColorId   PRIMARY KEY
,ProductId	    int					 NOT NULL				  CONSTRAINT FK_Production_Color_ProductId FOREIGN KEY REFERENCES Production.Product(ProductId)
,ColorCode      nvarchar(200)		 NOT NULL
,Price          int					 NOT NULL
,IsDelete       bit					 NOT NULL			      CONSTRAINT DF_Production_Color_IsDelete		     DEFAULT(0)  
,ModifiedDate   Datetime			 NOT NULL				  CONSTRAINT DF_Production_Color_ModifiedDate	     DEFAULT(GETDATE())
,rowguid	    uniqueidentifier	 NOT NULL				  CONSTRAINT DF_Production_Color_rowguid	         DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE INDEX I_NC_Production_Color_Price ON Production.Color(Price)
GO
CREATE INDEX I_NC_Production_Color_ColorCode ON Production.Color(ColorCode)
GO

CREATE TABLE Production.ProductGallery
(
 ProductGalleryId  int			     NOT NULL  identity(1,1)    CONSTRAINT PK_Production_ProductGallery_ProductGalleryId     PRIMARY KEY
,ProductId	       int			     NOT NULL				    CONSTRAINT FK_Production_ProductGallery_ProductId		     FOREIGN KEY REFERENCES Production.Product(ProductId)
,ImageName         nvarchar(400)     NOT NULL
,DisplayPrority    tinyint				 NULL
,IsDelete       bit					 NOT NULL					CONSTRAINT DF_Production_ProductGallery_IsDelete		     DEFAULT(0)  
,ModifiedDate   Datetime			 NOT NULL					CONSTRAINT DF_Production_ProductGallery_ModifiedDate	     DEFAULT(GETDATE())
,rowguid	    uniqueidentifier	 NOT NULL					CONSTRAINT DF_Production_ProductGallery_rowguid	             DEFAULT(NEWSEQUENTIALID())
)
GO

CREATE TABLE Production.ProductVisited
( 
 VisiteId       int					 NOT NULL  identity(1,1)   CONSTRAINT PK_Production_ProductVisited_VisiteId			PRIMARY  KEY
,ProductId	    int					 NOT NULL				   CONSTRAINT FK_Production_ProductVisited_ProductId		FOREIGN KEY REFERENCES Production.Product(ProductId)
,UserId         int						 NULL				   CONSTRAINT FK_Production_ProductVisited_UserID			FOREIGN KEY REFERENCES Users.[User](UserID)   
,UserIp         char(200)			 NOT NULL        
,ModifiedDate   Datetime			 NOT NULL				   CONSTRAINT DF_Production_ProductVisited_ModifiedDate    DEFAULT(GETDATE())
,rowguid	    uniqueidentifier	 NOT NULL				   CONSTRAINT DF_Production_ProductVisited_rowguid	        DEFAULT(NEWSEQUENTIALID())
)
GO

CREATE TABLE Production.ProductFeature
(
 ProductFeatureId    int			 NOT NULL identity(1,1)   CONSTRAINT  PK_Production_ProductFeature_ProductFeatureId PRIMARY  KEY
,ProductId		     int			 NOT NULL				   CONSTRAINT FK_Production_ProductFeature_ProductId		 FOREIGN KEY REFERENCES Production.Product(ProductId)
,FeatureTitle        nvarchar(300)	 NOT NULL
,FeatureValue        nvarchar(300)	 NOT NULL
,ModifiedDate   Datetime			 NOT NULL				   CONSTRAINT DF_Production_ProductFeature_ModifiedDate    DEFAULT(GETDATE())
,rowguid	    uniqueidentifier	 NOT NULL				   CONSTRAINT DF_Production_ProductFeature_rowguid	        DEFAULT(NEWSEQUENTIALID())
)
GO 
CREATE TABLE  Production.Productcomment
(
 ProductcommentId		 int			  NOT NULL identity(1,1)    CONSTRAINT PK_Production_Productcomment_ProductcommentId  PRIMARY KEY
,ProductId				 int			  NOT NULL                  CONSTRAINT FK_Production_Productcomment_ProductId         FOREIGN KEY REFERENCES Production.Product(ProductId)
,UserId					 int			  NOT NULL			        CONSTRAINT FK_Production_Productcomment_UserId			  FOREIGN KEY REFERENCES Users.[User](UserID)  
,SellerId                int              NOT NULL                  CONSTRAINT FK_Production_Productcomment_SellerId          FOREIGN KEY REFERENCES Store.Seller(SellerId)
,Title					 nvarchar(200)    NOT NULL				
,Comment				 nvarchar(900)    NOT NULL              
,IsDelete				 bit			  NOT NULL					CONSTRAINT DF_Production_Productcomment_IsDelete		     DEFAULT(0)  
,CreateDate   Datetime				      NOT NULL					CONSTRAINT DF_Production_Productcomment_CreateDate	         DEFAULT(GETDATE())
,rowguid	    uniqueidentifier	      NOT NULL					CONSTRAINT DF_Production_Productcomment_rowguid	             DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE TABLE Production.ProductRating
(
 ProductRatingId				int		         NOT NULL identity(1,1)	CONSTRAINT PK_Production_ProductRating_ProductRatingId				    PRIMARY KEY
,ProductId					    int			     NOT NULL                  CONSTRAINT FK_Production_ProductRating_ProductId					    FOREIGN KEY REFERENCES Production.Product(ProductId)
,ConstructionQuality			tinyint          NOT NULL                  CONSTRAINT DF_Production_ProductRating_ConstructionQuality		    DEFAULT(1)
,PurchaseValueRelativeToPrice   tinyint          NOT NULL                  CONSTRAINT DF_Production_ProductRating_PurchaseValueRelativeToPrice  DEFAULT(1)
,Innovation					    tinyint          NOT NULL                  CONSTRAINT DF_Production_ProductRating_Innovation			        DEFAULT(1)
,FeaturesAndCapabilities		tinyint          NOT NULL                  CONSTRAINT DF_Production_ProductRating_FeaturesAndCapabilities		DEFAULT(1)
,EaseOfUse						tinyint          NOT NULL                  CONSTRAINT DF_Production_ProductRating_EaseOfUse						DEFAULT(1)
,DesignAndAppearance			tinyint          NOT NULL                  CONSTRAINT DF_Production_ProductRating_DesignAndAppearance			DEFAULT(1)
,ModifiedDate   Datetime						 NOT NULL				   CONSTRAINT DF_Production_ProductRating_ModifiedDate					DEFAULT(GETDATE())
,rowguid	    uniqueidentifier				 NOT NULL				   CONSTRAINT DF_Production_ProductRating_rowguid						DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE TABLE Production.FavoriteProductUser
(
 FavoriteProductUserId int		      NOT NULL identity(1,1)    CONSTRAINT PK_Production_FavoriteProductUser_FavoriteProductUserId  PRIMARY KEY
,ProductId			   int			  NOT NULL                  CONSTRAINT FK__Production_FavoriteProductUser_ProductId				FOREIGN KEY REFERENCES Production.Product(ProductId)
,UserId				   int			  NOT NULL			        CONSTRAINT FK__Production_FavoriteProductUser_UserId			    FOREIGN KEY REFERENCES Users.[User](UserID)  
,IsDelete				 bit		  NOT NULL				CONSTRAINT DF_Production_FavoriteProductUser_IsDelete		        DEFAULT(0)  
,CreateDate   Datetime				  NOT NULL				CONSTRAINT DF_Production_FavoriteProductUse_CreateDate	            DEFAULT(GETDATE())
,rowguid	    uniqueidentifier	  NOT NULL				CONSTRAINT DF_Production_FavoriteProductUse_rowguid	                DEFAULT(NEWSEQUENTIALID())
)
GO
  /*#########################################################################  Sales  ##########################################################################################*/
CREATE SCHEMA Sales
GO
CREATE TABLE Sales.SalesOrderHeader
(
 SalesOrderId   int				 NOT NULL identity(1,1)  CONSTRAINT PK_Sales_SalesOrderHeader_SalesOrderId	   PRIMARY KEY
,UserId         int				 NOT NULL				 CONSTRAINT FK_Sales_SalesOrderHeader_UserID		   FOREIGN KEY REFERENCES Users.[User](UserID)  
,IsPaiy          bit			 NOT NULL                CONSTRAINT DF_Sales_SalesOrderHeader_IsPay            DEFAULT(0)
,OrderSum       int			     NOT NULL                CONSTRAINT DF_Sales_SalesOrderHeader_OrderSum         DEFAULT(0) 
,TracingCode    nvarchar(300)	     NULL
,PaymentDate    Datetime			 NULL
,Descriptions   nvarchar(500)        NULL
,CreateDate     Datetime		 NOT NULL				 CONSTRAINT DF_Sales_SalesOrderHeader_CreateDate    DEFAULT(GETDATE())
,ModifiedDate   Datetime		 NOT NULL				 CONSTRAINT DF_Sales_SalesOrderHeader_ModifiedDate	DEFAULT(GETDATE())
,rowguid	    uniqueidentifier NOT NULL				 CONSTRAINT DF_Sales_SalesOrderHeader_rowguid	    DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE TABLE Sales.SalesOrderDetail
(
 SalesOrderDetailId   int		  NOT NULL	identity(1,1)	CONSTRAINT PK_Sales_SalesOrderDetail_SalesOrderDetailId  PRIMARY KEY
,SalesOrderId		  int		  NOT NULL					CONSTRAINT FK_Sales_SalesOrderDetail_SalesOrderId		 FOREIGN KEY REFERENCES Sales.SalesOrderHeader(SalesOrderId)
,ProductId			  int		  NOT NULL					CONSTRAINT FK_Sales_SalesOrderDetail_ProductId			 FOREIGN KEY REFERENCES Production.Product(ProductId)
,ColorId              int			  NULL					CONSTRAINT FK_Sales_SalesOrderDetail_ColorId             FOREIGN KEY REFERENCES Production.Color(ColorId)
,ColorPrice           int			  NULL
,OrderQty             int		  NOT NULL
,Price                int		  NOT NULL					CONSTRAINT DF_Sales_SalesOrderDetail_Price				DEFAULT(0)
,ModifiedDate   Datetime		  NOT NULL				    CONSTRAINT DF_Sales_SalesOrderDetail_ModifiedDate		DEFAULT(GETDATE())
,rowguid	    uniqueidentifier  NOT NULL					CONSTRAINT DF_Sales_SalesOrderDetail_rowguid	        DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE TABLE Store.SellerWallet
(
 SellerWalletId  int			  NOT NULL identity(1,1)   CONSTRAINT PK_Store_SellerWallet_SellerWalletId    PRIMARY KEY
,SellerId		 int			  NOT NULL				   CONSTRAINT FK_Store_SellerWallet_SellerId		  FOREIGN KEY REFERENCES Store.Seller(SellerId)
,Price           int				  NULL           
,Descriptions	nvarchar(500)         NULL
,TransactionType tinyint		      NULL
,CreateDate   Datetime			  NOT NULL					CONSTRAINT DF_Store_SellerWallet_CreateDate			DEFAULT(GETDATE())
,ModifiedDate   Datetime		  NOT NULL				    CONSTRAINT DF_Store_SellerWallet_ModifiedDate		DEFAULT(GETDATE())
,rowguid	    uniqueidentifier  NOT NULL					CONSTRAINT DF_Store_SellerWallet_rowguid	        DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE TABLE Sales.SalesInforamtion
(
 SalesInforamtionId    int  		    NOT NULL identity(1,1)  CONSTRAINT PK_Sales_SalesInforamtion_SalesInforamtionId  PRIMARY KEY
,UserId                int				NOT NULL				CONSTRAINT FK_Sales_SalesInforamtion_UserID		         FOREIGN KEY REFERENCES Users.[User](UserID)
,SalesOrderId		  int		  NOT NULL						CONSTRAINT FK_Sales_SalesInforamtion_SalesOrderId		 FOREIGN KEY REFERENCES Sales.SalesOrderHeader(SalesOrderId)
,AddressId			   int				NOT NULL				CONSTRAINT FK_Sales_SalesInforamtion_AddressId			 FOREIGN KEY REFERENCES Users.[Address](AddressID)
,ReceiverName          nvarchar(250)    NOT NULL
,ReceiverMobile        nvarchar(50)     NOT NULL 
,ReceiverNaationalId   nvarchar(50)     NOT NULL
,CreateDate   Datetime				    NOT NULL				CONSTRAINT DF_Sales_SalesInforamtion_CreateDate			DEFAULT(GETDATE())
,ModifiedDate   Datetime			    NOT NULL				CONSTRAINT DF_Sales_SalesInforamtion_ModifiedDate		DEFAULT(GETDATE())
,rowguid	    uniqueidentifier	    NOT NULL				CONSTRAINT DF_Sales_SalesInforamtiont_rowguid	        DEFAULT(NEWSEQUENTIALID())
)
GO

  /*#########################################################################  Discount  ##########################################################################################*/
CREATE SCHEMA  Discount
GO
CREATE TABLE Discount.ProductDiscount
(
 ProductDiscountId  int			  NOT NULL identity(1,1)     CONSTRAINT PK_Discount_ProductDiscount_ProductDiscountId	  PRIMARY KEY
,ProductId			int			  NOT NULL					 CONSTRAINT FK_Discount_ProductDiscount_ProductId			  FOREIGN KEY REFERENCES Production.Product(ProductId)
,[Percentage]       int			  NOT NULL                   CONSTRAINT CK_Discount_ProductDiscount_Percentage			  CHECK([Percentage]>0AND[Percentage]<=100)
,ExpierDate   Datetime			  NOT NULL				     CONSTRAINT DF_Discount_ProductDiscount_ExpierDate			  DEFAULT(GETDATE())
,DiscountNumber int					  NULL
,ModifiedDate   Datetime		  NOT NULL				     CONSTRAINT DF_Store_ProductDiscount_ModifiedDate		   DEFAULT(GETDATE())
,rowguid	    uniqueidentifier  NOT NULL					 CONSTRAINT DF_Store_ProductDiscount_rowguid	           DEFAULT(NEWSEQUENTIALID())
)
GO
CREATE TABLE Discount.ProductDiscountUse
(
 ProductDiscountUseId  int		  NOT NULL identity(1,1)     CONSTRAINT PK_Discount_ProductDiscountUse_ProductDiscountUseId   PRIMARY KEY
,ProductDiscountId     int		  NOT NULL					 CONSTRAINT FK_Discount_ProductDiscountUse_ProductDiscountId	  FOREIGN KEY REFERENCES Discount.ProductDiscount(ProductDiscountId)
,UserId				   int		  NOT NULL					 CONSTRAINT FK_Discount_ProductDiscountUse_UserID				  FOREIGN KEY REFERENCES Users.[User](UserID)
,CreateDate   Datetime		      NOT NULL					 CONSTRAINT DF_Discount_ProductDiscountUse_CreateDate		      DEFAULT(GETDATE())
,ModifiedDate   Datetime		  NOT NULL				     CONSTRAINT DF_Discount_ProductDiscountUse_ModifiedDate		      DEFAULT(GETDATE())
,rowguid	    uniqueidentifier  NOT NULL					 CONSTRAINT DF_Discount_ProductDiscountUse_rowguid	              DEFAULT(NEWSEQUENTIALID())
)
GO


