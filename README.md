[Matching API repository]([https://github.com/username/example-repo](https://github.com/tsyrulb/joinjoy-matching))

The JoinJoy Web API is a .NET backend service that manages users and activities for the JoinJoy platform. It connects to other parts of the system, like the Flask API for advanced recommendations, Azure SQL Database to store data, and Azure Blob Storage for profile photos.
![Description of GIF](JoinJoy4.gif)

Key points:

- Handles user registration, login, and profile updates, including personal details and photos.
- Manages activity creation and retrieval.
- Uses JWT tokens for secure user authentication.
- Communicates with the Flask API to get smarter user and activity recommendations.
- Integrates with Azure SQL Database for data storage and Azure Blob Storage for profile images.
- Uses Redis to cache data and improve performance.

To set it up, you need:
- .NET 6 or higher
- Access to Azure SQL Database with a valid connection string
- Access to Azure Blob Storage
- The Flask API endpoint
- A Redis instance

You can run the Web API on your local machine or inside a Docker container. Environment variables and configuration files let you customize connections and secrets, such as database keys and JWT secrets.

Key endpoints:
- Register and login users
- Upload profile photos
- Create and list activities
- Get recommended users or activities (through the Flask API)

Challenges solved:
- Securing user data with JWT-based authentication
- Efficiently handling large data sets with Entity Framework Core
- Integrating multiple external services (Azure SQL, Blob Storage, Flask API) for a smooth user experience

This Web API can be deployed to various cloud platforms and easily integrated with the rest of the JoinJoy system.
