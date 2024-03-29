USE [QYManage]
GO
/****** Object:  Table [dbo].[Article]    Script Date: 2016/4/11 7:12:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Article](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[Type] [int] NOT NULL,
	[Contents] [varchar](5000) NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[AddUser] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Article] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Department]    Script Date: 2016/4/11 7:12:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Department](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[No] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ParentId] [int] NULL,
	[Grade] [int] NOT NULL,
 CONSTRAINT [PK_Department_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Files]    Script Date: 2016/4/11 7:12:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Files](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[No] [varchar](50) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Uploader] [varchar](50) NOT NULL,
	[UploadTime] [datetime] NOT NULL,
 CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Message]    Script Date: 2016/4/11 7:12:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Message](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SenderId] [int] NOT NULL,
	[RecipientId] [int] NOT NULL,
	[IsReaded] [bit] NOT NULL,
	[SendDate] [datetime] NOT NULL,
	[Contents] [varchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 2016/4/11 7:12:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Permission](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Url] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Position]    Script Date: 2016/4/11 7:12:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Position](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[No] [varchar](50) NOT NULL,
	[Grade] [int] NOT NULL,
 CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Role]    Script Date: 2016/4/11 7:12:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[SysName] [varchar](50) NULL,
	[PermissionIds] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 2016/4/11 7:12:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Staff](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[DepartmentId] [int] NULL,
	[PositionId] [int] NULL,
	[Sex] [int] NOT NULL,
	[IDCardNum] [varchar](50) NOT NULL,
	[BirthDate] [datetime] NOT NULL,
	[NativePlace] [int] NOT NULL,
	[Nation] [int] NOT NULL,
	[Education] [int] NOT NULL,
	[GraduatingAcademy] [varchar](100) NOT NULL,
	[MaritalStatus] [int] NOT NULL,
	[EntryTime] [datetime] NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Mobile] [varchar](50) NOT NULL,
	[Address] [varchar](100) NOT NULL,
	[UserId] [int] NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SysConfig]    Script Date: 2016/4/11 7:12:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysConfig](
	[Name] [varchar](50) NOT NULL,
	[SysName] [varchar](50) NOT NULL,
	[Value] [varchar](50) NOT NULL,
 CONSTRAINT [PK_SystemConfig] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Task]    Script Date: 2016/4/11 7:12:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Task](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[Contents] [varchar](100) NOT NULL,
	[Status] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[AddStaffId] [int] NOT NULL,
	[ExecutorId] [int] NOT NULL,
	[FinishedTime] [datetime] NULL,
	[ExpectedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 2016/4/11 7:12:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NULL,
	[Creator] [varchar](50) NULL,
	[RoleIds] [varchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Permission] ON 

INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (4, N'首页访问', N'/home/index')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (5, N'部门管理', N'/department/index')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (6, N'职位管理', N'/position/index')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (7, N'员工管理', N'/staff/index')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (8, N'发布任务', N'/task/index')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (9, N'执行任务', N'/task/mytask')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (10, N'文件管理', N'/file/index')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (11, N'文章管理', N'/article/index')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (12, N'用户管理', N'/user/index')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (13, N'角色管理', N'/role/index')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (14, N'权限管理', N'/permission/index')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (15, N'系统参数', N'/sysconfig/index')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (16, N'文章详情', N'/article/detail')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (17, N'文章列表', N'/article/list')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (18, N'文章编辑', N'/article/edit')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (19, N'文章添加', N'/article/add')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (20, N'修改密码', N'/user/changepassword')
INSERT [dbo].[Permission] ([Id], [Name], [Url]) VALUES (21, N'信息中心', N'/message/index')
SET IDENTITY_INSERT [dbo].[Permission] OFF
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([Id], [Name], [SysName], [PermissionIds]) VALUES (7, N'员工账号', N'staffUser', N'16,17,10,4,21,8,9,20')
SET IDENTITY_INSERT [dbo].[Role] OFF
INSERT [dbo].[SysConfig] ([Name], [SysName], [Value]) VALUES (N'账号默认密码', N'defaultPassword', N'111111')
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [UserName], [Password], [CreateTime], [Creator], [RoleIds]) VALUES (15, N'xavierchan@163.com', N'96e79218965eb72c92a549dd5a330112', NULL, NULL, N'7')
INSERT [dbo].[User] ([Id], [UserName], [Password], [CreateTime], [Creator], [RoleIds]) VALUES (16, N'test@11.com', N'96e79218965eb72c92a549dd5a330112', NULL, NULL, N'7')
INSERT [dbo].[User] ([Id], [UserName], [Password], [CreateTime], [Creator], [RoleIds]) VALUES (17, N'test@00.com', N'96e79218965eb72c92a549dd5a330112', NULL, NULL, N'7')
INSERT [dbo].[User] ([Id], [UserName], [Password], [CreateTime], [Creator], [RoleIds]) VALUES (18, N'dsa', N'c4ca4238a0b923820dcc509a6f75849b', NULL, NULL, N'7')
SET IDENTITY_INSERT [dbo].[User] OFF
ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DF_Message_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'No'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级部门ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'ParentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门等级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'Grade'
GO
