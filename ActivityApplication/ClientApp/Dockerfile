# Use Node.js image
FROM node:14 AS build

# Install .NET SDK
RUN curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel Current

# Add dotnet to PATH
ENV PATH="$PATH:/root/.dotnet"

# Set working directory
WORKDIR /app

# Install dependencies
COPY package.json package-lock.json ./
RUN npm install

# Copy source code
COPY . .

# Build the app
RUN npm run build

# Development stage with hot reload
FROM build AS dev

# Set environment variables for hot reload
ENV CHOKIDAR_USEPOLLING=true

# Start the app
CMD ["npm", "start"]
