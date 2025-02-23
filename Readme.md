# Spotify Telex Integration API Documentation

## Overview
This API retrieves the latest music releases across genres from Spotify and integrates with Telex, a notification system that calls this API and sends updates to a designated Telex channel.

## Prerequisites
- .NET 6 or later installed
- A Spotify API key
- Telex integration configured

## Setup
1. Clone the repository:
   ```sh
   git clone <repository-url>
   cd <project-directory>
   ```
2. Restore dependencies:
   ```sh
   dotnet restore
   ```
3. Configure environment variables:
   - `SPOTIFY_CLIENT_ID`: Your Spotify client ID
   - `SPOTIFY_CLIENT_SECRET`: Your Spotify client secret
   - `TELEX_WEBHOOK`: Your Telex webhook
4. Run the API:
   ```sh
   dotnet run
   ```

## Base URL
```
https://baseURL
```

## Endpoints
### 1. API Health Check
**Endpoint:**
```
GET /
```
**Response:**
```json
{
  "message": "API is active - {token}",
  "status": 200
}
```

### 2. Get Telex Integration Config
**Endpoint:**
```
GET /integration.json
```
**Response:**
```json
{
  "data":{}
}
```

### 3. Get Latest Music Releases
**Endpoint:**
```
GET /GetLatestBarrz
```
**Response:**
```json
[
  "Artist1 - Song1",
  "Artist2 - Song2",
  "Artist3 - Song3"
]
```
**Status Code:** `200`

### 4. Receive Telex Message
**Endpoint:**
```
POST /tick

- sample url : https://hng12.bsite.net/tick
```
**Sample Request Body:**
```json
{
  "channel_id": "01952b42-efee-711d-bf7e-314d1942b740",
  "return_url": "https://ping.telex.im/v1/webhooks/01952b42-efee-711d-bf7e-314d1942b740",
  "settings" : [
    {
      "label" : "interval",
      "type" : "text",
      "required" : true,
      "default" : "0 8 * * 1-6"
    }
  ]
}
```
**Response:**
```json
{
  "status": "Accepted"
}
```
**Status Code:** `202`

## License
This project is open-source and available under the MIT License.

## Author
Developed by BahdMan.

