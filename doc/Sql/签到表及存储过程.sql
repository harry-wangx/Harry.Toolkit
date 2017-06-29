use TestDb;
go

IF OBJECT_ID (N'dbo.QianDao', N'U') IS NOT NULL
	DROP TABLE dbo.QianDao;
GO

create table dbo.QianDao
(
	Id bigint identity(1,1) primary key,
	
	PromotionId int not null default(0), --����,
	UserId varchar(50) not null, --�û����
	DayStamp int not null, --���.ÿ�/ÿ�û�/ÿ���,ֻ����һ����¼.����ǩ���,ÿ��һ��ֵ.
	
	[Counter] int not null,--��ǰ��ۼ�ǩ������
	ContinuousCounter int not null,-- ����ǩ��������
	
	Tag varchar(50) null , --�Զ�����
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


--ǩ��
IF OBJECT_ID (N'dbo.up_qiandao_add', N'P') IS NOT NULL
	DROP proc dbo.up_qiandao_add;
GO
create proc dbo.up_qiandao_add
@PromotionId int,
@UserId varchar(50)
as 
begin
	declare @now datetime =  GETDATE() --��ǰʱ�� cast('2017-7-3' as datetime) -- 
	declare @dayStamp int --ʱ���(��ȷ����)
	declare @result int --���ؽ��,1Ϊǩ���ɹ�,0Ϊ��ǩ��,-1Ϊ����
	declare @counter int --�µ�ǩ������
	declare @continuousCounter int --�µ�����ǩ������

	set @dayStamp=DATEDIFF(dd,'1970-1-1',@now)
	
	if(select COUNT(1) from dbo.QianDao with(nolock) where 
		PromotionId=@PromotionId 
		and DayStamp=@dayStamp
		and UserId=@UserId) >0
		set @result=0 --�����Ѿ�ǩ��,ֱ�ӷ���
	else
		begin
			declare @lastDayStamp int --���һ��ǩ��ʱ���
			declare @lastCounter int --���һ��ǩ������
			declare @lastContinuousCounter int --���һ������ǩ������
			
			select top 1  @lastDayStamp=DayStamp,@lastCounter=[Counter],@lastContinuousCounter=ContinuousCounter
			  from dbo.QianDao where 
				PromotionId=@PromotionId 
				and DayStamp<@dayStamp
				and UserId=@UserId
				order by Id desc
			
			set @counter=ISNULL(@lastCounter,0)+1
			set @lastDayStamp=ISNULL(@lastDayStamp,0)
			if(@dayStamp-@lastDayStamp>1)
				set @continuousCounter=1--˵��������ǩ��
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
	--���������Ϣ
	select @result as Result,
	@userId as UserId,
	@counter as [Counter],
	@continuousCounter as ContinuousCounter,
	@now as CreateTime,
	CAST(SCOPE_IDENTITY() AS bigint) AS Id
end
go


--��ȡ���һ��ǩ����¼
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

