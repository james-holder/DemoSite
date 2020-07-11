CREATE PROCEDURE [dbo].[sp_SaleLookup]
	@CashierId nvarchar(128),
	@SaleDate datetime2
AS
BEGIN
	SET NOCOUNT ON
	Select Id
	FROM dbo.Sale 
	WHERE CashierId = @CashierId
	AND SaleDate = @SaleDate
END

