use TestDb;
go

IF OBJECT_ID (N'dbo.QianDao', N'U') IS NOT NULL
	DROP TABLE dbo.QianDao;
GO

create table dbo.QianDao
(
	Id bigint identity(1,1) primary key,
	
	PromotionId int not null default(0), --活动编号,
	UserId varchar(50) not null, --用户编号
	DayStamp int not null, --时间戳(精确到天).每活动/每用户/时间戳,只能有一条记录.
	
	[Counter] int not null,--当前活动签到数量
	
	Row_Version int not null default(0), --版本号.需要处理并发操作时用.
	CreateTime datetime not null default(getdate()),
	Tag varchar(50) null  --自定义标记
)
go

--创建索引
create unique  nonclustered index IX_QianDao
on dbo.QianDao(
	DayStamp asc,
	PromotionId asc,
	UserId asc
)
include([Counter])
with(FILLFACTOR=70)
go


--签到
IF OBJECT_ID (N'dbo.up_qiandao_add', N'P') IS NOT NULL
	DROP proc dbo.up_qiandao_add;
GO
create proc dbo.up_qiandao_add
@PromotionId int, --活动编号
@UserId varchar(50), --用户编号
@Type int =1, --签到类型,1为累计签到,2为连续签到
@MaxCount int =30,--最大签到次数
@AutoStart bit=0, --签到完成后,是否自动新一轮签到
@now datetime =null --签到时间
as 
begin
	if(@Type<>1 and @Type<>2)
		RAISERROR ('Parameter ''Type'' must be 1 or 2.' , 16, 1) WITH NOWAIT
	
	declare @dayStamp int --时间戳(精确到日)
	declare @result int --返回结果,1为签到成功,0为已签到,-1为错误
	declare @counter int --新的签到计数
	declare @lastDayStamp int --最后一次签到时间戳
	declare @lastCounter int --最后一次签到计数

	set @now =ISNULL(@now,GETDATE())
	set @dayStamp=DATEDIFF(dd,'1970-1-1',@now)
	
	select top 1  @lastDayStamp=DayStamp,@lastCounter=[Counter]
	  from dbo.QianDao with(nolock,index=IX_QianDao) where 
	    DayStamp>@dayStamp-@MaxCount
		and PromotionId=@PromotionId 
		and UserId=@UserId
		order by Id desc
		
	set @lastDayStamp=ISNULL(@lastDayStamp,0)
	set @lastCounter=ISNULL(@lastCounter,0)
	
	if(@lastDayStamp>=@dayStamp)
		set @result=-1 --当天已经签到,直接返回
	else if( @AutoStart=0 and @lastCounter>=@MaxCount)
		set @result=-2 --已达最大签到次数,直接返回
	else
		begin
			if(@lastCounter>=@MaxCount)
				set @counter=1
			else
			begin
				if(@Type=1)
					set @counter=@lastCounter+1 --累计签到
				else
				begin
					if(@dayStamp-@lastDayStamp>1)
						set @counter=1     --说明非连续签到
					else
						set @counter=@lastCounter+1				
				end
			end
				
			begin try
				insert into dbo.QianDao(PromotionId,UserId,DayStamp,[Counter],CreateTime,Row_Version)
				values(@PromotionId,@UserId,@dayStamp,@counter,@now,0)
				set @result=1
			end try
			begin catch
				set @result=-1000
				set @counter=null
			end catch 

		end
	--返回相关信息
	select @result as Result,
	@userId as UserId,
	@counter as [Counter],
	@now as CreateTime,
	CAST(SCOPE_IDENTITY() AS bigint) AS Id
end
go


----获取最后一次签到记录
--IF OBJECT_ID (N'dbo.up_qiandao_get_last', N'P') IS NOT NULL
--	DROP proc dbo.up_qiandao_get_last;
--GO
--create proc dbo.up_qiandao_get_last
--@PromotionId int,
--@UserId varchar(50)
--as 
--begin 
--select top 1 * from dbo.QianDao with(nolock) where 
--				PromotionId=@PromotionId 
--				and UserId=@UserId
--				order by Id desc
--end


--execute dbo.up_qiandao_add 1,'abc',2,3,1,'2017-1-1'


