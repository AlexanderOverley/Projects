SELECT m.ItemID, m.Description, m.QuantityOnHand 
FROM PET..Merchandise m
LEFT OUTER JOIN PET..OrderItem oi ON m.ItemID = oi.ItemID
LEFT OUTER JOIN PET..MerchandiseOrder mo ON oi.PONumber = mo.PONumber
WHERE m.QuantityOnHand > 100
AND oi.ItemID IS NULL OR YEAR(mo.OrderDate) <> 2004

SELECT m.ItemID, m.Description, m.QuantityOnHand 
FROM PET..Merchandise m
WHERE
m.ItemID IN (SELECT m.ItemID FROM PET..Merchandise m
LEFT OUTER JOIN PET..OrderItem oi ON m.ItemID = oi.ItemID
LEFT OUTER JOIN PET..MerchandiseOrder mo ON oi.PONumber = mo.PONumber
WHERE m.QuantityOnHand > 100
AND oi.ItemID IS NULL OR YEAR(mo.OrderDate) <> 2004)


WITH target AS(SELECT a.SupplierID, a.OrderDate, ('Sold Us Animals in June') AS 'Type of Sale'
FROM PET..AnimalOrder a
WHERE MONTH(a.OrderDate) = 6 
UNION
SELECT m.SupplierID, m.OrderDate, ('Sold Us Merchandise in June') AS 'Type of Sale'
FROM PET..MerchandiseOrder m
WHERE MONTH(m.OrderDate) = 6 
)
SELECT s.Name, t.[Type of Sale] FROM PET..Supplier s, target t
WHERE t.SupplierID = s.SupplierID

SELECT DISTINCT m.ItemID, m.Description, m.ListPrice 
FROM PET..Merchandise m
LEFT OUTER JOIN PET..SaleItem i ON m.ItemID = i.ItemID
LEFT OUTER JOIN PET..Sale s ON i.SaleID = s.SaleID
WHERE m.ListPrice > 50 AND MONTH(s.SaleDate) <> 7

WITH target AS(SELECT i.SaleID, i.ItemID, MONTH(s.SaleDate) AS SaleDate
FROM PET..Merchandise m LEFT OUTER JOIN PET..SaleItem i ON m.ItemID = i.ItemID
LEFT OUTER JOIN PET..Sale s ON s.SaleID = i.SaleID
WHERE MONTH(s.SaleDate) = 7
)
SELECT DISTINCT m.ItemID, m.Description, m.ListPrice
FROM PET..Merchandise m
WHERE m.ListPrice > 50 AND NOT EXISTS (SELECT * 
					FROM target t 
					WHERE m.ItemID = t.ItemID)


SELECT i.SaleID, i.ItemID, MONTH(s.SaleDate) AS SaleDate
FROM PET..Merchandise m LEFT OUTER JOIN PET..SaleItem i ON m.ItemID = i.ItemID
LEFT OUTER JOIN PET..Sale s ON s.SaleID = i.SaleID
WHERE MONTH(s.SaleDate) = 7