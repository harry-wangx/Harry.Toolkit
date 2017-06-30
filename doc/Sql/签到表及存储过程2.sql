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
	DayStamp int not null, --ʱ���(��ȷ����).ÿ�/ÿ�û�/ʱ���,ֻ����һ����¼.
	
	[Counter] int not null,--��ǰ�ǩ������
	
	Row_Version int not null default(0), --�汾��.��Ҫ����������ʱ��.
	CreateTime datetime not null default(getdate()),
	Tag varchar(50) null  --�Զ�����
)
go

--��������
create unique  nonclustered index IX_QianDao
on dbo.QianDao(
	DayStamp asc,
	PromotionId asc,
	UserId asc
)
include([Counter])
with(FILLFACTOR=70)
go


--ǩ��
IF OBJECT_ID (N'dbo.up_qiandao_add', N'P') IS NOT NULL
	DROP proc dbo.up_qiandao_add;
GO
create proc dbo.up_qiandao_add
@PromotionId int, --����
@UserId varchar(50), --�û����
@Type int =1, --ǩ������,1Ϊ�ۼ�ǩ��,2Ϊ����ǩ��
@MaxCount int =30,--���ǩ������
@AutoStart bit=0, --ǩ����ɺ�,�Ƿ��Զ���һ��ǩ��
@now datetime =null --ǩ��ʱ��
as 
begin
	if(@Type<>1 and @Type<>2)
		RAISERROR ('Parameter ''Type'' must be 1 or 2.' , 16, 1) WITH NOWAIT
	
	declare @dayStamp int --ʱ���(��ȷ����)
	declare @result int --���ؽ��,1Ϊǩ���ɹ�,0Ϊ��ǩ��,-1Ϊ����
	declare @counter int --�µ�ǩ������
	declare @lastDayStamp int --���һ��ǩ��ʱ���
	declare @lastCounter int --���һ��ǩ������

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
		set @result=-1 --�����Ѿ�ǩ��,ֱ�ӷ���
	else if( @AutoStart=0 and @lastCounter>=@MaxCount)
		set @result=-2 --�Ѵ����ǩ������,ֱ�ӷ���
	else
		begin
			if(@lastCounter>=@MaxCount)
				set @counter=1
			else
			begin
				if(@Type=1)
					set @counter=@lastCounter+1 --�ۼ�ǩ��
				else
				begin
					if(@dayStamp-@lastDayStamp>1)
						set @counter=1     --˵��������ǩ��
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
	--���������Ϣ
	select @result as Result,
	@userId as UserId,
	@counter as [Counter],
	@now as CreateTime,
	CAST(SCOPE_IDENTITY() AS bigint) AS Id
end
go


----��ȡ���һ��ǩ����¼
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


