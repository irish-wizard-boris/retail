# Abysalto Retail API – Cart Module

An **ASP.NET Core 8 Web API** implemented as part of a **modular monolith architecture**. This module manages **shopping cart operations**, uses **SQLite** as a portable database, and is **fully containerized with Docker** for easy setup and execution.

---

## 🛠️ Prerequisites

Ensure the following tools are installed on your machine:

### 🔹 [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
### 🔹 [Docker Desktop](https://www.docker.com/products/docker-desktop/)
### 🔹 [EF Core CLI Tools](https://learn.microsoft.com/ef/core/cli/dotnet)

Install command:

```bash
dotnet tool install --global dotnet-ef
```

---

## 🚀 Installation & Setup

### 1️⃣ Initialize the Database

Before running the API inside a container, generate the physical **SQLite database file** so EF Core migrations are applied correctly.

```powershell
# Navigate to the source folder
cd retail/src

# Generate the cart.db file using EF Core migrations
dotnet ef database update \
  --project Modules/Cart/Cart.Infrastructure \
  --startup-project API
```

> ⚠️ Ensure the `src/db` directory exists before running this command.

---

### 2️⃣ Build the Docker Image

From the project root, build the Docker image using the provided Dockerfile:

```powershell
docker build -t retail-api -f Dockerfile.txt .
```

---

### 3️⃣ Launch the Container

Run the container using a **bind mount** so the SQLite database file on your host machine is shared with the container.

**Windows (PowerShell):**

```powershell
docker rm -f retail-api-instance

docker run -d \
  -p 5000:8080 \
  --name retail-api-instance \
  -v "${PWD}/db:/app/db" \
  retail-api
```

---

## 🔍 API Verification

Once the container is running, verify that the database schema is correctly applied.

**Admin Cart View**

```
http://localhost:5000/api/admin/cart
```

### Expected Result

* An empty JSON array `[]` if no carts exist
* Or a list of cart objects if data is present

---

## ⚠️ Troubleshooting

### SQLite Error: `no such column: UnitPrice`

This error indicates that the local `cart.db` file is **out of sync** with the EF Core migrations.

**Fix:**

1. Stop and remove the container:

   ```powershell
   docker rm -f retail-api-instance
   ```
2. Delete the database file:

   ```powershell
   src/db/cart.db
   ```
3. Re-run **Step 1 – Initialize the Database**
4. Restart the container

---

### Docker Volume Errors

If you encounter a Docker daemon error related to invalid characters:

* Make sure you are running the command from **PowerShell**
* Ensure your current directory is `src` so `${PWD}` resolves correctly

---

## 📁 Project Structure (Database)

* **Migrations**
  `src/Modules/Cart/Infrastructure/Data/Migrations/`

* **SQLite Database File**
  `src/db/cart.db` *(excluded from Git)*

---

## 💡 Pro Tip: Final Check

Ensure the `src/db` folder exists before running `dotnet ef database update`. Otherwise, SQLite may attempt to create the database inside the `bin` directory, leading to unexpected behavior.

---
