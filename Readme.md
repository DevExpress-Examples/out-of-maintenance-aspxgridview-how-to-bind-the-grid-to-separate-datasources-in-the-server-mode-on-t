# ASPxGridView - How to bind the grid to separate datasources in the server mode on the fly


<p>This example demonstrates how to bind the grid to separate datasources in the server mode on the fly.<br />
This is an ASP.NET  version of the CTE technology described in the <a href="https://www.devexpress.com/Support/Center/p/K18528">How to use custom SQL queries as a data source in XPO via Common Table Expressions (CTE)</a> KB Article<br />
</p><p>To accomplish this task, send a callback to the grid. <br />
Create an instance of the XpoHelper class (see the <a href="https://www.devexpress.com/Support/Center/p/K18061">How to use XPO in an ASP.NET (Web) application</a> article for more information). Use your own GetConnectionString method in the XpoHelper class based on your real database connection.</p><p>Obtain the ColumnSchema of the database and use it to create the grid's datasource via GetXCS method.<br />
</p><p>Since we are changing the grid's datasource dynamically, it is necessary to clear all old columns before binding to the new datasource.</p><p>Do not forget to restore the grid's hierarchy in the Page_Init method by binding the grid to the current datasource.</p>

<br/>


