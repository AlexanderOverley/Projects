SELECT m.ItemID, m.Description, m.ListPrice 
FROM PET..Merchandise m 
WHERE m.ListPrice > (SELECT AVG(m.ListPrice) FROM PET..Merchandise m)

/*2.	List the employees and their total merchandise 
sales expressed as a percentage of total merchandise 
sales for all employees. TotalSales is Sum(SalePrice*Quantity). 
PctSales is TotalSales for an employee / TotalSales for all employees.*/

CREATE VIEW VSale2 AS
SELECT e.EmployeeID, e.FirstName, e.LastName, i.SaleID, SUM(i.SalePrice*i.Quantity) saletot
FROM PET..Employee e, PET..Sale s, Pet..SaleItem i
WHERE e.EmployeeID = s.EmployeeID
AND s.SaleID = i.SaleID
GROUP BY e.EmployeeID, i.SaleID, e.FirstName, e.LastName
ORDER BY e.LastName

CREATE VIEW VSaleTotComp AS
SELECT EmployeeID, FirstName, LastName, SUM(saletot) saletotcomp 
FROM VSale2 
GROUP BY EmployeeID, FirstName, LastName
ORDER BY FirstName

WITH EmpTot AS(
SELECT SUM(SaleTotComp) EmpTot FROM VSaleTotComp
)
SELECT EmployeeId, FirstName, LastName, saletotcomp, saletotcomp/EmpTot PctSale 
FROM EmpTot, VSaleTotComp
ORDER BY PctSale DESC

CREATE VIEW VMayTotal AS
SELECT c.CustomerID, c.FirstName, c.LastName, s.SaleID, s.SaleDate, SUM(i.SalePrice*i.Quantity) MayTotal
FROM PET..Customer c, Pet..Sale s, Pet..SaleItem i
WHERE c.CustomerID = s.CustomerID
AND s.SaleID = i.SaleID
AND s.SaleDate Between '2004-05-01' AND '2004-05-31'
GROUP BY c.CustomerID, c.FirstName, c.LastName, s.SaleID, s.SaleDate
Order By c.CustomerID

CREATE VIEW VOctTotal AS
SELECT c.CustomerID, c.FirstName, c.LastName, s.SaleID, s.SaleDate, SUM(i.SalePrice*i.Quantity) OctTotal
FROM PET..Customer c, Pet..Sale s, Pet..SaleItem i
WHERE c.CustomerID = s.CustomerID
AND s.SaleID = i.SaleID
AND s.SaleDate Between '2004-10-01' AND '2004-10-31'
GROUP BY c.CustomerID, c.FirstName, c.LastName, s.SaleID, s.SaleDate
Order By c.CustomerID

SELECT DISTINCT m.CustomerId, m.LastName, m.FirstName, m.MayTotal, o.OctTotal
FROM VMayTotal m, VOctTotal o
WHERE m.CustomerId = o.CustomerId
AND m.MayTotal > 100 
AND o.OctTotal > 50
ORDER BY m.LastName

SELECT * FROM VOctTotal



