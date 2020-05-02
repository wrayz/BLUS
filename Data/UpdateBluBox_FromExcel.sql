UPDATE BluBoxs 
	SET CountryCode = c.CountryCode,
		ShippingDate = b.Date,
		UseType = d.ParameterValue
FROM BluBoxs a INNER JOIN 
	 BluBox_v1 b ON (a.SN = b.[Serial No#]) LEFT JOIN
	 Countries c ON (b.Location = c.CountryName) LEFT JOIN
	 GlobalParameters d ON (b.[Use for] = d.ParameterDesc)