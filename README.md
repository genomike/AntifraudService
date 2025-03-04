# Servicio Antifraude

## Descripción general
El Servicio Antifraude es un microservicio diseñado para validar transacciones financieras y actualizar su estado basado en criterios predefinidos. Utiliza una arquitectura hexagonal para asegurar la separación de responsabilidades y mantenibilidad. El servicio se comunica con otros componentes a través de Kafka para mensajería y utiliza una base de datos para el almacenamiento de transacciones.

## Funcionalidades
- **Creación de Transacciones**: Permite la creación de transacciones financieras con un estado inicial de "pendiente".
- **Validación de Transacciones**: Valida las transacciones basándose en los siguientes criterios:
  - Las transacciones con un valor mayor a 2000 son automáticamente rechazadas.
  - El valor acumulado de transacciones diarias no debe exceder 20000.
- **Actualización de Estado de Transacciones**: Actualiza el estado de las transacciones según los resultados de la validación, enviando mensajes de vuelta al creador de la transacción.

## Arquitectura
El proyecto sigue una arquitectura hexagonal, que consiste en las siguientes capas:
- **Capa de Dominio**: Contiene la lógica central del negocio, incluyendo entidades, objetos de valor y excepciones de dominio.
- **Capa de Aplicación**: Gestiona la lógica de aplicación, incluyendo comandos, consultas y servicios para la validación de transacciones.
- **Capa de Infraestructura**: Maneja el acceso a datos, mensajería y dependencias externas.
- **Capa de API**: Expone endpoints HTTP para operaciones de transacciones.

## Stack Tecnológico
- **.NET 8**: El framework principal para construir el microservicio.
- **Entity Framework Core**: Para interacciones con la base de datos.
- **Kafka**: Para mensajería entre servicios.
- **PostgreSQL**: Como base de datos para almacenar datos de transacciones.

## Primeros pasos
1. **Clonar el Repositorio**: 
   ```bash
   git clone <repository-url>
   cd AntifraudService
   ```

2. **Construir la Solución**: 
   ```bash
   dotnet build AntifraudService.sln
   ```

3. **Ejecutar la Aplicación**: Utiliza Docker Compose para iniciar los servicios.
   ```bash
   docker-compose up
   ```

4. **Acceder a la API**: La API estará disponible en `http://localhost:<port>`.

## Endpoints
- **Crear Transacción**: 
  - **POST** `/api/transactions`
  - Request Body:
    ```json
    {
      "sourceAccountId": "Guid",
      "targetAccountId": "Guid",
      "transferTypeId": 1,
      "value": 120
    }
    ```

- **Obtener Transacción**: 
  - **GET** `/api/transactions/{transactionExternalId}`
  - Query Parameters:
    - `createdAt`: Date

## Pruebas
Se incluyen pruebas unitarias para cada capa de la aplicación. Para ejecutar las pruebas, utiliza:
```bash
dotnet test
```