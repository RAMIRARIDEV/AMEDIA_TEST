GO
CREATE TABLE [dbo].[GenereForMovie](
	[cod_pelicula] [int] NULL,
	[cod_genero] [int] NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[AlquilerPelicula](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[cod_pelicula] [int] NULL,
	[cod_usuario] [int] NULL,
	[precio] [float] NULL,
	[fecha] [date] NULL,
	[devuelta] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AlquilerPelicula]  WITH CHECK ADD  CONSTRAINT [fk_cod_pelicula] FOREIGN KEY([cod_pelicula])
REFERENCES [dbo].[tPelicula] ([cod_pelicula])
GO

ALTER TABLE [dbo].[AlquilerPelicula] CHECK CONSTRAINT [fk_cod_pelicula]
GO

ALTER TABLE [dbo].[AlquilerPelicula]  WITH CHECK ADD  CONSTRAINT [fk_cod_usuario] FOREIGN KEY([cod_usuario])
REFERENCES [dbo].[tUsers] ([cod_usuario])
GO

ALTER TABLE [dbo].[AlquilerPelicula] CHECK CONSTRAINT [fk_cod_usuario]
GO

CREATE TABLE [dbo].[VentaPelicula](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[cod_pelicula] [int] NULL,
	[cod_usuario] [int] NULL,
	[precio] [float] NULL,
	[fecha] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


--LISTA DE SPS




CREATE PROCEDURE [dbo].[addGenere] 
	@txt_desc VARCHAR(200),

	@codReturn int OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

		INSERT INTO tGenero VALUES(@txt_desc)

		SET @codReturn = @@IDENTITY

	
	RETURN 

END
GO


CREATE PROCEDURE [dbo].[AddMovie] 
	@txt_desc VARCHAR(500),
	@cant_disponibles_alquiler INT,
	@cant_disponibles_venta INT,
	@precio_alquiler decimal(18, 2),
	@precio_venta decimal(18, 2),

	@codReturn VARCHAR(200) OUTPUT
AS
BEGIN


	SET NOCOUNT ON;

		INSERT INTO tPelicula(txt_desc,cant_disponibles_alquiler,cant_disponibles_venta,precio_alquiler,precio_venta)
		VALUES(@txt_desc,@cant_disponibles_alquiler,@cant_disponibles_venta,@precio_alquiler,@precio_venta)

		SET @codReturn = 'Ingresado con exito.'

	
	RETURN 

END
GO



CREATE PROCEDURE [dbo].[AddUser] 
	@nro_doc VARCHAR(50),
	@txt_user VARCHAR(200),
	@txt_password VARCHAR(50),
	@txt_nombre VARCHAR(200),
	@txt_apellido VARCHAR(200),

	@cod_rol INT,
	@sn_activo INT,
	@codReturn VARCHAR(200) OUTPUT
AS
BEGIN


	SET NOCOUNT ON;

	IF EXISTS (SELECT TOP 1 cod_usuario FROM tUsers WHERE UPPER(@nro_doc) = UPPER(nro_doc))
	BEGIN
		SET @codReturn = 'El documento ingresado ya existe en la base de datos.'
	END
	ELSE
	BEGIN
		INSERT INTO tUsers(txt_user,txt_password,txt_nombre,txt_apellido,nro_doc,cod_rol,sn_activo)
		VALUES(@txt_user,@txt_password,@txt_nombre,@txt_apellido,@nro_doc,@cod_rol,@sn_activo)

		SET @codReturn = 'Ingresado con exito.'

	END
	
	RETURN 

END
GO


CREATE PROCEDURE [dbo].[AsignGenereToMovie] 
	@cod_genero INT,
	@cod_pelicula INT,

	@codReturn VARCHAR(200) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

		IF NOT EXISTS ( SELECT cod_pelicula FROM GenereForMovie where @cod_pelicula = cod_pelicula)
		BEGIN
			INSERT INTO GenereForMovie (cod_pelicula,cod_genero) VALUES ( @cod_pelicula,@cod_genero)
			SET @codReturn = 'Realizado con exito'
		END
		ELSE
		BEGIN
			SET @codReturn = 'La pelicula ya posee un genero asignado'
		END
	
	RETURN 

END
GO



CREATE PROCEDURE [dbo].[GetMoviesForRent] 
	@codReturn VARCHAR(200) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT * FROM TPELICULA WHERE CANT_DISPONIBLES_ALQUILER >0
	
	RETURN 

END
GO



CREATE PROCEDURE [dbo].[GetMoviesForSell] 
	@codReturn VARCHAR(200) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT * FROM TPELICULA WHERE CANT_DISPONIBLES_VENTA >0
	
	RETURN 

END
GO


CREATE PROCEDURE [dbo].[MovieRents] 
	@cod_usuario INT,
	@codReturn VARCHAR(200) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

		SELECT COD_PELICULA,PRECIO,FECHA FROM ALQUILERPELICULA WHERE COD_USUARIO = @cod_usuario
	
	RETURN 

END
GO


CREATE PROCEDURE [dbo].[MovieRentsHistority] 
	@codReturn VARCHAR(200) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

		SELECT tp.cod_pelicula, count(ap.id) as cantidad, SUM(ap.precio) as recaudado  FROM tpelicula tp 
		inner join AlquilerPelicula ap on ap.cod_pelicula = tp.cod_pelicula
		group by tp.cod_pelicula
	
	RETURN 

END
GO


CREATE PROCEDURE [dbo].[NotRefunded] 
	@codReturn VARCHAR(200) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT COD_PELICULA, COD_USUARIO from AlquilerPelicula where devuelta = 0 OR DEVUELTA IS NULL
	
	RETURN 

END
GO

CREATE PROCEDURE [dbo].[RefoundMovie] 
	@cod_pelicula int,
		@cod_usuario int,

	@codReturn VARCHAR(200) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE TPELICULA SET CANT_DISPONIBLEs_ALQUILER = CANT_DISPONIBLEs_ALQUILER +1 WHERE @cod_pelicula = cod_pelicula
	UPDATE ALQUILERPELICULA SET DEVUELTA = 1 WHERE @cod_pelicula = COD_PELICULA AND cod_usuario = @cod_usuario
	
	RETURN 

END
GO


CREATE PROCEDURE [dbo].[RemoveMovie] 
	@cod_pelicula INT,

	@codReturn VARCHAR(200) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

		UPDATE tPelicula SET cant_disponibles_alquiler = 0,cant_disponibles_venta =0
		WHERE cod_pelicula = @cod_pelicula

		SET @codReturn = 'Actualizado con exito.'

	
	RETURN 

END
GO


CREATE PROCEDURE [dbo].[RentMovie] 
	@cod_pelicula INT,
	@cod_usuario INT,
	@precio decimal(18,2),
	@codReturn VARCHAR(200) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

		IF ( SELECT cant_disponibles_alquiler FROM tPelicula where @cod_pelicula = cod_pelicula) > 0
		BEGIN
			INSERT INTO AlquilerPelicula (cod_pelicula,cod_usuario,precio,fecha) VALUES ( @cod_pelicula,@cod_usuario,@precio,GETDATE())
			SET @codReturn = 'Realizado con exito'
		END
		ELSE
		BEGIN
			SET @codReturn = 'La pelicula no posee stock'
		END
	
	RETURN 

END
GO


CREATE PROCEDURE [dbo].[SellMovie] 
	@cod_pelicula INT,
	@cod_usuario INT,
	@precio decimal(18,2),
	@codReturn VARCHAR(200) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

		IF ( SELECT cant_disponibles_venta FROM tPelicula where @cod_pelicula = cod_pelicula) > 0
		BEGIN
			INSERT INTO VentaPelicula (cod_pelicula,cod_usuario,precio,fecha) VALUES ( @cod_pelicula,@cod_usuario,@precio,GETDATE())
			SET @codReturn = 'Realizado con exito'
		END
		ELSE
		BEGIN
			SET @codReturn = 'La pelicula no posee stock'
		END
	
	RETURN 

END
GO


CREATE PROCEDURE [dbo].[UpdateMovie] 
	@cod_pelicula INT,
	@txt_desc VARCHAR(500),
	@cant_disponibles_alquiler INT,
	@cant_disponibles_venta INT,
	@precio_alquiler decimal(18, 2),
	@precio_venta decimal(18, 2),

	@codReturn VARCHAR(200) OUTPUT
AS
BEGIN


	SET NOCOUNT ON;

		UPDATE tPelicula SET txt_desc = @txt_desc ,cant_disponibles_alquiler = @cant_disponibles_alquiler,cant_disponibles_venta =@cant_disponibles_venta,
		precio_alquiler = @precio_alquiler,precio_venta = @precio_venta
		WHERE cod_pelicula = @cod_pelicula

		SET @codReturn = 'Actualizado con exito.'

	
	RETURN 

END
GO

