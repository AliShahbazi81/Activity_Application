<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
</head>
<body>
    <h1>Activity Application</h1>
    <p>The Activity Application is a comprehensive platform designed for users to create, share, and join activities. It enables users to interact with each other through comments, follow activities, and manage their participation. The application is built with a .NET Core backend and a React frontend, ensuring a seamless and interactive user experience.</p>
    <h2>Features</h2>
    <ul>
        <li><strong>User Authentication:</strong> Secure login and registration functionality.</li>
        <li><strong>Activity Management:</strong> Users can create, update, and delete activities.</li>
        <li><strong>Comments:</strong> Support for adding and viewing comments on activities.</li>
        <li><strong>Image Upload:</strong> Users can upload images related to the activities.</li>
        <li><strong>Real-time Updates:</strong> Utilizes SignalR for real-time updates in activity participation and comments.</li>
    </ul>
    <h2>Technology Stack</h2>
    <ul>
        <li><strong>Frontend:</strong> React, TypeScript, MobX for state management.</li>
        <li><strong>Backend:</strong> .NET 5.0, Entity Framework Core, SignalR for real-time web functionality.</li>
        <li><strong>Database:</strong> SQLite for development, with support for migration to SQL Server or PostgreSQL.</li>
        <li><strong>Authentication:</strong> JWT based authentication.</li>
        <li><strong>Cloud:</strong> Integration with Cloudinary for image hosting.</li>
    </ul>
    <h2>Getting Started</h2>
    <h3>Prerequisites</h3>
    <ul>
        <li>.NET 5.0 SDK</li>
        <li>Node.js</li>
    </ul>
    <h3>Setup</h3>
    <ol>
        <li><strong>Clone the repository</strong>
            <pre><code>git clone https://github.com/AliShahbazi81/Activity_Application.git
cd Activity_Application</code></pre>
        </li>
        <li><strong>Setup the backend</strong>
            <p>Navigate to the backend project directory and restore dependencies.</p>
            <pre><code>cd ActivityApplication
dotnet restore</code></pre>
            <p>Update the database.</p>
            <pre><code>dotnet ef database update</code></pre>
            <p>Run the backend.</p>
            <pre><code>dotnet run</code></pre>
        </li>
        <li><strong>Setup the frontend</strong>
            <p>Navigate to the frontend project directory.</p>
            <pre><code>cd ../ActivityApplication/ClientApp
npm install</code></pre>
            <p>Start the React application.</p>
            <pre><code>npm start</code></pre>
        </li>
    </ol>
    <h2>Contributing</h2>
    <p>Contributions to the Activity Application project are welcome. Please feel free to submit pull requests or open issues for any improvements or bug fixes.</p>
    <h2>License</h2>
    <p>This project is licensed under the MIT License - see the LICENSE.md file for details.</p>
</body>
</html>
