CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id int = 0
AS
BEGIN
	SET NOCOUNT ON 
	SET NOCOUNT ON;

	SELECT 
	ID,
	ProductName,
	[Description],
	RetailPrice,
	QuantityInStock,
	IsTaxable
	FROM dbo.Product
	WHERE ID = @Id

END

