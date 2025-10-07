# **Todo API (.NET 8/9)**

This is a robust, controller-based ASP.NET Core Web API for managing a simple Todo list. It uses **in-memory storage** (for simplicity and demonstration), includes standard CRUD operations, and features advanced observability endpoints (Health Checks and Prometheus Metrics).

## **🚀 Features**

* **CRUD Operations:** Create, Read, Update tasks.
* **In-Memory Persistence:** Data is stored in IMemoryCache and persists only for the lifetime of the application instance.
* **Timestamping:** Automatically tracks CreatedAt and UpdatedAt timestamps for each task.
* **API Documentation:** Uses Swagger/OpenAPI for easy exploration and testing.
* **Observability:**
  * **Health Endpoint (/healthz):** Returns the current application status and version.
  * **Metrics Endpoint (/metrics):** Exposes performance metrics (request counts, response times) in a Prometheus-compatible format.
* **Containerization:** Configured with a multi-stage Dockerfile using the Red Hat UBI image.

## **⚙️ Setup and Installation**

### **Prerequisites**

* [.NET 8/9 SDK](https://dotnet.microsoft.com/download)
* [Docker](https://www.docker.com/products/docker-desktop) (Optional, for containerization)
* A tool for API testing (e.g., Swagger UI, Postman, curl).

### **Running Locally**

1. **Build and Run:**
   dotnet run

2. **Access:** The API will typically be available at https://localhost:\<port\>.

### **Container Setup**

The application uses a multi-stage Dockerfile based on the Red Hat UBI .NET images.

1. Build the Image (Setting the Version):
   Use a build argument to dynamically set the application's version tag.

  ```bash
   # Set the version environment variable
   export APP_VERSION="1.0.0-$(date \+%Y%m%d)"

   # Build the image, passing the version argument
   docker build --build-arg APP\_VERSION=${APP\_VERSION} -t todoapi-image:${APP\_VERSION} .

  ```

2. Run the Container:
   Map the exposed container port 8080 to a host port (e.g., 5000).

  ```bash
    docker run -d -p 5000:8080 -name todoapi todoapi-image:${APP\_VERSION}
  ```

## **📋 API Endpoints**

Once running, you can access the interactive documentation at the Swagger UI endpoint:

**Swagger UI:** https://localhost:\<port\>/swagger

| Method | Path | Description |
| :---- | :---- | :---- |
| GET | /api/Todo | Retrieves a list of all Todo items. |
| GET | /api/Todo/{id} | Retrieves a single Todo item by ID. |
| POST | /api/Todo | Creates a new Todo item. |
| PUT | /api/Todo/{id} | Updates an existing Todo item (used for status or title change). |

### **Data Model (TodoItem)**

public class TodoItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsComplete { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

## **📊 Observability Endpoints**

### **1\. Health Check Endpoint**

This endpoint returns a simple JSON status and the application's version, ideal for uptime checks and version tracking.

**Endpoint:** GET /healthz

| Key | Description |
| :---- | :---- |
| status | The overall health status (e.g., "Healthy"). |
| version | The application's version number (e.g., "1.0.0.0"). |

**Example Response:**

```json
{
  "status": "Healthy",
  "version": "1.0.0.0"
}
```
