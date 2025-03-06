import pandas as pd
from sklearn.linear_model import LinearRegression
import pyodbc
import datetime

# Database connection string (Modify this as per your `web.config`)
DB_SERVER = "(localdb)\\ProjectModels"
DB_NAME = "NarcoticsDrugsManagement"
DB_USER = "sa"
DB_PASSWORD = "admin"

# Establish connection to SQL Server
conn = pyodbc.connect(f"DRIVER={{SQL Server}};SERVER={DB_SERVER};DATABASE={DB_NAME};UID={DB_USER};PWD={DB_PASSWORD}")

# Load data from SQL table
query = "SELECT DrugName, DateOFSale, QuantitySold FROM SaleReturnTable"
df = pd.read_sql(query, conn)

# Close connection
conn.close()

# Convert DateOFSale to numeric format for ML model
df['DateOFSale'] = pd.to_datetime(df['DateOFSale']).astype(int) / 10**9

# Train AI Model
model = LinearRegression()
X = df[['DateOFSale']]
y = df['QuantitySold']
model.fit(X, y)

# Predict next month's sales
next_month = datetime.datetime.now() + datetime.timedelta(days=30)
predicted_sales = model.predict([[next_month.timestamp()]])

# Print output for ASP.NET to capture
print(f"Predicted Sales: {int(predicted_sales[0])}")
