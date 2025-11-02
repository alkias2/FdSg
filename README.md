# Fishing Diary Application

A comprehensive fishing diary application that helps anglers record and analyze their fishing experiences with detailed environmental data integration.

## Overview

A comprehensive fishing diary application that records and displays fishing catches along with their environmental conditions. The application integrates with StormGlass API to provide detailed weather, tide, and lunar data for each fishing session.

## Features

### Catch Recording

- Date and time logging
- High-quality image capture (16:9 format)
- Location tracking with GPS coordinates
- District and area information

### Environmental Data Integration

1. **Weather Conditions** (via StormGlass API)

   - Air and water temperature
   - Cloud coverage
   - Wind speed and direction
   - Wave and current information

2. **Lunar Data**

   - Moon phase visualization
   - Moonrise/moonset times
   - Illumination percentage
   - Visual moon phase representation

3. **Tidal Information**

   - Tide height
   - Tide type (ebb/flow)
   - Timing information

## Technical Architecture

### Backend Structure

- **ASP.NET Core 6.0 MVC**
- **Entity Framework Core** for database management
- Layered architecture:
  - `Fd.Core`: Core infrastructure and extensions
  - `Fd.Data`: Entity Framework models and DbContext
  - `Fd.Services`: External services integration (StormGlass)
  - `Fd.Web`: MVC application
  - `Fd.WebFramework`: Web framework extensions

### Frontend Technologies

- Bootstrap for responsive design
- Font Awesome & Material Design Icons
- Custom CSS styling
- JavaScript for dynamic features

### Data Flow

1. **Data Collection**

   - Automatic weather data retrieval
   - Location-based information gathering
   - Image processing and storage

2. **Storage**

   - Entity Framework for data persistence
   - Concurrent access handling
   - Efficient caching mechanisms

3. **Presentation**

   - Responsive card-based UI
   - Interactive image viewing
   - Dynamic weather icons
   - Location visualization

## Best Practices

### Security

- Null checking implementation
- Exception handling
- Parameterized queries
- Data validation

### Performance

- Parallel data fetching
- Database caching
- Optimized image handling
- Responsive design

### Maintainability

- Clean architecture
- Separation of concerns
- XML documentation
- Consistent coding patterns

## Future Enhancements

1. Google Maps integration for locations
2. Catch statistics and analytics
3. Weather prediction features
4. Enhanced mobile responsiveness
5. Social sharing capabilities

## Technical Requirements

- .NET 6.0 SDK
- SQL Server
- StormGlass API access
- Modern web browser

## Getting Started

1. Clone the repository
2. Configure database connection
3. Set up StormGlass API credentials
4. Build and run the application

## Contributing

Contributions are welcome! Please feel free to submit pull requests.

## License

This project is licensed under the MIT License - see the LICENSE file for details.