
CREATE VIEW VSELECTCUSTOMERS AS
SELECT * FROM PET..Customer
WHERE ZIPCODE = 40342

SELECT * FROM VSELECTCUSTOMERS

SELECT * FROM PET..Merchandise

SELECT * FROM PET..OrderItem

SELECT * FROM Pet..SaleItem

CREATE VIEW VCOST AS
SELECT o.ItemID, AVG(o.Cost) AvgCost
FROM Pet..OrderItem o
GROUP BY o.ItemID

CREATE VIEW VSALE AS
SELECT s.ItemID, AVG(s.SalePrice) AvgSalePrice 
FROM PET..SaleItem s
GROUP BY s.ItemID

SELECT * FROM VSALE
SELECT * FROM VCOST

SELECT m.ItemID, m.Description, c.AvgCost, s.AvgSalePrice 
FROM VCOST c INNER JOIN PET..Merchandise m ON c.ItemID = m.ItemID
INNER JOIN VSALE s ON m.ItemID = s.ItemID
WHERE s.AvgSalePrice > 1.5*c.AvgCost

SELECT s.SupplierID, s.Name, mo.ShippingCost 
From PET..Supplier s, PET..MerchandiseOrder mo
WHERE s.SupplierID = mo.SupplierID

CREATE VIEW VORDERTOT AS
SELECT o.PONumber, SUM(o.Quantity*O.Cost) OrderTotalCost
FROM PET..OrderItem o
GROUP BY o.PONumber

SELECT * FROM VORDERTOT

CREATE VIEW VSUPPLIER AS
SELECT s.Name, s.SupplierID FROM PET..Supplier s

SELECT * FROM VSUPPLIER

CREATE VIEW VORDEROWNER AS
SELECT ss.SupplierID, ss.Name, mo.PONumber
FROM PET..MerchandiseOrder mo, VSUPPLIER ss
WHERE mo.SupplierID = ss.SupplierID
ORDER BY ss.SupplierID

SELECT * FROM VORDEROWNER ORDER BY SupplierID
SELECT * FROM VSUPPLIER
SELECT * FROM VORDERTOT

CREATE VIEW VPCTSHIPCOST AS
SELECT ss.SupplierID, ss.Name, ROUND(AVG(mo.ShippingCost/ot.OrderTotalCost), 2) PctShipCost
FROM VORDERTOT ot INNER JOIN PET..MerchandiseOrder mo ON ot.PONumber = mo.PONumber
INNER JOIN VSUPPLIER ss ON ss.SupplierID = mo.SupplierID 
GROUP BY ss.SupplierID, ss.Name
ORDER BY PctShipCost DESC

SELECT * FROM VPCTSHIPCOST c WHERE c.PctShipCost = (SELECT MAX(PctShipCost) FROM VPCTSHIPCOST)


CREATE VIEW VCUSTOMER AS
SELECT CustomerID, FirstName, LastName FROM PET..Customer

SELECT * FROM VCUSTOMER


SELECT * FROM PET..Sale s
SELECT * FROM PET..SaleAnimal a
SELECT * FROM VCUSTOMER c

CREATE VIEW VSUMSALE AS
SELECT c.CustomerID, c.FirstName, c.LastName, SUM(a.SalePrice) SumOfSalePrice
FROM PET..Sale s, PET..SaleAnimal a, VCUSTOMER c
WHERE c.CustomerID = s.CustomerID
AND s.SaleID = a.SaleID
GROUP BY c.CustomerID, c.FirstName, c.LastName
ORDER BY c.FirstName, c.LastName

SELECT * FROM VSUMSALE

CREATE VIEW VMERCHTOT AS
SELECT c.CustomerID, c.FirstName, c.LastName, SUM(s.Quantity*s.SalePrice) MerchTot
FROM PET..SaleItem s, PET..Sale ss, VCUSTOMER c
WHERE c.CustomerID = ss.CustomerID
AND ss.SaleID = s.SaleID
GROUP BY c.CustomerID, c.FirstName, c.LastName

SELECT * FROM VMERCHTOT

CREATE VIEW VGT AS
SELECT s.CustomerID,s.FirstName,s.LastName, m.MerchTot, s.SumOfSalePrice, 
(ISNULL(m.MerchTot,0)+ISNULL(s.SumOfSalePrice,0)) AS GrandTotal
FROM VMERCHTOT m INNER JOIN VSUMSALE s ON m.CustomerID = s.CustomerID 
GROUP BY s.CustomerID,s.FirstName,s.LastName, m.MerchTot, s.SumOfSalePrice

CREATE VIEW VCUSTOMER AS
SELECT CustomerID, FirstName, LastName FROM PET..Customer

SELECT * FROM VCUSTOMER


SELECT * FROM PET..Sale s
SELECT * FROM PET..SaleAnimal a
SELECT * FROM VCUSTOMER c

CREATE VIEW VSUMSALE AS
SELECT c.CustomerID, c.FirstName, c.LastName, SUM(a.SalePrice) SumOfSalePrice
FROM PET..Sale s, PET..SaleAnimal a, VCUSTOMER c
WHERE c.CustomerID = s.CustomerID
AND s.SaleID = a.SaleID
GROUP BY c.CustomerID, c.FirstName, c.LastName
ORDER BY c.FirstName, c.LastName

SELECT * FROM VSUMSALE

CREATE VIEW VMERCHTOT AS
SELECT c.CustomerID, c.FirstName, c.LastName, SUM(s.Quantity*s.SalePrice) MerchTot
FROM PET..SaleItem s, PET..Sale ss, VCUSTOMER c
WHERE c.CustomerID = ss.CustomerID
AND ss.SaleID = s.SaleID
GROUP BY c.CustomerID, c.FirstName, c.LastName

SELECT * FROM VMERCHTOT

CREATE VIEW VGT AS
SELECT s.CustomerID,s.FirstName,s.LastName, m.MerchTot, s.SumOfSalePrice, 
(ISNULL(m.MerchTot,0)+ISNULL(s.SumOfSalePrice,0)) AS GrandTotal
FROM VMERCHTOT m INNER JOIN VSUMSALE s ON m.CustomerID = s.CustomerID 
GROUP BY s.CustomerID,s.FirstName,s.LastName, m.MerchTot, s.SumOfSalePrice

SELECT * FROM VGT WHERE GrandTotal = (SELECT max(GrandTotal) as GrandTotal FROM VGT)