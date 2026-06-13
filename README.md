# TaskManager API

API REST para gestión de tareas con autenticación JWT, construida con .NET 9, Entity Framework Core y SQL Server.

## 🚀 Tecnologías

- .NET 9
- C#
- Entity Framework Core
- SQL Server
- JWT Authentication
- BCrypt
- Swagger / OpenAPI

## 📋 Endpoints

### Auth
- `POST /api/auth/register` — Registrar usuario
- `POST /api/auth/login` — Login y obtener token JWT

### Tareas (requieren token)
- `GET /api/tareas` — Listar tareas del usuario
- `POST /api/tareas` — Crear tarea
- `GET /api/tareas/{id}` — Obtener tarea por ID
- `PUT /api/tareas/{id}` — Actualizar tarea
- `DELETE /api/tareas/{id}` — Eliminar tarea

## ⚙️ Instalación

1. Clona el repositorio
```bash
git clone https://github.com/tuusuario/TaskManagerAPI.git
cd TaskManagerAPI
```

2. Configura la cadena de conexión en `appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=TaskManagerDB;User Id=TU_USUARIO;Password=TU_PASSWORD;TrustServerCertificate=True;"
}
```

3. Crea la base de datos
```bash
dotnet ef database update
```

4. Corre el proyecto
```bash
dotnet run
```

5. Abre Swagger en `http://localhost:5070/swagger`

## 🔐 Autenticación

1. Regístrate en `/api/auth/register`
2. Haz login en `/api/auth/login` y copia el token
3. En Swagger haz clic en **Authorize** y pega el token
4. Ya puedes usar los endpoints de tareas

## 👨‍💻 Autor

Diego Aguirre — [LinkedIn](https://www.linkedin.com/in/diego-aguirre-uribe-740274212/) — [GitHub](https://github.com/Diego51602)