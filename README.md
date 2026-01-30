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


### 1️⃣ Build the Docker Image

From the project root, build the Docker image using the provided Dockerfile:

```powershell
docker build -t retail-api -f Dockerfile.txt .
```

---

### 2️⃣ Launch the Container

Run the container using a **bind mount** so the SQLite database file on your host machine is shared with the container.

**Windows (PowerShell):**

```powershell
docker run -d -p 5000:8080 --name retail-api-instance -v "${PWD}/db:/app/db" retail-api
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

## 📝 TODO List

- Review and tidy up code structure
- Use internal classes to enforce proper DDD boundaries
- Add unit tests
- Optimize `Program.cs`
- Optimize `Dockerfile.txt`


