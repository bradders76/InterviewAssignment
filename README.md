## TimeTableScheduler

This application reads a CSV file containing track point data representing the start and end locations for train tracks. It then utilises a library to find the shortest path between two given stations.

### Usage

To use this application, follow these steps:

1. **Data Input**: Provide a CSV file containing track point data with columns for start location, end location, and distance between them.
2. **Parsing**: The application parses the track point data and builds a network of stations and their connections.
3. **Shortest Path**: You can then use the provided methods to find the shortest path between any two stations within the network.

### Implementation Details

The `StationNetwork` class implements the functionality required for parsing track points and finding the shortest path using Dijkstra's algorithm. Some key implementation details are as follows:

- **Parsing Track Points**: The `ParseTracks` method takes a list of track points and builds a network of stations and their connections. It also optimizes the process by setting location IDs for faster lookup.
- **Finding Shortest Path**: The `FindShortestPath` method finds the shortest path between two stations using Dijkstra's algorithm. It calculates the shortest distance and reconstructs the path accordingly.
- **Path Statistics**: The `FindShortestPathStats` method calculates statistics for the shortest path, including total distance and number of hops.

### Multithreading and Real-time Data Updates

For scenarios where track data points may change in real-time and multithreading is required, it's essential to ensure data integrity and thread safety. Consider the following approaches:

- **Data Locking**: Implement mechanisms to lock access to shared data structures, such as the list of track points, to prevent race conditions and ensure thread safety. Utilize locks or thread-safe collections to synchronize access to critical sections of code.

- **Real-time Updates**: If track data points are changing in real-time, implement a mechanism to handle dynamic updates to the network structure. This may involve periodically refreshing the network based on updated data or implementing event-driven updates to reflect changes instantly.

### Future Investigations

In addition to multithreading and real-time updates, future investigations may include:

- **Performance Analysis**: Conduct performance analysis using benchmarking techniques to identify bottlenecks and improve efficiency, especially in scenarios with concurrent access and dynamic data updates.

- **Data Validation**: Enhance data validation to handle faulty specifications and improve the robustness of the application, considering the dynamic nature of real-time data updates.

- **Scheduling and Time Delays**: Investigate adding scheduling capabilities to take into account time delays and train connection delays. This may involve incorporating time-based constraints into the shortest path algorithm or integrating with external scheduling systems.

### Libraries

- **Unit Testing**: MSTest is used for unit testing the application.

### Disclaimer

This application is a work in progress and may contain bugs or limitations. Use it at your own discretion.
