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
	DayStamp int not null, --标记.每活动/每用户/每标记,只能有一条记录.对于签到活动,每天一个值.
	
	[Counter] int not null,--当前活动累计签到数量
	ContinuousCounter int not null,-- 连续签到计数量
	
	Tag varchar(50) null , --自定义标记
	CreateTime datetime not null default(getdate())
)
go

create unique  nonclustered index IX_QianDao
on dbo.QianDao(
	PromotionId asc,
	DayStamp asc,
	UserId asc
)
with(FILLFACTOR=80)
go


--签到
IF OBJECT_ID (N'dbo.up_qiandao_add', N'P') IS NOT NULL
	DROP proc dbo.up_qiandao_add;
GO
create proc dbo.up_qiandao_add
@PromotionId int,
@UserId varchar(50)
as 
begin
	declare @now datetime =  GETDATE() --当前时间 cast('2017-7-3' as datetime) -- 
	declare @dayStamp int --时间戳(精确到日)
	declare @result int --返回结果,1为签到成功,0为已签到,-1为错误
	declare @counter int --新的签到计数
	declare @continuousCounter int --新的连续签到计数

	set @dayStamp=DATEDIFF(dd,'1970-1-1',@now)
	
	if(select COUNT(1) from dbo.QianDao with(nolock) where 
		PromotionId=@PromotionId 
		and DayStamp=@dayStamp
		and UserId=@UserId) >0
		set @result=0 --当天已经签到,直接返回
	else
		begin
			declare @lastDayStamp int --最后一次签到时间戳
			declare @lastCounter int --最后一次签到计数
			declare @lastContinuousCounter int --最后一次连续签到计数
			
			select top 1  @lastDayStamp=DayStamp,@lastCounter=[Counter],@lastContinuousCounter=ContinuousCounter
			  from dbo.QianDao where 
				PromotionId=@PromotionId 
				and DayStamp<@dayStamp
				and UserId=@UserId
				order by Id desc
			
			set @counter=ISNULL(@lastCounter,0)+1
			set @lastDayStamp=ISNULL(@lastDayStamp,0)
			if(@dayStamp-@lastDayStamp>1)
				set @continuousCounter=1--说明非连续签到
			else
				begin
					set @continuousCounter=ISNULL(@lastContinuousCounter,0)+1
				end
				
			begin try
				insert into dbo.QianDao(PromotionId,UserId,DayStamp,[Counter],ContinuousCounter,CreateTime)
				values(@PromotionId,@UserId,@dayStamp,@counter,@continuousCounter,@now)
				set @result=1
			end try
			begin catch
				set @result=-1
				set @counter=null
				set @continuousCounter=null
			end catch 

		end
	--返回相关信息
	select @result as Result,
	@userId as UserId,
	@counter as [Counter],
	@continuousCounter as ContinuousCounter,
	@now as CreateTime,
	CAST(SCOPE_IDENTITY() AS bigint) AS Id
end
go


--获取最后一次签到记录
IF OBJECT_ID (N'dbo.up_qiandao_get_last', N'P') IS NOT NULL
	DROP proc dbo.up_qiandao_get_last;
GO
create proc dbo.up_qiandao_get_last
@PromotionId int,
@UserId varchar(50)
as 
begin 
select top 1 * from dbo.QianDao where 
				PromotionId=@PromotionId 
				and UserId=@UserId
				order by Id desc
end


--execute dbo.up_qiandao_add 1,'abc'
--execute dbo.up_qiandao_get_last 1,'abc'

